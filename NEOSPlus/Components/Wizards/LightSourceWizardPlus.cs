// MIT License

// Copyright (c) 2023 marsmaantje

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 

using BaseX;
using FrooxEngine.UIX;
using System;
using FrooxEngine;
using FrooxEngine.Undo;

namespace LightSourcesWizardPlus
{
    [Category("Add-ons/Wizards")]
    public class LightSourcesWizardPlus :
      Component,
      IDeveloperInterface
    {
        public readonly SyncRef<Slot> Root;
        public readonly Sync<bool> ProcessPointLights;
        public readonly Sync<bool> ProcessSpotLights;
        public readonly Sync<bool> ProcessDirectionalLights;
        public readonly Sync<bool> ProcessDisabled;
        public readonly Sync<ShadowType> TargetShadowType;
        public readonly Sync<bool> FilterColors;
        public readonly Sync<color> Color;
        private readonly SyncRef<TextField> _tag;
        private readonly SyncRef<FloatTextEditorParser> _intensityField;
        private readonly SyncRef<FloatTextEditorParser> _rangeField;
        private readonly SyncRef<FloatTextEditorParser> _spotAngleField;
        private readonly SyncRef<FloatTextEditorParser> _maxColorVariance;

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
            NeosCanvasPanel neosCanvasPanel = Slot.AttachComponent<NeosCanvasPanel>();
            neosCanvasPanel.Panel.AddCloseButton();
            neosCanvasPanel.Panel.AddParentButton();
            neosCanvasPanel.Panel.Title = GetLocalized("Wizard.LightSources.Title");
            neosCanvasPanel.CanvasSize = new float2(500f, 1100f);
            neosCanvasPanel.PhysicalHeight = 0.5f;

            //Layout
            UIBuilder ui = new UIBuilder(neosCanvasPanel.Canvas);
            ui.VerticalLayout(4f);
            ui.Style.MinHeight = 24f;
            ui.Style.PreferredHeight = 24f;

            //Header
            UIBuilder uiBuilder1 = ui;
            Alignment? alignment1 = new Alignment?();
            uiBuilder1.Text("Light Source Wizard Plus", alignment: alignment1);

            //Slot Reference
            UIBuilder uiBuilder2 = ui;
            Alignment? alignment2 = new Alignment?();
            uiBuilder2.Text("Process Root", alignment: alignment2);
            ui.Next("Root");
            ui.Current.AttachComponent<RefEditor>().Setup((ISyncRef)Root);

            //Edit Point Lights
            UIBuilder uiBuilder3 = ui;
            Func<BooleanMemberEditor> elementBuilder1 = (Func<BooleanMemberEditor>)(() => ui.BooleanMemberEditor((IField)ProcessPointLights));
            uiBuilder3.HorizontalElementWithLabel<BooleanMemberEditor>("Point Lights", 0.8f, elementBuilder1);

            //Edit Spot Lights
            UIBuilder uiBuilder4 = ui;
            Func<BooleanMemberEditor> elementBuilder2 = (Func<BooleanMemberEditor>)(() => ui.BooleanMemberEditor((IField)ProcessSpotLights));
            uiBuilder4.HorizontalElementWithLabel<BooleanMemberEditor>("Spot Lights", 0.8f, elementBuilder2);

            //Edit Directional Lights
            UIBuilder uiBuilder5 = ui;
            Func<BooleanMemberEditor> elementBuilder3 = (Func<BooleanMemberEditor>)(() => ui.BooleanMemberEditor((IField)ProcessDirectionalLights));
            uiBuilder5.HorizontalElementWithLabel<BooleanMemberEditor>("Directional Lights", 0.8f, elementBuilder3);

            //Edit Disabled Lights
            UIBuilder uiBuilder6 = ui;
            Func<BooleanMemberEditor> elementBuilder4 = (Func<BooleanMemberEditor>)(() => ui.BooleanMemberEditor((IField)ProcessDisabled));
            uiBuilder6.HorizontalElementWithLabel<BooleanMemberEditor>("Disabled Lights", 0.8f, elementBuilder4);

            //Only edit lights with Tag
            SyncRef<TextField> tag = _tag;
            UIBuilder uiBuilder7 = ui;
            Func<TextField> elementBuilder5 = (Func<TextField>)(() => ui.TextField());
            TextField textField = uiBuilder7.HorizontalElementWithLabel<TextField>("Tag filter", 0.8f, elementBuilder5);
            tag.Target = textField;

            //Only edit Lights with Color
            UIBuilder uiBuilder21 = ui;
            Func<BooleanMemberEditor> elementBuilder7 = (Func<BooleanMemberEditor>)(() => ui.BooleanMemberEditor((IField)FilterColors));
            uiBuilder21.HorizontalElementWithLabel<BooleanMemberEditor>("Filter Colors", 0.8f, elementBuilder7);

            //Color Filter
            UIBuilder uibuilder20 = ui;
            Func<ColorMemberEditor> elementBuilder6 = (Func<ColorMemberEditor>)(() => ui.ColorMemberEditor((IField)Color));
            uibuilder20.HorizontalElementWithLabel<ColorMemberEditor>("Color Filter", 0.3f, elementBuilder6);
            ui.NestOut();
            ui.NestOut();
            ui.NestOut();

            //Color Variance
            ui.Text("Max variance:");
            _maxColorVariance.Target = ui.FloatField(0.0f, 1f, int.MaxValue);

            UIBuilder uiBuilder8 = ui;
            uiBuilder8.Text("-------");

            //Set Shadow
            ui.EnumMemberEditor((IField)TargetShadowType);
            UIBuilder uiBuilder9 = ui;
            ButtonEventHandler action1 = new ButtonEventHandler(SetShadowType);
            uiBuilder9.Button("Set Shadow Type", action1);

            UIBuilder uiBuilder10 = ui;
            uiBuilder10.Text("-------");

            //Multiply Intensity
            _intensityField.Target = ui.FloatField(0.0f, float.PositiveInfinity, int.MaxValue);
            UIBuilder uiBuilder11 = ui;
            ButtonEventHandler action2 = new ButtonEventHandler(MultiplyIntensity);
            uiBuilder11.Button("Multiply Intensity", action2);

            //Set Intensity
            UIBuilder uiBuilder18 = ui;
            ButtonEventHandler action7 = new ButtonEventHandler(SetIntensity);
            uiBuilder18.Button("Set Intensity", action7);

            UIBuilder uiBuilder12 = ui;
            uiBuilder12.Text("-------");

            //Multiply Range
            _rangeField.Target = ui.FloatField(0.0f, float.PositiveInfinity, int.MaxValue);
            UIBuilder uiBuilder13 = ui;
            ButtonEventHandler action3 = new ButtonEventHandler(MultiplyRange);
            uiBuilder13.Button("Multiply Range", action3);

            //Set Range
            UIBuilder uiBuilder19 = ui;
            ButtonEventHandler action8 = new ButtonEventHandler(SetRange);
            uiBuilder19.Button("Set Range", action8);

            UIBuilder uiBuilder22 = ui;
            uiBuilder22.Text("-------");

            //Set Spot Angle
            _spotAngleField.Target = ui.FloatField(0.0f, 180.0f, int.MaxValue);
            UIBuilder uiBuilder23 = ui;
            ButtonEventHandler action9 = new ButtonEventHandler(SetSpotAngle);
            uiBuilder23.Button("Set Spot Angle", action9);

            UIBuilder uiBuilder14 = ui;
            uiBuilder14.Text("-------");

            //Enable Lights
            UIBuilder uiBuilder15 = ui;
            ButtonEventHandler action4 = new ButtonEventHandler(Enable);
            uiBuilder15.Button("Enable Lights", action4);

            //Disable Lights
            UIBuilder uiBuilder16 = ui;
            ButtonEventHandler action5 = new ButtonEventHandler(Disable);
            uiBuilder16.Button("Disable Lights", action5);

            //Remove Lights
            UIBuilder uiBuilder17 = ui;
            ButtonEventHandler action6 = new ButtonEventHandler(Remove);
            uiBuilder17.Button("Delete Lights", action6);
        }

