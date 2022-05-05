using SunFileManager.GUI.Input.Controls;

namespace SunFileManager.GUI.Input
{
    partial class frmDigitInputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDigitInputBox));
            this.txtNameInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtIntValueInput = new SunFileManager.GUI.Input.Controls.IntInput();
            this.SuspendLayout();
            // 
            // txtNameInput
            // 
            this.txtNameInput.Location = new System.Drawing.Point(56, 8);
            this.txtNameInput.MaxLength = 50;
            this.txtNameInput.Name = "txtNameInput";
            this.txtNameInput.Size = new System.Drawing.Size(156, 20);
            this.txtNameInput.TabIndex = 0;
            this.txtNameInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyDown);
            this.txtNameInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNameInput_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Value:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 60);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(114, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtIntValueInput
            // 
            this.txtIntValueInput.Location = new System.Drawing.Point(56, 34);
            this.txtIntValueInput.MaxLength = 50;
            this.txtIntValueInput.Name = "txtIntValueInput";
            this.txtIntValueInput.Size = new System.Drawing.Size(156, 20);
            this.txtIntValueInput.TabIndex = 1;
            this.txtIntValueInput.Text = "0";
            this.txtIntValueInput.Value = 0;
            this.txtIntValueInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIntValueInput_KeyDown);
            this.txtIntValueInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtIntValueInput_KeyUp);
            // 
            // frmDigitInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 91);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIntValueInput);
            this.Controls.Add(this.txtNameInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDigitInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtIntValueInput_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNameInput;
        private IntInput txtIntValueInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}