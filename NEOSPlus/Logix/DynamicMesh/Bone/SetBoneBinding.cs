using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category("LogiX/NeosPlus/Mesh/Bone")]
    public class SetBoneBinding : LogixNode
    {
        public readonly Input<Vertex> Vertex;
        [OldName("BoneBinging")] public readonly Input<BoneBinding> BoneBinding;
        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var vert = Vertex.Evaluate();
                if (!Vertex.IsConnected)
                {
                    Failed.Trigger();
                    return;
                }

                vert.BoneBinding = BoneBinding.Evaluate();
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}