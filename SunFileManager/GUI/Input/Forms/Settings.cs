using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : MaterialForm
    {
        private frmFileManager mainForm = null;

        public frmSettings(Form parentForm)
        {
            mainForm = parentForm as frmFileManager;
            InitializeComponent();
        }

        private void ApplyChangedSettings()
        {
            // Applies the changes once OK or Apply is clicked.
            mainForm.ApplySettings();
        }

        private void GatherSettings()
        {
            Program.UserSettings.DarkMode = chkDarkMode.Checked;
            Program.UserSettings.AutoParseImages = chkAutoParseImages.Checked;
            Program.UserSettings.NodeWarnings = chkNodeWarnings.Checked;
            Program.UserSettings.FileBoxes = chkFileBoxes.Checked;
            Program.UserSettings.NodeLines = radNodeLines.Checked;
            Program.UserSettings.HighlightLine = radHighlightLine.Checked;

            Program.UserSettings.Save();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            //DetachCheckedEventHandler();
            chkDarkMode.Checked = Program.UserSettings.DarkMode;
            chkAutoParseImages.Checked = Program.UserSettings.AutoParseImages;
            chkNodeWarnings.Checked = Program.UserSettings.NodeWarnings;
            chkFileBoxes.Checked = Program.UserSettings.FileBoxes;
            //AttachCheckedEventHandler();
            radNodeLines.Checked = Program.UserSettings.NodeLines;
            radHighlightLine.Checked = Program.UserSettings.HighlightLine;

            btnApply.Enabled = false;
        }

        private void DetachCheckedEventHandler()
        {
            chkDarkMode.CheckedChanged -= CheckBox_Checked;
            chkAutoParseImages.CheckedChanged -= CheckBox_Checked;
            chkNodeWarnings.CheckedChanged -= CheckBox_Checked;
            chkFileBoxes.CheckedChanged -= CheckBox_Checked;
        }

        private void AttachCheckedEventHandler()
        {
            chkDarkMode.CheckedChanged += CheckBox_Checked;
            chkAutoParseImages.CheckedChanged += CheckBox_Checked;
            chkNodeWarnings.CheckedChanged += CheckBox_Checked;
            chkFileBoxes.CheckedChanged += CheckBox_Checked;
        }

        private void CheckBox_Checked(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        #region Buttons

        private void btnOk_Click(object sender, EventArgs e)
        {
            GatherSettings();
            ApplyChangedSettings();
            Dispose();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            GatherSettings();
            ApplyChangedSettings();
            btnApply.Enabled = false;
        }

        #endregion Buttons
    }
}