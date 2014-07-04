using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using IdmProgressMapTranslateProgram.Factory;

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

        private List<BaseFactory> _factories;

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
            this._parser = new OwlXmlParser();
            this._graph = this._parser.ParseOwl(this._inputOntologyPath);
            Constant.BPMN_TARGET_NAMESPACE = _graph.NameSpaces["xmlns:bpmn"];

            this._factories = new List<BaseFactory>();

            foreach (Type type in this.GetType().Assembly.DefinedTypes)
            {
                if (type.IsSubclassOf(typeof(BaseFactory)))
                {
                    object instance = type.Assembly.CreateInstance(
                        type.FullName,
                        false,
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new object[] { this._page, this._graph },
                        null, null);

                    this._factories.Add((BaseFactory)instance);
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

                if (shape.Name.Contains("Pool / Lane"))
                {
                    this.CreateIndividual<LaneFactory>(shape);
                }
                else if (shape.Name.Contains("Gateway"))
                {
                    this.CreateIndividual<GatewayFactory>(shape);
                }
                else if (shape.Name.Contains("Task"))
                {
                    this.CreateIndividual<TaskFactory>(shape);
                }
                else if (shape.Name.Contains("Start Event"))
                {
                    this.CreateIndividual<StartEventFactory>(shape);
                }
                else if (shape.Name.Contains("End Event"))
                {
                    this.CreateIndividual<EndEventFactory>(shape);
                }
                else if (shape.Name.Contains("Intermediate Event"))
                {
                    this.CreateIndividual<IntermediateCatchEventFactory>(shape);
                }
                else if (shape.Name.Contains("Data Object"))
                {
                    this.CreateIndividual<DataObjectFactory>(shape);
                }
                else if (shape.Name.Contains("Message Flow"))
                {
                    this.CreateIndividual<MessageFlowFactory>(shape);
                }
                else if (shape.Name.Contains("Sequence Flow"))
                {
                    this.CreateIndividual<SequenceFlowFactory>(shape);
                }
                else if (shape.Name.Contains("Expanded Sub-Process"))
                {
                    //TODO
                }
                else if (shape.Name.Contains("Collapsed Sub-Process"))
                {
                    //TODO
                }
                else if (shape.Name.Contains("Dynamic Connector"))
                {
                    //BETTER DO NOTHING
                    //ToolKit.SysoutFlowRelationship(shape);
                }
                else if (shape.Name.Contains("Box"))
                {
                    //BETTER DO NOTHING
                    //Console.WriteLine(shape.Text);
                }
                else if (shape.Name.Contains("Sheet"))
                {
                    //BETTER DO NOTHING
                    //Console.WriteLine(shape.Text);
                }
                else
                {
                    Console.WriteLine(shape.Name);
                }

            }

        }



        private void finishAutomation()
        {
            this._document.Close();
            this._application.Quit();
        }

        private void CreateIndividual<T>(Shape shape)
        {
            IOwlIndividual individual = (
                from factory in this._factories.ToArray()
                where factory.GetType() == typeof(T)
                select factory).First().Create(shape);
            this._graph.Nodes.Add(individual);
        }
    
    }
}
