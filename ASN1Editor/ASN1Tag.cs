using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVMDLL.Text;
using System.IO;

namespace ASN1Editor
{
    public class ASN1Tag : IEnumerable<ASN1Tag>
    {
        public ASN1.Class Class { get; protected internal set; }
        public bool Constructed { get; protected internal set; }
        public int Identifier { get; protected internal set; }
        public long StartByte { get; protected internal set; }
        public long Length { get; protected internal set; }
        protected internal byte[] Data { get; set; }

        private List<ASN1Tag> SubTags { get; set; }
        private ASN1Tag ParentTag { get; set; }

        public ASN1.TagNumber TagNumber
        {
            get
            {
                return Constructed ? ASN1.TagNumber.NonePrimitive : (ASN1.TagNumber)Identifier;
            }
        }

        public string ShortDescription
        {
            get
            {
                if (Class == ASN1.Class.Universal && Constructed == false && Identifier == 0 && Length == 0)
                    return "EOC";
                else if (Class == ASN1.Class.Universal && Identifier < 31)
                    return ASN1.GetUTNDescription(Identifier);
                else
                    return string.Concat("[", Identifier, "]");
            }
        }

        public string DataText
        {
            get
            {
                return Constructed ? "Constructed" : ASN1TagDataReader.GetDataString(this);
            }
        }

        protected internal ASN1Tag()
        {
            SubTags = new List<ASN1Tag>();
            ParentTag = null;
        }

        protected internal void AddSubTag(ASN1Tag subTag)
        {
            SubTags.Add(subTag);
            subTag.ParentTag = this;
        }

        protected internal string ToShortText()
        {
            ConsoleTable table = new ConsoleTable();
            table.SetTitles("Start", "Length", "C/P", "ID", "Class", "Data");
            ToShortText(table, 0);
            return table.ToString();
        }

        protected internal void ToShortText(ConsoleTable output, int indentLevel)
        {
            output.AddRow(
                StartByte.ToString(),
                (Length == ASN1.IndefiniteLength) ? "inf" : Length.ToString(),
                Constructed ? "cons" : "prim",
                GetFullId(),
                string.Concat(string.Empty.PadLeft(indentLevel * 2, ' '), ShortDescription),
                DataText
            );

            if (Constructed)
            {
                foreach (ASN1Tag subTag in SubTags)
                {
                    subTag.ToShortText(output, indentLevel + 1);
                }
            }
        }

        private string GetFullId()
        {
            if (ParentTag != null)
            {
                return string.Concat(ParentTag.GetFullId(), ".", Identifier);
            }
            else
            {
                return Identifier.ToString();
            }
        }

        public void Write(Stream stream)
        {
            #region Tag
            byte tagByte = (byte)((((int)Class) << 6) | ((Constructed ? 1 : 0) << 5));
            if (Identifier < 31)
            {
                tagByte = (byte)(tagByte | Identifier);
                stream.WriteByte(tagByte);
            }
            else
            {
                tagByte |= 31;
                stream.WriteByte(tagByte);
                int n = 0;
                while ((Identifier >> (n * 7)) > 0)
                {
                    n++;
                }

                while (n > 0)
                {
                    n--;
                    tagByte = (byte)((Identifier >> (n * 7)) & (0x7F));
                    if (n >= 1) tagByte |= 0x80;
                    stream.WriteByte(tagByte);
                }
            }
            #endregion

            #region Length
            byte lenByte;
            if (Length >= 0 && Length <= 127)
            {
                lenByte = (byte)Length;
                stream.WriteByte(lenByte);
            }
            else if (Length == ASN1.IndefiniteLength)
            {
                //throw new NotImplementedException();
                lenByte = 0x80;
                stream.WriteByte(lenByte);
            }
            else if (Length > 0)
            {
                int numBytes = (int)Math.Ceiling(Math.Log(Length + 1) / Math.Log(256));
                if (numBytes >= 0x7F) throw new IOException("Don't know how to write length with 127 bytes");
                lenByte = (byte)(0x80 | (numBytes & 0x7F));
                stream.WriteByte(lenByte);

                for (int i = 0; i < numBytes; ++i)
                {
                    lenByte = (byte)((Length >> (8 * (numBytes - i - 1))) & 0xFF);
                    stream.WriteByte(lenByte);
                }
            }
            else
            {
                throw new IOException("Don't know how to write negative length.");
            }
            #endregion

            #region Data
            if (Constructed)
            {
                foreach (ASN1Tag subTag in SubTags)
                {
                    subTag.Write(stream);
                }
            }
            else
            {
                stream.Write(Data, 0, Data.Length);
            }
            #endregion
        }

        #region IEnumerable<ASN1Tag> Members

        public IEnumerator<ASN1Tag> GetEnumerator()
        {
            return SubTags.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
