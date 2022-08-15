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
	[NodeName("Get Child Recursive Slots")]
	[Category(new string[] {  "LogiX/Slots" })]
	public class GetChildRecursiveSlots : LogixOperator<ArrayX<Slot>>, IValue<ArrayX<Slot>>, IChangeable, IWorldElement
	{
		public readonly Input<Slot> slot;

		public readonly Impulse Loaded;

		public readonly RefArrayX<Slot> Value;

		public override ArrayX<Slot> Content => this.Value;
		ArrayX<Slot> IValue<ArrayX<Slot>>.Value
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
			Value.Clear();
			List<Slot> t = slot.Evaluate().GetAllChildren();
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
