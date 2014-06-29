using Microsoft.Office.Interop.Visio;

namespace IdmProgressMapTranslateProgram
{
    class Translator
    {

        public void execute(string sourcePath)
        {

            Application application;

            application = new Application();

            Document doc;

            doc = application.Documents.OpenEx(sourcePath, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenCopy);

            Page page;

            page = doc.Pages[1];

            for (int i = 1; i <= page.Shapes.Count; i++)
            {
                Shape shape = page.Shapes[i];

                //ROLE
                if (shape.Name.Contains("Pool / Lane"))
                {

                }

                if (shape.Name.Contains("Message Flow"))
                {

                    int sourceID = (int)shape.GluedShapes(VisGluedShapesFlags.visGluedShapesIncoming2D, "", null).GetValue(0);

                    int destinationID = (int)shape.GluedShapes(VisGluedShapesFlags.visGluedShapesOutgoing2D, "", null).GetValue(0);

                    Shape sourceShape = page.Shapes.get_ItemFromID(sourceID);

                    Shape destinationShape = page.Shapes.get_ItemFromID(destinationID);

                }

                if (shape.Name.Contains("Task"))
                {

                }

                if (shape.Name.Contains("Sheet"))
                {

                }

                if (shape.Name.Contains("Data Object"))
                {

                }

            }

        }

    }
}
