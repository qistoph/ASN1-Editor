using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Asn1Lib
{
    public class OidNode
    {
        public int Id { get; protected set; }
        public string OidString { get; protected set; }
        public string Description { get; protected set; }

        protected Dictionary<int, OidNode> SubNodes { get; set; }

        public OidNode()
        {
            SubNodes = new Dictionary<int, OidNode>();
        }

        public OidNode Add(int[] oid, int oidOffset, string description)
        {
            if (oidOffset >= oid.Length)
            {
                throw new ArgumentException("oidOffset cannot be >= oid.Length.");
            }

            int id = oid[oidOffset];

            if (oidOffset == oid.Length - 1)
            { // Final node
                if (SubNodes.ContainsKey(id))
                {
                    throw new ArgumentException(string.Concat("OID ", id, "already present in " + OidString));
                }
            }

            if (!SubNodes.ContainsKey(id))
            {
                OidNode node = new OidNode();
                node.Id = id;
                node.OidString = OidToString(oid, 0, oid.Length);
                SubNodes.Add(id, node);

                if (oidOffset == oid.Length - 1)
                {
                    node.Description = description;
                    return node;
                }
            }

            return SubNodes[id].Add(oid, oidOffset + 1, description);
        }

        public OidNode Get(int subId)
        {
            return SubNodes.ContainsKey(subId) ? SubNodes[subId] : null;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            ToString(str, 0);
            return str.ToString();
        }

        protected internal virtual void ToString(StringBuilder buf, int indent)
        {
            Indent(buf, indent);
            buf.Append("OidNode [").AppendLine();
            indent++;

            Indent(buf, indent);
            buf.Append("Id: ").Append(Id).AppendLine();

            Indent(buf, indent);
            buf.Append("OidString: ").Append(OidString).AppendLine();

            Indent(buf, indent);
            buf.Append("Description: ").Append(Description).AppendLine();

            foreach (OidNode node in SubNodes.Values)
            {
                node.ToString(buf, indent);
            }

            indent--;
            Indent(buf, indent);
            buf.Append("]").AppendLine();
        }

        private static string OidToString(int[] oid, int start, int length)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < length && i < oid.Length; ++i)
            {
                if (i > 0) str.Append('.');
                str.Append(oid[start + i]);
            }

            return str.ToString();
        }

        public static int[] ParseOid(string oid)
        {
            string[] partsStr = oid.Split('.');
            int[] parts = new int[partsStr.Length];

            for (int i = 0; i < parts.Length; ++i)
            {
                parts[i] = int.Parse(partsStr[i]);
            }

            return parts;
        }

        protected static void Indent(StringBuilder buf, int indent)
        {
            while (indent-- > 0) buf.Append("  ");
        }
    }
}
