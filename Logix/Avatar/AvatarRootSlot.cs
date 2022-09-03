using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;
using BaseX;
using FrooxEngine.CommonAvatar;


namespace FrooxEngine.LogiX.Avatar
{
    [Category("LogiX/Avatar")]
    [NodeName("Avatar Root Slot")]
    public class AvatarRootSlot : LogixOperator<Slot>
    {
        public readonly Input<User> User;
        public override Slot Content
        {
            get
            {
                User user = User.Evaluate();
                if (user == null)
                    return null;
                Slot slot = user.Root.Slot;
                foreach (var item in slot.GetAllChildren())
                {
                    if (item.GetComponent<AvatarRoot>() != null)
                        return item;
                }
                return null;
            }
        }
        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}