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
    [NodeDefaultType(typeof(GetValue<float>))]
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

        protected override Type FindOverload(NodeTypes connectingTypes)
        {
            if (connectingTypes.inputs.TryGetValue("Field", out var value))
            {
                Type type = value.EnumerateInterfacesRecursively().FirstOrDefault((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValue<>));
                if (type != null)
                {
                    return typeof(GetValue<>).MakeGenericType(type.GetGenericArguments()[0]);
                }
                return null;
            }
            return null;
        }
    }
}