using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Set")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsSet<dummy, IList<dummy>>))]
    public class CollectionsSet<T, TU> : LogixNode where TU : IList<T>
    {
        public readonly Input<TU> Collection;
        public readonly Input<int> Index;
        public readonly Input<T> Value;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        protected override string Label => $"Set {typeof(T).GetNiceName()} In {typeof(TU).GetNiceName()}";
        [ImpulseTarget]
        public void Write()
        {
            var collection = Collection.Evaluate();
            var index = Index.EvaluateRaw();
            var value = Value.EvaluateRaw();
            if (collection == null || value == null || index < 0 || index > collection.Count)
            {
                OnFail.Trigger();
                return;
            }
            try
            {
                collection[index] = value;
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
            return typeof(CollectionsInsert<,>).MakeGenericType(enumerableGeneric, input);
        }
    }
}