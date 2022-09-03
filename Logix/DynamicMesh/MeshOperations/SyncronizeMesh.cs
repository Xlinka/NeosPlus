using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
using System.Runtime.CompilerServices;

/// credit
/// faloan
/// 
namespace FrooxEngine.LogiX.Assets
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class SyncronizeMesh : LogixNode
    {
        public readonly Input<DynamicMeshEX> DynamicMesh;
        public readonly Impulse OnRefresh;

        [ImpulseTarget]
        public void Syncronize()
        {
            DynamicMesh.Evaluate()?.SyncronizeMesh();
            OnRefresh.Trigger();
        }
       
    }
}
