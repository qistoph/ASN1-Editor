using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ASN1Editor
{
    public static class ASN1
    {
        public enum Class
        {
            Universal = 0,
            Application = 1,
            ContextSpecific = 2,
            Private = 3
        }

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

        public const int IndefiniteLength = -2;
        public const int EocIdentifier = 0;
        public const int EocLength = 0;

        public static ASN1Tag Decode(Stream stream)
        {
            return Decode(stream, true);
        }

        public static ASN1Tag Decode(Stream stream, bool recursive)
        {
            ASN1Tag node = new ASN1Tag();

            node.StartByte = stream.Position;
            ReadIdentifier(node, stream);
            ReadLength(node, stream);

            if (node.Constructed)
            {
                if (recursive)
                {
                    while ((stream.Position < node.StartByte + node.Length) || (node.Length == IndefiniteLength))
                    {
                        ASN1Tag subTag = Decode(stream, true);
                        node.AddSubTag(subTag);
                        if (subTag.Identifier == EocIdentifier && subTag.Length == EocLength)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (node.Length > int.MaxValue)
                {
                    throw new IOException("Can't read primitive data with more than " + int.MaxValue + " bytes.");
                }
                ReadData(node, stream);
            }

            return node;
        }

        private static void ReadIdentifier(ASN1Tag node, Stream stream)
        {
            int b = stream.ReadByte();
            if (b < 0) throw new IOException("Unexpected end of ASN.1 data while reading identifier.");

            //  8  7 | 6 | 5  4  3  2  1
            // Class |P/C|   Tag number
            node.Class = (ASN1.Class)(b >> 6);
            node.Constructed = (b & 0x20) != 0;

            if ((b & 0x1f) != 0x1f)
            {
                // Single byte identifier
                node.Identifier = b & 0x1f;
            }
            else
            {
                // Multi byte identifier

                node.Identifier = 0;
                do
                {
                    b = stream.ReadByte();
                    if (b < 0) throw new IOException("Unexpected end of ASN.1 data while reading multi byte identifier.");

                    node.Identifier <<= 7; // also happens first time, but that's ok, it's still 0
                    node.Identifier |= b & 0x7F;

                    if (node.Identifier == 0) throw new IOException("Invalid ASN.1 identifier (0 while reading constructed identifier).");
                } while ((b & 0x80) != 0);
            }
        }

        private static void ReadLength(ASN1Tag node, Stream stream)
        {
            long length = stream.ReadByte();

            if ((length & 0x80) == 0)
            {
                // Short form length
                node.Length = length & 0x7F;
            }
            else
            {
                // Long form length
                if ((length ^ 0x80) == 0)
                {
                    // indefinte length; terminated by EOC
                    node.Length = IndefiniteLength;
                }
                else
                {
                    byte[] l_bytes = new byte[(int)(length ^ 0x80)];
                    if (l_bytes.Length > 8)
                    {
                        throw new IOException("Can't handle length field of more than 8 bytes.");
                    }

                    int read = stream.Read(l_bytes, 0, l_bytes.Length);
                    if (read != l_bytes.Length)
                    {
                        throw new IOException("Not enough bytes to read length.");
                    }

                    node.Length = 0;
                    for (int i = 0; i < l_bytes.Length; ++i)
                    {
                        node.Length <<= 8;
                        node.Length |= l_bytes[i];
                    }
                }
            }
        }

        private static void ReadData(ASN1Tag node, Stream stream)
        {
            node.Data = new byte[(int)node.Length];
            int read = stream.Read(node.Data, 0, node.Data.Length);
            if (read != node.Data.Length)
            {
                throw new IOException("Unable to read enough data bytes.");
            }

            //if (node.Identifier == (int)ASN1.TagNumber.BitString)
            //{
            //    MemoryStream ms = new MemoryStream(node.Data, 1, node.Data.Length - 1);
            //    ASN1Tag subTag = ASN1.Decode(ms);
            //    node.AddSubTag(subTag);
            //}
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
    }
}
