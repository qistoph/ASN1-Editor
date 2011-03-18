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
            OidDb oidDb = OidDb.FromFile("OID.txt");
            //Console.WriteLine(oidDb.ToString());

            ASN1TagDataReader.OidDb = oidDb;
            ASN1Tag rootNode;

            //using (FileStream fs = new FileStream(@"c:\cygwin\tmp\pkcs7_test\out.p7m", FileMode.Open, FileAccess.Read))
            using (FileStream fs = new FileStream(@"c:\cygwin\tmp\BZKtesthulpmddl_V4.der", FileMode.Open, FileAccess.Read))
            {
                rootNode = ASN1.Decode(fs);
                //Console.WriteLine(rootNode.ToShortText());
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            form.ShowAsn1(rootNode);
            Application.Run(form);
        }
    }
}
