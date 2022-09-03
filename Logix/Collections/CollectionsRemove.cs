using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Remove")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsRemove<dummy, IList<dummy>>))]
    public class CollectionsRemove<T, TU> : LogixNode where TU : IList<T>
    {
        public readonly Input<TU> Collection;
        public readonly Input<int> Index;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        protected override string Label => $"Remove {typeof(T).GetNiceName()} From {typeof(TU).GetNiceName()}";
        [ImpulseTarget]
        public void Remove()
        {
            var collection = Collection.Evaluate();
            var index = Index.EvaluateRaw();
            if (collection == null || index < 0 || index > collection.Count)
            {
                OnFail.Trigger();
                return;
            }
            try
            {
                collection.RemoveAt(index);
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
                input.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>))
                    ?.GetGenericArguments()[0];
            return typeof(CollectionsRemove<,>).MakeGenericType(enumerableGeneric, input);
        }
    }
}