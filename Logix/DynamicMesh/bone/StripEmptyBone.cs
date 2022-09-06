using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Bone" })]
    public class StripEmptyBones : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Impulse OK;
        public readonly Impulse Failed;
        public readonly Output<int> amount;
        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;

                }
                amount.Value = mesh.Mesh.StripEmptyBones();
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}