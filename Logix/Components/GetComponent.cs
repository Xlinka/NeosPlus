﻿using System;
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

        public static readonly List<Type> BlacklistedTypes = new List<Type>() // Replace with permissions config
        {
            typeof(FinalIK.VRIK),
            typeof(FinalIK.VRIKAvatar)
        };

        public override Component Content
        {
            get
            {
                Slot slot = Slot.Evaluate();
                if (slot == null)
                    return null;
                Slot search = slot;
                string compName = ComponentName.EvaluateRaw();
                while (!search.IsRootSlot)
                {
                    if (search.GetComponent("FrooxEngine.SlotProtection") != null)
                        return null;
                    search = search.Parent;
                }
                Component component = slot.GetComponent(compName);
                if (BlacklistedTypes.Contains(component.GetType()))
                    return null;
                return component;
            }
        }
    }
}