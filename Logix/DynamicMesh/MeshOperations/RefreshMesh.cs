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
    [Category(new string[] { "LogiX/Mesh" })]
    public class RefreshMesh : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Impulse OnRefresh;
        [ImpulseTarget]
        public void DoRefresh()
        {
            DynamicMesh.Evaluate()?.RefreshMesh();
            OnRefresh.Trigger();
        }
    }
}
