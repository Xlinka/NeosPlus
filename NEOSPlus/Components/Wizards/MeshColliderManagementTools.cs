// MIT License

// Copyright (c) 2021 Zyzyl

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

using System;
using System.Collections.Generic;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;
using FrooxEngine.Undo;

namespace MeshColliderManagementTools
{
    public enum ReplacementColliderComponent
    {
        BoxCollider,
        SphereCollider,
        CapsuleCollider,
        CylinderCollider,
        ConvexHullCollider
    }

    public enum SetupBoundsType
    {
        None,
        SetupFromLocalBounds,
        SetupFromPreciseBounds,
    }

    public enum UseTagMode
    {
        IgnoreTag,
        IncludeOnlyWithTag,
        ExcludeAllWithTag
    }

    // Wizard which allows batch or individual deletion or replacement of MeshColliders.
    [Category("NeosPlus/Wizards")]
    public class MeshColliderManagementWizard : Component
    {
        public readonly Sync<bool> IgnoreInactive;
        public readonly Sync<bool> IgnoreDisabled;
        public readonly Sync<bool> IgnoreNonPersistent;
        public readonly Sync<bool> IgnoreUserHierarchies;
        public readonly Sync<bool> PreserveColliderSettings;
        public readonly Sync<bool> SetIgnoreRaycasts;
        public readonly Sync<bool> SetCharacterCollider;
        public readonly Sync<float> Mass;
        public readonly Sync<float> HighlightDuration;
        public readonly Sync<color> HighlightColor;
        public readonly Sync<ColliderType> setColliderType;
        public readonly Sync<SetupBoundsType> setupBoundsType;
        public readonly Sync<ReplacementColliderComponent> replacementColliderComponent;
        public readonly Sync<UseTagMode> useTagMode;
        public readonly SyncRef<Slot> ProcessRoot;
        public readonly SyncRef<TextField> tag;
        public readonly SyncRef<Text> resultsText;
        private int _count;
        private color _buttonColor;
        private LocaleString _text;
        private Slot _scrollAreaRoot;
        private UIBuilder _listBuilder;

