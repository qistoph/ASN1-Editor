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

        private ASN1Tag rootNode;
        private HexViewer hexViewer;

        public Form1()
        {
            InitializeComponent();

            hexViewer = new HexViewer();
            hexViewer.Attach(this);
        }

        public void ShowAsn1(ASN1Tag root)
        {
            treeView1.Nodes.Clear();
            ASN1TreeNode treeNode = new ASN1TreeNode(root);
            treeView1.Nodes.Add(treeNode);

            AddSubNodes(treeNode, root);
            treeView1.ExpandAll();
        }

        private void AddSubNodes(TreeNode rootTree, ASN1Tag rootAsn1)
        {
            foreach (ASN1Tag subAsn1 in rootAsn1)
            {
                ASN1TreeNode subTree = new ASN1TreeNode(subAsn1);
                rootTree.Nodes.Add(subTree);

                AddSubNodes(subTree, subAsn1);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                OpenFile(openFileDialog.FileName);
            }
        }

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

                    stsFile.Text = string.Concat("File: ", file);
                    stsSize.Text = string.Concat("Size: ", fs.Length.ToString(), " bytes");
                }
                ShowAsn1(rootNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error while opening file: " + ex.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            ResumeLayout();
        }

        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] data;

            if (rootNode != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ASN1.Encode(ms, rootNode);
                    data = ms.ToArray();
                }
            }
            else
            {
                data = new byte[0];
            }

            ASN1TreeNode selectedNode = treeView1.SelectedNode as ASN1TreeNode;
            if (selectedNode == null)
            {
                hexViewer.View(data);
            }
            else
            {
                // TODO: length is incorrect since it's only about data length, not including the header
                System.Diagnostics.Debug.Assert(selectedNode.Asn1Node.StartByte <= int.MaxValue);
                System.Diagnostics.Debug.Assert(selectedNode.Asn1Node.Length <= int.MaxValue);
                hexViewer.View(data, (int)selectedNode.Asn1Node.StartByte, (int)selectedNode.Asn1Node.Length);
            }

            if (!hexViewer.Visible)
            {
                hexViewer.Show(this);
                this.MaximizedBounds = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width - hexViewer.Width, Screen.PrimaryScreen.WorkingArea.Height);
            }
        }
    }
}
