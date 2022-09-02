namespace FrooxEngine.LogiX.Users
{
    public class SetUserScaling
    {
        [NodeName("Set is User Scaling")]
        [Category(new string[] { "LogiX/Users" })]
        public class SetISUserScaling : LogixNode
        {        
            public readonly Input<User> User;
            public readonly Input<bool> Value;
            public readonly Impulse OnDone;
            public readonly Impulse OnFail;

            [ImpulseTarget]
            public void Set()
            {
                var locomotionModule = User.EvaluateRaw()?.Root?.GetRegisteredComponent<LocomotionController>();
                if (locomotionModule != null)
                {
                    locomotionModule.ScalingEnabled.Value = Value.EvaluateRaw();
                    OnDone.Trigger();
                }
                else
                {
                    OnFail.Trigger();
                }
            }
        }
    }
}
