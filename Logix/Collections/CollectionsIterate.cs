using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Foreach")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsIterate<dummy, IEnumerable<dummy>>))]
    public class CollectionsIterate<T, TU>  : LogixNode where TU : IEnumerable<T>
    {
        public readonly Impulse LoopStart;
        public readonly Impulse LoopIteration;
        public readonly Impulse LoopEnd;
        public readonly Output<T> Iteration;
        public readonly Input<TU> Collection;
        //public readonly Input<bool> Reverse;
        protected override string Label => $"Foreach {typeof(T).GetNiceName()} In {typeof(TU).GetNiceName()}";
        
        [ImpulseTarget]
        public void Run()
        {
            if (!Enabled) return;
            LoopStart.Trigger();
            var collection = Collection.EvaluateRaw();
            if (collection == null)
            {
                LoopEnd.Trigger();
                return;
            }
            foreach (var iteration in collection)
            {
                Iteration.Value = iteration;
                LoopIteration.Trigger();
                if (Logix.ImpulsesBlocked) break;
            }
            Iteration.Value = default;
            LoopEnd.Trigger();
        }

        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsOverload(connectingTypes, "Collection", typeof(IEnumerable<>),
                typeof(CollectionsIterate<,>));
    }
}