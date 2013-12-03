using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MiniLogger
{
    public class StringLogComparer : IComparer<string>
    {
        public StringLogComparer()
        {
        }

        public int Compare(string x, string y)
        {
            string xSeverity = x.Split('|')[0];
            string ySeverity = y.Split('|')[0];

            int xValue = GiveValue(xSeverity);
            int yValue = GiveValue(ySeverity);

            return xValue == yValue ? x.CompareTo(y) : xValue.CompareTo(yValue);
        }

        private int GiveValue(string xSeverity)
        {
            switch (xSeverity)
            {
                case "Fatal" :
                    return 5;
                case "Error" :
                    return 4;
               case "Warning" :
                    return 3;
               case "Info" :
                    return 2;
                case "Log" :
                    return 1;
                default :
                    return 0;
            }
        }
    }
}
