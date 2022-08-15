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
	[NodeName("Duplicate Component")]
	[Category(new string[] { "LogiX/Slots/Components", "LogiX/Components", "AbcFastGrab" })]
	[NodeDefaultType(typeof(DuplicateComponent<Component>))]
	public class DuplicateComponent<T> : LogixNode, IChangeable, IWorldElement where T:Component 
	{
		public readonly Input<Slot> slot;

		public readonly Input<T> from;

		public readonly Impulse Dupped;

		public readonly Output<T> Comp;

		[ImpulseTarget]
		public void Dup()
		{
			if(slot.Evaluate() is null)
            {
				return;
            }
			if (from.Evaluate() is null)
			{
				return;
			}
			T t = slot.Evaluate().DuplicateComponent(from.Evaluate());
			Comp.Value = t;
			Dupped.Trigger();
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("from", out var value))
			{
				return typeof(DuplicateComponent<>).MakeGenericType(value);	
			}
			return null;
		}
	}
}
