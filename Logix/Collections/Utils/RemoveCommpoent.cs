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
	[NodeName("Remove Component")]
	[Category(new string[] { "LogiX/Components"})]
	public class RemoveComponent : LogixNode, IChangeable, IWorldElement
	{
		public readonly Input<Component> comp;

		public readonly Impulse Removed;

		[ImpulseTarget]
		public void Remove()
		{
			if(comp.Evaluate() is null)
            {
				return;
            }
			comp.Evaluate().Destroy();
			Removed.Trigger();
		}
		
	}
}
