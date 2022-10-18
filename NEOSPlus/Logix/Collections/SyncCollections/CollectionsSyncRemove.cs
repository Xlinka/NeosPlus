using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [HiddenNode]
    [NodeName("SyncRemove")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsSyncRemove<dummy, SyncFieldList<dummy>>))]
    public class CollectionsSyncRemove<T, TU> : LogixNode where TU : ISyncList, IEnumerable<T>
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
                CollectionsHelper<TU, T>.Remove(collection, index);
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
                typeof(CollectionsRemove<,>), typeof(CollectionsSyncRemove<,>));
    }
}