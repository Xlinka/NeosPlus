using FrooxEngine.LogiX;

/// credit
/// faloan
/// 
namespace FrooxEngine
{
	[Category("LogiX/Mesh/Operations")]
	public class ClearMesh : LogixNode
	{
		public readonly Input<DynamicMesh> DynamicMesh;
		public readonly Impulse Cleared;
		public readonly Impulse Failled;
		[ImpulseTarget]
		public void Process()
		{
			try
			{
				var mesh = DynamicMesh.Evaluate();
				if (mesh?.Mesh == null) Failled.Trigger();
				else
				{
					mesh.Mesh.Clear();
					mesh.Mesh.ClearBones();
					mesh.Mesh.ClearSubmeshes();
				}
				Cleared.Trigger();
			}
			catch
			{
				Failled.Trigger();
			}
		}
	}
}