using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Value")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
    public class GetValue<T> : LogixOperator<T>
    {
        public readonly Input<IValue<T>> Field;

        public override T Content
        {
            get
            {
                var field = Field.EvaluateRaw();
                if (field != null)
                    return field.Value;
                return default(T);
            }
        }
    }
}