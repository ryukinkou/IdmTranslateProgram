using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram.Factory
{
    public class SequenceFlowFactory : FlowFactory
    {

        public SequenceFlowFactory(Page page, IOwlGraph graph)
            : base(page, graph)
        {

        }

        public override IOwlIndividual Create(Shape shape)
        {
            IOwlIndividual individual = new OwlIndividual(
                ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.FlowElementNaming(shape)),
                (OwlNode)base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "sequenceFlow")]);

            individual = base.BuildProperty(shape, individual);

            return individual;
        }

        public override void BuildRelationship(Shape shape)
        {
            //ToolKit.SysoutFlowRelationship(shape);
        }

    }
}
