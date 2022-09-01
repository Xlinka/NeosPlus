using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Attach Component")]
    [Category("LogiX/Components")]
    public class AttachComponent : LogixNode
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        [AsOutput]
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Attach()
        {
			Slot slot = Slot.Evaluate();
			if (slot != null)
			{
                slot.AttachComponent(ComponentName.EvaluateRaw());
                OnDone.Trigger();
			}
			else
			{
				OnFail.Trigger();
			}
		}
    }
}