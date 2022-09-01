using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class SetVertex : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<int> Index;
        public readonly Input<float3> Vertex;

        public readonly Impulse OK;
        public readonly Impulse Failed;
        [ImpulseTarget]
        public void Process()
        {
            var mesh = DynamicMesh.Evaluate();
            var index = Index.Evaluate();
            var vert = Vertex.Evaluate();
            if (mesh?.Mesh == null && index > mesh.Mesh.VertexCount)
            {
                Failed.Trigger();
                return;

            }
            mesh.Mesh.SetVertex(index, vert);
            
            OK.Trigger();
        }
    }
}