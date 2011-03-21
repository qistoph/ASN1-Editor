using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ASN1Editor
{
    public class ASN1TreeNode : TreeNode
    {
        public ASN1Tag Asn1Node { get; private set; }

        public ASN1TreeNode(ASN1Tag asn1Node)
        {
            this.Asn1Node = asn1Node;

            this.Text = string.Concat("(", asn1Node.StartByte, ", ", (asn1Node.Length == ASN1.IndefiniteLength ? "inf" : asn1Node.Length.ToString()), ") ", asn1Node.ShortDescription);
            if (!asn1Node.Constructed)
            {
                this.Text += string.Concat(": ", asn1Node.DataText);
            }
            if (asn1Node.Class == ASN1.Class.Universal && asn1Node.Identifier <= 31)
            {
                this.ImageIndex = Asn1ImageIndexes[asn1Node.Identifier];
            }
            else
            {
                this.ImageIndex = 32;
            }
            this.SelectedImageIndex = this.ImageIndex;

            this.ContextMenu = CreateContextMenu();
        }

        protected ContextMenu CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem mnuDumpNode = menu.MenuItems.Add("Dump node", DumpNode);

            MenuItem mnuDumpData = menu.MenuItems.Add("Dump data", DumpDataBinary);

            return menu;
        }

        private enum DumpType
        {
            Node,
            DataBinary,
        }

        private void DumpNode(object sender, EventArgs e)
        {
            Dump(DumpType.Node);
        }

        private void DumpDataBinary(object sender, EventArgs e)
        {
            Dump(DumpType.DataBinary);
        }

        private void Dump(DumpType dumpType)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            if (DialogResult.OK == fileDialog.ShowDialog())
            {
                //TODO: mode IO stuff to other class
                using(FileStream fs = new FileStream(fileDialog.FileName, FileMode.Create, FileAccess.Write)) {

                switch (dumpType)
                {
                    case DumpType.Node:
                        this.Asn1Node.Write(fs);
                        break;
                    case DumpType.DataBinary:
                        this.Asn1Node.WriteData(fs);
                        break;
                    default:
                        break;
                }
                }
            }
        }

        private int[] Asn1ImageIndexes = new int[] {
            32, // 0, TODO: look for best icon
            21, // Boolean = 1,
             1, // Integer = 2,
            23, // BitString = 3,
            25, // OctetString = 4,
            18, // Null = 5,
            11, // ObjectIdentifier = 6,
            11, // ObjectDescriptor = 7,
             6, // InstanceOf = 8,
             1, // Real = 9,
             1, // Enumerated = 10,
             1, // EmbeddedPdv = 11, TODO: look for best icon
             7, // Utrf8String = 12,
            35, // RelativeOid = 13,
             0, // 14, TODO: look for best icon
             0, // 15, TODO: look for best icon
            32, // Sequence = 16,
            33, // Set = 17,
             8, // NumericString = 18,
             7, // PrintableString = 19,
             7, // TeletextString = 20,
             7, // VideotexString = 21,
             7, // Ia5String = 22,
            34, // UtcTime = 23,
            34, // GeneralizedTime = 24,
             7, // GraphicString = 25,
             7, // VisibleString = 26,
             7, // GeneralString = 27,
             7, // UniversalString = 28,
             7, // CharacterString = 29,
             7, // BmpString = 30
            24, // Context specific = 31
            32, // Root = 32
        };

    }
}
