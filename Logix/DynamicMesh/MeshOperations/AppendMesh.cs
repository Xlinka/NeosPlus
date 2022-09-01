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
    public class AppendMesh : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<IAssetProvider<Mesh>> Appended;
        public readonly Input<float3> Position;
        public readonly Input<floatQ> Rotation;
        public readonly Input<float3> Scale;
        public readonly Input<bool> Submesh;
        public readonly Input<int> SubmeshIndex;

        public readonly Impulse OK;
        public readonly Impulse Failed;
        [ImpulseTarget]
        public void Process()
        {
            var mesh = DynamicMesh.Evaluate();
            var appendmesh = Appended.Evaluate();
            var pos = Position.Evaluate();
            var rot = Rotation.Evaluate();
            var scl = Scale.Evaluate(float3.One);
            var sub = Submesh.Evaluate(true);
            var mi = SubmeshIndex.Evaluate();
            if (mesh?.Mesh == null || appendmesh?.Asset == null)
            {
                Failed.Trigger();
                return;

            }
            var matrix = float4x4.Transform(pos, rot, scl);
            if(sub) mesh.Mesh.Append(appendmesh.Asset.Data, sub, matrix);
            else mesh.Mesh.Append(appendmesh.Asset.Data, sub, matrix, idk => mi);
            
            OK.Trigger();
        }
    }
}