using System;
using BaseX;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    [Category("NeosPlus/Wizards")]
    public class LightSourceWizard : Component, IDeveloperInterface, IComponent, IComponentBase, IDestroyable, IWorker,
        IWorldElement, IUpdatable, IChangeable, IAudioUpdatable, IInitializable, ILinkable
    {
        public readonly SyncRef<Slot> Root;

        public readonly Sync<bool> ProcessPointLights;

        public readonly Sync<bool> ProcessSpotLights;

        public readonly Sync<bool> ProcessDirectionalLights;

        public readonly Sync<bool> ProcessDisabled;

        public readonly Sync<bool> ProcessColor;

        public readonly Sync<color> Color;

        public readonly SyncRef<TextField> ProcessTags;

        public readonly SyncRef<FloatTextEditorParser> _intensityField;

        public readonly SyncRef<FloatTextEditorParser> _intensityFieldMultiplicative;

        public readonly SyncRef<FloatTextEditorParser> _extentField;

        protected override void OnAttach()
        {
            base.OnAttach();
            ProcessPointLights.Value = true;
            ProcessSpotLights.Value = true;
            ProcessDirectionalLights.Value = true;
            ProcessDisabled.Value = false;
            ProcessColor.Value = false;

            Slot.Name = "Light Source Wizard";
            Slot.Tag = "Developer";
            NeosCanvasPanel neosCanvasPanel = Slot.AttachComponent<NeosCanvasPanel>();
            neosCanvasPanel.Panel.AddCloseButton();
            neosCanvasPanel.Panel.Title = this.GetLocalized("Wizard.LightSources.Title");
            neosCanvasPanel.CanvasSize = new float2(600f, 600f);
            neosCanvasPanel.PhysicalHeight = 0.5f;
            UIBuilder ui = new UIBuilder(neosCanvasPanel.Canvas);
            ui.VerticalLayout(4f);
            ui.Style.MinHeight = 24f;
            ui.Style.PreferredHeight = 24f;
            UIBuilder uIBuilder = ui;
            LocaleString text = "Wizard.LightSources.Header".AsLocaleKey("<b>{0}</b>");
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
            uIBuilder5.HorizontalElementWithLabel(in text, 0.8f,
                () => ui.BooleanMemberEditor(ProcessDirectionalLights));
            UIBuilder uIBuilder6 = ui;
            text = "Wizard.LightSources.DisabledLights".AsLocaleKey();
            uIBuilder6.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(ProcessDisabled));
            SyncRef<TextField> tag = ProcessTags;
            UIBuilder uIBuilder7 = ui;
            text = "Wizard.LightSources.WithTag".AsLocaleKey();
            tag.Target = uIBuilder7.HorizontalElementWithLabel(in text, 0.8f, () => ui.TextField());
            // UIBuilder uIBuilder8 = ui;
            // uIBuilder8.ColorMemberEditor(Color, "LightSources with Color");
            text = "-------";

            UIBuilder uIBuilder9 = ui;
            uIBuilder9.Text(in text);
            _intensityField.Target = ui.FloatField(0f, 8f);
            UIBuilder uIBuilder10 = ui;
            text = "Set light intensity to value".AsLocaleKey();
            uIBuilder10.Button(in text, ChangeIntensity);
            text = "-------";

            uIBuilder10.Text(in text);
            _intensityFieldMultiplicative.Target = ui.FloatField(0f, 8f);
            UIBuilder uIBuilder11 = ui;
            text = "Multiply light intensity by value".AsLocaleKey();
            uIBuilder11.Button(in text, ChangeIntensityMultiplicative);
            UIBuilder uIBuilder12 = ui;
            text = "-------";

            UIBuilder uIBuilder13 = ui;
            uIBuilder13.Text(in text);
            _extentField.Target = ui.FloatField(0f, 50f);
            UIBuilder uIBuilder14 = ui;
            text = "Set light range to value".AsLocaleKey();
            uIBuilder14.Button(in text, SetExtent);
            text = "-------";
        }

        protected override void OnStart()
        {
            base.OnStart();
            Slot.GetComponentInChildrenOrParents<Canvas>()?.MarkDeveloper();
        }

        [SyncMethod]
        private void ChangeIntensity(IButton button, ButtonEventData eventData)
        {
            ForeachLight(delegate(Light l) { l.Intensity.Value = _intensityField.Target.ParsedValue; });
        }

        [SyncMethod]
        private void ChangeIntensityMultiplicative(IButton button, ButtonEventData eventData)
        {
            ForeachLight(delegate(Light l) { l.Intensity.Value *= _intensityFieldMultiplicative.Target.ParsedValue; });
        }

        [SyncMethod]
        private void SetExtent(IButton button, ButtonEventData eventData)
        {
            ForeachLight(delegate(Light l)
            {
                switch (l.LightType.Value)
                {
                    case LightType.Spot:
                        var globalPosSpot = l.Slot.GlobalPosition;
                        var spotHit = Physics.RaycastOne(globalPosSpot, l.Slot.Down, _extentField.Target.ParsedValue);
                        if (spotHit.HasValue)
                        {
                            var clamped = MathX.Clamp(spotHit.Value.Distance, 0, _extentField.Target.ParsedValue);
                            l.Range.Value = clamped;
                            l.Intensity.Value = clamped / 10;
                        }
                        else
                        {
                            l.Range.Value = _extentField.Target.ParsedValue;
                            l.Intensity.Value = _extentField.Target.ParsedValue / 10;
                        }

                        break;
                    case LightType.Point:
                        var globalPosPoint = l.Slot.GlobalPosition;
                        ICollider pointHit = Physics.SphereBoolSweepOne(globalPosPoint, float3.Zero,
                            _extentField.Target.ParsedValue);
                        if (pointHit == null)
                        {
                            l.Range.Value = _extentField.Target.ParsedValue;
                            l.Intensity.Value = _extentField.Target.ParsedValue / 10;
                            return;
                        }

                        for (int i = 0; i < _extentField.Target.ParsedValue; i++)
                        {
                            pointHit = Physics.SphereBoolSweepOne(globalPosPoint, float3.Zero, i);
                            if (pointHit != null)
                                break;
                        }

                        var dist = MathX.Distance(pointHit.Slot.GlobalPosition, globalPosPoint);
                        l.Range.Value = dist;
                        l.Intensity.Value = dist / 10;
                        break;
                }
            });
        }

        private void ForeachLight(Action<Light> process)
        {
            string tag = ProcessTags.Target.TargetString;
            float4 canidate = Color.Value;
            foreach (Light componentsInChild in (Root.Target ?? World.RootSlot).GetComponentsInChildren(
                         delegate(Light l)
                         {
                             if (!ProcessDisabled.Value && (!l.Enabled || !l.Slot.IsActive))
                             {
                                 return false;
                             }

                             float4 copy = l.Color.Value;
                             return (string.IsNullOrEmpty(tag) || !(l.Slot.Tag != tag)) &&
                                    (!ProcessColor.Value || MathX.Approximately(in copy, in canidate)) &&
                                    l.LightType.Value switch
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