using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Bone" })]
    public class AddBone : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<string> Name;


        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {

                //mesh.Asset.Data.AddBone();
                //mesh.Asset.Data.RemoveBone()
                var mesh = DynamicMesh.Evaluate();
                var name = Name.Evaluate();
                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;

                }
                mesh.Mesh.AddBone(name);
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}