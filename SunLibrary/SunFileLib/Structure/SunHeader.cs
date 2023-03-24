namespace SunLibrary.SunFileLib.Structure
{
    /// <summary>
    /// Class which creates the file header for our SunFile.
    /// <br>Contains metadata such as identifying string ("magic number"), copyright, file size, and the offset to the start of file data.</br>
    /// </summary>
    public class SunHeader
    {
        private string identifier;
        private string ascii;
        private string copyright;
        private ulong filesize;
        private uint filestart;

        /// <summary>
        /// Identifying string at beginning of file.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Sun Ascii art in the header.
        /// </summary>
        public string Ascii
        {
            get { return ascii; }
            set { ascii = value; }
        }

        /// <summary>
        /// Copyright information.
        /// </summary>
        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }

        /// <summary>
        /// Size of the file after the header.
        /// </summary>
        public ulong FileSize
        {
            get { return filesize; }
            set { filesize = value; }
        }

        /// <summary>
        /// The offset within the file where data begins.
        /// Also known as the header size.
        /// </summary>
        public uint FileStart
        {
            get { return filestart; }
            set { filestart = value; }
        }

        public void RecalculateFileStart()
        {//                                FileSize      + FileStart
            filestart = (uint)(identifier.Length + sizeof(ulong) + sizeof(uint) + copyright.Length + 1);
        }

        /// <summary>
        /// Initializes the default SunHeader.
        /// </summary>
        public static SunHeader Default()
        {
            SunHeader header = new SunHeader();
            header.identifier = "SunSouna";
            header.ascii = "               .        " +
                "     \\ | /      " +
                "   '-.;;;.-'    " +
                "  -==;;;;;==-   " +
                "   .-';;;'-.    " +
                "     / | \\      " +
                "       .";
            header.copyright = "Copyright © 2020 Souna";
            header.filesize = 0;
            header.filestart = 155; // Header size. (sig + ascii + filesize + filestart + copyright) = 155
            return header;
        }
    }
}