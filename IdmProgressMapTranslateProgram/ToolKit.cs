using Microsoft.Office.Interop.Visio;
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

        public static string GetFullName(string targetNamespace, string name)
        {

            if (targetNamespace.EndsWith("#"))
            {
                return targetNamespace + name;
            }
            else
            {
                return targetNamespace + "#" + name;
            }

        }

        public static Shape QueryFlowRelationship(Page page, Shape shape, VisGluedShapesFlags flag)
        {
            Array idArray = shape.GluedShapes(flag, "", null);

            if (idArray.Length == 1)
            {
                int id = (int)shape.GluedShapes(flag, "", null).GetValue(0);
                Shape sourceShape = page.Shapes.get_ItemFromID(id);
                return sourceShape;
            }
            else
            {
                return null;
            }
        }

        public static string FlowElementNaming(Page page, Shape shape)
        {

            Shape incoming = ToolKit.QueryFlowRelationship(page, shape, VisGluedShapesFlags.visGluedShapesIncoming2D);

            Shape outgoing = ToolKit.QueryFlowRelationship(page, shape, VisGluedShapesFlags.visGluedShapesIncoming2D);

            string connector = "_";

            if (!string.IsNullOrEmpty(shape.Text.Trim()))
            {
                connector = "_" + shape.Text.Trim() + "_";
            }

            return ToolKit.StringShift(incoming.Text) + connector + ToolKit.StringShift(outgoing.Text);
        }

    }
}
