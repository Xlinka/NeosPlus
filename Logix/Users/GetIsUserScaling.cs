namespace FrooxEngine.LogiX.Users
{
    public class GetUserScaling
    {
        [NodeName("Get is User Scaling")]
        [Category(new string[] { "LogiX/Users" })]
        public class GetIsUserScaling : LogixOperator<bool>
        {
            public readonly Input<User> User;
            public override bool Content
            {
                get
                {
                    var state = User.EvaluateRaw()?.Root?.GetRegisteredComponent<LocomotionController>()?.ScalingEnabled.Value;
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
}
