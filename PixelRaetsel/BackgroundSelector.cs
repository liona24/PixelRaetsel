using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelRaetsel
{
    public partial class BackgroundSelector : Form
    {
        public BackgroundSelector(MainForm parent, List<Color> colors)
        {
            InitializeComponent();

            for (int i = 0; i < colors.Count; i++)
            {
                var pan = new Panel();
                pan.BackColor = colors[i];
                pan.Size = new Size(30, 20);
                pan.Location = new Point(10 + 35 * i, 25);
                pan.MouseClick += (sender, e) =>
                {
                    parent.Background = pan.BackColor.ToArgb();
                    Close();
                };
                Controls.Add(pan);
            }

            Invalidate();
        }
    }
}
