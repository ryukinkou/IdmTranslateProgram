using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram
{
    public class ToolKit
    {

        public static string StringShift(string str)
        {

            Dictionary<string, string> shiftPattern = new Dictionary<string, string>();
            shiftPattern.Add(" ", "_");
            shiftPattern.Add(@"\", "_");
            shiftPattern.Add(@"/", "_");
            shiftPattern.Add(@"&", "and");

            foreach (string key in shiftPattern.Keys)
            {
                str = str.Replace(key, shiftPattern[key]);
            }

            List<string> endsWithPattern = new List<string>();
            endsWithPattern.Add(".");
            endsWithPattern.Add("?");

            foreach (string pattern in endsWithPattern)
            {
                if (str.EndsWith(pattern))
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }

            return str.Trim();

        }

    }
}
