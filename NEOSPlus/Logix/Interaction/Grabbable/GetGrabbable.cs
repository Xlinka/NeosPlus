using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Interaction
{
    [Category("LogiX/Interaction/Grabbable")]
    [NodeName("Get Grabbable")]
    public class GetGrabbable : LogixOperator<IGrabbable>
    {
        public readonly Input<Slot> Instance;

        public override IGrabbable Content
        {
            get
            {
                var instance = Instance.EvaluateRaw();

                if (instance == null)
                    return null;

                var grabbable = instance.GetComponent<Grabbable>();

                return grabbable;
            }
        }
    }
}