using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Component")]
    [Category("LogiX/Components")]
    public class GetComponent : LogixOperator<Component>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        public static readonly List<Type> BlacklistedTypes = new List<Type>()
        {
            typeof(FinalIK.VRIK),
            typeof(FinalIK.VRIKAvatar)
        };

        public override Component Content
        {
            get
            {
                Component component = Slot.EvaluateRaw().GetComponent(ComponentName.EvaluateRaw());
                if (BlacklistedTypes.Contains(component.GetType()))
                    return null;
                return component;
            }
        }
    }
}