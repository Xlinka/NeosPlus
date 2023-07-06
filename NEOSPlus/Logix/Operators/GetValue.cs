using BaseX;
using System;
using System.Linq;

namespace FrooxEngine.LogiX.Operators
{
    /// <summary>
    /// Retrieves the value of any <c>IValue&lt;T\&gt;</c> data type.
    /// </summary>
    /// <remarks>
    /// By JackTheFoxOtter
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    [Category("LogiX/Operators")]
    [NodeName("Get Value")]
    public class GetValue<T> : LogixOperator<T>
    {
        public readonly Input<IValue<T>> Target;
        public override T Content
        {
            get
            {
                IValue<T> evaluatedTarget = Target.EvaluateRaw();
                if (evaluatedTarget != null)
                    return evaluatedTarget.Value;
                return default;
            }
        }

        protected override Type FindOverload(NodeTypes connectingTypes)
        {
            if (connectingTypes.inputs.TryGetValue("Target", out Type targetType))
            {
                Type targetType2 = targetType.EnumerateInterfacesRecursively().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValue<>));
                if (targetType2 != null)
                    return typeof(GetValue<>).MakeGenericType(targetType2.GetGenericArguments()[0]);
            }
            return null;
        }
    }
}