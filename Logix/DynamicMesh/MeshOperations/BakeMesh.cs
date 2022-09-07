
/// credit
/// faloan
/// 
namespace FrooxEngine.LogiX.Assets
{
	[Category("LogiX/Mesh/Operations")]
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
