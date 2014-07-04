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

        public static Shape QueryFlowRelationship(Shape shape, VisGluedShapesFlags flag)
        {
            Array idArray = shape.GluedShapes(flag, "", null);

            if (idArray.Length == 1)
            {
                int id = (int)shape.GluedShapes(flag, "", null).GetValue(0);
                Shape sourceShape = shape.ContainingPage.Shapes.get_ItemFromID(id);

                foreach (Shape element in sourceShape.Shapes)
                {
                    if (element.Name.Contains("Data Object"))
                    {
                        //Console.WriteLine(element.Text);
                    }
                }

                return sourceShape;
            }
            else
            {
                return null;
            }
        }

        public static string FlowElementNaming(Shape shape)
        {

            Shape incoming = ToolKit.QueryFlowRelationship(shape, VisGluedShapesFlags.visGluedShapesIncoming2D);

            Shape outgoing = ToolKit.QueryFlowRelationship(shape,VisGluedShapesFlags.visGluedShapesOutgoing2D);

            string connector = "to";

            if (!string.IsNullOrEmpty(shape.Text.Trim()))
            {
                connector = shape.Text.Trim();
            }

            if (!string.IsNullOrEmpty(incoming.Text))
            {
                connector = "_" + connector;
            }

            if (!string.IsNullOrEmpty(outgoing.Text))
            {
                connector = connector + "_";
            }

            return ToolKit.StringShift(incoming.Text) + connector + ToolKit.StringShift(outgoing.Text);
        }

        public static void SysoutFlowRelationship(Shape shape)
        {

            Console.WriteLine(
                "-- " +
                ToolKit.QueryFlowRelationship(
                shape,
                VisGluedShapesFlags.visGluedShapesIncoming2D).Text +
                " => " +
                ToolKit.QueryFlowRelationship(
                shape,
                VisGluedShapesFlags.visGluedShapesOutgoing2D).Text +
                " --");

        }

    }
}
