using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram.Factory
{
    public class MessageFlowFactory : FlowFactory
    {

        public MessageFlowFactory(Page page, IOwlGraph graph)
            : base(page, graph)
        {

        }

        public override IOwlIndividual Create(Shape shape)
        {
            IOwlIndividual individual = new OwlIndividual(
                ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.FlowElementNaming(shape)),
                (OwlNode)base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "messageFlow")]);

            individual = base.BuildProperty(shape, individual);

            return individual;
        }

        public override void BuildRelationship(Shape shape)
        {

            Shape incomingShape = ToolKit.QueryFlowRelationship(shape, VisGluedShapesFlags.visGluedShapesIncoming2D)[0];

            Shape outgoingShape = ToolKit.QueryFlowRelationship(shape, VisGluedShapesFlags.visGluedShapesOutgoing2D)[0];

            Shape[] incomingShapes = ToolKit.ShapeToArray(incomingShape);
            Shape[] outgoingShapes = ToolKit.ShapeToArray(outgoingShape);

            foreach (Shape incoming in incomingShapes)
            {
                foreach (Shape outgoing in outgoingShapes)
                {

                    IOwlNode incomingNode = base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.StringShift(incoming.Text))];
                    IOwlNode outgoingNode = base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.StringShift(outgoing.Text))];

                    string relationship = ToolKit.FlowElementNaming(incoming, shape, outgoing);
                    IOwlNode relationshipNode = new OwlNode(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, relationship));
                    IOwlEdge incomingEdge = new OwlEdge(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "incoming"));
                    IOwlEdge outgoingEdge = new OwlEdge(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "outgoing"));
                    incomingEdge.ChildNode = new OwlLiteral(relationship,"en-us","xsd:string");
                    incomingNode.AttachChildEdge(incomingEdge);

                    outgoingEdge.ChildNode = new OwlLiteral(relationship, "en-us", "xsd:string");
                    outgoingNode.AttachChildEdge(outgoingEdge);

                }
            }

        }

    }
}
