﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

//How to install xna because this shit is impossible to find now
//https://flatredball.com/visual-studio-2019-xna-setup

using HaCreator.MapEditor;
using HaCreator.Wz;
using MapleLib.WzLib;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace HaCreator
{
    internal static class Program
    {
        public static FileManager SfManager;
        public static WzInformationManager InfoManager;
        public static WzSettingsManager SettingsManager;
        public const string Version = "2.2.0";
        public static bool AbortThreads = false;
        public static bool Restarting;

        public static string GetLocalSettingsFolder()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string our_folder = Path.Combine(appdata, "HaCreator");
            if (!Directory.Exists(our_folder))
                Directory.CreateDirectory(our_folder);
            return our_folder;
        }

        public static string GetLocalSettingsPath()
        {
            return Path.Combine(GetLocalSettingsFolder(), "Settings.sun");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Startup
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif

            Properties.Resources.Culture = CultureInfo.CurrentCulture;
            InfoManager = new WzInformationManager();
            SettingsManager = new WzSettingsManager(GetLocalSettingsPath(), typeof(UserSettings), typeof(ApplicationSettings), typeof(Microsoft.Xna.Framework.Color));
            SettingsManager.Load();

            MultiBoard.RecalculateSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Program run here
            GUI.Initialization initForm = new GUI.Initialization();
            Application.Run(initForm);

            // Shutdown
            if (initForm.editor != null)
                initForm.editor.hcsm.backupMan.ClearBackups();
            SettingsManager.Save();
            if (Restarting)
            {
                Application.Restart();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            new ThreadExceptionDialog((Exception)e.ExceptionObject).ShowDialog();
            Environment.Exit(-1);
        }
    }
}