using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class AddVertex : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;

        public readonly Output<Vertex> Vertex;
        public readonly Input<Vertex> VertexCopy;

        public readonly Impulse OK;
        public readonly Impulse Failed;

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
                if (VertexCopy.IsConnected)
                {
                    Vertex.Value = mesh.Mesh.AddVertex(VertexCopy.Evaluate());
                }
                else
                {
                    Vertex.Value = mesh.Mesh.AddVertex();
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