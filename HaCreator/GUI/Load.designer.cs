namespace HaCreator.GUI
{
    partial class Load
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Load));
            this.loadButton = new System.Windows.Forms.Button();
            this.WZSelect = new System.Windows.Forms.RadioButton();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.mapBrowser = new HaCreator.CustomControls.MapBrowser();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadButton.Enabled = false;
            this.loadButton.Location = new System.Drawing.Point(171, 492);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(200, 32);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = "Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // WZSelect
            // 
            this.WZSelect.AutoSize = true;
            this.WZSelect.Checked = true;
            this.WZSelect.Location = new System.Drawing.Point(12, 13);
            this.WZSelect.Name = "WZSelect";
            this.WZSelect.Size = new System.Drawing.Size(43, 17);
            this.WZSelect.TabIndex = 6;
            this.WZSelect.TabStop = true;
            this.WZSelect.Text = "WZ";
            this.WZSelect.UseVisualStyleBackColor = true;
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(65, 12);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(200, 20);
            this.searchBox.TabIndex = 7;
            // 
            // mapBrowser
            // 
            this.mapBrowser.Location = new System.Drawing.Point(12, 36);
            this.mapBrowser.Name = "mapBrowser";
            this.mapBrowser.Size = new System.Drawing.Size(533, 440);
            this.mapBrowser.TabIndex = 8;
            // 
            // Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 532);
            this.Controls.Add(this.mapBrowser);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.WZSelect);
            this.Controls.Add(this.loadButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load";
            this.Load += new System.EventHandler(this.Load_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Load_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.RadioButton WZSelect;
        private System.Windows.Forms.TextBox searchBox;
        private CustomControls.MapBrowser mapBrowser;
    }
}