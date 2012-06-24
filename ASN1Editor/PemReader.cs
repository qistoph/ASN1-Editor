using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Asn1Editor
{
    public class PemReader
    {
        public const string PemStart = "-----BEGIN ";
        public const string PemEnd = "-----END ";

        public static byte[] ReadPem(Stream stream)
        {
            bool started = false;
            bool ended = false;
            StringBuilder dataString = new StringBuilder();

            StreamReader reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!started && line.StartsWith(PemStart))
                {
                    started = true;
                    continue;
                }
                if (line.StartsWith(PemEnd))
                {
                    ended = true;
                    break;
                }
                if (started)
                {
                    dataString.Append(line);
                }
            }

            if (!started || !ended)
            {
                throw new ArgumentException("Invalid PEM data.");
            }

            return Convert.FromBase64String(dataString.ToString());
        }
    }
}
