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
            this.imageListDataType = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.stsFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
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
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ImageIndex = 29;
            this.treeView1.ImageList = this.imageListDataType;
            this.treeView1.Location = new System.Drawing.Point(0, 24);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(292, 227);
            this.treeView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.hexToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // hexToolStripMenuItem
            // 
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.hexToolStripMenuItem.Text = "&Hex";
            this.hexToolStripMenuItem.Click += new System.EventHandler(this.hexToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All Files|*.*";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsFile,
            this.stsSize});
            this.statusStrip2.Location = new System.Drawing.Point(0, 251);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(292, 22);
            this.statusStrip2.TabIndex = 3;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // stsFile
            // 
            this.stsFile.Name = "stsFile";
            this.stsFile.Size = new System.Drawing.Size(213, 17);
            this.stsFile.Spring = true;
            this.stsFile.Text = "File:";
            this.stsFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stsSize
            // 
            this.stsSize.Name = "stsSize";
            this.stsSize.Size = new System.Drawing.Size(33, 17);
            this.stsSize.Text = "Size: ";
            this.stsSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ASN.1 Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListDataType;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem hexToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel stsFile;
        private System.Windows.Forms.ToolStripStatusLabel stsSize;
    }
}

