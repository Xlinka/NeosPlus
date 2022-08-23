using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine.LogiX.Users
{
    public class GetScalingEnabled
    {
        [NodeName("Get User Scaling")]
        [Category(new string[] { "LogiX/Users" })]
        public class GetUserScaling : LogixOperator<bool>
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
