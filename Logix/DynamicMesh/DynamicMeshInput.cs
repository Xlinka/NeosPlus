using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine
{
	[Category(new string[] { "LogiX/Mesh" })]
	public class DynamicMeshInput : LogixOperator<DynamicMesh>
	{
		public readonly AssetRef<Mesh> Mesh;

		protected readonly SyncRef<Text> _text;

		public override DynamicMesh Content => Mesh.Target as DynamicMesh;

		protected override string Label => null;
		protected override void OnAttach()
		{
			base.OnAttach();
			Mesh.Target = Slot.AttachComponent<DynamicMesh>();
		}
		protected override void OnGenerateVisual(Slot root)
		{
			UIBuilder uIBuilder = GenerateUI(root, 128f, 72f);
			uIBuilder.Root.AttachComponent<ReferenceGrabReceiver>().TargetReference.Target = Mesh;
			uIBuilder.VerticalLayout(4f);
			uIBuilder.Style.MinHeight = 64f;
			SyncRef<Text> text = _text;
			LocaleString text2 = "Dynamic Mesh:\n---";
			text.Target = uIBuilder.Text(in text2);
			_text.Target.Slot.AttachComponent<Button>();
		}

		protected override void OnChanges()
		{
			base.OnChanges();
			if (Mesh.Target?.GetType() != typeof(DynamicMesh))
			{
				Mesh.Target = null;
			}
			if (_text.Target != null)
			{
				_text.Target.Content.Value = "Dynamic Mesh:\n" + (Content?.Slot.Name ?? "<i>null</i>");
			}
		}
	}
}