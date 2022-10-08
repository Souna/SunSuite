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
            this.chkAutoParseImages = new System.Windows.Forms.CheckBox();
            this.chkNodeWarnings = new System.Windows.Forms.CheckBox();
            this.chkDarkMode = new System.Windows.Forms.CheckBox();
            this.chkFileBoxes = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.radNodeLines = new System.Windows.Forms.RadioButton();
            this.radHighlightLine = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // chkAutoParseImages
            // 
            this.chkAutoParseImages.AutoSize = true;
            this.chkAutoParseImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoParseImages.Location = new System.Drawing.Point(12, 35);
            this.chkAutoParseImages.Name = "chkAutoParseImages";
            this.chkAutoParseImages.Size = new System.Drawing.Size(200, 22);
            this.chkAutoParseImages.TabIndex = 1;
            this.chkAutoParseImages.Text = "Auto Parse Images (*.img)";
            this.chkAutoParseImages.UseVisualStyleBackColor = true;
            this.chkAutoParseImages.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkNodeWarnings
            // 
            this.chkNodeWarnings.AutoSize = true;
            this.chkNodeWarnings.Enabled = false;
            this.chkNodeWarnings.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNodeWarnings.Location = new System.Drawing.Point(12, 58);
            this.chkNodeWarnings.Name = "chkNodeWarnings";
            this.chkNodeWarnings.Size = new System.Drawing.Size(324, 22);
            this.chkNodeWarnings.TabIndex = 2;
            this.chkNodeWarnings.Text = "Warnings when removing or modifying nodes";
            this.chkNodeWarnings.UseVisualStyleBackColor = true;
            this.chkNodeWarnings.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkDarkMode
            // 
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Enabled = false;
            this.chkDarkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDarkMode.Location = new System.Drawing.Point(12, 12);
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.Size = new System.Drawing.Size(109, 22);
            this.chkDarkMode.TabIndex = 0;
            this.chkDarkMode.Text = "Dark Theme";
            this.chkDarkMode.UseVisualStyleBackColor = true;
            this.chkDarkMode.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkFileBoxes
            // 
            this.chkFileBoxes.AutoSize = true;
            this.chkFileBoxes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileBoxes.Location = new System.Drawing.Point(12, 81);
            this.chkFileBoxes.Name = "chkFileBoxes";
            this.chkFileBoxes.Size = new System.Drawing.Size(160, 22);
            this.chkFileBoxes.TabIndex = 4;
            this.chkFileBoxes.Text = "Show boxes on files";
            this.chkFileBoxes.UseVisualStyleBackColor = true;
            this.chkFileBoxes.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 196);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(99, 35);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(117, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(222, 196);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(99, 35);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // radNodeLines
            // 
            this.radNodeLines.AutoSize = true;
            this.radNodeLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radNodeLines.Location = new System.Drawing.Point(12, 104);
            this.radNodeLines.Name = "radNodeLines";
            this.radNodeLines.Size = new System.Drawing.Size(212, 22);
            this.radNodeLines.TabIndex = 9;
            this.radNodeLines.TabStop = true;
            this.radNodeLines.Text = "Display lines between nodes";
            this.radNodeLines.UseVisualStyleBackColor = true;
            this.radNodeLines.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // radHighlightLine
            // 
            this.radHighlightLine.AutoSize = true;
            this.radHighlightLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radHighlightLine.Location = new System.Drawing.Point(12, 126);
            this.radHighlightLine.Name = "radHighlightLine";
            this.radHighlightLine.Size = new System.Drawing.Size(235, 22);
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
            this.ClientSize = new System.Drawing.Size(334, 243);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SunFile Manager Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAutoParseImages;
        private System.Windows.Forms.CheckBox chkNodeWarnings;
        private System.Windows.Forms.CheckBox chkDarkMode;
        private System.Windows.Forms.CheckBox chkFileBoxes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.RadioButton radNodeLines;
        private System.Windows.Forms.RadioButton radHighlightLine;
    }
}