using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Asn1Lib
{
    public class Asn1Tag : IEnumerable<Asn1Tag>
    {
        public Asn1.Class Class { get; protected internal set; }
        public bool Constructed { get; protected internal set; }
        public int Identifier { get; protected internal set; }
        public int FullIdentifier { get; protected internal set; }
        public long StartByte { get; protected internal set; }
        public bool IndefiniteLength { get; protected internal set; }
        public byte[] Data { get; protected internal set; }

        /// <summary>
        /// When encoding always encode identifiers in multiple bytes, even if it's <31
        /// </summary>
        public bool ForceMultiByteIdentifier { get; set; }

        private List<Asn1Tag> SubTags { get; set; }
        private Asn1Tag ParentTag { get; set; }

        public Asn1.TagNumber TagNumber
        {
            get
            {
                return Constructed ? Asn1.TagNumber.NonePrimitive : (Asn1.TagNumber)Identifier;
            }
        }

        public Asn1Tag this[int id]
        {
            get
            {
                return this[id, 0];
            }
        }

        public Asn1Tag this[int id, int index]
        {
            get
            {
                foreach (Asn1Tag subTag in this)
                {
                    if (subTag.FullIdentifier == id)
                    {
                        if (index == 0)
                        {
                            return subTag;
                        }
                        index--;
                    }
                }
                throw new ArgumentException("Sub Tag not found.");
            }
        }

        public string ShortDescription
        {
            get
            {
                if (Class == Asn1.Class.Universal && Constructed == false && Identifier == 0 && Data.Length == 0)
                    return "EOC";
                else if (Class == Asn1.Class.Universal && Identifier < 31)
                    return Asn1.GetUTNDescription(Identifier);
                else
                    return string.Concat("[", Identifier, "]");
            }
        }

        public string DataText
        {
            get
            {
                return Constructed ? "Constructed" : Asn1TagDataReader.GetDataString(this);
            }
        }

        public int SubTagsCount { get { return SubTags.Count; } }

        protected internal Asn1Tag()
        {
            SubTags = new List<Asn1Tag>();
            ParentTag = null;
        }

        protected internal Asn1Tag(Asn1.Class asn1Class, bool constructed, int identifier)
            : this()
        {
            Class = asn1Class;
            Constructed = constructed;
            Identifier = identifier;

            //  8  7 | 6 | 5  4  3  2  1
            // Class |P/C|   Tag number
            FullIdentifier = (((int)asn1Class) << 6) | (constructed ? 0x20 : 0x00) | (identifier & 0x1F);
        }

        public Asn1Tag(Asn1.Class asn1Class, int identifier, byte[] data)
            : this(asn1Class, false, identifier)
        {
            Data = (byte[])data.Clone();
        }

        public Asn1Tag(Asn1.Class asn1Class, int identifier, params Asn1Tag[] subTags)
            : this(asn1Class, true, identifier)
        {
            foreach (Asn1Tag subTag in subTags)
            {
                AddSubTag(subTag);
            }
        }

        protected internal void AddSubTag(Asn1Tag subTag)
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
                (IndefiniteLength) ? "inf" : Asn1.GetTotalByteCount(this).ToString(),
                Constructed ? "cons" : "prim",
                Identifier.ToString(),
                string.Concat(string.Empty.PadLeft(indentLevel * 2, ' '), ShortDescription),
                DataText
            );

            if (Constructed)
            {
                foreach (Asn1Tag subTag in SubTags)
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

        #region IEnumerable<Asn1Tag> Members

        public IEnumerator<Asn1Tag> GetEnumerator()
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
