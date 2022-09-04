using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class MeshInput : LogixOperator<IAssetProvider<Mesh>>
    {
        public readonly AssetRef<Mesh> Mesh;

        protected readonly SyncRef<Text> _text;

        public override IAssetProvider<Mesh> Content => Mesh.Target;

        protected override string Label => null;

        protected override void OnGenerateVisual(Slot root)
        {
            UIBuilder uIBuilder = GenerateUI(root, 128f, 72f);
            uIBuilder.Root.AttachComponent<ReferenceGrabReceiver>().TargetReference.Target = Mesh;
            uIBuilder.VerticalLayout(4f);
            uIBuilder.Style.MinHeight = 64f;
            SyncRef<Text> text = _text;
            LocaleString text2 = "Mesh:\n---";
            text.Target = uIBuilder.Text(in text2);
            _text.Target.Slot.AttachComponent<Button>();
        }

        protected override void OnChanges()
        {
            base.OnChanges();
            if (_text.Target != null)
            {
                _text.Target.Content.Value = "Mesh:\n" + (Content?.Slot.Name ?? "<i>null</i>");
            }
        }
    }
}