using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ASN1Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ASN1TagDataReader.OidDb = OidDb.FromFile("OID.txt");

            Form1 form = new Form1();
            Application.Run(form);
        }
    }
}
