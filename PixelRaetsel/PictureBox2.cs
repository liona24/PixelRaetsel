using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using GraphicsUtility;
using SmartTools;

namespace PixelRaetsel
{
    class PictureBox2 : Panel
    {
        struct P2
        {
            public readonly int X, Y;
            public P2(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override int GetHashCode()
            {
                return -X ^ Y;
            }
        }
        private Bitmap img_org = null;
        private Frame img_gray = null;
        private Frame img_gray_small = null;
        private Bitmap img_display = null;
        private P2 sampleStep = new P2(1, 1);

        //key is position of insertion, sampled
        //value is the index of the region which owns it
        private Dictionary<P2, int> regionSeeds = new Dictionary<P2, int>();
        private List<Color> colors = new List<Color>();
        private int activeRegion = -1;

        private bool recordMouseTrace = false;

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
        public int SampleStepX { get { return sampleStep.X; } }
        public int SampleStepY { get { return sampleStep.Y; } }
        public bool DeleteTrace { get; set; }
        public int OverlayAlpha { get; set; }

        public PictureBox2()
        {
            this.AutoScroll = true;
            this.DoubleBuffered = true;
            this.AutoScrollMinSize = new Size(0, 0);
            OverlayAlpha = 128;
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
            
        /// <summary>
        /// Segmentates the image using the seeds planted.
        /// </summary>
        public void Segmentate()
        {
            var seeds = new List<Vec2I>();
            var labels = new List<int>();
            foreach (var kvp in regionSeeds)
            {
                seeds.Add(new Vec2I(kvp.Key.X, kvp.Key.Y));
                labels.Add(kvp.Value + 1);
            }
            var rwSeg = new RandomWalkerSegmenter<byte>(seeds.ToArray(), labels.ToArray());
            var res = rwSeg.Segmentate(img_gray_small);
            for (int i = 0; i < img_gray_small.Width; i++)
            {
                for (int j = 0; j < img_gray_small.Height; j++)
                {
                    int index = res[i][j] - 1;
                    var pos = new P2(i, j);
                    if (index >= 0 && !regionSeeds.ContainsKey(pos))
                        regionSeeds.Add(pos, index);
                }
            }
            Invalidate();
        }
        /// <summary>
        /// Writes the according color of each seed into an array
        /// </summary>
        public int[][] GetSeedMap()
        {
            var res = new int[img_gray_small.Width][];
            for (int i = 0; i < img_gray_small.Width; i++)
                res[i] = new int[img_gray_small.Height];

            foreach (var kvp in regionSeeds)
                res[kvp.Key.X][kvp.Key.Y] = colors[kvp.Value].ToArgb();
            return res;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
            if (img_display != null)
                e.Graphics.DrawImage(img_display, 0, 0);
            //Draw grid
            if (sampleStep.X > 1)
            {
                using (Pen p = new Pen(Color.Red, 0.5f))
                {
                    int width = img_display.Width;
                    int height = img_display.Height;
                    for (int i = 0; i < width; i += sampleStep.X)
                        e.Graphics.DrawLine(p, new PointF(i - 0.5f * sampleStep.X, 0), new PointF(i - 0.5f * sampleStep.X, height));
                }
            }
            if (sampleStep.Y > 1)
            {
                using (Pen p = new Pen(Color.Red, 0.5f))
                {
                    int width = img_display.Width;
                    int height = img_display.Height;
                    for (int i = 0; i < height; i += sampleStep.Y)
                        e.Graphics.DrawLine(p, new PointF(0, i - 0.5f * sampleStep.Y), new PointF(width, i - 0.5f * sampleStep.Y));
                }
            }

            //Draw seeds as overlay
            foreach (var kvp in regionSeeds)
            {
                var color = Color.FromArgb(OverlayAlpha, colors[kvp.Value]);
                using (Brush b = new SolidBrush(color))
                {
                    e.Graphics.FillRectangle(b, new RectangleF(-sampleStep.X * 0.5f + kvp.Key.X * sampleStep.X,
                        -sampleStep.Y * 0.5f + kvp.Key.Y * sampleStep.Y, sampleStep.X, sampleStep.Y));
                }
            }
            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (activeRegion != -1)
            {
                recordMouseTrace = true;
                int x = (e.X - AutoScrollPosition.X) / sampleStep.X;
                int y = (e.Y - AutoScrollPosition.Y) / sampleStep.Y;
                if (x >= 0 && x < img_gray_small.Width && y >= 0 && y < img_gray_small.Height)
                {
                    var key = new P2(x, y);

                    if (DeleteTrace)
                        regionSeeds.Remove(key);
                    else
                    {
                        if (regionSeeds.ContainsKey(key))
                            regionSeeds[key] = activeRegion;
                        else
                            regionSeeds.Add(key, activeRegion);
                    }
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (recordMouseTrace)
            {
                int x = (e.X - AutoScrollPosition.X) / sampleStep.X;
                int y = (e.Y - AutoScrollPosition.Y) / sampleStep.Y;
                if (x >= 0 && x < img_gray_small.Width && y >= 0 && y < img_gray_small.Height)
                {
                    var key = new P2(x, y);
                    if (DeleteTrace)
                        regionSeeds.Remove(key);
                    else
                    {
                        if (regionSeeds.ContainsKey(key))
                            regionSeeds[key] = activeRegion;
                        else
                            regionSeeds.Add(key, activeRegion);
                    }
                }
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            recordMouseTrace = false;
            Invalidate();
        }
        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
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
            colors.Add(color);
            activeRegion = colors.Count - 1;
        }

        /// <summary>
        /// Samples the original image with the given sample steps and draws the resulting over scaled image to the panel 
        /// </summary>
        public void ChangeResolution(int sampleStepX, int sampleStepY)
        {
            if (colors.Count > 0)
            {
                var dict = new Dictionary<P2, int>();
                foreach (var kvp in regionSeeds)
                {
                    int superX = kvp.Key.X * sampleStep.X;
                    int superY = kvp.Key.Y * sampleStep.Y;
                    var nKey = new P2(superX / sampleStepX, superY / sampleStepY);
                    if (!dict.ContainsKey(nKey))
                        dict.Add(nKey, kvp.Value);
                }
                regionSeeds = dict;
            }

            sampleStep = new P2(sampleStepX, sampleStepY);
            img_gray_small = new Frame(img_gray.Width / sampleStepX, img_gray.Height / sampleStepY);
            Imaging.Scale(img_gray, img_gray_small, InterpolationType.NearestNeighbor);
            var tmp = new Frame(img_org.Width, img_org.Height);
            Imaging.Scale(img_gray_small, tmp, InterpolationType.NearestNeighbor);
            if (img_display != null)
                img_display.Dispose();

            img_display = tmp.GetBitmap();

            Invalidate();
        }
        
        public new void Dispose()
        {
            base.Dispose();
            img_org.Dispose();
            img_org = null;
            img_display.Dispose();
            img_display = null;
            img_gray = null;
            img_gray_small = null;
        }

        private void reset()
        {
            sampleStep = new P2(1, 1);
            regionSeeds = new Dictionary<P2, int>(); 
            colors = new List<Color>();
            activeRegion = -1;
            recordMouseTrace = false;
        }
    }
}
