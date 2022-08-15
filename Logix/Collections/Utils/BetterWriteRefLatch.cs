using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;
using BaseX;

namespace FrooxEngine.Logix.Collections.Utils
{
	[Category(new string[] { "LogiX/BetterActions" })]
	[NodeName("Write Ref Latch")]
	[NodeDefaultType(typeof(BetterWriteRefLatch<IWorldElement>))]
	public class BetterWriteRefLatch<T> : LogixNode where T : class, IWorldElement
	{
		public readonly Input<T> SetValue;

		public readonly Input<T> ResetValue;

		public readonly Impulse OnSet;

		public readonly Impulse OnReset;

		[AsOutput]
		public readonly Input<ISyncRef> Target;

		[ImpulseTarget]
		public void Set()
		{
			DoSet(SetValue);
			OnSet.Trigger();
		}

		[ImpulseTarget]
		public void Reset()
		{
			DoSet(ResetValue);
			OnReset.Trigger();
		}

		private void DoSet(Input<T> source)
		{
			ISyncRef syncRef = Target.Evaluate();
			if (syncRef != null)
			{
				syncRef.Target = source.Evaluate();
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("Target", out var value))
			{
				Type type = value.FindGenericBaseClass(typeof(SyncRef<>));
				if (type != null)
				{
					return typeof(BetterWriteRefLatch<>).MakeGenericType(type.GetGenericArguments()[0]);
				}
			}
			if (connectingTypes.inputs.TryGetValue("SetValue", out value) && typeof(IWorldElement).IsAssignableFrom(value))
			{
				return typeof(BetterWriteRefLatch<>).MakeGenericType(value);
			}
			if (connectingTypes.inputs.TryGetValue("ResetValue", out value) && typeof(IWorldElement).IsAssignableFrom(value))
			{
				return typeof(BetterWriteRefLatch<>).MakeGenericType(value);
			}
			return null;
		}

	}
}
