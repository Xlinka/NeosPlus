using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Remove Component")]
    [Category("LogiX/Components")]
    public class RemoveComponent : LogixNode
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        [AsOutput]
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Remove()
        {
            Slot slot = Slot.Evaluate();
            if (slot != null)
            {
                string name = ComponentName.EvaluateRaw();
                Component component = slot.GetComponent(name);
                slot.RemoveComponent(component);
                OnDone.Trigger();
            }
            else
            {
                OnFail.Trigger();
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}