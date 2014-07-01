using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram
{
    public class GatewayFactory
    {

        private Shape _shape;
        private IOwlGraph _graph;
        private IOwlClass _gatewayClass;
        private IOwlEdge _hasGatewayDirection;

        public GatewayFactory(IOwlGraph graph)
        {
            this._graph = graph;
            this._gatewayClass = (IOwlClass)graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#gateway"];
            this._hasGatewayDirection = new OwlEdge(Constant.BPMN_TARGET_NAMESPACE + "#hasGatewayDirection");
        }

        public IOwlIndividual CreateGateway(Shape shape)
        {
            this._shape = shape;
            IOwlIndividual gateway = new OwlIndividual(Constant.BPMN_TARGET_NAMESPACE + "#" + ToolKit.StringShift(this._shape.Text), (OwlNode)this._gatewayClass);
            gateway = this.calculateGatewayDirection(gateway);
            return gateway;

        }

        /* 
         * formal Table 8.46
         * Unspecified: There are no constraints. The Gateway MAY have any number of incoming and outgoing Sequence Flows.
         * Converging: This Gateway MAY have multiple incoming Sequence Flows but MUST have no more than one (1) outgoing Sequence Flow.
         * Diverging: This Gateway MAY have multiple outgoing Sequence Flows but MUST have no more than one (1) incoming Sequence Flow.
         * Mixed: This Gateway contains multiple outgoing and multiple incoming Sequence Flows.
         */
        private IOwlIndividual calculateGatewayDirection(IOwlIndividual gateway)
        {
            int incomingNumber = this._shape.GluedShapes(VisGluedShapesFlags.visGluedShapesIncoming1D, "", null).Length;
            int outgoingNumber = this._shape.GluedShapes(VisGluedShapesFlags.visGluedShapesOutgoing1D, "", null).Length;

            OwlIndividual gatewayDirection;

            if (incomingNumber > 1 && outgoingNumber <= 1)
            {
                //Converging
                gatewayDirection = (OwlIndividual)this._graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#Converging"];

            }
            else if (incomingNumber <= 1 && outgoingNumber > 1)
            {
                //Diverging
                gatewayDirection = (OwlIndividual)this._graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#Diverging"];
            }
            else if (incomingNumber > 1 && outgoingNumber > 1)
            {
                //Mixed
                gatewayDirection = (OwlIndividual)this._graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#Mixed"];
            }
            else
            {
                //Unspecified
                gatewayDirection = (OwlIndividual)this._graph.Nodes[Constant.BPMN_TARGET_NAMESPACE + "#Unspecified"];
                
            }

            this._hasGatewayDirection.ChildNode = gatewayDirection;

            gateway.AttachChildEdge(this._hasGatewayDirection);

            return gateway;

        }

    }
}
