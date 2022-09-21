using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Clear")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsClear<Sync<dummy>, SyncFieldList<dummy>>))]
    public class CollectionsClear<T, TU> : LogixNode where TU : SyncElementList<T> where T : class, ISyncMember, new()
    {
        public readonly Input<TU> Collection;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        protected override string Label => $"Remove {typeof(T).GetNiceName()} From {typeof(TU).GetNiceName()}";

        [ImpulseTarget]
        public void Remove()
        {
            var collection = Collection.Evaluate();
            if (collection == null)
            {
                OnFail.Trigger();
                return;
            }
            try
            {
                collection.Clear();
            }
            catch
            {
                OnFail.Trigger();
                return;
            }
            OnDone.Trigger();
        }
        
        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsOverload(connectingTypes, "Collection", typeof(SyncElementList<>),
                typeof(CollectionsClear<,>));
    }
}