using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram
{
    public class LaneFactory : BaseFactory
    {

        public LaneFactory(Page page, IOwlGraph graph)
            : base(page, graph)
        {

        }

        public override IOwlIndividual Create(Shape shape)
        {
            IOwlIndividual individual = new OwlIndividual(
                ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, ToolKit.StringShift(shape.Text)),
                (OwlNode)base._graph.Nodes[ToolKit.GetFullName(Constant.BPMN_TARGET_NAMESPACE, "lane")]);

            individual = base.BuildProperty(shape, individual);

            return individual;
        }

    }
}
