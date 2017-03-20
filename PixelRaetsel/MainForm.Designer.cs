namespace PixelRaetsel
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openImgDialog = new System.Windows.Forms.OpenFileDialog();
            this.btn_new = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btn_selectColor = new System.Windows.Forms.Button();
            this.btn_sampleStepDown = new System.Windows.Forms.Button();
            this.btn_sampleStepUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radio_delete = new System.Windows.Forms.RadioButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_fill = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.check_backgroundAutomatic = new System.Windows.Forms.CheckBox();
            this.btn_zoom = new System.Windows.Forms.Button();
            this.textBox_zoom = new System.Windows.Forms.TextBox();
            this.panel1 = new PixelRaetsel.PictureBox2();
            this.SuspendLayout();
            // 
            // openImgDialog
            // 
            this.openImgDialog.Filter = "JPEG Dateien|*.jpg";
            // 
            // btn_new
            // 
            this.btn_new.Location = new System.Drawing.Point(12, 12);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(75, 23);
            this.btn_new.TabIndex = 1;
            this.btn_new.Text = "Neu";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_selectColor
            // 
            this.btn_selectColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_selectColor.Enabled = false;
            this.btn_selectColor.Location = new System.Drawing.Point(818, 57);
            this.btn_selectColor.Name = "btn_selectColor";
            this.btn_selectColor.Size = new System.Drawing.Size(75, 23);
            this.btn_selectColor.TabIndex = 2;
            this.btn_selectColor.Text = "Neue Farbe";
            this.btn_selectColor.UseVisualStyleBackColor = true;
            this.btn_selectColor.Click += new System.EventHandler(this.btn_selectColor_Click);
            // 
            // btn_sampleStepDown
            // 
            this.btn_sampleStepDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_sampleStepDown.Enabled = false;
            this.btn_sampleStepDown.Location = new System.Drawing.Point(888, 12);
            this.btn_sampleStepDown.Name = "btn_sampleStepDown";
            this.btn_sampleStepDown.Size = new System.Drawing.Size(26, 23);
            this.btn_sampleStepDown.TabIndex = 4;
            this.btn_sampleStepDown.Text = "-";
            this.btn_sampleStepDown.UseVisualStyleBackColor = true;
            this.btn_sampleStepDown.Click += new System.EventHandler(this.btn_sampleStepDown_Click);
            // 
            // btn_sampleStepUp
            // 
            this.btn_sampleStepUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_sampleStepUp.Enabled = false;
            this.btn_sampleStepUp.Location = new System.Drawing.Point(920, 12);
            this.btn_sampleStepUp.Name = "btn_sampleStepUp";
            this.btn_sampleStepUp.Size = new System.Drawing.Size(24, 23);
            this.btn_sampleStepUp.TabIndex = 5;
            this.btn_sampleStepUp.Text = "+";
            this.btn_sampleStepUp.UseVisualStyleBackColor = true;
            this.btn_sampleStepUp.Click += new System.EventHandler(this.btn_sampleStepUp_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(825, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Auflösung:";
            // 
            // radio_delete
            // 
            this.radio_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radio_delete.AutoSize = true;
            this.radio_delete.Enabled = false;
            this.radio_delete.Location = new System.Drawing.Point(818, 86);
            this.radio_delete.Name = "radio_delete";
            this.radio_delete.Size = new System.Drawing.Size(66, 17);
            this.radio_delete.TabIndex = 7;
            this.radio_delete.TabStop = true;
            this.radio_delete.Text = "Löschen";
            this.radio_delete.UseVisualStyleBackColor = true;
            this.radio_delete.CheckedChanged += new System.EventHandler(this.radio_delete_CheckedChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "JPEG files|*.jpg";
            // 
            // btn_fill
            // 
            this.btn_fill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_fill.Enabled = false;
            this.btn_fill.Location = new System.Drawing.Point(869, 634);
            this.btn_fill.Name = "btn_fill";
            this.btn_fill.Size = new System.Drawing.Size(75, 23);
            this.btn_fill.TabIndex = 8;
            this.btn_fill.Text = "Füllen";
            this.btn_fill.UseVisualStyleBackColor = true;
            this.btn_fill.Click += new System.EventHandler(this.btn_fill_Click);
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(560, 12);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 9;
            this.btn_save.Text = "Speichern";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // check_backgroundAutomatic
            // 
            this.check_backgroundAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check_backgroundAutomatic.AutoSize = true;
            this.check_backgroundAutomatic.Checked = true;
            this.check_backgroundAutomatic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_backgroundAutomatic.Enabled = false;
            this.check_backgroundAutomatic.Location = new System.Drawing.Point(641, 16);
            this.check_backgroundAutomatic.Name = "check_backgroundAutomatic";
            this.check_backgroundAutomatic.Size = new System.Drawing.Size(141, 17);
            this.check_backgroundAutomatic.TabIndex = 10;
            this.check_backgroundAutomatic.Text = "Hintergrund automatisch";
            this.check_backgroundAutomatic.UseVisualStyleBackColor = true;
            this.check_backgroundAutomatic.CheckedChanged += new System.EventHandler(this.check_backgroundAutomatic_CheckedChanged);
            // 
            // btn_zoom
            // 
            this.btn_zoom.Enabled = false;
            this.btn_zoom.Location = new System.Drawing.Point(265, 12);
            this.btn_zoom.Name = "btn_zoom";
            this.btn_zoom.Size = new System.Drawing.Size(75, 23);
            this.btn_zoom.TabIndex = 11;
            this.btn_zoom.Text = "Zoom";
            this.btn_zoom.UseVisualStyleBackColor = true;
            this.btn_zoom.Click += new System.EventHandler(this.btn_zoom_Click);
            // 
            // textBox_zoom
            // 
            this.textBox_zoom.Enabled = false;
            this.textBox_zoom.Location = new System.Drawing.Point(212, 14);
            this.textBox_zoom.Name = "textBox_zoom";
            this.textBox_zoom.Size = new System.Drawing.Size(47, 20);
            this.textBox_zoom.TabIndex = 12;
            this.textBox_zoom.Text = "1.0";
            this.textBox_zoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.ActiveRegion = -1;
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.DeleteTrace = false;
            this.panel1.Location = new System.Drawing.Point(12, 57);
            this.panel1.Name = "panel1";
            this.panel1.OverlayAlpha = 128;
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 669);
            this.Controls.Add(this.textBox_zoom);
            this.Controls.Add(this.btn_zoom);
            this.Controls.Add(this.check_backgroundAutomatic);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_fill);
            this.Controls.Add(this.radio_delete);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_sampleStepUp);
            this.Controls.Add(this.btn_sampleStepDown);
            this.Controls.Add(this.btn_selectColor);
            this.Controls.Add(this.btn_new);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "PixelRätsel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox2 panel1;
        private System.Windows.Forms.OpenFileDialog openImgDialog;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btn_selectColor;
        private System.Windows.Forms.Button btn_sampleStepDown;
        private System.Windows.Forms.Button btn_sampleStepUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radio_delete;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_fill;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.CheckBox check_backgroundAutomatic;
        private System.Windows.Forms.Button btn_zoom;
        private System.Windows.Forms.TextBox textBox_zoom;
    }
}

