using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

/// credit
/// faloan
/// 
namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class ClearMesh : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Impulse Cleared;
   
        [ImpulseTarget]
        public void Process()
        {
            var mesh = DynamicMesh.Evaluate();
           
            if (mesh?.Mesh == null )
            {
            }
            else
            {
                mesh.Mesh.Clear();
            }

            Cleared.Trigger();
        }
    }
}