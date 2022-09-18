using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
	[Category("LogiX/Mesh/Vertex")]
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
				Vertex.Value = VertexCopy.IsConnected
					? mesh.Mesh.AddVertex(VertexCopy.Evaluate())
					: mesh.Mesh.AddVertex();
				OK.Trigger();
			}
			catch
			{
				Failed.Trigger();
			}
		}
	}
}