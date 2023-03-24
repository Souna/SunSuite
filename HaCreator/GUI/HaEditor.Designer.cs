namespace HaCreator.GUI
{
    partial class HaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HaEditor));
            this.vS2015LightTheme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.ribbon = new HaCreator.GUI.HaRibbon();
            this.tabs = new HaCreator.ThirdParty.TabPages.PageCollection();
            this.multiBoard = new HaCreator.MapEditor.MultiBoard();
            this.dockPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.Controls.Add(this.panel1);
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(1113, 728);
            this.dockPanel.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.multiBoard);
            this.panel1.Controls.Add(this.tabs);
            this.panel1.Controls.Add(this.wpfHost);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1113, 728);
            this.panel1.TabIndex = 7;
            this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // wpfHost
            // 
            this.wpfHost.Location = new System.Drawing.Point(3, 160);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(1098, 318);
            this.wpfHost.TabIndex = 0;
            this.wpfHost.Text = "elementHost1";
            this.wpfHost.Child = this.ribbon;
            // 
            // tabs
            // 
            this.tabs.CurrentPage = null;
            this.tabs.Location = new System.Drawing.Point(540, 263);
            this.tabs.Name = "tabs";
            this.tabs.Size = new System.Drawing.Size(208, 215);
            this.tabs.TabColor = System.Drawing.Color.LightSteelBlue;
            this.tabs.TabIndex = 2;
            this.tabs.Text = "pageCollection1";
            this.tabs.TopMargin = 3;
            // 
            // multiBoard
            // 
            this.multiBoard.DeviceReady = false;
            this.multiBoard.Location = new System.Drawing.Point(376, 332);
            this.multiBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.multiBoard.Name = "multiBoard";
            this.multiBoard.SelectedBoard = null;
            this.multiBoard.Size = new System.Drawing.Size(88, 104);
            this.multiBoard.TabIndex = 3;
            // 
            // HaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 728);
            this.Controls.Add(this.dockPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HaEditor";
            this.Text = "HaPigCreator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HaEditor_FormClosed);
            this.Load += new System.EventHandler(this.HaEditor_Load);
            this.dockPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost wpfHost;
        private HaCreator.ThirdParty.TabPages.PageCollection tabs;
        private HaCreator.MapEditor.MultiBoard multiBoard;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vs2015DarkTheme;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vs2015BlueTheme;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.Panel panel1;
        private HaRibbon ribbon;
    }
}