using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;
using BaseX;
using FrooxEngine.CommonAvatar;


namespace FrooxEngine.LogiX.Slots
{
    [Category("LogiX/Slots")]
    [NodeName("Get Nth Parent")]
    public class GetNthParent: LogixOperator<Slot>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<int> N;
        public override Slot Content
        {
            get
            {
                Slot slot = Slot.Evaluate();
                if (slot == null)
                    return null;
                int n = N.EvaluateRaw();
                if (n < 0)
                    n = 0;
                if (n > 256)
                    n = 256;
                for (int i = 0; i < n; i++)
                {
                    slot = slot.Parent;
                    if (slot == null)
                        return World?.RootSlot;
                }
                return slot;
            }
        }
        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}