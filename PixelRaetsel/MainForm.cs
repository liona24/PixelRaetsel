using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SmartTools;

namespace PixelRaetsel
{
    public partial class MainForm : Form
    {
        List<string> additionalComponents = new List<string>();
        List<Color> colors = new List<Color>();

        //This is propably not a clean solution, but w.e
        public int? Background = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void enableProcessingControls()
        {
            btn_selectColor.Enabled = true;
            btn_sampleStepDown.Enabled = true;
            btn_sampleStepUp.Enabled = true;
            radio_delete.Enabled = true;
            btn_save.Enabled = true;
            btn_fill.Enabled = true;
            check_backgroundAutomatic.Enabled = true;
            label1.Enabled = true;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            var result = openImgDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var fileStream = openImgDialog.OpenFile();
                var bmp = new Bitmap(fileStream, false);
                panel1.SetImage(bmp);
                enableProcessingControls();
                colors = new List<Color>();
                Background = null;
                check_backgroundAutomatic.Checked = true;
                foreach (string s in additionalComponents)
                    Controls.RemoveByKey(s);
                Invalidate();
            }
        }

        private void btn_selectColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                panel1.AddRegion(colorDialog1.Color);
                colors.Add(colorDialog1.Color);
                int index = panel1.ActiveRegion;
                string name = string.Format("Farbe {0}", index);

                var colorPanel = new Panel();
                colorPanel.Name = name + " panel";
                colorPanel.BackColor = colorDialog1.Color;
                colorPanel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                colorPanel.Size = new Size(12, 20);
                colorPanel.Location = new Point(Width - 63, index * 23 + 109);
                colorPanel.MouseClick += (sender2, e2) =>
                {
                    var result2 = colorDialog1.ShowDialog();
                    if (result2 == DialogResult.OK)
                    {
                        panel1.SetColor(index, colorDialog1.Color);
                        colors[index] = colorDialog1.Color;
                        colorPanel.BackColor = colorDialog1.Color;
                        check_backgroundAutomatic.Checked = true;
                        colorPanel.Invalidate();
                    }

                };

                var radioBtn = new RadioButton();
                radioBtn.Text = "Farbe " + index.ToString();
                radioBtn.Name = name;
                radioBtn.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                radioBtn.Location = new Point(Width - 154, index * 23 + 109);
                radioBtn.CheckedChanged += (sender2, e2) =>
                {
                    if (radioBtn.Checked)
                        panel1.ActiveRegion = index;
                };


                Controls.Add(colorPanel);
                Controls.Add(radioBtn);
                additionalComponents.Add(radioBtn.Name);
                additionalComponents.Add(colorPanel.Name);

