using System;
using System.Collections.Generic;

namespace Lab1_150348.Comparers
{
    public class FilePatchComparer: IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            if (x.Length > y.Length) return 1;

            if (x.Length < y.Length) return -1;

            return String.CompareOrdinal(x, y);
        }
    }
}