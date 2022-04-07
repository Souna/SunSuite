
namespace SunFileManager.GUI.Input.Forms
{
    partial class frmVectorInputBox
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtXValueInput = new SunFileManager.GUI.Input.IntInput();
            this.txtNameInput = new System.Windows.Forms.TextBox();
            this.txtYValueInput = new SunFileManager.GUI.Input.IntInput();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(114, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 60);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name:";
            // 
            // txtXValueInput
            // 
            this.txtXValueInput.Location = new System.Drawing.Point(56, 34);
            this.txtXValueInput.MaxLength = 50;
            this.txtXValueInput.Name = "txtXValueInput";
            this.txtXValueInput.Size = new System.Drawing.Size(51, 20);
            this.txtXValueInput.TabIndex = 1;
            this.txtXValueInput.Text = "0";
            this.txtXValueInput.Value = 0;
            this.txtXValueInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXValueInput_KeyDown);
            this.txtXValueInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVectorInputBox_KeyUp);
            // 
            // txtNameInput
            // 
            this.txtNameInput.Location = new System.Drawing.Point(56, 8);
            this.txtNameInput.MaxLength = 50;
            this.txtNameInput.Name = "txtNameInput";
            this.txtNameInput.Size = new System.Drawing.Size(156, 20);
            this.txtNameInput.TabIndex = 0;
            this.txtNameInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyDown);
            this.txtNameInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVectorInputBox_KeyUp);
            // 
            // txtYValueInput
            // 
            this.txtYValueInput.Location = new System.Drawing.Point(161, 34);
            this.txtYValueInput.MaxLength = 50;
            this.txtYValueInput.Name = "txtYValueInput";
            this.txtYValueInput.Size = new System.Drawing.Size(51, 20);
            this.txtYValueInput.TabIndex = 2;
            this.txtYValueInput.Text = "0";
            this.txtYValueInput.Value = 0;
            this.txtYValueInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtYValueInput_KeyDown);
            this.txtYValueInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVectorInputBox_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "X-Value:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Y-Value:";
            // 
            // frmVectorInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 91);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtYValueInput);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtXValueInput);
            this.Controls.Add(this.txtNameInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmVectorInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVectorInputBox_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private IntInput txtXValueInput;
        private System.Windows.Forms.TextBox txtNameInput;
        private IntInput txtYValueInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}