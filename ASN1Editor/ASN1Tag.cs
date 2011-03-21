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
        public long DataLength { get; protected internal set; }
        public long HeaderLength { get; protected internal set; }
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
                if (Class == ASN1.Class.Universal && Constructed == false && Identifier == 0 && DataLength == 0)
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
                (DataLength == ASN1.IndefiniteLength) ? "inf" : DataLength.ToString(),
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
