using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASN1Editor
{
    public static class Extensions
    {
        public static void Indent(this StringBuilder stringBuilder, int indention)
        {
            while (indention-- > 0)
            {
                stringBuilder.Append(' ');
            }
        }
    }
}
