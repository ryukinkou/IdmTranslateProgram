using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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

        private Application _application;
        private Document _document;
        private Page _page;

        private IOwlParser _parser;
        private IOwlGraph _graph;

        private GatewayFactory _gatewayFactory;
        private LaneFactory _laneFactory;
        private TaskFactory _taskFactory;
        private IntermediateCatchEventFactory _intermediateCatchEventFactory;

        public void execute()
        {
            
            this.prepareAutomation();

            this.readOntology();

            this.executeTranslation();

            this.saveOntology();

            this.finishAutomation();
        }

        private void readOntology()
        {
            _parser = new OwlXmlParser();
            _graph = this._parser.ParseOwl(this._inputOntologyPath);
            Constant.BPMN_TARGET_NAMESPACE = _graph.NameSpaces["xmlns:bpmn"];

            this._gatewayFactory = new GatewayFactory(this._page, this._graph);
            this._laneFactory = new LaneFactory(this._page, this._graph);
            this._taskFactory = new TaskFactory(this._page, this._graph);

            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo[] infos = typeof(Translator).GetFields(flag);

            foreach (FieldInfo info in infos)
            {
                if (info.FieldType.IsSubclassOf(typeof(BaseFactory)))
                {
                    Console.WriteLine(info.Name);

                    //Find typedReference now
                    //info.SetValue(this, new object());

                    Assembly assembly = info.FieldType.Assembly;

                    object o = assembly.CreateInstance(info.FieldType.FullName, false, BindingFlags.Public, null, new object[] { this._page, this._graph }, null, null);

                }
            }


        }

        private void saveOntology()
        {
            IOwlGenerator generator = new OwlXmlGenerator();
            generator.GenerateOwl(this._graph, this._outputOntologyPath);
        }

        private void prepareAutomation()
        {
            this._application = new Application();
            this._document = this._application.Documents.OpenEx(
                this._progressMapPath, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenCopyOfNaming);
            this._page = this._document.Pages[1];
        }

        private void executeTranslation()
        {

            for (int i = 1; i <= _page.Shapes.Count; i++)
            {
                Shape shape = _page.Shapes[i];

                //Pool / Lane => lane
                if (shape.Name.Contains("Pool / Lane"))
                {
                    IOwlIndividual lane = this._laneFactory.Create(shape);
                    this._graph.Nodes.Add(lane);
                }

                //Gateway => gateway
                if (shape.Name.Contains("Gateway"))
                {
                    IOwlIndividual gateway = this._gatewayFactory.Create(shape);
                    this._graph.Nodes.Add(gateway);
                }

                //Task => task
                if (shape.Name.Contains("Task"))
                {
                    IOwlIndividual task = this._taskFactory.Create(shape);
                    this._graph.Nodes.Add(task);
                }

                /*
                End Event

                Sequence Flow

                Start Event

                Collapsed Sub-Process
                */

                //Intermediate Event => Event
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

                    Shape sourceShape = _page.Shapes.get_ItemFromID(sourceID);

                    Shape destinationShape = _page.Shapes.get_ItemFromID(destinationID);

                }

                //TODO
                if (shape.Name.Contains("Box"))
                {

                }

                if (shape.Name.Contains("Dynamic Connector"))
                {
                    /* TODO
                    Console.WriteLine(
                        ToolKit.QueryFlowRelationship(this._page, shape, VisGluedShapesFlags.visGluedShapesIncoming2D).Text +
                        " => " +
                        ToolKit.QueryFlowRelationship(this._page, shape, VisGluedShapesFlags.visGluedShapesOutgoing2D).Text);
                     */
                }

                //DO NOTHING
                if (shape.Name.Contains("Sheet"))
                {
                    //Console.WriteLine(shape.Text);
                }

            }

        }



        private void finishAutomation()
        {
            this._document.Close();
            this._application.Quit();
        }


    
    }
}
