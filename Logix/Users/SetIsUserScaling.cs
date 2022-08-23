using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine.LogiX.Users
{
    public class SetScalingEnabled
    {
        [NodeName("Set User Scaling")]
        [Category(new string[] { "LogiX/Users" })]
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
}
