using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Asn1Lib
{
    public class ConsoleTable
    {
        private List<string> Titles;
        private List<List<string>> Rows;

        public string ColumnSeperator { get; set; }
        public string HeaderSeperator { get; set; }
        public string RowSeperator { get; set; }

        public ConsoleTable()
        {
            Titles = new List<string>();
            Rows = new List<List<string>>();
            ColumnSeperator = " | ";
            HeaderSeperator = "=";
            RowSeperator = "-";
        }

        public void SetTitles(params string[] args)
        {
            Titles.Clear();
            foreach (string str in args)
                Titles.Add(str);
        }

        public void AddRow(params string[] args)
        {
            List<string> row = new List<string>(args.Length);
            foreach (string str in args) row.Add(str);
            Rows.Add(row);
        }

        public override string ToString()
        {
            List<int> maxLengths = new List<int>();
            int totalLength = 0;

            for (int i = 0; i < Titles.Count; ++i)
            {
                if (i >= maxLengths.Count)
                    maxLengths.Add(Titles[i].Length);
                else if (Titles[i].Length > maxLengths[i])
                    maxLengths[i] = Titles[i].Length;
            }

            foreach (List<string> row in Rows)
            {
                for (int i = 0; i < row.Count; ++i)
                {
                    if (i >= maxLengths.Count)
                        maxLengths.Add(row[i].Length);
                    else if (maxLengths.Count < i || row[i].Length > maxLengths[i])
                        maxLengths[i] = row[i].Length;
                }
            }
            totalLength = maxLengths.Sum() + (maxLengths.Count - 1) * ColumnSeperator.Length;

            StringBuilder result = new StringBuilder();
            if (Titles.Count > 0)
            {
                for (int i = 0; i < maxLengths.Count; ++i)
                {
                    if (i > 0) result.Append(ColumnSeperator);
                    string str = i < Titles.Count ? Titles[i] : string.Empty;
                    result.Append(str.PadRight(maxLengths[i], ' '));
                }

                if (HeaderSeperator != null && HeaderSeperator.Length > 0)
                {
                    result.Append(Environment.NewLine);
                    if (HeaderSeperator.Length > totalLength)
                    {
                        result.Append(HeaderSeperator.Substring(0, totalLength));
                    }
                    else
                    {
                        int len = 0;
                        while (len < totalLength)
                        {
                            result.Append(HeaderSeperator);
                            len += HeaderSeperator.Length;
                        }
                    }
                }
            }

            foreach (List<string> row in Rows)
            {
                result.Append(Environment.NewLine);
                for (int i = 0; i < maxLengths.Count; ++i)
                {
                    if (i > 0) result.Append(ColumnSeperator);
                    string str = i < row.Count ? row[i] : string.Empty;
                    result.Append(str.PadRight(maxLengths[i], ' '));
                }
            }

            return result.ToString();
        }
    }
}
