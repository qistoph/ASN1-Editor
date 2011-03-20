using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASN1Editor
{
    public partial class HexViewer : Form
    {
        public HexViewer()
        {
            InitializeComponent();
        }

        public void View(byte[] data)
        {
            View(data, 0, 0);
        }

        public void View(byte[] data, int highlightStart, int highlightLength)
        {
            txtHex.Clear();

            StringBuilder asciiString = new StringBuilder();
            int i;
            for (i = 0; i < data.Length; ++i)
            {
                if (i % 16 == 0)
                {
                    txtHex.SelectionStart = txtHex.Text.Length;
                    txtHex.SelectionColor = Color.LightGray;

                    txtHex.AppendText(i.ToString("X8"));
                    txtHex.AppendText(" ");
                }

                if (i >= highlightStart && i < (highlightStart + highlightLength))
                {
                    txtHex.SelectionColor = Color.Red;
                }
                else
                {
                    txtHex.SelectionColor = Color.Black;
                }

                txtHex.AppendText(" ");
                txtHex.AppendText(data[i].ToString("X2"));

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
                    txtHex.SelectionColor = Color.Black;
                    txtHex.AppendText("  ");
                    txtHex.AppendText(asciiString.ToString());
                    txtHex.AppendText(Environment.NewLine);
                    asciiString = new StringBuilder();
                }
            }

            // resume where we left off
            i--;
            if (i % 16 != 15)
            {
                for (; i % 16 != 15; i++)
                {
                    txtHex.AppendText("   ");
                }

                if (i % 16 == 15)
                {
                    txtHex.AppendText("  ");
                    txtHex.AppendText(asciiString.ToString());
                    txtHex.AppendText(Environment.NewLine);
                }
            }

            txtHex.SelectionStart = 0;
        }
    }
}
