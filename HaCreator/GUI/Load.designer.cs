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
            this.searchBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mapBrowser = new HaCreator.CustomControls.MapBrowser();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadButton.Location = new System.Drawing.Point(171, 492);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(200, 32);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = "Start";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(86, 12);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(179, 20);
            this.searchBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Map Search:";
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
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapBrowser);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.loadButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Map";
            this.Load += new System.EventHandler(this.Load_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Load_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox searchBox;
        private CustomControls.MapBrowser mapBrowser;
        private System.Windows.Forms.Label label1;
    }
}