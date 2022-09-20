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
    [NodeName("Get")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsGet<dummy, IEnumerable<dummy>>))]
    public class CollectionsGet<T, TU> : LogixOperator<T> where TU : IEnumerable<T>
    {
        public readonly Input<TU> Collection;
        public readonly Input<int> Index;
        protected override string Label => $"Get {typeof(T).GetNiceName()} From {typeof(TU).GetNiceName()}";

        public override T Content
        {
            get
            {
                var enumerable = Collection.EvaluateRaw();
                var index = Index.EvaluateRaw();
                if (enumerable == null || index < 0) return default;
                var array = enumerable.ToArray();
                if (array.Length == 0 || index > array.Length) return default;
                return array[index];
            }
        }

        protected override Type FindOverload(NodeTypes connectingTypes) =>
            NodeExtensions.CollectionsOverload(connectingTypes, "Collection", typeof(IEnumerable<>),
                typeof(CollectionsGet<,>));
    }
}