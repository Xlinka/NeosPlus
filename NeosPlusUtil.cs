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
            for (int i = list.Count - 1; i >= 0; i--)
            {
                ISyncMember syncMember = list.GetElement(i);
                if (syncMember == null)
                {
                    list.RemoveElement(i);
                }
            }
        }
    }
}