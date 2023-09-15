using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using NEOSPlus;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Count")]
    [Category("LogiX/NeosPlus/Collections")]
    [NodeDefaultType(typeof(CollectionsCount<dummy, IEnumerable<dummy>>))]
    public class CollectionsCount<T, TU> : LogixOperator<int> where TU : IEnumerable<T>
    {
        public readonly Input<TU> Collection;
        protected override string Label => $"Count Collection {typeof(TU).GetNiceName()}";

        public override int Content
        {
            get
            {
                var enumerable = Collection.EvaluateRaw();
                return enumerable == null ? 0 : enumerable.Count();
            }
        }

        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsOverload(connectingTypes, "Collection", typeof(IEnumerable<>),
                typeof(CollectionsCount<,>));
    }
}