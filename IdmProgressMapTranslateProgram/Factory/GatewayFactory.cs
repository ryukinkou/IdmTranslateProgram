using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;

namespace IdmProgressMapTranslateProgram
{
    public class GatewayFactory : BaseFactory
    {

        public GatewayFactory(Page page, IOwlGraph graph)
            : base(page, graph)
        {

        }

        public override IOwlIndividual Create(Shape shape)
        {
            IOwlIndividual individual = new OwlIndividual(
                ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.StringShift(shape.Text)),
                (OwlNode)base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "gateway")]);

            individual = this.CalculateGatewayDirection(shape, individual);
            individual = this.BuildFlowRelationship(shape, individual);
            individual = this.BuildProperty(shape, individual);

            return individual;
        }

        private IOwlIndividual CalculateGatewayDirection(Shape shape, IOwlIndividual individual)
        {

            /* 
             * formal Table 8.46
             * Unspecified: There are no constraints. The Gateway MAY have any number of incoming and outgoing Sequence Flows.
             * Converging: This Gateway MAY have multiple incoming Sequence Flows but MUST have no more than one (1) outgoing Sequence Flow.
             * Diverging: This Gateway MAY have multiple outgoing Sequence Flows but MUST have no more than one (1) incoming Sequence Flow.
             * Mixed: This Gateway contains multiple outgoing and multiple incoming Sequence Flows.
            */

            int incomingNumber = shape.GluedShapes(VisGluedShapesFlags.visGluedShapesIncoming1D, "", null).Length;
            int outgoingNumber = shape.GluedShapes(VisGluedShapesFlags.visGluedShapesOutgoing1D, "", null).Length;

            OwlIndividual gatewayDirection;

            if (incomingNumber > 1 && outgoingNumber <= 1)
            {
                //Converging
                gatewayDirection = (OwlIndividual)this._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "Converging")];

            }
            else if (incomingNumber <= 1 && outgoingNumber > 1)
            {
                //Diverging
                gatewayDirection = (OwlIndividual)this._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "Diverging")];
            }
            else if (incomingNumber > 1 && outgoingNumber > 1)
            {
                //Mixed
                gatewayDirection = (OwlIndividual)this._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "Mixed")];
            }
            else
            {
                //Unspecified
                gatewayDirection = (OwlIndividual)this._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "Unspecified")];

            }

            IOwlEdge hasGatewayDirection = new OwlEdge(ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "hasGatewayDirection"));
            hasGatewayDirection.ChildNode = gatewayDirection;

            individual.AttachChildEdge(hasGatewayDirection);

            return individual;

        }

        private IOwlIndividual BuildFlowRelationship(Shape shape, IOwlIndividual individual)
        {

            Array incomingFlow = shape.GluedShapes(VisGluedShapesFlags.visGluedShapesIncoming1D, "", null);

            foreach (int id in incomingFlow)
            {
                Shape flow = this._page.Shapes.get_ItemFromID(id);
                //Console.WriteLine(ToolKit.FlowElementNaming(this._page, flow));
                //TODO build relationship
            }

            Array outgoingFlow = shape.GluedShapes(VisGluedShapesFlags.visGluedShapesOutgoing1D, "", null);

            foreach (int ID in outgoingFlow)
            {
                Shape flow = this._page.Shapes.get_ItemFromID(ID);
                //Console.WriteLine(ToolKit.FlowElementNaming(this._page, flow));
                //TODO build relationship
            }

            return individual;
        }

        private new IOwlIndividual BuildProperty(Shape shape, IOwlIndividual individual)
        {
            return base.BuildProperty(shape, individual);
        }

    }
}
