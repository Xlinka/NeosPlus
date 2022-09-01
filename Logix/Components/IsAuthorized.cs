using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Is Authorized")]
    [Category("LogiX/Components")]
    public class IsAuthorized : LogixOperator<bool>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        public static readonly List<Type> BlacklistedTypes = new List<Type>() // Replace with permissions config
        {
            typeof(FinalIK.VRIK),
            typeof(FinalIK.VRIKAvatar)
        };

        public override bool Content
        {
            get
            {
                Slot slot = Slot.Evaluate();
                if (slot == null)
                    return false;
                string compName = ComponentName.EvaluateRaw();
                while (!slot.IsRootSlot)
                {
                    if (slot.GetComponent("FrooxEngine.SlotProtection") != null)
                        return false;
                    slot = slot.Parent;
                }
                Component component = Slot.Evaluate().GetComponent(compName);
                if (BlacklistedTypes.Contains(component.GetType()))
                    return false;
                return true;
            }
        }
    }
}