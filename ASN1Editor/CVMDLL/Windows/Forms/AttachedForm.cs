using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVMDLL.Windows.Forms
{
    //TODO: actually use AttachedStyle and add 'LimitMaximizeBounds'
    public class AttachedForm : Form
    {
        public Form AttachedToForm { get; private set; }
        public AnchorStyles AttachedStyle { get; private set; }

        public void Attach(Form form, AnchorStyles anchorStyle)
        {
            if (form != null)
            {
                form.Move -= new EventHandler(AttachedFormMove);
                form.Resize -= new EventHandler(AttachedFormResize);
            }

            AttachedToForm = form;
            AttachedStyle = anchorStyle;

            if (form != null)
            {
                form.Move += new EventHandler(AttachedFormMove);
                form.Resize += new EventHandler(AttachedFormResize);

                SyncPosition();
                SyncSize();
            }
        }

        protected void SyncPosition()
        {
            this.Left = AttachedToForm.Right;
            this.Top = AttachedToForm.Top;
        }

        protected void SyncSize()
        {
            this.Height = AttachedToForm.Height;
        }

        protected void AttachedFormMove(object sender, EventArgs e)
        {
            SyncPosition();
        }

        protected void AttachedFormResize(object sender, EventArgs e)
        {
            SyncPosition();
            SyncSize();
        }
    }
}
