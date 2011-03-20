using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ASN1Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        public void ShowAsn1(ASN1Tag root)
        {
            treeView1.Nodes.Clear();
            TreeNode treeNode = CreateTreeNode(root);
            treeView1.Nodes.Add(treeNode);

            AddSubNodes(treeNode, root);
            treeView1.ExpandAll();
        }

        private void AddSubNodes(TreeNode rootTree, ASN1Tag rootAsn1)
        {
            foreach (ASN1Tag subAsn1 in rootAsn1)
            {
                TreeNode subTree = CreateTreeNode(subAsn1);
                rootTree.Nodes.Add(subTree);

                AddSubNodes(subTree, subAsn1);
            }
        }

        private TreeNode CreateTreeNode(ASN1Tag nodeAsn1)
        {
            TreeNode nodeTree = new TreeNode();
            nodeTree.Text = string.Concat("(", nodeAsn1.StartByte, ", ", (nodeAsn1.Length == ASN1.IndefiniteLength ? "inf" : nodeAsn1.Length.ToString()), ") ", nodeAsn1.ShortDescription);
            if (!nodeAsn1.Constructed)
            {
                nodeTree.Text += string.Concat(": ", nodeAsn1.DataText);
            }
            if (nodeAsn1.Class == ASN1.Class.Universal && nodeAsn1.Identifier <= 31)
            {
                nodeTree.ImageIndex = Asn1ImageIndexes[nodeAsn1.Identifier];
            }
            else
            {
                nodeTree.ImageIndex = 32;
            }
            nodeTree.SelectedImageIndex = nodeTree.ImageIndex;

            return nodeTree;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                OpenFile(openFileDialog.FileName);
            }
        }

        ASN1Tag rootNode;
        private void OpenFile(string file)
        {
            SuspendLayout();

            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        byte[] pemData = PemReader.ReadPem(fs);

                        using (MemoryStream ms = new MemoryStream(pemData))
                        {
                            rootNode = ASN1.Decode(ms);
                        }
                    }
                    catch (ArgumentException)
                    {
                        fs.Position = 0;
                        rootNode = ASN1.Decode(fs);
                    }
                }
                ShowAsn1(rootNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error while opening file: " + ex.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            ResumeLayout();
        }

        private void writeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("temp.out", FileMode.Create, FileAccess.Write))
            {
                rootNode.Write(fs);
            }
        }
    }
}
