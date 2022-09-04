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
        public readonly Input<int> Submesh;
        public readonly Input<bool> JustVerts;

        public readonly Impulse OK;
        public readonly Impulse Failed;
        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                var appendmesh = Appended.Evaluate();
                var pos = Position.Evaluate();
                var rot = Rotation.Evaluate();
                var scl = Scale.Evaluate(float3.One);
                var sub = Submesh.Evaluate(-1);
                var justVerts = JustVerts.Evaluate();

                if (mesh?.Mesh == null || appendmesh?.Asset == null)
                {
                    Failed.Trigger();
                    return;

                }
                var matrix = float4x4.Transform(pos, rot, scl);
                if (sub < 0)
                {
                    mesh.Mesh.Append(appendmesh.Asset.Data, !justVerts, matrix);
                }
                else
                {
                    mesh.Mesh.Append(appendmesh.Asset.Data, !justVerts, matrix, x => sub);
                }
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}