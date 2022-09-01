﻿using System;
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
        public static readonly List<Type> BlacklistedTypes = new List<Type>() // Replace with permissions config
        {
            typeof(FinalIK.VRIK),
            typeof(FinalIK.VRIKAvatar)
        };

        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;

        [AsOutput]
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Attach()
        {
            Slot slot = Slot.Evaluate();
            if (slot == null)
                return null;
            Slot search = slot;
            string compName = ComponentName.EvaluateRaw();
            while (!search.IsRootSlot)
            {
                if (search.GetComponent("FrooxEngine.SlotProtection") != null)
                {
                    OnFail.Trigger();
                    return;
                }
                search = search.Parent;
            }
            Component component = slot.GetComponent(compName);
            if (BlacklistedTypes.Contains(component.GetType()))
            {
                OnFail.Trigger();
                return;
            }
            slot.AttachComponent(compName);
            OnDone.Trigger();
		}
    }
}