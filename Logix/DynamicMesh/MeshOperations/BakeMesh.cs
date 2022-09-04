using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
/// credit
/// faloan
/// 
namespace FrooxEngine.LogiX.Assets
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class BakeMesh : LogixNode
    {
        public readonly Input<ProceduralMesh> Mesh;
        public readonly Impulse OnRefresh;

        [ImpulseTarget]
        public void Syncronize()
        {

            Mesh.Evaluate()?.BakeMesh();
            OnRefresh.Trigger();
        }
       
    }
}
