namespace SunFileManager.GUI.Container
{
    partial class Panning_PictureBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlPan = new System.Windows.Forms.Panel();
            this.picPan = new System.Windows.Forms.PictureBox();
            this.pnlPan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPan)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPan
            // 
            this.pnlPan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPan.BackColor = System.Drawing.Color.Transparent;
            this.pnlPan.Controls.Add(this.picPan);
            this.pnlPan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlPan.Location = new System.Drawing.Point(0, 0);
            this.pnlPan.Name = "pnlPan";
            this.pnlPan.Size = new System.Drawing.Size(344, 233);
            this.pnlPan.TabIndex = 0;
            // 
            // picPan
            // 
            this.picPan.BackColor = System.Drawing.Color.Transparent;
            this.picPan.Location = new System.Drawing.Point(0, 0);
            this.picPan.Name = "picPan";
            this.picPan.Size = new System.Drawing.Size(343, 232);
            this.picPan.TabIndex = 0;
            this.picPan.TabStop = false;
            this.picPan.Paint += new System.Windows.Forms.PaintEventHandler(this.picPan_Paint);
            this.picPan.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picPan_MouseDoubleClick);
            this.picPan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPan_MouseDown);
            this.picPan.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPan_MouseMove);
            // 
            // Panning_PictureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPan);
            this.Name = "Panning_PictureBox";
            this.Size = new System.Drawing.Size(344, 233);
            this.Load += new System.EventHandler(this.Panning_PictureBox_Load);
            this.Resize += new System.EventHandler(this.Panning_PictureBox_Resize);
            this.pnlPan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel pnlPan;
        public System.Windows.Forms.PictureBox picPan;
    }
}
