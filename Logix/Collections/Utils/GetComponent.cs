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
	[NodeName("Get Component")]
	[Category(new string[] { "LogiX/Slots/Components", "LogiX/Components", "AbcFastGrab" })]
	[NodeDefaultType(typeof(GetComponent<Component>))]
	public class GetComponent<T> : LogixNode, IChangeable, IWorldElement where T:Component
	{
		public readonly Input<Slot> slot;

		public readonly Output<T> Comp;

        protected override void OnInputChange()
		{
			if(slot.Evaluate() is null)
            {
				return;
            }
			T t = slot.Evaluate().GetComponent<T>();
			Comp.Value = t;
		}
	}
}
