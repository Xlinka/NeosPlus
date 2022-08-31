using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Avatar
{
    [NodeName("Nearest User")]
    [Category("LogiX/Avatar")]
    public class NearestUser : LogixNode
    {
        public readonly Input<Slot> Reference;
        public readonly Input<User> IgnoreUser;
        public readonly Input<bool> IgnoreAFK;
        public readonly Output<User> User;
        public readonly Output<float> Distance;

        private User _nearestUser;
        private float _nearestDistance = float.MaxValue;

        protected override void OnCommonUpdate()
        {
            // Literally just Nearest Hand but without the Hand stuff
            base.OnCommonUpdate();
            Slot slot = Reference.Evaluate() ?? base.Slot;
            User user = IgnoreUser.Evaluate();
            bool flag = IgnoreAFK.Evaluate(def: false);
            _nearestDistance = float.MaxValue;
            _nearestUser = null;
            foreach (User allUser in base.World.AllUsers)
            {
                if (allUser == user || (!allUser.IsPresentInWorld && flag))
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

        protected override void OnEvaluate()
        {
            User.Value = _nearestUser;
            Distance.Value = _nearestDistance;
        }
    }
}
