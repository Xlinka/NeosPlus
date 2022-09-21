using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [HiddenNode]
    [NodeName("SyncAppend")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsSyncAppend<dummy, SyncFieldList<dummy>>))]
    public class CollectionsSyncAppend<T, TU> : LogixNode where TU : ISyncList, IEnumerable<T>
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
                CollectionsHelper<TU, T>.Append(collection, value);
            }
            catch
            {
                OnFail.Trigger();
                return;
            }
            OnDone.Trigger();
        }
        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsSyncOverload(connectingTypes, "Collection", typeof(ICollection<>),
                typeof(CollectionsAppend<,>), typeof(CollectionsSyncAppend<,>));
    }
}