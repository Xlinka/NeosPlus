using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Math
{
	[Category("LogiX/Mesh/Vertex")]
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