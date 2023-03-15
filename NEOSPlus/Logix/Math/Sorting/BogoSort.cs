using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Operators;
using FrooxEngine.UIX;
using NEOSPlus;
using Random = System.Random;

namespace FrooxEngine.Logix.Math.Sorting
{
    [NodeName("BogoSort")]
    [Category("LogiX/Math")]
    [NodeDefaultType(typeof(BogoSort<float>))]
    public class BogoSort<T> : LogixNode where T : IComparable
    {
        public readonly SyncList<Input<T>> ValueInputs;
        public readonly Input<bool> Backward;
        public readonly SyncList<Output<T>> ValueOutputs;

        protected override void OnAttach()
        {
            base.OnAttach();
            for (var i = 0; i < 2; i++)
            {
                ValueOutputs.Add();
                ValueInputs.Add();
            }
        }

        protected override void OnEvaluate()
        {
            var inputs = ValueInputs.Select(t => t.EvaluateRaw()).ToList();
            inputs = BogoSortAlgorithm(inputs);
            if (Backward.EvaluateRaw()) inputs.Reverse();
            for (var i = 0; i < ValueOutputs.Count; i++) ValueOutputs[i].Value = inputs[i];
        }

        protected override void OnGenerateVisual(Slot root) =>
            GenerateUI(root).GenerateListButtons(Add, Remove);

        protected override Type FindOverload(NodeTypes connectingTypes) =>
            (from input in connectingTypes.inputs
             where input.Key.StartsWith("ValueInputs") &&
                   (typeof(T).GetTypeCastCompatibility(input.Value) == TypeCastCompatibility.Implicit ||
                    ValueInputs.All(i => !i.IsConnected))
             select typeof(BogoSort<>).MakeGenericType(input.Value)).FirstOrDefault();

        [SyncMethod]
        private void Add(IButton button, ButtonEventData eventData)
        {
            ValueInputs.Add();
            ValueOutputs.Add();
            RefreshLogixBox();
        }

        [SyncMethod]
        private void Remove(IButton button, ButtonEventData eventData)
        {
            if (ValueInputs.Count <= 2) return;
            ValueInputs.RemoveAt(ValueInputs.Count - 1);
            ValueOutputs.RemoveAt(ValueOutputs.Count - 1);
            RefreshLogixBox();
        }

        private List<T> BogoSortAlgorithm(List<T> list)
        {
            Random random = new Random();
            while (!IsSorted(list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int randomIndex = random.Next(list.Count);
                    T temp = list[i];
                    list[i] = list[randomIndex];
                    list[randomIndex] = temp;
                }
            }

            return list;
        }

        private bool IsSorted(List<T> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].CompareTo(list[i]) > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}