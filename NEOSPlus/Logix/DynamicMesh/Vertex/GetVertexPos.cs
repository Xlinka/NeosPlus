using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Mesh/Vertex")]
    public class GetVertexPos : LogixOperator<float3>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;

        public override float3 Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                return mesh?.Asset == null ? float3.Zero : mesh.Asset.Data.GetVertex(Index.Evaluate()).Position;
            }
        }
    }
}