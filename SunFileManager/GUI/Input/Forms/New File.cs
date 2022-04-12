using SunFileManager.SunFileLib;
using System;
using System.IO;
using System.Windows.Forms;

namespace SunFileManager.GUI
{
    /// <summary>
    /// The form for creating new SunFiles.
    /// </summary>
    public partial class frmNewFile : Form
    {
        private frmFileManager mainform = null;
        private string name = null;

        public frmNewFile()
        {
            InitializeComponent();
        }

        public frmNewFile(Form parentform)
        {
            mainform = parentform as frmFileManager;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Create SunFile with specified name, save it to disk, then open it for use in the treeview.
        /// </summary>
        public void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text != string.Empty)
            {
                // If there are any spaces in the beginning or end of the file name.
                if (txtFileName.Text.StartsWith(" ") || txtFileName.Text.EndsWith(" "))
                {
                    txtFileName.Focus();
                    txtFileName.SelectAll();
                    return;
                }
                name = txtFileName.Text;
            }
            else return;

            // If we're creating a duplicate SunFile in the default path
            if (File.Exists(frmFileManager.DefaultPath + "\\" + name + ".sun"))
            {
                MessageBox.Show(name + ".sun" + " is an already existing file.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //// Flip through every node to see if we're trying to create a SunFile node which already exists.
            //// idk a better way to do this
            //for (int i = 0; i < mainform.sunTreeView.Nodes.Count; i++)
            //{
            //    if (mainform.sunTreeView.Nodes[i].Name == name + ".sun")
            //    {
            //        MessageBox.Show(name + " is an already existing Node. Try creating one with a different name.");
            //        return;
            //    }
            //}

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 || name == string.Empty)
            {
                MessageBox.Show("Invalid file name.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!name.EndsWith(".sun"))
                name += ".sun";

            // Create the file.
            var fullpath = Path.Combine(frmFileManager.DefaultPath, name);
            SunFile file = new SunFile(name, fullpath);

            mainform.sunTreeView.Nodes.Add(new SunNode(file));
            mainform.manager.sunFiles.Add(file);
            Close();
        }

        private void txtFileName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void txtFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCreate_Click(null, null);
            }
        }
    }
}