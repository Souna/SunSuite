using SunFileManager.Config;
using SunFileManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : Form
    {
        private frmFileManager mainForm = null;

        public frmSettings(Form parentForm)
        {
            mainForm = parentForm as frmFileManager;
            InitializeComponent();
        }

        private void ApplySettingsChanges()
        {
            // Applies the changes once OK or Apply is clicked.
            mainForm.ApplySettings();
        }

        private void GatherSettings()
        {
            Program.UserSettings.UseDark = chkDarkMode.Checked;
            Program.UserSettings.ParseImagesAutomatically = chkAutoParseImages.Checked;
            Program.UserSettings.DisplayNodeWarnings = chkNodeWarnings.Checked;
            Program.UserSettings.DisplayLinesBetweenNodes = chkNodeLines.Checked;
            Program.UserSettings.DisplayLinesOnRootNodes = chkNodeRootLines.Checked;
            Program.UserSettings.HighlightWholeWidth = chkHighlightWholeWidth.Checked;

            Program.UserSettings.Save();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            chkDarkMode.Checked = Program.UserSettings.UseDark;
            chkAutoParseImages.Checked = Program.UserSettings.ParseImagesAutomatically;
            chkNodeWarnings.Checked = Program.UserSettings.DisplayNodeWarnings;
            chkNodeLines.Checked = Program.UserSettings.DisplayLinesBetweenNodes;
            chkNodeRootLines.Checked = Program.UserSettings.DisplayLinesOnRootNodes;
            chkHighlightWholeWidth.Checked = Program.UserSettings.HighlightWholeWidth;

            btnApply.Enabled = false;
        }

        private void CheckBox_Checked(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        #region Buttons

        private void btnOk_Click(object sender, EventArgs e)
        {
            GatherSettings();
            ApplySettingsChanges();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            GatherSettings();
            ApplySettingsChanges();
            btnApply.Enabled = false;
        }

        #endregion Buttons
    }
}