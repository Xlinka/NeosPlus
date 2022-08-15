using BaseX;
using NEOSPlus;
using System.Linq;

namespace FrooxEngine.LogiX.Math
{
    // node for https://github.com/Neos-Metaverse/NeosPublic/issues/3702    
    // needs different typings... Frozen help...
    [NodeName("Min/Max")]
    [Category("LogiX/Math")]
    [NodeDefaultType(typeof(Sort<float>))]

    public class MixMax : LogixNode 
    {
        public readonly SyncList<Input<float>> ValueInputs;
        public readonly Output<float> Min;
        public readonly Output<float> Max;

        protected override void OnAttach()
        {
            base.OnAttach();
            for (var i = 0; i < 2; i++)
            {
                ValueInputs.Add();
            }
        }

        float min = 0;
        float max = 0;

        protected override void OnEvaluate()
        {
            // I don't know but it works
            var inputs = ValueInputs.Select(t => t.EvaluateRaw()).ToArray();

            min = ValueInputs[0].EvaluateRaw(0f);
            for (int i = 0; i < ValueInputs.Count; i++)
            {
                float input = ValueInputs[i].EvaluateRaw(0f);
                Min.Value = min = MathX.Min(min, input);
                Max.Value = max = MathX.Max(max, input);
            }
        }
        protected override void OnGenerateVisual(Slot root) => GenerateUI(root).GenerateListButtons(Add, Remove);

        [SyncMethod]
        private void Add(IButton button, ButtonEventData eventData)
        {
            ValueInputs.Add();
            RefreshLogixBox();
        }
        [SyncMethod]
        private void Remove(IButton button, ButtonEventData eventData)
        {
            if (ValueInputs.Count <= 2) return;
            ValueInputs.RemoveAt(ValueInputs.Count - 1);
            RefreshLogixBox();
        }
    }
}
