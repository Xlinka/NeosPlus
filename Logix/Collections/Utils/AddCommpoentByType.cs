using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;

namespace FrooxEngine.Logix.Collections.Utils
{
	[NodeName("Add Component")]
	[Category(new string[] { "LogiX/Components"})]
	public class AddComponentByType : LogixNode, IChangeable, IWorldElement
	{
		public readonly Input<Slot> slot;
		
		public readonly Input<Type> CompType;

		public readonly Impulse Added;

		public readonly Output<Component> Comp;

		[ImpulseTarget]
		public void Add()
		{
			if(slot.Evaluate() is null)
            {
				return;
            }
            var t = slot.Evaluate().AttachComponent(CompType.EvaluateRaw());
			Comp.Value = t;
			NotifyOutputsOfChange();
			Added.Trigger();
		}
	
	}
}
