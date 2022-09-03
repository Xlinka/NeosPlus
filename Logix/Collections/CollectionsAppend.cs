using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Append")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsAppend<dummy, ICollection<dummy>>))]
    public class CollectionsAppend<T, TU> : LogixNode where TU : ICollection<T>
    {
        public readonly Input<TU> Collection;
        public readonly Input<T> Value;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        protected override string Label => $"Append {typeof(T).GetNiceName()} To {typeof(TU).GetNiceName()}";
        [ImpulseTarget]
        public void Append()
        {
            var collection = Collection.EvaluateRaw();
            var value = Value.EvaluateRaw();
            if (collection == null || value == null)
            {
                OnFail.Trigger();
                return;
            }
            try
            {
                collection.Add(value);
            }
            catch
            {
                OnFail.Trigger();
                return;
            }
            OnDone.Trigger();
        }
        protected override Type FindOverload(NodeTypes connectingTypes)
        {
            var input = connectingTypes.inputs["Collection"];
            var enumerableGeneric =
                input.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    ?.GetGenericArguments()[0];
            return typeof(CollectionsAppend<,>).MakeGenericType(enumerableGeneric, input);
        }
    }
}