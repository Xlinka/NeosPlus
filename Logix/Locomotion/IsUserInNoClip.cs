﻿using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Locomotion
{
	[NodeName("Is User in NoClip")]
	[Category(new string[] { "LogiX/Locomotion" })]
	public class IsUserInNoClip : LogixOperator<bool>
	{
		public readonly Input<User> User;
		public override bool Content
		{
			get
			{
				return User.EvaluateRaw()?.Root?.GetRegisteredComponent<LocomotionController>()?.ActiveModule.GetType() ==
					   typeof(NoclipLocomotion);
			}
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			MarkChangeDirty();
		}
	}
}
