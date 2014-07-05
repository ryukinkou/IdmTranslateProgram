using Microsoft.Office.Interop.Visio;
using OwlDotNetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdmProgressMapTranslateProgram.Factory
{
    public abstract class FlowFactory : BaseFactory
    {

       public FlowFactory(Page page, IOwlGraph graph)
            : base(page, graph)
        {

        }

        public abstract void BuildRelationship(Shape shape);

    }
}
