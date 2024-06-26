﻿namespace SunFileManager
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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenMapFile = new System.Windows.Forms.Button();
            this.lblSelectedNodeType = new System.Windows.Forms.Label();
            this.cmsNodes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtPropertyValue = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblPropertyName = new System.Windows.Forms.Label();
            this.txtPropertyName = new System.Windows.Forms.TextBox();
            this.chkAnimateGif = new System.Windows.Forms.CheckBox();
            this.btnApplyPropertyChanges = new System.Windows.Forms.Button();
            this.lblVectorXVal = new System.Windows.Forms.Label();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.soundPlayer = new SunFileManager.GUI.SoundPlayer();
            this.panning_PictureBox = new SunFileManager.GUI.Container.Panning_PictureBox();
            this.btnOpenStringFile = new System.Windows.Forms.Button();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // sunTreeView
            // 
            this.sunTreeView.AllowDrop = true;
            this.sunTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sunTreeView.HotTracking = true;
            this.sunTreeView.ImageIndex = 0;
            this.sunTreeView.ImageList = this.imageList1;
            this.sunTreeView.Location = new System.Drawing.Point(13, 91);
            this.sunTreeView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.sunTreeView.MinimumSize = new System.Drawing.Size(255, 175);
            this.sunTreeView.Name = "sunTreeView";
            this.sunTreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sunTreeView.SelectedImageIndex = 0;
            this.sunTreeView.ShowRootLines = false;
            this.sunTreeView.Size = new System.Drawing.Size(326, 577);
            this.sunTreeView.TabIndex = 0;
            this.sunTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.sunTreeView_ItemDrag);
            this.sunTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sunTreeView_AfterSelect);
            this.sunTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.sunTreeView_DragDrop);
            this.sunTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.sunTreeView_DragEnter);
            this.sunTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.sunTreeView_DragOver);
            this.sunTreeView.DragLeave += new System.EventHandler(this.sunTreeView_DragLeave);
            this.sunTreeView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.sunTreeView_GiveFeedback);
            this.sunTreeView.DoubleClick += new System.EventHandler(this.sunTreeView_DoubleClick);
            this.sunTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sunTreeView_KeyDown);
            this.sunTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sunTreeView_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "sun.ico");
            this.imageList1.Images.SetKeyName(1, "3d.png");
            this.imageList1.Images.SetKeyName(2, "folder.png");
            this.imageList1.Images.SetKeyName(3, "Input.png");
            this.imageList1.Images.SetKeyName(4, "Decimal.png");
            this.imageList1.Images.SetKeyName(5, "String.png");
            this.imageList1.Images.SetKeyName(6, "Image.png");
            this.imageList1.Images.SetKeyName(7, "Vector.png");
            this.imageList1.Images.SetKeyName(8, "Sound.png");
            this.imageList1.Images.SetKeyName(9, "folderblue.png");
            this.imageList1.Images.SetKeyName(10, "Property.png");
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(3, 64);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(6, 1, 0, 1);
            this.menuStripMain.Size = new System.Drawing.Size(1100, 24);
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
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.New;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.newToolStripMenuItem.Text = "&New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.Open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.Save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sunDirectoryToolStripMenuItem});
            this.addToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.Add;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // sunDirectoryToolStripMenuItem
            // 
            this.sunDirectoryToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.Directory;
            this.sunDirectoryToolStripMenuItem.Name = "sunDirectoryToolStripMenuItem";
            this.sunDirectoryToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sunDirectoryToolStripMenuItem.Text = "SunDirectory";
            this.sunDirectoryToolStripMenuItem.Click += new System.EventHandler(this.sunDirectoryToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 22);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::SunFileManager.Properties.Resources.Property;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // btnOpenMapFile
            // 
            this.btnOpenMapFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenMapFile.Location = new System.Drawing.Point(11, 692);
            this.btnOpenMapFile.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOpenMapFile.Name = "btnOpenMapFile";
            this.btnOpenMapFile.Size = new System.Drawing.Size(110, 21);
            this.btnOpenMapFile.TabIndex = 3;
            this.btnOpenMapFile.Text = "Open Map";
            this.btnOpenMapFile.UseVisualStyleBackColor = true;
            this.btnOpenMapFile.Click += new System.EventHandler(this.btnCreateTestFile_Click);
            // 
            // lblSelectedNodeType
            // 
            this.lblSelectedNodeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedNodeType.AutoSize = true;
            this.lblSelectedNodeType.Location = new System.Drawing.Point(11, 676);
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
            this.txtPropertyValue.Location = new System.Drawing.Point(418, 137);
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
            this.lblValue.Location = new System.Drawing.Point(344, 143);
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
            this.lblPropertyName.Location = new System.Drawing.Point(344, 108);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(68, 20);
            this.lblPropertyName.TabIndex = 7;
            this.lblPropertyName.Text = "Property";
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtPropertyName.Location = new System.Drawing.Point(418, 102);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.ReadOnly = true;
            this.txtPropertyName.Size = new System.Drawing.Size(205, 29);
            this.txtPropertyName.TabIndex = 8;
            this.txtPropertyName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkAnimateGif
            // 
            this.chkAnimateGif.AutoSize = true;
            this.chkAnimateGif.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAnimateGif.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chkAnimateGif.Location = new System.Drawing.Point(991, 91);
            this.chkAnimateGif.Name = "chkAnimateGif";
            this.chkAnimateGif.Size = new System.Drawing.Size(109, 29);
            this.chkAnimateGif.TabIndex = 10;
            this.chkAnimateGif.Text = "Animate";
            this.chkAnimateGif.UseVisualStyleBackColor = true;
            this.chkAnimateGif.CheckedChanged += new System.EventHandler(this.chkAnimateGif_CheckedChanged);
            // 
            // btnApplyPropertyChanges
            // 
            this.btnApplyPropertyChanges.Location = new System.Drawing.Point(629, 123);
            this.btnApplyPropertyChanges.Name = "btnApplyPropertyChanges";
            this.btnApplyPropertyChanges.Size = new System.Drawing.Size(87, 23);
            this.btnApplyPropertyChanges.TabIndex = 12;
            this.btnApplyPropertyChanges.Text = "Apply Changes";
            this.btnApplyPropertyChanges.UseVisualStyleBackColor = true;
            this.btnApplyPropertyChanges.Click += new System.EventHandler(this.btnApplyPropertyChanges_Click);
            // 
            // lblVectorXVal
            // 
            this.lblVectorXVal.AutoSize = true;
            this.lblVectorXVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVectorXVal.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblVectorXVal.Location = new System.Drawing.Point(414, 178);
            this.lblVectorXVal.Name = "lblVectorXVal";
            this.lblVectorXVal.Size = new System.Drawing.Size(66, 20);
            this.lblVectorXVal.TabIndex = 13;
            this.lblVectorXVal.Text = "X-Value";
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.Transparent;
            this.elementHost1.Location = new System.Drawing.Point(344, 501);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(465, 167);
            this.elementHost1.TabIndex = 11;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.soundPlayer;
            // 
            // panning_PictureBox
            // 
            this.panning_PictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panning_PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.panning_PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panning_PictureBox.Canvas = null;
            this.panning_PictureBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panning_PictureBox.Location = new System.Drawing.Point(344, 102);
            this.panning_PictureBox.Name = "panning_PictureBox";
            this.panning_PictureBox.Size = new System.Drawing.Size(762, 605);
            this.panning_PictureBox.TabIndex = 9;
            // 
            // btnOpenStringFile
            // 
            this.btnOpenStringFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenStringFile.Location = new System.Drawing.Point(125, 692);
            this.btnOpenStringFile.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOpenStringFile.Name = "btnOpenStringFile";
            this.btnOpenStringFile.Size = new System.Drawing.Size(110, 21);
            this.btnOpenStringFile.TabIndex = 14;
            this.btnOpenStringFile.Text = "Open String";
            this.btnOpenStringFile.UseVisualStyleBackColor = true;
            this.btnOpenStringFile.Click += new System.EventHandler(this.btnOpenStringFile_Click);
            // 
            // frmFileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1106, 718);
            this.Controls.Add(this.btnOpenStringFile);
            this.Controls.Add(this.lblVectorXVal);
            this.Controls.Add(this.btnApplyPropertyChanges);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblPropertyName);
            this.Controls.Add(this.chkAnimateGif);
            this.Controls.Add(this.panning_PictureBox);
            this.Controls.Add(this.txtPropertyName);
            this.Controls.Add(this.txtPropertyValue);
            this.Controls.Add(this.lblSelectedNodeType);
            this.Controls.Add(this.btnOpenMapFile);
            this.Controls.Add(this.sunTreeView);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
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
        private System.Windows.Forms.Button btnOpenMapFile;
        private System.Windows.Forms.Label lblSelectedNodeType;
        private System.Windows.Forms.ContextMenuStrip cmsNodes;
        private System.Windows.Forms.TextBox txtPropertyValue;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblPropertyName;
        private System.Windows.Forms.TextBox txtPropertyName;
        private GUI.Container.Panning_PictureBox panning_PictureBox;
        private System.Windows.Forms.CheckBox chkAnimateGif;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.Button btnApplyPropertyChanges;
        private System.Windows.Forms.Label lblVectorXVal;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private GUI.SoundPlayer soundPlayer;
        private System.Windows.Forms.ImageList imageListDrag;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.Button btnOpenStringFile;
    }
}

