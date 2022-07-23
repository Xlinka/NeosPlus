using System;
using System.Linq;
using BaseX;
using FrooxEngine.UIX;
//Credit to FroZen https://github.com/Neos-Metaverse/NeosPublic/issues/3703
//Adds Primative Sort Node
namespace FrooxEngine.LogiX.Operators
{
    [NodeName("Sort")]
    [Category("LogiX/Math")]
    [NodeDefaultType(typeof(Sort<float>))]
    public class Sort<T> : LogixNode where T : IComparable
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
            inputs.Sort();
            if (Backward.EvaluateRaw()) inputs.Reverse();
            for (var i = 0; i < ValueOutputs.Count; i++) ValueOutputs[i].Value = inputs[i];
        }
        protected override void OnGenerateVisual(Slot root)
        {
            var uIBuilder = GenerateUI(root);
            uIBuilder.Panel();
            uIBuilder.HorizontalFooter(32f, out var footer, out var _);
            var uIBuilder2 = new UIBuilder(footer);
            uIBuilder2.HorizontalLayout(4f);
            LocaleString text = "+";
            var tint = color.White;
            uIBuilder2.Button(in text, in tint, Add);
            text = "-";
            tint = color.White;
            uIBuilder2.Button(in text, in tint, Remove);
        }
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
    }
}