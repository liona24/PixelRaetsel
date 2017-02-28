using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using GraphicsUtility;
using SmartTools;

namespace PixelRaetsel
{
    class PictureBox2 : Panel
    {
        private Bitmap img_org = null;
        private Frame img_gray = null;
        private Bitmap img_display = null;
        private Vec2I sampleStep = new Vec2I(1, 1);

        private List<List<Vec2I>> regionSeeds = new List<List<Vec2I>>();
        private List<Color> colors = new List<Color>();
        private int activeRegion = -1;

        public int RegionCount { get { return colors.Count; } }
        public int ActiveRegion
        {
            get { return activeRegion; }
            set
            {
                if (value >= 0 && value < colors.Count)
                    activeRegion = value;
            }
        }

        public PictureBox2()
        {
            this.AutoScroll = true;
            this.DoubleBuffered = true;
            this.AutoScrollMinSize = new Size(0, 0);
        }

        /// <summary>
        /// Sets the default image
        /// </summary>
        public void SetImage(Bitmap img)
        {
            reset();
            img_org = img;
            img_gray = new Frame(img);
            Imaging.EqualizeHist(img_gray);
            img_display = img_gray.GetBitmap();

            using (var gr = CreateGraphics())
                AutoScrollMinSize = new Size(img.Width, img.Height);
            Invalidate();
        }

        /// <summary>
        /// Changes the color of the region at the given index
        /// </summary>
        public void SetColor(int index, Color color)
        {
            colors[index] = color;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
            if (img_display != null)
                e.Graphics.DrawImage(img_display, 0, 0);

            //Draw grid
            if (sampleStep.X > 1)
            {
                using (Pen p = new Pen(Color.Red, 0.5f))
                {
                    int width = (int)e.Graphics.ClipBounds.Width;
                    int height = (int)e.Graphics.ClipBounds.Height;
                    for (int i = 0; i < width; i += sampleStep.X)
                        e.Graphics.DrawLine(p, new Point(i, 0), new Point(i, height));
                }
            }
            if (sampleStep.Y > 1)
            {
                using (Pen p = new Pen(Color.Red, 0.5f))
                {
                    int width = (int)e.Graphics.ClipBounds.Width;
                    int height = (int)e.Graphics.ClipBounds.Height;
                    for (int i = 0; i < height; i += sampleStep.Y)
                        e.Graphics.DrawLine(p, new Point(0, i), new Point(width, i));
                }
            }

            //Draw seeds as overlay
            foreach (var i in regionSeeds.Zip(colors, (l, c) => Tuple.Create(l, c)))
            {
                var color = i.Item2;
                using (Brush b = new SolidBrush(Color.FromArgb(128, color)))
                {
                    foreach (var seed in i.Item1)
                        e.Graphics.FillRectangle(b, new Rectangle(seed.X, seed.Y, sampleStep.X, sampleStep.Y));
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Resets the panel to initial state
        /// </summary>
        public void RollBack()
        {
            SetImage(img_org);
        }

        /// <summary>
        /// Adds a new region, to which seeds can be added. Sets active region to the new one.
        /// </summary>
        public void AddRegion(Color color)
        {
            regionSeeds.Add(new List<Vec2I>());
            colors.Add(color);
            activeRegion = regionSeeds.Count - 1;
        }
        /// <summary>
        /// Adds a seed point to the currently active region
        /// </summary>
        /// <param name="position">The position of the seed point</param>
        public void AddSeedToActive(Vec2I position)
        {
            regionSeeds[activeRegion].Add(position);
        }

        /// <summary>
        /// Samples the original image with the given sample steps and draws the resulting over scaled image to the panel 
        /// </summary>
        public void ChangeResolution(int sampleStepX, int sampleStepY)
        {
            sampleStep = new Vec2I(sampleStepX, sampleStepY);
            var tmp = new Frame(img_gray.Width / sampleStepX, img_gray.Height / sampleStepY);
            //TODO this step seems kind of too much work for the result needed
            Imaging.Scale(img_gray, tmp, InterpolationType.NearestNeighbor);
            var tmp2 = new Frame3(img_org.Width, img_org.Height);
            Imaging.Scale(tmp, tmp2, InterpolationType.NearestNeighbor);
            img_display = tmp2.GetBitmap();
            Invalidate();
        }
        
        public new void Dispose()
        {
            base.Dispose();
            img_org.Dispose();
            img_display.Dispose();
            img_gray = null;
        }

        private void reset()
        {
            sampleStep = new Vec2I(1, 1);
            regionSeeds = new List<List<Vec2I>>();
            colors = new List<Color>();
            activeRegion = -1;
        }
    }
}
