using BaseX;
using FrooxEngine.UIX;
using System.Linq;
using static FrooxEngine.LODGroup;

namespace FrooxEngine
{
    [Category("Add-ons/Wizards")]
    public class LODWizard : Component
    {
        private const string LOD_GROUP_PREFIX = "<LOD_GROUP>";

        private const string LOD_1 = "<LOD_1>";
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> ScreenRelativeTransitionHeight_1;
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> FadeTransitionWidth_1;

        private const string LOD_2 = "<LOD_2>";
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> ScreenRelativeTransitionHeight_2;
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> FadeTransitionWidth_2;

        private const string LOD_3 = "<LOD_3>";
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> ScreenRelativeTransitionHeight_3;
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> FadeTransitionWidth_3;

        private const string LOD_4 = "<LOD_4>";
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> ScreenRelativeTransitionHeight_4;
        [Range(0f, 1f, "0.00")]
        public readonly Sync<float> FadeTransitionWidth_4;

        public readonly SyncRef<Slot> ProcessRoot;
        public readonly Sync<bool> CrossFade;
        public readonly Sync<bool> AnimateCrossFading;

        protected override void OnAttach()
        {
            base.OnAttach();
            CrossFade.Value = false;
            AnimateCrossFading.Value = false;

            Slot.Name = "Map Wizard";
            Slot.Tag = "Developer";
            NeosCanvasPanel neosCanvasPanel = Slot.AttachComponent<NeosCanvasPanel>();
            neosCanvasPanel.Panel.AddCloseButton();
            neosCanvasPanel.Panel.Title = this.GetLocalized("LOD Wizard");
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
            ui.Next("Process Root");
            ui.Current.AttachComponent<RefEditor>().Setup(ProcessRoot);
            UIBuilder uIBuilder3 = ui;

            text = "Cross Fade".AsLocaleKey();
            uIBuilder3.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(CrossFade));
            UIBuilder uIBuilder4 = ui;
            text = "Animate Cross Fading".AsLocaleKey();
            uIBuilder4.HorizontalElementWithLabel(in text, 0.8f, () => ui.BooleanMemberEditor(AnimateCrossFading));

            UIBuilder uIBuilder5 = ui;
            text = "LOD Group 1 ScreenRelativeTransitionHeight".AsLocaleKey();
            uIBuilder5.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(ScreenRelativeTransitionHeight_1));
            UIBuilder uIBuilder6 = ui;
            text = "LOD Group 1 FadeTransitionWidth".AsLocaleKey();
            uIBuilder6.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(FadeTransitionWidth_1));
            UIBuilder uIBuilder7 = ui;
            text = "LOD Group 2 ScreenRelativeTransitionHeight".AsLocaleKey();
            uIBuilder7.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(ScreenRelativeTransitionHeight_2));
            UIBuilder uIBuilder8 = ui;
            text = "LOD Group 2 FadeTransitionWidth".AsLocaleKey();
            uIBuilder8.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(FadeTransitionWidth_2));
            UIBuilder uIBuilder9 = ui;
            text = "LOD Group 3 ScreenRelativeTransitionHeight".AsLocaleKey();
            uIBuilder9.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(ScreenRelativeTransitionHeight_3));
            UIBuilder uIBuilder10 = ui;
            text = "LOD Group 3 FadeTransitionWidth".AsLocaleKey();
            uIBuilder10.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(FadeTransitionWidth_3));
            UIBuilder uIBuilder11 = ui;
            text = "LOD Group 4 ScreenRelativeTransitionHeight".AsLocaleKey();
            uIBuilder11.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(ScreenRelativeTransitionHeight_4));
            UIBuilder uIBuilder12 = ui;
            text = "LOD Group 4 FadeTransitionWidth".AsLocaleKey();
            uIBuilder12.HorizontalElementWithLabel(in text, 0.8f, () => ui.PrimitiveMemberEditor(FadeTransitionWidth_4));

            UIBuilder uIBuilder13 = ui;
            text = "-------";
            uIBuilder13.Text(in text);
            UIBuilder uIBuilder14 = ui;
            text = "Setup LOD Groups".AsLocaleKey();
            uIBuilder14.Button(in text, SetupLODs);
            UIBuilder uIBuilder15 = ui;
            text = "-------";
            uIBuilder15.Text(in text);
        }

        [SyncMethod]
        private void SetupLODs(IButton button, ButtonEventData eventData)
        {
            var lodGroupCanidate = ProcessRoot.Target.GetComponent<LODGroup>();
            if (lodGroupCanidate == null)
            {
                lodGroupCanidate = ProcessRoot.Target.AttachComponent<LODGroup>();
                lodGroupCanidate.CrossFade.Value = CrossFade.Value;
                lodGroupCanidate.AnimateCrossFading.Value = AnimateCrossFading.Value;
            }

            // Note: Given FrooxEngine's implementation of the AddLOD method, it is impossible to nest LOD groups within eachother.
            // This is because it calls Renderers.AddRange(root.GetComponentsInChildren<MeshRenderer>()), which is recursive.

            // In other words, your heirachy should look something like this:
            //
            // ProcessRoot
            // ...
            // - ...
            // - <LOD_GROUP>My Group 
            // -- <LOD_1>Level 1 
            // -- <LOD_2>Level 2
            // -- <LOD_3>Level 3
            // -- <LOD_4>Level 4
            // -- ...
            // - ...
            // ...
            //

            // Where additional slots are ignored. Here, this would populate ProcessRoot's LODGroup with 4 LODs, 
            // Adhering to Unity's LOD standard. I don't think this implicity supports the culled state,
            // Might be best to have a ColliderUserTracker to drive the entire mesh root.

            LOD lod;
            foreach (var child in ProcessRoot.Target.GetAllChildren().Where(x => x.Name.StartsWith(LOD_GROUP_PREFIX)))
            {
                switch (child.Name)
                {
                    case LOD_1:
                        lod = lodGroupCanidate.AddLOD(ScreenRelativeTransitionHeight_1.Value, child);
                        lod.FadeTransitionWidth.Value = FadeTransitionWidth_1;
                        break;
                    case LOD_2:
                        lod = lodGroupCanidate.AddLOD(ScreenRelativeTransitionHeight_2.Value, child);
                        lod.FadeTransitionWidth.Value = FadeTransitionWidth_2;
                        break;
                    case LOD_3:
                        lod = lodGroupCanidate.AddLOD(ScreenRelativeTransitionHeight_3.Value, child);
                        lod.FadeTransitionWidth.Value = FadeTransitionWidth_3;
                        break;
                    case LOD_4:
                        lod = lodGroupCanidate.AddLOD(ScreenRelativeTransitionHeight_4.Value, child);
                        lod.FadeTransitionWidth.Value = FadeTransitionWidth_4;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
