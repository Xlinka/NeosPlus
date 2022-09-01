using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Field from Slot")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
    public class GetFieldFromSlot<T> : LogixOperator<IValue<T>>
    {
        public readonly Input<Slot> Slot;
        public readonly Input<string> ComponentName;
        public readonly Input<string> FieldName;

        public override IValue<T> Content
        {
            get
            {
                var comp = Slot.EvaluateRaw().GetComponent(ComponentName.EvaluateRaw());
                if (comp.GetSyncMember(FieldName.EvaluateRaw()) is IValue<T> field)
                    return field;
                return null;
            }
        }
    }
}