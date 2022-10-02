using System;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    public static class NeosPlusUtils
    {
        public static void SetUpActionTrigger(this Button button, Action action) =>
            button.Slot.AttachComponent<ButtonActionTrigger>().OnPressed.Target = action;

        public static void CleanList(this ISyncList list) // Code from https://github.com/EIA485/NeosBetterLogixWires
        {
            for (var i = list.Count - 1; i >= 0; i--)
                if (list.GetElement(i) == null)
                    list.RemoveElement(i);
        }
    }
}