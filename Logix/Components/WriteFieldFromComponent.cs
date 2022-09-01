using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

namespace FrooxEngine.LogiX.Components
{
	[NodeName("Write Field from Component")]
	[Category("LogiX/Components")]
	[GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
	public class WriteFieldFromComponent<T> : LogixNode
	{
		public readonly Input<Component> Component;
		public readonly Input<string> FieldName;
		public readonly Input<T> Value;

		[AsOutput]
		public readonly Impulse OnDone;
		public readonly Impulse OnFail;

		[ImpulseTarget]
		public void Write()
		{
			var comp = Component.Evaluate();
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
	}
}