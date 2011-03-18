namespace ASN1Editor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node6", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node5", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode7});
            this.imageListDataType = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // imageListDataType
            // 
            this.imageListDataType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataType.ImageStream")));
            this.imageListDataType.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDataType.Images.SetKeyName(0, "");
            this.imageListDataType.Images.SetKeyName(1, "");
            this.imageListDataType.Images.SetKeyName(2, "");
            this.imageListDataType.Images.SetKeyName(3, "");
            this.imageListDataType.Images.SetKeyName(4, "");
            this.imageListDataType.Images.SetKeyName(5, "");
            this.imageListDataType.Images.SetKeyName(6, "");
            this.imageListDataType.Images.SetKeyName(7, "");
            this.imageListDataType.Images.SetKeyName(8, "");
            this.imageListDataType.Images.SetKeyName(9, "");
            this.imageListDataType.Images.SetKeyName(10, "");
            this.imageListDataType.Images.SetKeyName(11, "");
            this.imageListDataType.Images.SetKeyName(12, "");
            this.imageListDataType.Images.SetKeyName(13, "");
            this.imageListDataType.Images.SetKeyName(14, "");
            this.imageListDataType.Images.SetKeyName(15, "");
            this.imageListDataType.Images.SetKeyName(16, "");
            this.imageListDataType.Images.SetKeyName(17, "");
            this.imageListDataType.Images.SetKeyName(18, "");
            this.imageListDataType.Images.SetKeyName(19, "");
            this.imageListDataType.Images.SetKeyName(20, "");
            this.imageListDataType.Images.SetKeyName(21, "");
            this.imageListDataType.Images.SetKeyName(22, "");
            this.imageListDataType.Images.SetKeyName(23, "");
            this.imageListDataType.Images.SetKeyName(24, "");
            this.imageListDataType.Images.SetKeyName(25, "");
            this.imageListDataType.Images.SetKeyName(26, "");
            this.imageListDataType.Images.SetKeyName(27, "");
            this.imageListDataType.Images.SetKeyName(28, "");
            this.imageListDataType.Images.SetKeyName(29, "");
            this.imageListDataType.Images.SetKeyName(30, "");
            this.imageListDataType.Images.SetKeyName(31, "");
            this.imageListDataType.Images.SetKeyName(32, "");
            this.imageListDataType.Images.SetKeyName(33, "");
            this.imageListDataType.Images.SetKeyName(34, "");
            this.imageListDataType.Images.SetKeyName(35, "");
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ImageIndex = 29;
            this.treeView1.ImageList = this.imageListDataType;
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Node1";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Node4";
            treeNode5.Name = "Node7";
            treeNode5.Text = "Node7";
            treeNode6.Name = "Node6";
            treeNode6.Text = "Node6";
            treeNode7.Name = "Node5";
            treeNode7.Text = "Node5";
            treeNode8.Name = "Node3";
            treeNode8.Text = "Node3";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode8});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(268, 249);
            this.treeView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListDataType;
        private System.Windows.Forms.TreeView treeView1;
    }
}

