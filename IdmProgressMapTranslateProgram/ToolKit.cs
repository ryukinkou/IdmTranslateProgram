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

        public static Shape[] QueryFlowRelationship(Shape shape, VisGluedShapesFlags flag)
        {
            Array idArray = shape.GluedShapes(flag, "", null);

            List<Shape> shapes = new List<Shape>();

            foreach (int id in idArray)
            {
                Shape gluedShape = shape.ContainingPage.Shapes.get_ItemFromID(id);
                shapes.Add(gluedShape);
            }

            return shapes.ToArray();
        }

        public static string FlowElementNaming(Shape shape)
        {

            Shape incomingShape = ToolKit.QueryFlowRelationship(shape, VisGluedShapesFlags.visGluedShapesIncoming2D)[0];

            Shape outgoingShape = ToolKit.QueryFlowRelationship(shape, VisGluedShapesFlags.visGluedShapesOutgoing2D)[0];

            return ToolKit.FlowElementNaming(incomingShape, shape, outgoingShape);

        }

        public static string FlowElementNaming(Shape incomingShape, Shape connectorShape, Shape outgoingShape)
        {

            string prefix = incomingShape.Text;

            string suffix = outgoingShape.Text;

            string connector = "to";

            if (!string.IsNullOrEmpty(connectorShape.Text.Trim()))
            {
                connector = connectorShape.Text.Trim();
            }
            if (!string.IsNullOrEmpty(prefix))
            {
                connector = "_" + connector;
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                connector = connector + "_";
            }

            return ToolKit.StringShift(prefix + connector + suffix);
        }

        public static void SysoutFlowRelationship(Shape shape)
        {
            Console.WriteLine(ToolKit.FlowElementNaming(shape));
        }

        public static Shape[] ShapeToArray(Shape shape)
        {

            List<Shape> shapes = new List<Shape>();

            if (shape.Name.Contains("Sheet") && shape.Shapes.Count > 1)
            {
                foreach (Shape insideShape in shape.Shapes)
                {
                    shapes.Add(insideShape);
                }
            }
            else
            {
                shapes.Add(shape);
            }

            return shapes.ToArray();
        }

    }
}
