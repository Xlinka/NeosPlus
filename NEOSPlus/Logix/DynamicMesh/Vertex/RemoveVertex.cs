using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category("LogiX/Mesh/Vertex")]
    public class RemoveVertex : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;


        public readonly Input<int> Index;

        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                var index = Index.Evaluate();
                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;
                }

                if (mesh?.Mesh.VertexCount <= index)
                {
                    Failed.Trigger();
                    return;
                }

                mesh.Mesh.RemoveVertex(index);

                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}