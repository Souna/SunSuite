/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using SunLibrary.SunFileLib.Structure;

namespace HaCreator.Wz
{
    public class WzMainDirectory
    {
        private SunFile file;
        private SunDirectory directory;

        public WzMainDirectory(SunFile file)
        {
            this.file = file;
            this.directory = file.SunDirectory;
        }

        public WzMainDirectory(SunFile file, SunDirectory directory)
        {
            this.file = file;
            this.directory = directory;
        }

        public SunFile File
        { get { return file; } }
        public SunDirectory MainDir
        { get { return directory; } }
    }
}