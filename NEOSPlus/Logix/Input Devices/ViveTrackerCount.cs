using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Input_Devices
{
    [Category("LogiX/NeosPlus/Input Devices")]
    public class ViveTrackerCount : LogixNode
    {
        public readonly Input<User> User;
        public readonly Output<int> Count;

        public readonly Sync<int> _count;

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            User user = User.Evaluate();
            if (user == base.LocalUser)
            {
                _count.Value = InputInterface.GetDevices<ViveTracker>().Count;
            }
            else
            {
                if (World.IsAuthority && user == null)
                {
                    _count.Value = -1;
                }
            }
        }

        protected override void OnEvaluate()
        {
            Count.Value = _count.Value;
        }
    }
}
