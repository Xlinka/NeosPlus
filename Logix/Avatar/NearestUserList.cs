using System.Collections.Generic;
using BaseX;
using FrooxEngine.LogiX;
using NEOSPlus;
namespace FrooxEngine.LogiX.Avatar
{
    [NodeName("Nearest User")]
    [Category("LogiX/Avatar")]
    public class NearestUserList : LogixNode
    {
        public readonly Input<Slot> Reference;
        public readonly SyncList<Input<User>> IgnoreUsers;
        public readonly Input<bool> IgnoreAFK;
        public readonly Output<User> User;
        public readonly Output<float> Distance;

        private User _nearestUser;
        private float _nearestDistance = float.MaxValue;

        protected override void OnAttach()
        {
            base.OnAttach();
            IgnoreUsers.Add();
        }

        protected override void OnCommonUpdate()
        {
            // Literally just Nearest Hand but without the Hand stuff
            base.OnCommonUpdate();
            Slot slot = Reference.Evaluate() ?? base.Slot;
            bool flag = IgnoreAFK.Evaluate(def: false);
            _nearestDistance = float.MaxValue;
            _nearestUser = null;

            var usr = new List<User>();
            for (int i = 0; i < IgnoreUsers.Count; i++)
            {
                usr.Add(IgnoreUsers.GetElement(i).Evaluate());
            }

            foreach (User allUser in base.World.AllUsers)
            {
                if (usr.Exists(e => e == allUser) || (!allUser.IsPresentInWorld && flag))
                {
                    continue;
                }

                Slot slot2 = allUser.Root?.HeadSlot;
                if (slot2 != null)
                {
                    float3 a = slot2.GlobalPosition;
                    float3 b = slot.GlobalPosition;
                    float num = MathX.Distance(in a, in b);
                    if (num < _nearestDistance)
                    {
                        _nearestDistance = num;
                        _nearestUser = allUser;
                    }
                }
            }

            MarkChangeDirty();
        }

        protected override void OnGenerateVisual(Slot root) =>
            GenerateUI(root).GenerateListButtons(Add, Remove);

        protected override void OnEvaluate()
        {
            User.Value = _nearestUser;
            Distance.Value = _nearestDistance;
        }


        [SyncMethod]
        private void Add(IButton button, ButtonEventData eventData)
        {
            IgnoreUsers.Add();
            RefreshLogixBox();
        }

        [SyncMethod]
        private void Remove(IButton button, ButtonEventData eventData)
        {
            if (IgnoreUsers.Count <= 1) return;
            IgnoreUsers.RemoveAt(IgnoreUsers.Count - 1);
            RefreshLogixBox();
        }
    }
}
