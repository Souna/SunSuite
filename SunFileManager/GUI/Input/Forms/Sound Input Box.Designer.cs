namespace SunFileManager.GUI.Input
{
    partial class frmSoundInputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSoundInputBox));
            this.txtNameInput = new System.Windows.Forms.TextBox();
            this.txtSoundPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCanvasDimensions = new System.Windows.Forms.Label();
            this.lbltxtSize = new System.Windows.Forms.Label();
            this.lbltxtType = new System.Windows.Forms.Label();
            this.lblCanvasType = new System.Windows.Forms.Label();
            this.lbltxtDimensions = new System.Windows.Forms.Label();
            this.lblCanvasSize = new System.Windows.Forms.Label();
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
            this.txtNameInput.Size = new System.Drawing.Size(214, 20);
            this.txtNameInput.TabIndex = 1;
            this.txtNameInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyDown);
            this.txtNameInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameInput_KeyPress);
            this.txtNameInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyUp);
            this.txtNameInput.MouseHover += new System.EventHandler(this.txtNameInput_MouseHover);
            // 
            // txtSoundPath
            // 
            this.txtSoundPath.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtSoundPath.Location = new System.Drawing.Point(79, 6);
            this.txtSoundPath.MinimumSize = new System.Drawing.Size(181, 20);
            this.txtSoundPath.Name = "txtSoundPath";
            this.txtSoundPath.ReadOnly = true;
            this.txtSoundPath.Size = new System.Drawing.Size(214, 20);
            this.txtSoundPath.TabIndex = 0;
            this.txtSoundPath.Click += new System.EventHandler(this.txtSoundPath_Click);
            this.txtSoundPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSoundPath_KeyUp);
            this.txtSoundPath.MouseHover += new System.EventHandler(this.txtSoundPath_MouseHover);
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
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sound Path:";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(12, 62);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(144, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(162, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCanvasDimensions
            // 
            this.lblCanvasDimensions.AutoSize = true;
            this.lblCanvasDimensions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCanvasDimensions.ForeColor = System.Drawing.Color.Green;
            this.lblCanvasDimensions.Location = new System.Drawing.Point(115, 101);
            this.lblCanvasDimensions.Name = "lblCanvasDimensions";
            this.lblCanvasDimensions.Size = new System.Drawing.Size(0, 20);
            this.lblCanvasDimensions.TabIndex = 7;
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
            // lblCanvasType
            // 
            this.lblCanvasType.AutoSize = true;
            this.lblCanvasType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCanvasType.ForeColor = System.Drawing.Color.Green;
            this.lblCanvasType.Location = new System.Drawing.Point(115, 141);
            this.lblCanvasType.Name = "lblCanvasType";
            this.lblCanvasType.Size = new System.Drawing.Size(0, 20);
            this.lblCanvasType.TabIndex = 10;
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
            // lblCanvasSize
            // 
            this.lblCanvasSize.AutoSize = true;
            this.lblCanvasSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCanvasSize.ForeColor = System.Drawing.Color.Green;
            this.lblCanvasSize.Location = new System.Drawing.Point(115, 121);
            this.lblCanvasSize.Name = "lblCanvasSize";
            this.lblCanvasSize.Size = new System.Drawing.Size(0, 20);
            this.lblCanvasSize.TabIndex = 12;
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
            this.panning_PictureBox.Canvas = null;
            this.panning_PictureBox.Location = new System.Drawing.Point(3, 179);
            this.panning_PictureBox.MaximumSize = new System.Drawing.Size(1472, 775);
            this.panning_PictureBox.MinimumSize = new System.Drawing.Size(2, 2);
            this.panning_PictureBox.Name = "panning_PictureBox";
            this.panning_PictureBox.Size = new System.Drawing.Size(2, 2);
            this.panning_PictureBox.TabIndex = 13;
            // 
            // frmSoundInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(305, 97);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSoundPath);
            this.Controls.Add(this.txtNameInput);
            this.Controls.Add(this.panning_PictureBox);
            this.Controls.Add(this.lblCanvasSize);
            this.Controls.Add(this.lbltxtDimensions);
            this.Controls.Add(this.lblCanvasType);
            this.Controls.Add(this.lbltxtType);
            this.Controls.Add(this.lbltxtSize);
            this.Controls.Add(this.lblCanvasDimensions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1500, 1000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 136);
            this.Name = "frmSoundInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSoundInputBox_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSoundPath_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNameInput;
        private System.Windows.Forms.TextBox txtSoundPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCanvasDimensions;
        private System.Windows.Forms.Label lbltxtSize;
        private System.Windows.Forms.Label lbltxtType;
        private System.Windows.Forms.Label lblCanvasType;
        private System.Windows.Forms.Label lbltxtDimensions;
        private System.Windows.Forms.Label lblCanvasSize;
        private Container.Panning_PictureBox panning_PictureBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}