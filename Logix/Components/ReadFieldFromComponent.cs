using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Read Field from Component")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
    public class ReadFieldFromComponent<T> : LogixOperator<T>
    {
        public readonly Input<Component> Component;
        public readonly Input<string> ComponentName;
        public readonly Input<string> FieldName;

        public override T Content
        {
            get
            {
                var comp = Component.Evaluate();
                if (comp.GetSyncMember(FieldName.EvaluateRaw()) is IValue<T> field)
                    return field.Value;
                return default(T);
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}