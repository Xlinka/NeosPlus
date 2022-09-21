using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [HiddenNode]
    [NodeName("SyncInsert")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsSyncInsert<dummy, SyncFieldList<dummy>>))]
    public class CollectionsSyncInsert<T, TU> : LogixNode where TU : ISyncList, IEnumerable<T>
    {
        public readonly Input<TU> Collection;
        public readonly Input<int> Index;
        public readonly Input<T> Value;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        protected override string Label => $"Insert {typeof(T).GetNiceName()} To {typeof(TU).GetNiceName()}";

        [ImpulseTarget]
        public void Insert()
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
                CollectionsHelper<TU, T>.Insert(collection, index, value);
            }
            catch
            {
                OnFail.Trigger();
                return;
            }

            OnDone.Trigger();
        }
        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsSyncOverload(connectingTypes, "Collection", typeof(IList<>),
                typeof(CollectionsInsert<,>), typeof(CollectionsSyncInsert<,>));
    }
}