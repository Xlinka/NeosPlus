using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category("LogiX/Mesh/Triangle")]
    public class SetTriangleIndexies : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<int> Index;
        public readonly Input<int> Submesh;
        public readonly Input<int> Vertex0;
        public readonly Input<int> Vertex1;
        public readonly Input<int> Vertex2;

        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                var index = Index.Evaluate();
                var sub = Submesh.Evaluate();
                var v0 = Vertex0.Evaluate();
                var v1 = Vertex1.Evaluate();
                var v2 = Vertex2.Evaluate();
                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;
                }

                mesh.Mesh.SetTriangle(index, v0, v1, v2, sub);
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}