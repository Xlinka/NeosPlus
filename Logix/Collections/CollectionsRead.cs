using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Collections
{
    [NodeName("Read Collection")]
    [Category("LogiX/Collections")]
    [NodeDefaultType(typeof(CollectionsRead<dummy, IEnumerable<dummy>>))]
    public class CollectionsRead<T, TU> : LogixOperator<T> where TU : IEnumerable<T>
    {
        public readonly Input<TU> Input;
        public readonly Input<int> Index;
        protected override string Label => $"Read {typeof(TU).GetNiceName()}";
        public override T Content
        {
            get
            {
                var enumerable = Input.EvaluateRaw();
                var index = Index.EvaluateRaw();
                if (enumerable == null || index < 0) return default;
                var array = enumerable.ToArray();
                if (array.Length == 0 || index > array.Length) return default;
                return array[index];
            }
        }
        protected override Type FindOverload(NodeTypes connectingTypes)
        {
            var input = connectingTypes.inputs["Input"];
            var interfaces = input.GetInterfaces();
            var enumerableGeneric =
                interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    ?.GetGenericArguments()[0];
            return typeof(CollectionsRead<,>).MakeGenericType(enumerableGeneric, input);
        }
    }
}