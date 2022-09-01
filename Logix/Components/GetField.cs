using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Field")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitives, typeof(RefID))]
    public class GetField<T> : LogixOperator<IValue<T>>
    {
        public readonly Input<Component> Component;
        public readonly Input<string> FieldName;

        public override IValue<T> Content
        {
            get
            {
                var comp = Component.EvaluateRaw();
                if (comp.GetSyncMember(FieldName.EvaluateRaw()) is IValue<T> field)
                    return field;
                return null;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}