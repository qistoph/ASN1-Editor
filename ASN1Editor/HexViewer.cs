using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVMDLL.Windows.Forms;

namespace ASN1Editor
{
    public partial class HexViewer : AttachedForm
    {
        public HexViewer()
        {
            InitializeComponent();
        }

        public void View(byte[] data, int highlightStart, int highlightLength)
        {
            View(data);
            Highlight(highlightStart, highlightLength);
        }

        public void View(byte[] data)
        {
            txtHex.Clear();

            StringBuilder hexString = new StringBuilder();
            StringBuilder asciiString = new StringBuilder();
            int i;
            for (i = 0; i < data.Length; ++i)
            {
                if (i % 16 == 0)
                {
                //    txtHex.SelectionStart = txtHex.Text.Length;
                //    txtHex.SelectionColor = Color.DarkGray;

                    //    txtHex.AppendText(i.ToString("X8"));
                    //    txtHex.AppendText(" ");
                    hexString.Append(i.ToString("X8"));
                    hexString.Append(" ");

                //    txtHex.SelectionColor = Color.Black;
                }

                hexString.Append(" ");
                hexString.Append(data[i].ToString("X2"));
                //txtHex.AppendText(" ");
                //txtHex.AppendText(data[i].ToString("X2"));

                if (data[i] >= 32 && data[i] <= 126)
                {
                    asciiString.Append((char)data[i]);
                }
                else
                {
                    asciiString.Append('.');
                }

                if (i % 16 == 15)
                {
                    hexString.Append("  ");
                    hexString.Append(asciiString.ToString());
                    hexString.Append(Environment.NewLine);
                    //txtHex.AppendText("  ");
                    //txtHex.AppendText(asciiString.ToString());
                    //txtHex.AppendText(Environment.NewLine);
                    asciiString = new StringBuilder();
                }
            }

            // resume where we left off
            i--;
            if (i % 16 != 15)
            {
                for (; i % 16 != 15; i++)
                {
                    hexString.Append("   ");
                    //txtHex.AppendText("   ");
                }

                if (i % 16 == 15)
                {
                //    txtHex.AppendText("  ");
                //    txtHex.AppendText(asciiString.ToString());
                //    txtHex.AppendText(Environment.NewLine);
                    hexString.Append("  ");
                    hexString.Append(asciiString.ToString());
                    hexString.Append(Environment.NewLine);
                }
            }

            //txtHex.SelectionStart = 0;
            txtHex.Text = hexString.ToString();
        }

        private int HighlightStart = 0;
        private int HighlightLength = 0;
        private int bytesOffset = 8 + 2;
        private int interByteSize = 1;
        private int lineLength = /*bytesOffset*/ (8+2) + 16 * (2 + /*interByteSize*/ 1) + 2 + 16;

        public void Highlight(int highlightStart, int highlightLength)
        {
            SetBytesColor(HighlightStart, HighlightLength, Color.Black);

            HighlightStart = highlightStart;
            HighlightLength = highlightLength;

            SetBytesColor(HighlightStart, HighlightLength, Color.Red);

            txtHex.SelectionStart = (highlightStart / 16) * lineLength + bytesOffset + (highlightStart % 16) * (2 + interByteSize);
            txtHex.SelectionLength = 0;
            //TODO: fix flickering when: txtHex.ScrollToCaret();
        }

        private void SetBytesColor(int start, int length, Color color)
        {
            int end = start + length;

            for (int i = start; i < end; )
            {
                int lineNr = i / 16;
                int lineEnd = end;
                if (lineEnd > (lineNr + 1) * 16)
                {
                    lineEnd = (lineNr + 1) * 16;
                }

                txtHex.SelectionStart = lineNr * lineLength + bytesOffset + (i % 16) * (2 + interByteSize);
                txtHex.SelectionLength = (lineEnd - lineNr * 16 - (i % 16)) * (2 + interByteSize);
                txtHex.SelectionColor = color;

                i = lineEnd;
            }
        }

        private void HexViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }
    }
}
