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

using System.Collections.Generic;
using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;

namespace ComponentBreakdown
{
    [Category("Add-ons/Wizards")]
    public class ComponentBreakdown :
        Component
    {
        public readonly SyncRef<Slot> Root;
        private readonly SyncRef<Slot> _listRoot;
        private UIBuilder listGenerator;

        protected override void OnAttach()
        {
            base.OnAttach();

            //initialize the panel
            NeosCanvasPanel neosCanvasPanel = Slot.AttachComponent<NeosCanvasPanel>();
            neosCanvasPanel.Panel.AddCloseButton();
            neosCanvasPanel.Panel.Title = "Component Breakdown";
            neosCanvasPanel.CanvasSize = new float2(600f, 1200f);
            neosCanvasPanel.PhysicalHeight = 0.5f;
            UIBuilder builder = new UIBuilder(neosCanvasPanel.Canvas);

            builder.VerticalLayout(5, 10);
            builder.Style.MinHeight = 24f;
            builder.Style.PreferredHeight = 24f;

            //slot selector
            builder.Text("Root: ");
            builder.Next("Root");
            builder.Current.AttachComponent<RefEditor>().Setup((ISyncRef)Root);

            //spacer
            builder.Text("------------");

            //button to generate the list
            ButtonEventHandler generateAction = new ButtonEventHandler(GenerateList);
            builder.Button("Generate", generateAction);

            //setup scrollable list of all components
            Slot slot = builder.ScrollArea().Slot;
            Slot scrollRect = slot.Parent;
            scrollRect.GetComponent<LayoutElement>().FlexibleHeight.Value = 1;
            ContentSizeFitter fitter = slot.AttachComponent<ContentSizeFitter>();
            fitter.VerticalFit.Value = SizeFit.MinSize;

            builder.NestInto(slot);
            slot.AttachComponent<VerticalLayout>().Spacing.Value = 5f;
            var layout = slot.GetComponent<VerticalLayout>();
            layout.Spacing.Value = 5f;
            layout.ForceExpandHeight.Value = false;
            layout.HorizontalAlign.Value = LayoutHorizontalAlignment.Left;
            _listRoot.Target = slot;
            builder.Style.TextAlignment = Alignment.MiddleLeft;
            listGenerator = builder;
        }

        [SyncMethod]
        private void GenerateList(IButton button, ButtonEventData data)
        {
            //get the list of components
            List<Component> components = new List<Component>();
            Root.Target.GetComponentsInChildren<Component>(components);

            Dictionary<string, int> counts = new Dictionary<string, int>();

            //count the occurences of each component type
            foreach (var component in components)
            {
                string componentTypeName = component.GetType().Name;
                if (counts.ContainsKey(componentTypeName))
                {
                    counts[componentTypeName]++;
                }
                else
                {
                    counts[componentTypeName] = 1;
                }
            }

            //sort counts by their value and add them to the visual list
            _listRoot.Target.DestroyChildren();
            foreach (var key in counts.Keys)
            {
                int value;
                counts.TryGetValue(key, out value);
                Slot slot = listGenerator.Text("" + value + ": " + key).Slot;
                slot.OrderOffset = -value;
                var element = slot.AttachComponent<LayoutElement>();
                element.MinHeight.Value = 24f;
                element.PreferredHeight.Value = 24f;
            }
        }
    }
}
