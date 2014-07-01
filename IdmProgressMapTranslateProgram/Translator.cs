using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IdmProgressMapTranslateProgram
{
    public class Translator
    {

        private string _progressMapPath;
        private string _inputOntologyPath;
        private string _outputOntologyPath;

        public string ProgressMapPath
        {
            get { return this._progressMapPath; }
            set { this._progressMapPath = value; }
        }

        public string InputOntologyPath
        {
            get { return this._inputOntologyPath; }
            set { this._inputOntologyPath = value; }
        }

        public string OutputOntologyPath
        {
            get { return this._outputOntologyPath; }
            set { this._outputOntologyPath = value; }
        }

        private Application application;
        private Document doc;
        private Page page;

        private IOwlParser parser;
        private IOwlGraph graph;

        private IOwlNode task;
        private IOwlNode lane;
        private IOwlNode messageFlow;
        private IOwlNode dataObject;

        private GatewayFactory _gatewayFactory;

        public void execute()
        {
            this.readOntology();

            this.prepareAutomation();

            this.executeTranslation();

            this.saveOntology();

            this.finishAutomation();
        }

        private void prepareAutomation()
        {
            this.application = new Application();
            this.doc = application.Documents.OpenEx(this._progressMapPath, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenCopyOfNaming);
        }

        private void executeTranslation()
        {

            this.page = this.doc.Pages[1];

            for (int i = 1; i <= page.Shapes.Count; i++)
            {
                Shape shape = page.Shapes[i];

                //Pool / Lane => lane
                if (shape.Name.Contains("Pool / Lane"))
                {
                    //OwlIndividual individual = new OwlIndividual(Constant.BPMN_TARGET_NAMESPACE + "#" + this.shift(shape.Text), (OwlNode)this.lane);
                    //graph.Nodes.Add(individual);
                }

                //TODO
                if (shape.Name.Contains("Box"))
                {
                    
                }

                //Gateway => gateway
                if (shape.Name.Contains("Gateway"))
                {
                    IOwlIndividual gateway = this._gatewayFactory.CreateGateway(shape);
                    graph.Nodes.Add(gateway);
                }

                if (shape.Name.Contains("Dynamic Connector"))
                {

                }

                /*
                Intermediate Event

                End Event

                Sequence Flow

                Start Event

                Collapsed Sub-Process
                */

                if (shape.Name.Contains("Task"))
                {
                    //OwlIndividual individual = new OwlIndividual(Constant.BPMN_TARGET_NAMESPACE + "#" + this.shift(shape.Text), (OwlNode)this.task);
                    //graph.Nodes.Add(individual);
                }

                if (shape.Name.Contains("Sheet"))
                {

                    //Console.WriteLine(shape.Text);

                    //OwlIndividual individual = new OwlIndividual(targetNamespace + "#" + this.shift(shape.Text), (OwlNode)this.sheet);
                    //graph.Nodes.Add(individual);
                }

                if (shape.Name.Contains("Intermediate Event"))
                {
                    //Console.WriteLine(shape.Text);
                }

                if (shape.Name.Contains("Data Object"))
                {
                    //OwlIndividual individual = new OwlIndividual(Constant.BPMN_TARGET_NAMESPACE + "#" + this.shift(shape.Text), (OwlNode)this.dataObject);
                    //graph.Nodes.Add(individual);
                }

                if (shape.Name.Contains("Message Flow"))
                {

                    int sourceID = (int)shape.GluedShapes(VisGluedShapesFlags.visGluedShapesIncoming2D, "", null).GetValue(0);

                    int destinationID = (int)shape.GluedShapes(VisGluedShapesFlags.visGluedShapesOutgoing2D, "", null).GetValue(0);

                    Shape sourceShape = page.Shapes.get_ItemFromID(sourceID);

                    Shape destinationShape = page.Shapes.get_ItemFromID(destinationID);

                }

            }

        }



        private void finishAutomation()
        {
            this.doc.Close();
            this.application.Quit();
        }

        private void readOntology() 
        {

            parser = new OwlXmlParser();
            graph = parser.ParseOwl(this._inputOntologyPath);

            Constant.BPMN_TARGET_NAMESPACE = graph.NameSpaces["xml:base"];

            this._gatewayFactory = new GatewayFactory(this.graph);

            //this.task = graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#task"];
            //this.dataObject = graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#dataObject"];
            //this.messageFlow = graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#messageFlow"];
            //this.lane = graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#lane"];

        }

        private void saveOntology()
        {
            IOwlGenerator generator = new OwlXmlGenerator();
            generator.GenerateOwl(this.graph, this._outputOntologyPath);
        }
    
    }
}
