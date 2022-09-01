using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;

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

        protected override Type FindOverload(NodeTypes connectingTypes)
        {
            var input = connectingTypes.inputs["Collection"];
            var enumerableGeneric =
                input.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    ?.GetGenericArguments()[0];
            return typeof(CollectionsIterate<,>).MakeGenericType(enumerableGeneric, input);
        }
    }
}