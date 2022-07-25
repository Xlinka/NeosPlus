using FrooxEngine.LogiX;
using FrooxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine.Logix.Slots
{
    [Category("LogiX/Slots")]
    [NodeName("Get Grandparent")]
    public class GetGrandparent : LogixOperator<Slot>
    {
        public readonly Input<Slot> Instance;
        public readonly Input<int> Grandparent;

        public override Slot Content
        {
            get
            {
                if (Instance.EvaluateRaw() == null || Grandparent.EvaluateRaw() < 0)
                {
                    return null;
                }

                Slot gp = Instance.EvaluateRaw();

                // We can also return all parents using GetAllParents/EnumerateParents
                for (int i = 0; i < Grandparent.EvaluateRaw(); i++)
                {
                    if (gp.Parent != null)
                    {
                        gp = gp.Parent;
                    }
                    else
                    {
                        return null;
                    }
                }

                return gp;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}
