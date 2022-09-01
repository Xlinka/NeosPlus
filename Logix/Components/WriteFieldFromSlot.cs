using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Write Field from Slot")]
    [Category("LogiX/Components")]
    public class WriteFieldFromSlot<T> : LogixNode
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;
		public readonly Input<string> FieldName;
		public readonly Input<T> Value;

		[AsOutput]
		public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
		public void Write()
		{
			var comp = Slot.EvaluateRaw().GetComponent(ComponentName.EvaluateRaw());
			if (!(comp.GetSyncMember(FieldName.EvaluateRaw()) is IValue<T> value))
            {
				OnFail.Trigger();
				return;
			}
			if (value != null)
			{
				if (value is ISyncMember syncMember && syncMember.IsDriven && !syncMember.IsHooked)
				{
					OnFail.Trigger();
					return;
				}
				if (value is ConflictingSyncElement conflictingSyncElement && conflictingSyncElement.DirectAccessOnly)
				{
					OnFail.Trigger();
					return;
				}
				value.Value = Value.Evaluate();
				OnDone.Trigger();
			}
			else
			{
				OnFail.Trigger();
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("Target", out var value))
			{
				Type type = value.EnumerateInterfacesRecursively().FirstOrDefault((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValue<>));
				if (type != null)
				{
					return typeof(WriteFieldFromSlot<>).MakeGenericType(type.GetGenericArguments()[0]);
				}
				return null;
			}
			if (connectingTypes.inputs.TryGetValue("Value", out value))
			{
				return typeof(WriteFieldFromSlot<>).MakeGenericType(value);
			}
			return null;
		}

		protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}