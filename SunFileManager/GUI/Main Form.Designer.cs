namespace SunFileManager
{
    partial class frmFileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileManager));
            this.sunTreeView = new System.Windows.Forms.TreeView();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreateMapSun = new System.Windows.Forms.Button();
            this.lblSelectedNodeType = new System.Windows.Forms.Label();
            this.cmsNodes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtPropertyValue = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblPropertyName = new System.Windows.Forms.Label();
            this.txtPropertyName = new System.Windows.Forms.TextBox();
            this.chkAnimateGif = new System.Windows.Forms.CheckBox();
            this.btnQuickImageInt = new System.Windows.Forms.Button();
            this.mainfrm_panning_PictureBox = new SunFileManager.GUI.Container.Panning_PictureBox();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // sunTreeView
            // 
            this.sunTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.sunTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sunTreeView.HotTracking = true;
            this.sunTreeView.Location = new System.Drawing.Point(13, 27);
            this.sunTreeView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sunTreeView.MinimumSize = new System.Drawing.Size(255, 175);
            this.sunTreeView.Name = "sunTreeView";
            this.sunTreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sunTreeView.Size = new System.Drawing.Size(255, 414);
            this.sunTreeView.TabIndex = 0;
            this.sunTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sunTreeView_AfterSelect);
            this.sunTreeView.DoubleClick += new System.EventHandler(this.sunTreeView_DoubleClick);
            this.sunTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sunTreeView_KeyDown);
            this.sunTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sunTreeView_MouseUp);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(6, 1, 0, 1);
            this.menuStripMain.Size = new System.Drawing.Size(604, 24);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sunDirectoryToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // sunDirectoryToolStripMenuItem
            // 
            this.sunDirectoryToolStripMenuItem.Name = "sunDirectoryToolStripMenuItem";
            this.sunDirectoryToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sunDirectoryToolStripMenuItem.Text = "SunDirectory";
            this.sunDirectoryToolStripMenuItem.Click += new System.EventHandler(this.sunDirectoryToolStripMenuItem_Click);
            // 
            // btnCreateMapSun
            // 
            this.btnCreateMapSun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateMapSun.Location = new System.Drawing.Point(11, 465);
            this.btnCreateMapSun.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCreateMapSun.Name = "btnCreateMapSun";
            this.btnCreateMapSun.Size = new System.Drawing.Size(110, 21);
            this.btnCreateMapSun.TabIndex = 3;
            this.btnCreateMapSun.Text = "Quick Map";
            this.btnCreateMapSun.UseVisualStyleBackColor = true;
            this.btnCreateMapSun.Click += new System.EventHandler(this.btnCreateMapSun_Click);
            // 
            // lblSelectedNodeType
            // 
            this.lblSelectedNodeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedNodeType.AutoSize = true;
            this.lblSelectedNodeType.Location = new System.Drawing.Point(11, 449);
            this.lblSelectedNodeType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectedNodeType.Name = "lblSelectedNodeType";
            this.lblSelectedNodeType.Size = new System.Drawing.Size(110, 13);
            this.lblSelectedNodeType.TabIndex = 4;
            this.lblSelectedNodeType.Text = "Selection Type: None";
            // 
            // cmsNodes
            // 
            this.cmsNodes.Name = "cmsNodes";
            this.cmsNodes.Size = new System.Drawing.Size(61, 4);
            this.cmsNodes.Text = "glop";
            // 
            // txtPropertyValue
            // 
            this.txtPropertyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPropertyValue.Location = new System.Drawing.Point(347, 65);
            this.txtPropertyValue.Name = "txtPropertyValue";
            this.txtPropertyValue.Size = new System.Drawing.Size(205, 29);
            this.txtPropertyValue.TabIndex = 5;
            this.txtPropertyValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblValue.Location = new System.Drawing.Point(273, 72);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(50, 20);
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "Value";
            // 
            // lblPropertyName
            // 
            this.lblPropertyName.AutoSize = true;
            this.lblPropertyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblPropertyName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblPropertyName.Location = new System.Drawing.Point(273, 36);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(68, 20);
            this.lblPropertyName.TabIndex = 7;
            this.lblPropertyName.Text = "Property";
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPropertyName.Location = new System.Drawing.Point(347, 29);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.Size = new System.Drawing.Size(205, 29);
            this.txtPropertyName.TabIndex = 8;
            this.txtPropertyName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkAnimateGif
            // 
            this.chkAnimateGif.AutoSize = true;
            this.chkAnimateGif.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAnimateGif.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chkAnimateGif.Location = new System.Drawing.Point(273, 71);
            this.chkAnimateGif.Name = "chkAnimateGif";
            this.chkAnimateGif.Size = new System.Drawing.Size(109, 29);
            this.chkAnimateGif.TabIndex = 10;
            this.chkAnimateGif.Text = "Animate";
            this.chkAnimateGif.UseVisualStyleBackColor = true;
            this.chkAnimateGif.CheckedChanged += new System.EventHandler(this.chkAnimateGif_CheckedChanged);
            // 
            // btnQuickImageInt
            // 
            this.btnQuickImageInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuickImageInt.Location = new System.Drawing.Point(157, 465);
            this.btnQuickImageInt.Name = "btnQuickImageInt";
            this.btnQuickImageInt.Size = new System.Drawing.Size(26, 22);
            this.btnQuickImageInt.TabIndex = 11;
            this.btnQuickImageInt.Text = "button1";
            this.btnQuickImageInt.UseVisualStyleBackColor = true;
            this.btnQuickImageInt.Click += new System.EventHandler(this.btnQuickImageInt_Click);
            // 
            // mainfrm_panning_PictureBox
            // 
            this.mainfrm_panning_PictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainfrm_panning_PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mainfrm_panning_PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mainfrm_panning_PictureBox.Canvas = null;
            this.mainfrm_panning_PictureBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mainfrm_panning_PictureBox.Location = new System.Drawing.Point(277, 101);
            this.mainfrm_panning_PictureBox.Name = "mainfrm_panning_PictureBox";
            this.mainfrm_panning_PictureBox.Size = new System.Drawing.Size(315, 378);
            this.mainfrm_panning_PictureBox.TabIndex = 9;
            // 
            // frmFileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 491);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblPropertyName);
            this.Controls.Add(this.btnQuickImageInt);
            this.Controls.Add(this.chkAnimateGif);
            this.Controls.Add(this.mainfrm_panning_PictureBox);
            this.Controls.Add(this.txtPropertyName);
            this.Controls.Add(this.txtPropertyValue);
            this.Controls.Add(this.lblSelectedNodeType);
            this.Controls.Add(this.btnCreateMapSun);
            this.Controls.Add(this.sunTreeView);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 530);
            this.Name = "frmFileManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SunFile Manager";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.TreeView sunTreeView;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sunDirectoryToolStripMenuItem;
        private System.Windows.Forms.Button btnCreateMapSun;
        private System.Windows.Forms.Label lblSelectedNodeType;
        private System.Windows.Forms.ContextMenuStrip cmsNodes;
        private System.Windows.Forms.TextBox txtPropertyValue;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblPropertyName;
        private System.Windows.Forms.TextBox txtPropertyName;
        private GUI.Container.Panning_PictureBox mainfrm_panning_PictureBox;
        private System.Windows.Forms.CheckBox chkAnimateGif;
        private System.Windows.Forms.Button btnQuickImageInt;
    }
}

