using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;

namespace IdmProgressMapTranslateProgram.Factory
{
    public abstract class BaseFactory
    {
        protected Page _page;
        protected IOwlGraph _graph;

        public BaseFactory(Page page, IOwlGraph graph)
        {
            this._page = page;
            this._graph = graph;
        }

        public abstract IOwlIndividual Create(Shape shape);

        protected IOwlIndividual BuildProperty(Shape shape, IOwlIndividual individual)
        {

            IOwlEdge idEdge = new OwlEdge(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "id"));
            idEdge.ChildNode = new OwlLiteral(shape.ID.ToString(), "en-us", "http://www.w3.org/2001/XMLSchema#string");

            individual.AttachChildEdge(idEdge);

            IOwlEdge nameEdge = new OwlEdge(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "name"));
            nameEdge.ChildNode = new OwlLiteral(shape.Name.ToString(), "en-us", "http://www.w3.org/2001/XMLSchema#string");

            individual.AttachChildEdge(nameEdge);

            return individual;
        }

    }
}
