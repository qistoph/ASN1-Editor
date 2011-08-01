using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace ASN1Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Assembly assem = Assembly.GetExecutingAssembly();
            string basePath = Path.GetDirectoryName(assem.Location);
            ASN1TagDataReader.OidDb = OidDb.FromFile(Path.Combine(basePath, "OID.txt"));

            Form1 form = new Form1();
            if (args.Length > 0)
            {
                form.OpenFile(args[0]);
            }
            Application.Run(form);
        }
    }
}
