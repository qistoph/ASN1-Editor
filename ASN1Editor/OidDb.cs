using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ASN1Editor
{
    public class OidDb : OidNode
    {
        private static Regex LineRegex = new Regex(@"^\s*((\d+\.?)+), (.*)");

        public OidDb()
        {
        }

        public void ParseFile(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = LineRegex.Match(line);
                    if(match.Success) {
                        Add(ParseOid(match.Groups[1].Value), 0, match.Groups[3].Value);
                    }
                }
            }
        }

        protected internal override void ToString(StringBuilder buf, int indent)
        {
            Indent(buf, indent);
            buf.Append("OidDb [").AppendLine();
            indent++;

            foreach (OidNode node in SubNodes.Values)
            {
                node.ToString(buf, indent);
            }

            indent--;
            Indent(buf, indent);
            buf.Append("]").AppendLine();
        }

        public static OidDb FromFile(string file)
        {
            OidDb oidDb = new OidDb();
            oidDb.ParseFile(file);
            return oidDb;
        }

    }
}
