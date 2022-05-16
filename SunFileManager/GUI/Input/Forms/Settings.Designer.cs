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
            this.chkNodeLines = new System.Windows.Forms.CheckBox();
            this.chkDarkMode = new System.Windows.Forms.CheckBox();
            this.chkNodeRootLines = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkAutoParseImages
            // 
            this.chkAutoParseImages.AutoSize = true;
            this.chkAutoParseImages.Location = new System.Drawing.Point(12, 48);
            this.chkAutoParseImages.Name = "chkAutoParseImages";
            this.chkAutoParseImages.Size = new System.Drawing.Size(181, 17);
            this.chkAutoParseImages.TabIndex = 1;
            this.chkAutoParseImages.Text = "Automatically parse images (.img)";
            this.chkAutoParseImages.UseVisualStyleBackColor = true;
            this.chkAutoParseImages.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkNodeWarnings
            // 
            this.chkNodeWarnings.AutoSize = true;
            this.chkNodeWarnings.Location = new System.Drawing.Point(12, 71);
            this.chkNodeWarnings.Name = "chkNodeWarnings";
            this.chkNodeWarnings.Size = new System.Drawing.Size(271, 17);
            this.chkNodeWarnings.TabIndex = 2;
            this.chkNodeWarnings.Text = "Display warnings when removing or modifying nodes";
            this.chkNodeWarnings.UseVisualStyleBackColor = true;
            this.chkNodeWarnings.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkNodeLines
            // 
            this.chkNodeLines.AutoSize = true;
            this.chkNodeLines.Location = new System.Drawing.Point(12, 94);
            this.chkNodeLines.Name = "chkNodeLines";
            this.chkNodeLines.Size = new System.Drawing.Size(160, 17);
            this.chkNodeLines.TabIndex = 3;
            this.chkNodeLines.Text = "Display lines between nodes";
            this.chkNodeLines.UseVisualStyleBackColor = true;
            this.chkNodeLines.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkDarkMode
            // 
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Enabled = false;
            this.chkDarkMode.Location = new System.Drawing.Point(12, 25);
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.Size = new System.Drawing.Size(107, 17);
            this.chkDarkMode.TabIndex = 0;
            this.chkDarkMode.Text = "Use Dark Theme";
            this.chkDarkMode.UseVisualStyleBackColor = true;
            this.chkDarkMode.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
            // 
            // chkNodeRootLines
            // 
            this.chkNodeRootLines.AutoSize = true;
            this.chkNodeRootLines.Location = new System.Drawing.Point(12, 117);
            this.chkNodeRootLines.Name = "chkNodeRootLines";
            this.chkNodeRootLines.Size = new System.Drawing.Size(152, 17);
            this.chkNodeRootLines.TabIndex = 4;
            this.chkNodeRootLines.Text = "Display lines for root nodes";
            this.chkNodeRootLines.UseVisualStyleBackColor = true;
            this.chkNodeRootLines.CheckedChanged += new System.EventHandler(this.CheckBox_Checked);
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
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 243);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkNodeRootLines);
            this.Controls.Add(this.chkDarkMode);
            this.Controls.Add(this.chkNodeLines);
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
        private System.Windows.Forms.CheckBox chkNodeLines;
        private System.Windows.Forms.CheckBox chkDarkMode;
        private System.Windows.Forms.CheckBox chkNodeRootLines;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}