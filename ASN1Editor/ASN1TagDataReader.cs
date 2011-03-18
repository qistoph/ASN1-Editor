using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ASN1Editor
{
    static internal class ASN1TagDataReader
    {
        public static OidDb OidDb { get; set; }

        public static string GetDataString(ASN1Tag tag)
        {
            if (tag.Class == ASN1.Class.Universal)
            {
                switch ((ASN1.TagNumber)tag.Identifier)
                {
                    case ASN1.TagNumber.ObjectIdentifier:
                        return GetOidDataString(tag);
                    case ASN1.TagNumber.BitString:
                        return GetBitDataString(tag);
                    case ASN1.TagNumber.PrintableString:
                        return GetPrintableDataString(tag);
                    case ASN1.TagNumber.UtcTime:
                        return GetUtcDataString(tag);
                }
            }

            return GetHexDataString(tag.Data, 0, Math.Min(64, tag.Data.Length));
        }

        private static string GetHexDataString(byte[] data, int start, int length)
        {
            char[] hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            StringBuilder str = new StringBuilder(length * 2);
            int end = start + length;
            for (int i = start; i < data.Length && i < end; ++i)
            {
                str.Append(hexChars[data[i] >> 4]);
                str.Append(hexChars[data[i] & 0xF]);
            }
            return str.ToString();
        }

        private static string GetOidDataString(ASN1Tag tag)
        {
            StringBuilder oidBuf = new StringBuilder();
            bool first = true;
            int value = 0;

            OidNode oidNode = OidDb;

            for (int i = 0; i < tag.Data.Length; ++i)
            {
                value <<= 7;
                value |= (tag.Data[i] & 0x7F);

                if ((tag.Data[i] & 0x80) == 0)
                {
                    if (first)
                    {
                        oidBuf.Append(value / 40);
                        oidBuf.Append('.');
                        oidBuf.Append(value % 40);
                        first = false;

                        if (oidNode != null) oidNode = oidNode.Get(value / 40);
                        if (oidNode != null) oidNode = oidNode.Get(value % 40);
                    }
                    else
                    {
                        oidBuf.Append('.');
                        oidBuf.Append(value);

                        if (oidNode != null) oidNode = oidNode.Get(value);
                    }

                    value = 0;
                }
            }

            if (oidNode != null) oidBuf.Append(" (").Append(oidNode.Description).Append(")");

            return oidBuf.ToString();
        }

        private static string GetBitDataString(ASN1Tag tag)
        {
            byte unusedBits = tag.Data[0];
            return string.Concat("unused: ", unusedBits, " ", GetHexDataString(tag.Data, 1, Math.Min(64, tag.Data.Length - 1)));
        }

        private static string GetPrintableDataString(ASN1Tag tag)
        {
            return ASCIIEncoding.ASCII.GetString(tag.Data);
        }

        public static DateTime GetUtcTime(ASN1Tag tag)
        {
            byte[] data = tag.Data;
            // YYMMDDhhmmssZ
            if (data.Length != 13) throw new IOException("Invalid UTC time, data length is not 13.");

            for (int i = 0; i < 12; i++)
            {
                if (data[i] < '0' || tag.Data[i] > '9')
                    throw new IOException("Invalid UTC time, contains illegal character.");
            }
            if (data[12] != 'Z')
                throw new IOException("Invalid UTC time, doesn't contain Z.");

            int year, month, day, hour, minute, second;
            year = GetNDigitValue(data, 0, 2);
            month = GetNDigitValue(data, 2, 2);
            day = GetNDigitValue(data, 4, 2);
            hour = GetNDigitValue(data, 6, 2);
            minute = GetNDigitValue(data, 8, 2);
            second = GetNDigitValue(data, 10, 2);

            year += (year < 86) ? 2000 : 1900;

            DateTime utcTime = new DateTime(year, month, day, hour, minute, second);

            return utcTime;
        }

        private static string GetUtcDataString(ASN1Tag tag)
        {
            DateTime utcTime = GetUtcTime(tag);
            return string.Concat(utcTime.ToString("yyyy-MM-dd HH:mm:ss"), " UTC");
        }

        private static int GetNDigitValue(byte[] data, int start, int digits)
        {
            int i = 0;
            int value = 0;
            while (digits-- > 0)
            {
                value *= 10;
                value += data[start + (i++)] - '0';
            }
            return value;
        }

    }
}