                radioBtn.Checked = true;
                Invalidate();
            }
        }

        private void btn_sampleStepDown_Click(object sender, EventArgs e)
        {
            panel1.ChangeResolution(panel1.SampleStepX + 1, panel1.SampleStepY + 1);
        }

        private void btn_sampleStepUp_Click(object sender, EventArgs e)
        {
            if (panel1.SampleStepX > 1)
                panel1.ChangeResolution(panel1.SampleStepX - 1, panel1.SampleStepY - 1);
        }

        private void radio_delete_CheckedChanged(object sender, EventArgs e)
        {
            panel1.DeleteTrace = radio_delete.Checked;
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            panel1.Segmentate();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var dlgResult = saveFileDialog1.ShowDialog();
            if (dlgResult != DialogResult.OK)
                return;

            UseWaitCursor = true;
            var map = panel1.GetSeedMap();
            int width = map.Length;
            int height = map[0].Length;
            var colorCount = new Dictionary<int, int>();
            var solution = new Frame3(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int argb = map[i][j];
                    var c = Color.FromArgb(argb);
                    solution.SetPixelAt(i, j, 0, c.B);
                    solution.SetPixelAt(i, j, 1, c.G);
                    solution.SetPixelAt(i, j, 2, c.R);

                    if (colorCount.ContainsKey(argb))
                        colorCount[argb]++;
                    else
                        colorCount.Add(argb, 1);
                }
            }
            var bmpSol = solution.GetBitmap();
            string noEnding = System.IO.Path.ChangeExtension(saveFileDialog1.FileName, null);
            bmpSol.Save(noEnding + "_loesung.jpg");
            bmpSol.Dispose();
            solution = null;

            int background = 0;
            if (this.Background != null)
                background = this.Background.Value;
            else
                background = colorCount.OrderByDescending((i) => i.Value).First().Key;

            var rowHeader = new List<long>[height];
            Action<List<long>, int, int> add = (lst, clr, cnt) =>
            {
                long toAdd = (long)clr << 32;
                toAdd += cnt;
                lst.Add(toAdd);
            };
            int count = 0;
            int color = 0;
            for (int i = 0; i < height; i++)
            {
                count = 0;
                bool counting = false;
                rowHeader[i] = new List<long>();
                for (int j = 0; j < width; j++)
                {
                    if (map[j][i] == background && counting)
                    {
                        counting = false;
                        add(rowHeader[i], color, count);
                        count = 0;
                    }
                    else if (map[j][i] != color && counting)
                    {
                        add(rowHeader[i], color, count);
                        count = 1;
                        color = map[j][i];
                    }
                    else if (map[j][i] != background)
                    {
                        counting = true;
                        color = map[j][i];
                        count++;
                    }
                }
                if (counting)
                    add(rowHeader[i], color, count);
            }

            var colHeader = new List<long>[width];
            for (int i = 0; i < width; i++)
            {
                count = 0;
                bool counting = false;
                colHeader[i] = new List<long>();

                for (int j = 0; j < height; j++)
                {
                    if (map[i][j] == background && counting)
                    {
                        counting = false;
                        add(colHeader[i], color, count);
                        count = 0;
                    }
                    else if (map[i][j] != color && counting)
                    {
                        add(colHeader[i], color, count);
                        count = 1;
                        color = map[i][j];
                    }
                    else if (map[i][j] != background)
                    {
                        counting = true;
                        color = map[i][j];
                        count++;
                    }
                }
                if (counting)
                    add(colHeader[i], color, count);
            }

            int maxItemsRow = rowHeader.Max((i) => i.Count);
            int maxItemsCol = colHeader.Max((i) => i.Count);

            float rowHeadSize = maxItemsRow * 0.7f + 0.5f;
            float colHeadSize = maxItemsCol * 0.7f + 0.5f;
            int ppcm = 80;
            var bmp = new Bitmap((int)(rowHeadSize + 2.0f + width * 0.5f) * ppcm, (int)(colHeadSize + 2.0f + height * 0.5f) * ppcm);
            var font = new Font("Verdana", ppcm / 10.0f * 3.0f, FontStyle.Regular, GraphicsUnit.Pixel);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                using (Pen pThin = new Pen(Color.Black, 0.02f * ppcm))
                using (Pen pBig = new Pen(Color.Black, 0.07f * ppcm))
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < colHeader[i].Count; j++)
                        {
                            long val = colHeader[i][colHeader[i].Count - 1 - j];
                            string text = (val & int.MaxValue).ToString();
                            var clr = Color.FromArgb((int)(val >> 32));
                            float offset = text.Length * 0.135f;
                            using (Brush b = new SolidBrush(clr))
                                g.DrawString(text, font, b, new PointF(ppcm * (rowHeadSize + 1.3f + i * 0.5f - offset),
                                    ppcm * (0.45f + colHeadSize - 0.7f * j)));
                        }
                        var pen = pThin;
                        if (i % 5 == 0)
                            pen = pBig;
                        g.DrawLine(pen, new PointF(ppcm * (rowHeadSize + 1.0f + i * 0.5f), ppcm),
                            new PointF(ppcm * (rowHeadSize + 1.0f + i * 0.5f), (1 + colHeadSize + height * 0.5f) * ppcm));

                    }
                    g.DrawLine(pBig, new PointF(ppcm * (rowHeadSize + width * 0.5f + 1), ppcm),
                        new PointF(ppcm * (rowHeadSize + width * 0.5f + 1), (1 + colHeadSize + height * 0.5f) * ppcm));

                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < rowHeader[i].Count; j++)
                        {
                            long val = rowHeader[i][rowHeader[i].Count - 1 - j];
                            string text = (val & int.MaxValue).ToString();
                            var clr = Color.FromArgb((int)(val >> 32));
                            float offset = text.Length * 0.175f;
                            using (Brush b = new SolidBrush(clr))
                                g.DrawString(text, font, b, new PointF(ppcm * (0.65f + rowHeadSize - offset - j * 0.7f),
                                    ppcm * (colHeadSize + 1.075f + i * 0.5f)));
                        }
                        var pen = pThin;
                        if (i % 5 == 0)
                            pen = pBig;

                        g.DrawLine(pen, new PointF(ppcm, ppcm * (colHeadSize + 1.0f + i * 0.5f)),
                            new PointF(ppcm * (rowHeadSize + 1.0f + width * 0.5f), ppcm * (colHeadSize + 1.0f + i * 0.5f)));
                    }
                    g.DrawLine(pBig, new PointF(ppcm, ppcm * (colHeadSize + 1.0f + height * 0.5f)),
                        new PointF(ppcm * (rowHeadSize + 1.0f + width * 0.5f), ppcm * (colHeadSize + 1.0f + height * 0.5f)));
                }
            }

            bmp.Save(saveFileDialog1.FileName);
            bmp.Dispose();
            UseWaitCursor = false;
        }

        private void check_backgroundAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            if (check_backgroundAutomatic.Checked)
                Background = null;
            else if (colors.Count > 0)
            {
                var backSelect = new BackgroundSelector(this, colors);
                backSelect.ShowDialog();
            }
        }
    }
}
