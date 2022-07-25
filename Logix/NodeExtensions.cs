using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace NEOSPlus
{
    public static class NodeExtensions
    {
        public static void GenerateListButtons(this UIBuilder ui, ButtonEventHandler plus, ButtonEventHandler minus)
        {
            var uIBuilder = ui;
            uIBuilder.Panel();
            uIBuilder.HorizontalFooter(32f, out var footer, out var _);
            var uIBuilder2 = new UIBuilder(footer);
            uIBuilder2.HorizontalLayout(4f);
            LocaleString text = "+";
            var tint = color.White;
            uIBuilder2.Button(in text, in tint, plus);
            text = "-";
            tint = color.White;
            uIBuilder2.Button(in text, in tint, minus);
        }
    }
}