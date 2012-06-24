using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Asn1Lib
{
    public static class Asn1
    {
        /// <summary>
        /// ASN.1 classes
        /// </summary>
        public enum Class
        {
            Universal = 0,
            Application = 1,
            ContextSpecific = 2,
            Private = 3
        }

        /// <summary>
        /// Universal primitive ASN.1 tag types
        /// </summary>
        public enum TagNumber
        {
            NonePrimitive = -1,
            Boolean = 1,
            Integer = 2,
            BitString = 3,
            OctetString = 4,
            Null = 5,
            ObjectIdentifier = 6,
            ObjectDescriptor = 7,
            InstanceOf = 8,
            Real = 9,
            Enumerated = 10,
            EmbeddedPdv = 11,
            Utrf8String = 12,
            RelativeOid = 13,
            Sequence = 16,
            Set = 17,
            NumericString = 18,
            PrintableString = 19,
            TeletextString = 20,
            VideotexString = 21,
            Ia5String = 22,
            UtcTime = 23,
            GeneralizedTime = 24,
            GraphicString = 25,
            VisibleString = 26,
            GeneralString = 27,
            UniversalString = 28,
            CharacterString = 29,
            BmpString = 30
        }

        /// <summary>
        /// End of Contents tag identifier (as specefied in X.690)
        /// </summary>
        public const int EocIdentifier = 0;

        /// <summary>
        /// End of Contents tag length (as specefied in X.690)
        /// </summary>
        public const int EocLength = 0;

        /// <summary>
        /// Decode ASN.1 tags from a stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>The root node read from the stream.</returns>
        public static Asn1Tag Decode(Stream stream)
        {
            return Decode(stream, false, true);
        }

        public static Asn1Tag Decode(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Decode(fs);
            }
        }

        /// <summary>
        /// Decode ASN.1 tags from a stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="singleNode">If true, read only one single node without subnodes.</param>
        /// <param name="readSubAsn1">If true, possible ASN.1 data in BitStrings will be decoded.</param>
        /// <returns>The root node read from the stream.</returns>
        public static Asn1Tag Decode(Stream stream, bool singleNode, bool readSubAsn1)
        {
            Asn1Tag node = new Asn1Tag();

            Asn1.Class nodeClass;
            bool constructed;
            bool indefLength;
            int fullIdentifier;
            bool forceMultiByteIdentifier;

            node.StartByte = stream.Position;
            node.Identifier = ReadIdentifier(stream, out nodeClass, out constructed, out fullIdentifier, out forceMultiByteIdentifier);
            node.Class = nodeClass;
            node.Constructed = constructed;
            node.FullIdentifier = fullIdentifier;
            node.ForceMultiByteIdentifier = forceMultiByteIdentifier;
            long dataLength = ReadLength(stream, out indefLength);
            long headerLength = stream.Position - node.StartByte;

            if (node.Constructed)
            {
                if (!singleNode)
                {
                    while ((stream.Position < node.StartByte + headerLength + dataLength) || indefLength)
                    {
                        Asn1Tag subTag = Decode(stream, singleNode, readSubAsn1);
                        node.AddSubTag(subTag);
                        if (subTag.Identifier == EocIdentifier && subTag.Data.Length == EocLength)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                ReadData(node, stream, dataLength);

                if (readSubAsn1)
                {
                    switch (node.Identifier)
                    {
                        case (int)Asn1.TagNumber.BitString:
                            ReadSubAsn1(node, 1);
                            break;
                        case (int)Asn1.TagNumber.OctetString:
                            ReadSubAsn1(node, 0);
                            break;
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Parse data in a (BitString) node if it's valid ASN.1 and add read nodes as sub nodes to this node.
        /// </summary>
        /// <param name="node">Node to read data from.</param>
        /// <returns>True if valid ASN.1 was read, false if not.</returns>
        private static bool ReadSubAsn1(Asn1Tag node, int dataOffset)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(node.Data, dataOffset, node.Data.Length - 1))
                {
                    Asn1Tag subTag = Asn1.Decode(ms);
                    subTag.ToShortText(); // To validate, if it fails with exception, it's not added

                    node.AddSubTag(subTag);
                    return true;
                }
            }
            catch (Exception ex) // TODO: narrow this down
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Read an ASN.1 tag identifier from a stream (as specified in X.690)
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="tagClass">Tag class of the tag</param>
        /// <param name="constructed">Indicates if the tag is constructed = true (or primitive = false)</param>
        /// <returns></returns>
        private static int ReadIdentifier(Stream stream, out Asn1.Class tagClass, out bool constructed, out int fullIdentifier, out bool forceMultiByteIdentifier)
        {
            int identifier;
            int b = stream.ReadByte();
            if (b < 0) throw new IOException("Unexpected end of ASN.1 data while reading identifier.");

            //  8  7 | 6 | 5  4  3  2  1
            // Class |P/C|   Tag number
            tagClass = (Asn1.Class)(b >> 6);
            constructed = (b & 0x20) != 0;
            fullIdentifier = 0;

            if ((b & 0x1f) != 0x1f)
            {
                // Single byte identifier
                identifier = b & 0x1f;
                fullIdentifier = b;
                forceMultiByteIdentifier = false;
            }
            else
            {
                // Multi byte identifier
                identifier = 0;
                fullIdentifier |= b;
                // juse to be sure we force encoding this to multiple bytes to maintain the structue as much as we can
                forceMultiByteIdentifier = true;

                do
                {
                    b = stream.ReadByte();
                    if (b < 0) throw new IOException("Unexpected end of ASN.1 data while reading multi byte identifier.");

                    identifier <<= 7; // also happens first time, but that's ok, it's still 0
                    identifier |= b & 0x7F;

                    fullIdentifier <<= 8;
                    fullIdentifier |= b;

                    if (identifier == 0) throw new IOException("Invalid ASN.1 identifier (0 while reading constructed identifier).");
                } while ((b & 0x80) != 0);
            }

            return identifier;
        }

        /// <summary>
        /// Read an ASN.1 tag's length from a stream (as specified in X.690)
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>Tag's length</returns>
        private static long ReadLength(Stream stream, out bool indefiniteLength)
        {
            long length = 0;
            int b = stream.ReadByte();

            if ((b & 0x80) == 0)
            {
                // Short form length
                length = b & 0x7F;
                indefiniteLength = false;
            }
            else
            {
                // Long form length
                if ((b ^ 0x80) == 0)
                {
                    // indefinte length; terminated by EOC
                    length = 0;
                    indefiniteLength = true;
                }
                else
                {
                    byte[] l_bytes = new byte[(int)(b ^ 0x80)];
                    if (l_bytes.Length > 8)
                    {
                        throw new IOException("Can't handle length field of more than 8 bytes.");
                    }

                    int read = stream.Read(l_bytes, 0, l_bytes.Length);
                    if (read != l_bytes.Length)
                    {
                        throw new IOException("Not enough bytes to read length.");
                    }

                    length = 0;
                    for (int i = 0; i < l_bytes.Length; ++i)
                    {
                        length <<= 8;
                        length |= l_bytes[i];
                    }
                    indefiniteLength = false;
                }
            }

            return length;
        }

        private static void ReadData(Asn1Tag node, Stream stream, long dataLength)
        {
            if (dataLength > int.MaxValue)
            {
                throw new IOException("Can't read primitive data with more than " + int.MaxValue + " bytes.");
            }

            node.Data = new byte[(int)dataLength];
            int read = stream.Read(node.Data, 0, node.Data.Length);
            if (read != node.Data.Length)
            {
                throw new IOException("Unable to read enough data bytes.");
            }
        }

        public static void Encode(string filename, Asn1Tag node)
        {
            CallWithFilestream(Encode, filename, node);
        }

        public static void EncodeData(string filename, Asn1Tag node)
        {
            CallWithFilestream(EncodeData, filename, node);
        }

        public static void ExportText(string filename, Asn1Tag node)
        {
            CallWithFilestream(ExportText, filename, node);
        }

        private static void CallWithFilestream(Action<Stream, Asn1Tag> method, string filename, Asn1Tag node)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                method(fs, node);
            }
        }

        public static void Encode(Stream stream, Asn1Tag node)
        {
            EncodeHeader(stream, node);
            EncodeData(stream, node);
        }

        public static void EncodeHeader(Stream stream, Asn1Tag node)
        {
            byte[] header = EncodeHeader(node);
            stream.Write(header, 0, header.Length);
        }

        public static byte[] EncodeHeader(Asn1Tag node)
        {
            //TODO: determine/rationalize the initial size (though it's not like we're working with megabytes)
            AutoGrowArray<byte> header = new AutoGrowArray<byte>(10);

            #region Tag
            byte tagByte = (byte)((((int)node.Class) << 6) | ((node.Constructed ? 1 : 0) << 5));
            if (node.Identifier < 31 && !node.ForceMultiByteIdentifier)
            {
                tagByte = (byte)(tagByte | node.Identifier);
                header.Add(tagByte);
            }
            else
            {
                tagByte |= 31;
                header.Add(tagByte);
                int n = 0;
                while ((node.Identifier >> (n * 7)) > 0)
                {
                    n++;
                }

                while (n > 0)
                {
                    n--;
                    tagByte = (byte)((node.Identifier >> (n * 7)) & (0x7F));
                    if (n >= 1) tagByte |= 0x80;
                    header.Add(tagByte);
                }
            }
            #endregion

            #region Length
            long dataLength;
            if (node.Constructed)
            {
                //TODO: don't calculate and generate the length of subnodes over and over again
                dataLength = node.Sum(subTag => GetTotalByteCount(subTag));
            }
            else
            {
                dataLength = node.Data.Length;
            }

            byte lenByte;
            if (node.IndefiniteLength)
            {
                lenByte = 0x80;
                header.Add(lenByte);
            }
            else if (dataLength >= 0 && dataLength <= 127)
            {
                lenByte = (byte)dataLength;
                header.Add(lenByte);
            }
            else if (dataLength > 0)
            {
                int numBytes = (int)Math.Ceiling(Math.Log(dataLength + 1) / Math.Log(256));
                if (numBytes >= 0x7F) throw new IOException("Don't know how to write length with 127 bytes");
                lenByte = (byte)(0x80 | (numBytes & 0x7F));
                header.Add(lenByte);

                for (int i = 0; i < numBytes; ++i)
                {
                    lenByte = (byte)((dataLength >> (8 * (numBytes - i - 1))) & 0xFF);
                    header.Add(lenByte);
                }
            }
            else
            {
                throw new IOException("Don't know how to write negative length.");
            }
            #endregion

            return header.ToArray();
        }

        public static long GetTotalByteCount(Asn1Tag node)
        {
            if (node.Constructed)
            {
                return Asn1.EncodeHeader(node).Length + node.Sum(subTag => GetTotalByteCount(subTag));
            }
            else
            {
                return Asn1.EncodeHeader(node).Length + node.Data.Length;
            }
        }

        public static void EncodeData(Stream stream, Asn1Tag node)
        {
            #region Data
            if (node.Constructed)
            {
                foreach (Asn1Tag subTag in node)
                {
                    Encode(stream, subTag);
                }
            }
            else
            {
                stream.Write(node.Data, 0, node.Data.Length);
            }
            #endregion
        }

        public static void ExportText(Stream stream, Asn1Tag node)
        {
            byte[] text = ASCIIEncoding.ASCII.GetBytes(node.ToShortText());
            stream.Write(text, 0, text.Length);
        }

        internal static string GetUTNDescription(int identifier)
        {
            switch (identifier)
            {
                case 1: return "BOOLEAN";
                case 2: return "INTEGER";
                case 3: return "BIT_STRING";
                case 4: return "OCTET_STRING";
                case 5: return "NULL";
                case 6: return "OBJECT_IDENTIFIER";
                case 7: return "OBJECTDESCRIPTOR";
                case 8: return "INSTANCE_OF";
                case 9: return "REAL";
                case 10: return "ENUMERATED";
                case 11: return "EMBEDDED_PDV";
                case 12: return "UTF8STRING";
                case 13: return "RELATIVE_OID";
                case 16: return "SEQUENCE";
                case 17: return "SET";
                case 18: return "NUMERICSTRING";
                case 19: return "PRINTABLESTRING";
                case 20: return "TELETEXSTRING";
                case 21: return "VIDEOTEXSTRING";
                case 22: return "IA5STRING";
                case 23: return "UTCTIME";
                case 24: return "GENERALIZEDTIME";
                case 25: return "GRAPHICSTRING";
                case 26: return "VISIBLESTRING";
                case 27: return "GENERALSTRING";
                case 28: return "UNIVERSALSTRING";
                case 29: return "CHARACTER_STRING";
                case 30: return "BMPSTRING";
                default: return "UNKNOWN";
            }
        }

        public static short GetShort(byte[] data)
        {
            System.Diagnostics.Debug.Assert(data.Length == 2);
            return (short)((data[0] << 8) | data[1]);
        }
    }
}
