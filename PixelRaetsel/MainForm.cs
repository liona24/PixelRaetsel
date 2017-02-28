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
                int index = panel1.ActiveRegion;
                string name = string.Format("Farbe {0}", index);

                var colorPanel = new Panel();
                colorPanel.Name = name + " panel";
                colorPanel.BackColor = colorDialog1.Color;
                colorPanel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                colorPanel.Size = new Size(12, 20);
                colorPanel.Location = new Point(Width - 63, index * 23 + 86);
                colorPanel.MouseClick += (sender2, e2) =>
                {
                    var result2 = colorDialog1.ShowDialog();
                    if (result2 == DialogResult.OK)
                    {
                        panel1.SetColor(index, colorDialog1.Color);
                        colorPanel.BackColor = colorDialog1.Color;
                        colorPanel.Invalidate();
                    }

                };

                var radioBtn = new RadioButton();
                radioBtn.Text = "Farbe " + index.ToString();
                radioBtn.Name = name;
                radioBtn.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                radioBtn.Location = new Point(Width - 154, index * 23 + 86);
                radioBtn.CheckedChanged += (sender2, e2) =>
                {
                    if (radioBtn.Checked)
                        panel1.ActiveRegion = index;
                };
                Controls.Add(colorPanel);
                Controls.Add(radioBtn);
                additionalComponents.Add(radioBtn.Name);
                additionalComponents.Add(colorPanel.Name);
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
    }
}
