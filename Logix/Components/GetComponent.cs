using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Component")]
    [Category("LogiX/Components")]
    public class GetComponent : LogixOperator<Component>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        public override Component Content
        {
            get
            {
                return Slot.EvaluateRaw().GetComponent(ComponentName.EvaluateRaw());
            }
        }
    }
}