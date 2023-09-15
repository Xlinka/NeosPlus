namespace FrooxEngine.LogiX.Users
{
    [Category(new string[] {"LogiX/NeosPlus/Users"})]
    [NodeName("Is User Scaling")]
    public class IsUserScaling : LogixOperator<bool>
    {
        public readonly Input<User> User;

        public override bool Content
        {
            get
            {
                var state = User.EvaluateRaw()?.Root?.GetRegisteredComponent<LocomotionController>()?.ScalingEnabled
                    .Value;
                return state.HasValue ? state.Value : false;
            }
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}