        [SyncMethod]
        private void SetShadowType(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.ShadowType.CreateUndoPoint(); l.ShadowType.Value = (ShadowType)TargetShadowType; }));

        [SyncMethod]
        private void MultiplyIntensity(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.Intensity.CreateUndoPoint(); l.Intensity.Value *= (float)_intensityField.Target.ParsedValue; }));

        [SyncMethod]
        private void SetIntensity(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.Intensity.CreateUndoPoint(); l.Intensity.Value = (float)_intensityField.Target.ParsedValue; }));

        [SyncMethod]
        private void MultiplyRange(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.Range.CreateUndoPoint(); l.Range.Value *= (float)_rangeField.Target.ParsedValue; }));

        [SyncMethod]
        private void SetRange(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.Range.CreateUndoPoint(); l.Range.Value = (float)_rangeField.Target.ParsedValue; }));

        [SyncMethod]
        private void SetSpotAngle(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.SpotAngle.CreateUndoPoint(); l.SpotAngle.Value = (float)_spotAngleField.Target.ParsedValue; }));

        [SyncMethod]
        private void Remove(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => l.UndoableDestroy()));

        [SyncMethod]
        private void Disable(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.EnabledField.CreateUndoPoint(); l.Enabled = false; }));

        [SyncMethod]
        private void Enable(IButton button, ButtonEventData eventData) => ForeachLight((Action<Light>)(l => { l.EnabledField.CreateUndoPoint(); l.Enabled = true; }));

        /// <summary>
        /// Easily loop over all lights selected and apply a process to them
        /// </summary>
        /// <param name="process">What to do with each light</param>
        private void ForeachLight(Action<Light> process)
        {
            string tag = _tag.Target.TargetString;
            color Col = Color.Value;
            World.BeginUndoBatch("Modify lights");
            foreach (Light componentsInChild in (Root.Target ?? World.RootSlot).GetComponentsInChildren<Light>((Predicate<Light>)(l =>
            {
                if (!ProcessDisabled.Value && (!l.Enabled || !l.Slot.IsActive) || !string.IsNullOrEmpty(tag) && l.Slot.Tag != tag || FilterColors && !ColorEquals(l.Color.Value, Col, _maxColorVariance.Target.ParsedValue))
                    return false;

                switch (l.LightType.Value)
                {
                    case LightType.Point:
                        return ProcessPointLights.Value;
                    case LightType.Directional:
                        return ProcessDirectionalLights.Value;
                    case LightType.Spot:
                        return ProcessSpotLights.Value;
                    default:
                        return false;
                }
            })))
                process(componentsInChild);
            World.EndUndoBatch();
        }

        /// <summary>
        /// Check if two colors are within a certain tolerance of each other
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        private bool ColorEquals(color col1, color col2, float tolerance) => (double)Math.Abs(col1.r - col2.r) < (double)tolerance && (double)Math.Abs(col1.g - col2.g) < (double)tolerance && (double)Math.Abs(col1.b - col2.b) < (double)tolerance;
    }
}
