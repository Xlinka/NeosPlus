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
	[NodeName("Get Components In Children")]
	[Category(new string[] { "LogiX/Slots/Components", "LogiX/Components", "AbcFastGrab" })]
	[NodeDefaultType(typeof(GetComponentsInChildren<Component>))]
	public class GetComponentsInChildren<T> : LogixOperator<ArrayX<T>>, IValue<ArrayX<T>>, IChangeable, IWorldElement where T:Component
	{
		public readonly Input<Slot> slot;

		public readonly Impulse Loaded;

		public readonly RefArrayX<T> Value;

		public override ArrayX<T> Content => this.Value;
		ArrayX<T> IValue<ArrayX<T>>.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				if (value != null)
				{
					this.Value.Copy(value);
				}
				else
				{
					this.Value.Clear();
				}
			}
		}

		[ImpulseTarget]
		public void Load()
		{
			if(slot.Evaluate() is null)
            {
				return;
            }
			List<T> t = slot.Evaluate().GetComponentsInChildren<T>();
			Value.Clear();
			foreach (var item in t)
            {
				Value.XAdd(item);
            }
			Loaded.Trigger();
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
