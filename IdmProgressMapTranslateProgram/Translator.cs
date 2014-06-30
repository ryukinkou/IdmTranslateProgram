using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections;

namespace IdmProgressMapTranslateProgram
{
    class Translator
    {

        private IOwlParser parser;
        private IOwlGraph graph;

        private IOwlNode task;

        private string targetNamespace;

        public void executeTranslation(string sourcePath)
        {

            Application application;

            application = new Application();

            Document doc;

            doc = application.Documents.OpenEx(sourcePath, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenCopyOfNaming);

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
                    //Console.WriteLine(shape.Text);

                    string a = shape.Text.Replace(@"\", "_").Replace("/", "_").Replace(" ", "_");

                    Console.WriteLine(a);

                    //OwlIndividual newTask = new OwlIndividual(targetNamespace + "#" + a, (OwlNode)this.task);
                    //graph.Nodes.Add(newTask);

                }

                if (shape.Name.Contains("Sheet"))
                {

                }

                if (shape.Name.Contains("Data Object"))
                {

                }

            }

        }

        public void readOntology(string ontologyPath) 
        {

            parser = new OwlXmlParser();
            graph = parser.ParseOwl(ontologyPath);

            //target namespace
            this.targetNamespace = graph.NameSpaces["xml:base"];

            //task
            this.task = graph.Nodes[this.targetNamespace + "#task"];


        }

        public void saveOntology(string ontologyPath)
        {
            IOwlGenerator generator = new OwlXmlGenerator();

            generator.StopOnErrors = false;

            generator.GenerateOwl(this.graph, ontologyPath);
            
        }
    
    }
}
