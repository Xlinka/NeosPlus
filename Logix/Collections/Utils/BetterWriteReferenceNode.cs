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
	[NodeName("Write Ref")]
	[Category(new string[] { "LogiX/BetterActions"})]
	[NodeDefaultType(typeof(BetterWriteReferenceNode<IWorldElement>))]
	public class BetterWriteReferenceNode<T> : LogixNode where T : class, IWorldElement
	{
		public readonly Input<T> Target;

		[AsOutput]
		public readonly Input<ISyncRef> Reference;

		public readonly Impulse OnDone;

		public readonly Impulse OnFail;

		[ImpulseTarget]
		public void Write()
		{
			ISyncRef syncRef = Reference.Evaluate();
			if (syncRef != null)
			{
				ISyncMember syncMember = syncRef;
				if (syncMember != null && syncMember.IsDriven && !syncMember.IsHooked)
				{
					OnFail.Trigger();
					return;
				}
				syncRef.Target = Target.Evaluate();
				OnDone.Trigger();
			}
			else
			{
				OnFail.Trigger();
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("Reference", out var value))
			{
				Type type = value.FindGenericBaseClass(typeof(SyncRef<>));
				if (type != null)
				{
					return typeof(BetterWriteReferenceNode<>).MakeGenericType(type.GetGenericArguments()[0]);
				}
			}
			if (connectingTypes.inputs.TryGetValue("Value", out value) && value.IsClass && typeof(IWorldElement).IsAssignableFrom(value))
			{
				return typeof(BetterWriteReferenceNode<>).MakeGenericType(value);
			}
			return null;
		}

	}
}
