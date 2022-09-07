namespace FrooxEngine.LogiX.Users
{
	[Category(new string[] { "LogiX/Users" })]
	[NodeName("Set User Scaling")]
	public class SetUserScaling : LogixNode
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
