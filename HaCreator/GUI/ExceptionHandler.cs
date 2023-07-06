/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.IO;
using System.Windows.Forms;

namespace HaCreator.GUI
{
    public partial class ExceptionHandler : Form
    {
        public static bool InitializationFinished = false;

        public string GetExceptionInfo(Exception e)
        {
            string result = e.Message + "\r\n\r\n" + e.Source + "\r\n\r\n" + e.StackTrace;
            if (e.InnerException != null)
                result += "\r\n\r\n" + GetExceptionInfo(e.InnerException);
            return result;
        }

        public ExceptionHandler(Exception e)
        {
            InitializeComponent();
            string logPath = Path.Combine(Application.StartupPath, "crashdump.log");
            File.WriteAllText(logPath, GetExceptionInfo(e));
            if (!InitializationFinished)
            {
                crashMessageLabel.Text = "Whoops! It looks like HaCreator crashed. The good news is it crashed before you started working on your map so you didn't lose anything (woohoo!).\r\nAdditionaly, an error log was saved to " + logPath;
                restartButton.Text = "Restart HaCreator";
                restartButton.Click += new EventHandler(Restart);
            }
            else
            {
                crashMessageLabel.Text = "Whoops! It looks like HaCreator crashed. The good news is a backup file containing the map you were working on will be dumped once you click on the button below.";
                restartButton.Text = "Dump map backup and restart HaCreator";
                restartButton.Click += new EventHandler(Backup);
            }
        }

        private void Restart(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void Backup(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}