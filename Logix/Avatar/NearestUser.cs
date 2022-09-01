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
            var slot = Reference.Evaluate() ?? Slot;
            var user = IgnoreUser.Evaluate();
            var flag = IgnoreAFK.Evaluate(def: false);
            _nearestDistance = float.MaxValue;
            _nearestUser = null;
            foreach (var allUser in World.AllUsers)
            {
                if (allUser == user || (!allUser.IsPresentInWorld && flag)) continue;
                var slot2 = allUser.Root?.HeadSlot;
                if (slot2 == null) continue;
                var a = slot2.GlobalPosition;
                var b = slot.GlobalPosition;
                var num = MathX.Distance(in a, in b);
                if (!(num < _nearestDistance)) continue;
                _nearestDistance = num;
                _nearestUser = allUser;
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
