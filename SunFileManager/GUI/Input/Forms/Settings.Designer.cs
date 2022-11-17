namespace SunFileManager.GUI.Input.Forms
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.chkAutoParseImages = new MaterialSkin.Controls.MaterialCheckbox();
            this.chkNodeWarnings = new MaterialSkin.Controls.MaterialCheckbox();
            this.chkDarkMode = new MaterialSkin.Controls.MaterialCheckbox();
            this.chkFileBoxes = new MaterialSkin.Controls.MaterialCheckbox();
            this.btnOk = new MaterialSkin.Controls.MaterialButton();
            this.btnCancel = new MaterialSkin.Controls.MaterialButton();
            this.btnApply = new MaterialSkin.Controls.MaterialButton();
            this.radNodeLines = new MaterialSkin.Controls.MaterialRadioButton();
            this.radHighlightLine = new MaterialSkin.Controls.MaterialRadioButton();
            this.SuspendLayout();
            // 
            // chkAutoParseImages
            // 
            this.chkAutoParseImages.AutoSize = true;
            this.chkAutoParseImages.Depth = 0;
            this.chkAutoParseImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoParseImages.Location = new System.Drawing.Point(19, 73);
            this.chkAutoParseImages.Margin = new System.Windows.Forms.Padding(0);
            this.chkAutoParseImages.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkAutoParseImages.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkAutoParseImages.Name = "chkAutoParseImages";
            this.chkAutoParseImages.ReadOnly = false;
            this.chkAutoParseImages.Ripple = true;
            this.chkAutoParseImages.Size = new System.Drawing.Size(221, 37);
            this.chkAutoParseImages.TabIndex = 1;
            this.chkAutoParseImages.Text = "Auto Parse Images (*.img)";
            this.chkAutoParseImages.UseVisualStyleBackColor = true;
            this.chkAutoParseImages.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkNodeWarnings
            // 
            this.chkNodeWarnings.AutoSize = true;
            this.chkNodeWarnings.Depth = 0;
            this.chkNodeWarnings.Enabled = false;
            this.chkNodeWarnings.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNodeWarnings.Location = new System.Drawing.Point(19, 110);
            this.chkNodeWarnings.Margin = new System.Windows.Forms.Padding(0);
            this.chkNodeWarnings.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkNodeWarnings.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkNodeWarnings.Name = "chkNodeWarnings";
            this.chkNodeWarnings.ReadOnly = false;
            this.chkNodeWarnings.Ripple = true;
            this.chkNodeWarnings.Size = new System.Drawing.Size(355, 37);
            this.chkNodeWarnings.TabIndex = 2;
            this.chkNodeWarnings.Text = "Warnings when removing or modifying nodes";
            this.chkNodeWarnings.UseVisualStyleBackColor = true;
            this.chkNodeWarnings.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkDarkMode
            // 
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Depth = 0;
            this.chkDarkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDarkMode.Location = new System.Drawing.Point(19, 36);
            this.chkDarkMode.Margin = new System.Windows.Forms.Padding(0);
            this.chkDarkMode.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkDarkMode.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.ReadOnly = false;
            this.chkDarkMode.Ripple = true;
            this.chkDarkMode.Size = new System.Drawing.Size(121, 37);
            this.chkDarkMode.TabIndex = 0;
            this.chkDarkMode.Text = "Dark Theme";
            this.chkDarkMode.UseVisualStyleBackColor = true;
            this.chkDarkMode.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkFileBoxes
            // 
            this.chkFileBoxes.AutoSize = true;
            this.chkFileBoxes.Depth = 0;
            this.chkFileBoxes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileBoxes.Location = new System.Drawing.Point(19, 147);
            this.chkFileBoxes.Margin = new System.Windows.Forms.Padding(0);
            this.chkFileBoxes.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkFileBoxes.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkFileBoxes.Name = "chkFileBoxes";
            this.chkFileBoxes.ReadOnly = false;
            this.chkFileBoxes.Ripple = true;
            this.chkFileBoxes.Size = new System.Drawing.Size(177, 37);
            this.chkFileBoxes.TabIndex = 4;
            this.chkFileBoxes.Text = "Show boxes on files";
            this.chkFileBoxes.UseVisualStyleBackColor = true;
            this.chkFileBoxes.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // btnOk
            // 
            this.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnOk.Depth = 0;
            this.btnOk.HighEmphasis = true;
            this.btnOk.Icon = null;
            this.btnOk.Location = new System.Drawing.Point(65, 264);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOk.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOk.Name = "btnOk";
            this.btnOk.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnOk.Size = new System.Drawing.Size(64, 36);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnOk.UseAccentColor = false;
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnCancel.Depth = 0;
            this.btnCancel.HighEmphasis = true;
            this.btnCancel.Icon = null;
            this.btnCancel.Location = new System.Drawing.Point(156, 264);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnCancel.Size = new System.Drawing.Size(77, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnCancel.UseAccentColor = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnApply.Depth = 0;
            this.btnApply.HighEmphasis = true;
            this.btnApply.Icon = null;
            this.btnApply.Location = new System.Drawing.Point(260, 264);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnApply.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnApply.Name = "btnApply";
            this.btnApply.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnApply.Size = new System.Drawing.Size(67, 36);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnApply.UseAccentColor = false;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // radNodeLines
            // 
            this.radNodeLines.AutoSize = true;
            this.radNodeLines.Depth = 0;
            this.radNodeLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radNodeLines.Location = new System.Drawing.Point(19, 184);
            this.radNodeLines.Margin = new System.Windows.Forms.Padding(0);
            this.radNodeLines.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radNodeLines.MouseState = MaterialSkin.MouseState.HOVER;
            this.radNodeLines.Name = "radNodeLines";
            this.radNodeLines.Ripple = true;
            this.radNodeLines.Size = new System.Drawing.Size(235, 37);
            this.radNodeLines.TabIndex = 9;
            this.radNodeLines.TabStop = true;
            this.radNodeLines.Text = "Display lines between nodes";
            this.radNodeLines.UseVisualStyleBackColor = true;
            this.radNodeLines.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // radHighlightLine
            // 
            this.radHighlightLine.AutoSize = true;
            this.radHighlightLine.Depth = 0;
            this.radHighlightLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radHighlightLine.Location = new System.Drawing.Point(19, 221);
            this.radHighlightLine.Margin = new System.Windows.Forms.Padding(0);
            this.radHighlightLine.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radHighlightLine.MouseState = MaterialSkin.MouseState.HOVER;
            this.radHighlightLine.Name = "radHighlightLine";
            this.radHighlightLine.Ripple = true;
            this.radHighlightLine.Size = new System.Drawing.Size(263, 37);
            this.radHighlightLine.TabIndex = 10;
            this.radHighlightLine.TabStop = true;
            this.radHighlightLine.Text = "Highlight whole line on selection";
            this.radHighlightLine.UseVisualStyleBackColor = true;
            this.radHighlightLine.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 376);
            this.Controls.Add(this.radHighlightLine);
            this.Controls.Add(this.radNodeLines);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkFileBoxes);
            this.Controls.Add(this.chkDarkMode);
            this.Controls.Add(this.chkNodeWarnings);
            this.Controls.Add(this.chkAutoParseImages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialCheckbox chkAutoParseImages;
        private MaterialSkin.Controls.MaterialCheckbox chkNodeWarnings;
        private MaterialSkin.Controls.MaterialCheckbox chkDarkMode;
        private MaterialSkin.Controls.MaterialCheckbox chkFileBoxes;
        private MaterialSkin.Controls.MaterialButton btnOk;
        private MaterialSkin.Controls.MaterialButton btnCancel;
        private MaterialSkin.Controls.MaterialButton btnApply;
        private MaterialSkin.Controls.MaterialRadioButton radNodeLines;
        private MaterialSkin.Controls.MaterialRadioButton radHighlightLine;
    }
}