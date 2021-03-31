namespace SunFileManager.GUI.Input
{
    partial class frmImageInputBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageInputBox));
            this.txtNameInput = new System.Windows.Forms.TextBox();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblImageDimensions = new System.Windows.Forms.Label();
            this.lbltxtSize = new System.Windows.Forms.Label();
            this.lbltxtType = new System.Windows.Forms.Label();
            this.lblImageType = new System.Windows.Forms.Label();
            this.lbltxtDimensions = new System.Windows.Forms.Label();
            this.lblImageSize = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panning_PictureBox = new SunFileManager.GUI.Container.Panning_PictureBox();
            this.SuspendLayout();
            // 
            // txtNameInput
            // 
            this.txtNameInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNameInput.Location = new System.Drawing.Point(79, 32);
            this.txtNameInput.MaxLength = 50;
            this.txtNameInput.MinimumSize = new System.Drawing.Size(181, 20);
            this.txtNameInput.Name = "txtNameInput";
            this.txtNameInput.Size = new System.Drawing.Size(181, 20);
            this.txtNameInput.TabIndex = 1;
            this.txtNameInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameInput_KeyPress);
            this.txtNameInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyUp);
            this.txtNameInput.MouseHover += new System.EventHandler(this.txtNameInput_MouseHover);
            // 
            // txtImagePath
            // 
            this.txtImagePath.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtImagePath.Location = new System.Drawing.Point(79, 6);
            this.txtImagePath.MinimumSize = new System.Drawing.Size(181, 20);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.ReadOnly = true;
            this.txtImagePath.Size = new System.Drawing.Size(181, 20);
            this.txtImagePath.TabIndex = 0;
            this.txtImagePath.Click += new System.EventHandler(this.txtImagePath_Click);
            this.txtImagePath.TextChanged += new System.EventHandler(this.txtImagePath_TextChanged);
            this.txtImagePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImagePath_KeyPress);
            this.txtImagePath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtImagePath_KeyUp);
            this.txtImagePath.MouseHover += new System.EventHandler(this.txtImagePath_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Image Path:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(29, 66);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(105, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(140, 66);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblImageDimensions
            // 
            this.lblImageDimensions.AutoSize = true;
            this.lblImageDimensions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblImageDimensions.ForeColor = System.Drawing.Color.Green;
            this.lblImageDimensions.Location = new System.Drawing.Point(115, 101);
            this.lblImageDimensions.Name = "lblImageDimensions";
            this.lblImageDimensions.Size = new System.Drawing.Size(0, 20);
            this.lblImageDimensions.TabIndex = 7;
            // 
            // lbltxtSize
            // 
            this.lbltxtSize.AutoSize = true;
            this.lbltxtSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbltxtSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbltxtSize.Location = new System.Drawing.Point(11, 121);
            this.lbltxtSize.Name = "lbltxtSize";
            this.lbltxtSize.Size = new System.Drawing.Size(44, 20);
            this.lbltxtSize.TabIndex = 8;
            this.lbltxtSize.Text = "Size:";
            this.lbltxtSize.Visible = false;
            // 
            // lbltxtType
            // 
            this.lbltxtType.AutoSize = true;
            this.lbltxtType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbltxtType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbltxtType.Location = new System.Drawing.Point(11, 141);
            this.lbltxtType.Name = "lbltxtType";
            this.lbltxtType.Size = new System.Drawing.Size(47, 20);
            this.lbltxtType.TabIndex = 9;
            this.lbltxtType.Text = "Type:";
            this.lbltxtType.Visible = false;
            // 
            // lblImageType
            // 
            this.lblImageType.AutoSize = true;
            this.lblImageType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblImageType.ForeColor = System.Drawing.Color.Green;
            this.lblImageType.Location = new System.Drawing.Point(115, 141);
            this.lblImageType.Name = "lblImageType";
            this.lblImageType.Size = new System.Drawing.Size(0, 20);
            this.lblImageType.TabIndex = 10;
            // 
            // lbltxtDimensions
            // 
            this.lbltxtDimensions.AutoSize = true;
            this.lbltxtDimensions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbltxtDimensions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbltxtDimensions.Location = new System.Drawing.Point(11, 101);
            this.lbltxtDimensions.Name = "lbltxtDimensions";
            this.lbltxtDimensions.Size = new System.Drawing.Size(96, 20);
            this.lbltxtDimensions.TabIndex = 11;
            this.lbltxtDimensions.Text = "Dimensions:";
            this.lbltxtDimensions.Visible = false;
            // 
            // lblImageSize
            // 
            this.lblImageSize.AutoSize = true;
            this.lblImageSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblImageSize.ForeColor = System.Drawing.Color.Green;
            this.lblImageSize.Location = new System.Drawing.Point(115, 121);
            this.lblImageSize.Name = "lblImageSize";
            this.lblImageSize.Size = new System.Drawing.Size(0, 20);
            this.lblImageSize.TabIndex = 12;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // panning_PictureBox
            // 
            this.panning_PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.panning_PictureBox.Image = null;
            this.panning_PictureBox.Location = new System.Drawing.Point(3, 179);
            this.panning_PictureBox.MaximumSize = new System.Drawing.Size(1472, 775);
            this.panning_PictureBox.MinimumSize = new System.Drawing.Size(2, 2);
            this.panning_PictureBox.Name = "panning_PictureBox";
            this.panning_PictureBox.Size = new System.Drawing.Size(2, 2);
            this.panning_PictureBox.TabIndex = 13;
            // 
            // frmImageInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(274, 97);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.txtNameInput);
            this.Controls.Add(this.panning_PictureBox);
            this.Controls.Add(this.lblImageSize);
            this.Controls.Add(this.lbltxtDimensions);
            this.Controls.Add(this.lblImageType);
            this.Controls.Add(this.lbltxtType);
            this.Controls.Add(this.lbltxtSize);
            this.Controls.Add(this.lblImageDimensions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1500, 1000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 136);
            this.Name = "frmImageInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNameInput;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblImageDimensions;
        private System.Windows.Forms.Label lbltxtSize;
        private System.Windows.Forms.Label lbltxtType;
        private System.Windows.Forms.Label lblImageType;
        private System.Windows.Forms.Label lbltxtDimensions;
        private System.Windows.Forms.Label lblImageSize;
        private Container.Panning_PictureBox panning_PictureBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}