        protected override void OnAwake()
        {
            base.OnAwake();
            IgnoreInactive.Value = true;
            IgnoreDisabled.Value = true;
            IgnoreNonPersistent.Value = true;
            IgnoreUserHierarchies.Value = true;
            setupBoundsType.Value = SetupBoundsType.SetupFromLocalBounds;
            PreserveColliderSettings.Value = true;
            HighlightDuration.Value = 1f;
            HighlightColor.Value = new color(1f, 1f, 1f);
            Mass.Value = 1f;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            // Create the UI for the wizard.
            Slot.Name = "MeshCollider Management Wizard";
            Slot.Tag = "Developer";
            var neosCanvasPanel = base.Slot.AttachComponent<NeosCanvasPanel>();
            neosCanvasPanel.Panel.AddCloseButton();
            neosCanvasPanel.Panel.AddParentButton();
            neosCanvasPanel.Panel.Title = "MeshCollider Management Wizard";
            neosCanvasPanel.CanvasSize = new float2(800f, 900f);
            var uIBuilder = new UIBuilder(neosCanvasPanel.Canvas);
            var rectList = uIBuilder.SplitHorizontally(0.5f, 0.5f);
            // Build left hand side UI - options and buttons.
            var uIBuilder2 = new UIBuilder(rectList[0].Slot);
            var layoutRoot = uIBuilder2.VerticalLayout(4f, 0f, new Alignment()).Slot;
            uIBuilder2.FitContent(SizeFit.Disabled, SizeFit.MinSize);
            uIBuilder2.Style.Height = 24f;
            var uIBuilder3 = uIBuilder2;
            // Slot reference to which changes will be applied.
            _text = "Process root slot:";
            uIBuilder3.Text(in _text);
            uIBuilder3.Next("Root");
            uIBuilder3.Current.AttachComponent<RefEditor>().Setup(ProcessRoot);
            uIBuilder3.Spacer(24f);
            // Basic filtering settings for which MeshColliders are accepted for changes or listing.
            _text = "Ignore inactive:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IgnoreInactive));
            _text = "Ignore disabled:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IgnoreDisabled));
            _text = "Ignore non-persistent:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f,
                () => uIBuilder3.BooleanMemberEditor(IgnoreNonPersistent));
            _text = "Ignore user hierarchies:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f,
                () => uIBuilder3.BooleanMemberEditor(IgnoreUserHierarchies));
            _text = "Tag:";
            tag.Target = uIBuilder3.HorizontalElementWithLabel(in _text, 0.2f, () => uIBuilder3.TextField());
            _text = "Tag handling mode:";
            uIBuilder3.Text(in _text);
            uIBuilder3.EnumMemberEditor(useTagMode);
            uIBuilder3.Spacer(24f);
            // Settings for highlighing individual colliders.
            _text = "Highlight duration:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.8f,
                () => uIBuilder3.PrimitiveMemberEditor(HighlightDuration));
            _text = "Highlight color:";
            uIBuilder3.Text(in _text);
            uIBuilder3.ColorMemberEditor(HighlightColor);
            uIBuilder3.Spacer(24f);
            // Controls for specific replacement collider settings.
            _text = "Replacement collider component:";
            uIBuilder3.Text(in _text);
            uIBuilder3.EnumMemberEditor(replacementColliderComponent);
            _text = "Replacement setup action:";
            uIBuilder3.Text(in _text);
            uIBuilder3.EnumMemberEditor(setupBoundsType);
            uIBuilder3.Spacer(24f);
            _text = "Preserve existing collider settings:";
            uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f,
                () => uIBuilder3.BooleanMemberEditor(PreserveColliderSettings));
            _text = "Set collision Type:";
            uIBuilder3.Text(in _text);
            var hideTextSlot = layoutRoot.GetAllChildren().Last();
            uIBuilder3.EnumMemberEditor(setColliderType);
            var hideEnumSlot = layoutRoot.GetAllChildren().Last().Parent.Parent;
            _text = "Collider Mass:";
            var hideFloatSlot = uIBuilder3
                .HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.PrimitiveMemberEditor(Mass)).Slot.Parent;
            _text = "Set CharacterCollider:";
            var hideBoolSlot1 = uIBuilder3
                .HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(SetCharacterCollider))
                .Slot.Parent;
            _text = "Set IgnoreRaycasts:";
            var hideBoolSlot2 = uIBuilder3
                .HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(SetIgnoreRaycasts))
                .Slot.Parent;
            uIBuilder3.Spacer(24f);
            // Hide some options if preserving existing settings.
            var valCopy = layoutRoot.AttachComponent<ValueCopy<bool>>();
            var boolValDriver = layoutRoot.AttachComponent<BooleanValueDriver<bool>>();
            var valMultiDriver = layoutRoot.AttachComponent<ValueMultiDriver<bool>>();
            valCopy.Source.Target = PreserveColliderSettings;
            valCopy.Target.Target = boolValDriver.State;
            boolValDriver.TrueValue.Value = false;
            boolValDriver.FalseValue.Value = true;
            boolValDriver.TargetField.Target = valMultiDriver.Value;
            for (var i = 0; i < 5; i++)
            {
                valMultiDriver.Drives.Add();
            }

            valMultiDriver.Drives[0].Target = hideTextSlot.ActiveSelf_Field;
            valMultiDriver.Drives[1].Target = hideEnumSlot.ActiveSelf_Field;
            valMultiDriver.Drives[2].Target = hideBoolSlot1.ActiveSelf_Field;
            valMultiDriver.Drives[3].Target = hideBoolSlot2.ActiveSelf_Field;
            valMultiDriver.Drives[4].Target = hideFloatSlot.ActiveSelf_Field;
            // Buttons for batch actions.
            _text = "List matching MeshColliders";
            uIBuilder3.Button(in _text, PopulateList);
            _text = "Replace all matching MeshColliders";
            uIBuilder3.Button(in _text, ReplaceAll);
            _text = "Remove all matching MeshColliders";
            uIBuilder3.Button(in _text, RemoveAll);
            uIBuilder3.Spacer(24f);
            _text = "------";
            resultsText.Target = uIBuilder3.Text(in _text);
            // Build right hand side UI - list of found MeshColliders.
            UIBuilder uIBuilder4 = new UIBuilder(rectList[1].Slot);
            uIBuilder4.ScrollArea();
            uIBuilder4.VerticalLayout(10f, 4f);
            _scrollAreaRoot = uIBuilder4.FitContent(SizeFit.Disabled, SizeFit.MinSize).Slot;
            // Prepare UIBuilder for addding elements to MeshCollider list.
            _listBuilder = uIBuilder4;
            _listBuilder.Style.MinHeight = 40f;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Slot.GetComponentInChildrenOrParents<Canvas>()?.MarkDeveloper();
        }

        private void CreateScrollListElement(MeshCollider mc)
        {
            var elementRoot = _listBuilder.Next("Element");
            var refField = elementRoot.AttachComponent<ReferenceField<MeshCollider>>();
            refField.Reference.Target = mc;
            var listBuilder2 = new UIBuilder(elementRoot);
            listBuilder2.NestInto(elementRoot);
            listBuilder2.VerticalLayout(4f, 4f);
            listBuilder2.HorizontalLayout(10f);
            _buttonColor = new color(1f, 1f, 1f);
            _text = "Jump To";
            listBuilder2.ButtonRef<Slot>(in _text, in _buttonColor, JumpTo, mc.Slot);
            _text = "Highlight";
            listBuilder2.ButtonRef<Slot>(in _text, in _buttonColor, Highlight, mc.Slot);
            _text = "Replace";
            listBuilder2.ButtonRef<MeshCollider>(in _text, in _buttonColor, Replace, mc);
            _text = "Remove";
            listBuilder2.ButtonRef<MeshCollider>(in _text, in _buttonColor, Remove, mc);
            listBuilder2.NestOut();
            listBuilder2.NestOut();
            listBuilder2.Current.AttachComponent<RefEditor>().Setup(refField.Reference);
        }

        private void ForeachMeshCollider(Action<MeshCollider> process)
        {
            if (ProcessRoot.Target != null)
            {
                foreach (var componentsInChild in ProcessRoot.Target.GetComponentsInChildren(
                             (MeshCollider mc) => (!IgnoreInactive.Value || mc.Slot.IsActive)
                                                  && (!IgnoreDisabled || mc.Enabled)
                                                  && (!IgnoreNonPersistent || mc.IsPersistent)
                                                  && (!IgnoreUserHierarchies || mc.Slot.ActiveUser == null)
                                                  && (useTagMode == UseTagMode.IgnoreTag
                                                      || (useTagMode == UseTagMode.IncludeOnlyWithTag &&
                                                          mc.Slot.Tag == tag.Target.TargetString)
                                                      || (useTagMode == UseTagMode.ExcludeAllWithTag &&
                                                          mc.Slot.Tag != tag.Target.TargetString))))
                {
                    process(componentsInChild);
                }
            }
            else ShowResults("No target root slot set.");
        }

        private bool CheckReplacementBoundsSetting()
        {
            return replacementColliderComponent == ReplacementColliderComponent.BoxCollider
                   || replacementColliderComponent == ReplacementColliderComponent.SphereCollider
                   || replacementColliderComponent == ReplacementColliderComponent.ConvexHullCollider
                   || ((replacementColliderComponent == ReplacementColliderComponent.CapsuleCollider
                        || replacementColliderComponent == ReplacementColliderComponent.CylinderCollider)
                       && setupBoundsType.Value != SetupBoundsType.SetupFromLocalBounds);
        }

        private void Highlight(IButton button, ButtonEventData eventData, Slot s) =>
            HighlightHelper.FlashHighlight(s, null, HighlightColor, HighlightDuration);

        private void JumpTo(IButton button, ButtonEventData eventData, Slot s) =>
            LocalUserRoot.JumpToPoint(s.GlobalPosition);

        private void PopulateList()
        {
            _scrollAreaRoot.DestroyChildren();
            ForeachMeshCollider(delegate(MeshCollider mc) { CreateScrollListElement(mc); });
        }

        private void PopulateList(IButton button, ButtonEventData eventData)
        {
            _count = 0;
            _scrollAreaRoot.DestroyChildren();
            ForeachMeshCollider(delegate(MeshCollider mc)
            {
                CreateScrollListElement(mc);
                _count++;
            });
            ShowResults($"{_count} matching MeshColliders listed.");
        }

        private void Remove(IButton button, ButtonEventData eventData, MeshCollider mc)
        {
            mc.UndoableDestroy();
            PopulateList();
            ShowResults($"MeshCollider removed.");
        }

        private void Replace(IButton button, ButtonEventData eventData, MeshCollider mc)
        {
            if (CheckReplacementBoundsSetting())
            {
                World.BeginUndoBatch("Replace MeshCollider");
                switch (replacementColliderComponent.Value)
                {
                    case ReplacementColliderComponent.BoxCollider:
                        var bc = mc.Slot.AttachComponent<BoxCollider>();
                        bc.CreateSpawnUndoPoint();
                        SetupNewCollider(bc, mc);
                        break;
                    case ReplacementColliderComponent.SphereCollider:
                        var sc = mc.Slot.AttachComponent<SphereCollider>();
                        sc.CreateSpawnUndoPoint();
                        SetupNewCollider(sc, mc);
                        break;
                    case ReplacementColliderComponent.ConvexHullCollider:
                        mc.Slot.AttachComponent<ConvexHullCollider>().CreateSpawnUndoPoint();
                        break;
                }

                mc.UndoableDestroy();
                World.EndUndoBatch();
                PopulateList();
                ShowResults($"MeshCollider replaced.");
            }
            else
                ShowResults($"{replacementColliderComponent.Value} cannot be used with {setupBoundsType.Value}");
        }

        private void RemoveAll(IButton button, ButtonEventData eventData)
        {
            World.BeginUndoBatch("Batch remove MeshColliders");
            _count = 0;
            ForeachMeshCollider(delegate(MeshCollider mc)
            {
                mc.UndoableDestroy();
                _count++;
            });
            World.EndUndoBatch();
            PopulateList();
            ShowResults($"{_count} matching MeshColliders removed.");
        }

        private void ReplaceAll(IButton button, ButtonEventData eventData)
        {
            if (CheckReplacementBoundsSetting())
            {
                World.BeginUndoBatch("Batch replace MeshColliders");
                _count = 0;
                ForeachMeshCollider(delegate(MeshCollider mc)
                {
                    switch (replacementColliderComponent.Value)
                    {
                        case ReplacementColliderComponent.BoxCollider:
                            var bc = mc.Slot.AttachComponent<BoxCollider>();
                            bc.CreateSpawnUndoPoint();
                            SetupNewCollider(bc, mc);
                            break;
                        case ReplacementColliderComponent.SphereCollider:
                            var sc = mc.Slot.AttachComponent<SphereCollider>();
                            sc.CreateSpawnUndoPoint();
                            SetupNewCollider(sc, mc);
                            break;
                        case ReplacementColliderComponent.ConvexHullCollider:
                            var chc = mc.Slot.AttachComponent<ConvexHullCollider>();
                            chc.CreateSpawnUndoPoint();
                            SetupNewCollider(chc, mc);
                            break;
                        case ReplacementColliderComponent.CapsuleCollider:
                            var capc = mc.Slot.AttachComponent<CapsuleCollider>();
                            capc.CreateSpawnUndoPoint();
                            SetupNewCollider(capc, mc);
                            break;
                        case ReplacementColliderComponent.CylinderCollider:
                            var cylc = mc.Slot.AttachComponent<CylinderCollider>();
                            cylc.CreateSpawnUndoPoint();
                            SetupNewCollider(cylc, mc);
                            break;
                    }

                    mc.UndoableDestroy();
                    _count++;
                });
                World.EndUndoBatch();
                PopulateList();
                ShowResults(
                    $"{_count} matching MeshColliders replaced with {replacementColliderComponent.ToString()}s.");
            }
            else
                ShowResults($"{replacementColliderComponent.Value} cannot be used with {setupBoundsType.Value}");
        }

        private void SetupNewCollider(BoxCollider bc, MeshCollider mc)
        {
            switch (setupBoundsType.Value)
            {
                case SetupBoundsType.None:
                    break;
                case SetupBoundsType.SetupFromLocalBounds:
                    bc.SetFromLocalBounds();
                    break;
                case SetupBoundsType.SetupFromPreciseBounds:
                    bc.SetFromLocalBoundsPrecise();
                    break;
            }
            if (PreserveColliderSettings)
            {
                bc.Type.Value = mc.Type.Value;
                bc.CharacterCollider.Value = mc.CharacterCollider.Value;
                bc.IgnoreRaycasts.Value = mc.IgnoreRaycasts.Value;
                bc.Mass.Value = mc.Mass.Value;
            }
            else
            {
                bc.Type.Value = setColliderType;
                bc.CharacterCollider.Value = SetCharacterCollider;
                bc.IgnoreRaycasts.Value = SetIgnoreRaycasts;
                bc.Mass.Value = Mass;
            }
        }

        private void SetupNewCollider(SphereCollider sc, MeshCollider mc)
        {
            switch (setupBoundsType.Value)
            {
                case SetupBoundsType.None:
                    break;
                case SetupBoundsType.SetupFromLocalBounds:
                    sc.SetFromLocalBounds();
                    break;
                case SetupBoundsType.SetupFromPreciseBounds:
                    sc.SetFromPreciseBounds();
                    break;
            }

            if (PreserveColliderSettings)
            {
                sc.Type.Value = mc.Type.Value;
                sc.CharacterCollider.Value = mc.CharacterCollider.Value;
                sc.IgnoreRaycasts.Value = mc.IgnoreRaycasts.Value;
                sc.Mass.Value = mc.Mass.Value;
            }
            else
            {
                sc.Type.Value = setColliderType;
                sc.CharacterCollider.Value = SetCharacterCollider;
                sc.IgnoreRaycasts.Value = SetIgnoreRaycasts;
                sc.Mass.Value = Mass;
            }
        }

        private void SetupNewCollider(CapsuleCollider capc, MeshCollider mc)
        {
            switch (setupBoundsType.Value)
            {
                case SetupBoundsType.None:
                    break;
                case SetupBoundsType.SetupFromPreciseBounds:
                    capc.SetFromExactCylinder();
                    break;
            }

            if (PreserveColliderSettings)
            {
                capc.Type.Value = mc.Type.Value;
                capc.CharacterCollider.Value = mc.CharacterCollider.Value;
                capc.IgnoreRaycasts.Value = mc.IgnoreRaycasts.Value;
                capc.Mass.Value = mc.Mass.Value;
            }
            else
            {
                capc.Type.Value = setColliderType;
                capc.CharacterCollider.Value = SetCharacterCollider;
                capc.IgnoreRaycasts.Value = SetIgnoreRaycasts;
                capc.Mass.Value = Mass;
            }
        }

        private void SetupNewCollider(CylinderCollider cylc, MeshCollider mc)
        {
            switch (setupBoundsType.Value)
            {
                case SetupBoundsType.None:
                    break;
                case SetupBoundsType.SetupFromPreciseBounds:
                    cylc.SetFromPreciseBounds();
                    break;
            }
            if (PreserveColliderSettings)
            {
                cylc.Type.Value = mc.Type.Value;
                cylc.CharacterCollider.Value = mc.CharacterCollider.Value;
                cylc.IgnoreRaycasts.Value = mc.IgnoreRaycasts.Value;
                cylc.Mass.Value = mc.Mass.Value;
            }
            else
            {
                cylc.Type.Value = setColliderType;
                cylc.CharacterCollider.Value = SetCharacterCollider;
                cylc.IgnoreRaycasts.Value = SetIgnoreRaycasts;
                cylc.Mass.Value = Mass;
            }
        }

        private void SetupNewCollider(ConvexHullCollider chc, MeshCollider mc)
        {
            if (PreserveColliderSettings)
            {
                chc.Type.Value = mc.Type.Value;
                chc.CharacterCollider.Value = mc.CharacterCollider.Value;
                chc.IgnoreRaycasts.Value = mc.IgnoreRaycasts.Value;
                chc.Mass.Value = mc.Mass.Value;
            }
            else
            {
                chc.Type.Value = setColliderType;
                chc.CharacterCollider.Value = SetCharacterCollider;
                chc.IgnoreRaycasts.Value = SetIgnoreRaycasts;
                chc.Mass.Value = Mass;
            }
        }

        private void ShowResults(string results) => resultsText.Target.Content.Value = results;
    }
}