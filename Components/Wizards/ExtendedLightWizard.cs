using BaseX;
using FrooxEngine.UIX;
using System;

namespace FrooxEngine
{
    public class ExtendedLightWizard : Component, IDeveloperInterface, IComponent, IComponentBase, IDestroyable, IWorker, IWorldElement, IUpdatable, IChangeable, IAudioUpdatable, IInitializable, ILinkable // Optinally we can extend : WorldLightSourcesWizard
	{
		public readonly SyncRef<Slot> Root;

		public readonly Sync<bool> ProcessPointLights;

		public readonly Sync<bool> ProcessSpotLights;

		public readonly Sync<bool> ProcessDirectionalLights;

		public readonly Sync<bool> ProcessDisabled;

		public readonly Sync<ShadowType> TargetShadowType;

		protected readonly SyncRef<TextField> _tag;

		protected readonly SyncRef<FloatTextEditorParser> _intensityField;

		protected readonly SyncRef<FloatTextEditorParser> _rangeField;

		protected override void OnAwake()
		{
			base.OnAwake();
			ProcessPointLights.Value = true;
			ProcessSpotLights.Value = true;
			ProcessDirectionalLights.Value = true;
			ProcessDisabled.Value = false;
		}

		protected override void OnAttach()
		{
			base.OnAttach();
			NeosCanvasPanel neosCanvasPanel = base.Slot.AttachComponent<NeosCanvasPanel>();
			neosCanvasPanel.Panel.AddCloseButton();
			neosCanvasPanel.Panel.Title = this.GetLocalized("Wizard.LightSources.Title");
			neosCanvasPanel.CanvasSize = new float2(300f, 600f);
			neosCanvasPanel.PhysicalHeight = 0.5f;
			UIBuilder ui = new UIBuilder(neosCanvasPanel.Canvas);
			ui.VerticalLayout(4f);
			ui.Style.MinHeight = 24f;
			ui.Style.PreferredHeight = 24f;
			UIBuilder uIBuilder = ui;

			LocaleString text = "ExtendedLightingWizard".AsLocaleKey("<b>{0}</b>");
			uIBuilder.Text(in text);
			UIBuilder uIBuilder2 = ui;
			text = "Wizard.LightSources.ProcessRoot".AsLocaleKey();
			uIBuilder2.Text(in text);
			ui.Next("Root");
			ui.Current.AttachComponent<RefEditor>().Setup(Root);
			UIBuilder uIBuilder3 = ui;

			text = "Wizard.LightSources.PointLights".AsLocaleKey();
			uIBuilder3.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(ProcessPointLights));
			UIBuilder uIBuilder4 = ui;
			text = "Wizard.LightSources.SpotLights".AsLocaleKey();
			uIBuilder4.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(ProcessSpotLights));
			UIBuilder uIBuilder5 = ui;
			text = "Wizard.LightSources.DirectionalLights".AsLocaleKey();
			uIBuilder5.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(ProcessDirectionalLights));
			UIBuilder uIBuilder6 = ui;
			text = "Wizard.LightSources.DisabledLights".AsLocaleKey();
			uIBuilder6.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(ProcessDisabled));
			SyncRef<TextField> tag = _tag;
			UIBuilder uIBuilder7 = ui;
			text = "Wizard.LightSources.WithTag".AsLocaleKey();
			tag.Target = uIBuilder7.HorizontalElementWithLabel(in text, 0.8f, () => ui.TextField());
			UIBuilder uIBuilder8 = ui;
			text = "-------";
		}

		[SyncMethod]
		private void SetIntensity(IButton button, ButtonEventData eventData)
		{
			ForeachLight(delegate (Light l)
			{
				l.Intensity.Value = _intensityField.Target.ParsedValue;
			});
		}

		// This method is private, can we store a reference to it at runtime?
		private void ForeachLight(Action<Light> process)
		{
			string tag = _tag.Target.TargetString;
			foreach (Light componentsInChild in (Root.Target ?? base.World.RootSlot).GetComponentsInChildren(delegate (Light l)
			{
				if (!ProcessDisabled.Value && (!l.Enabled || !l.Slot.IsActive))
				{
					return false;
				}
				return (string.IsNullOrEmpty(tag) || !(l.Slot.Tag != tag)) && l.LightType.Value switch
				{
					LightType.Point => ProcessPointLights.Value,
					LightType.Directional => ProcessDirectionalLights.Value,
					LightType.Spot => ProcessSpotLights.Value,
					_ => false,
				};
			}))
			{
				process(componentsInChild);
			}
		}

	}
}
