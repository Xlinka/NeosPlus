using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Bone" })]
    public class SetBoneBinding : LogixNode
    {
        public readonly Input<Vertex> Vertex;
        public readonly Input<BoneBinding> BoneBinging;


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
                vert.BoneBinding = BoneBinging.Evaluate();
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}