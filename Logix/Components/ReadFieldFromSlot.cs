using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Read Field from Slot")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
    public class ReadFieldFromSlot<T> : LogixOperator<T>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;
        public readonly Input<string> FieldName;

        public override T Content
        {
            get
            {
                var comp = Slot.EvaluateRaw().GetComponent(ComponentName.EvaluateRaw());
                if (comp.GetSyncMember(FieldName.EvaluateRaw()) is IValue<T> field)
                    return field.Value;
                return default(T);
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}