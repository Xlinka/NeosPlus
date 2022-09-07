using FrooxEngine.LogiX;

namespace FrooxEngine
{
	[Category(new string[] { "LogiX/Mesh/Triangle" })]
	public class RemoveTriangles : LogixNode
	{
		public readonly Input<DynamicMesh> DynamicMesh;
		public readonly Input<int> Submesh;
		public readonly Input<int> Index;
		public readonly Input<int> Count;

		public readonly Impulse OK;
		public readonly Impulse Failed;
		[ImpulseTarget]
		public void Process()
		{
			try
			{
				var mesh = DynamicMesh.Evaluate();
				var sub = Submesh.Evaluate(0);
				var index = Index.Evaluate(0);
				var count = Count.Evaluate(1);
				if (mesh?.Mesh == null)
				{
					Failed.Trigger();
					return;

				}
				if (mesh?.Mesh.SubmeshCount <= sub)
				{
					Failed.Trigger();
					return;

				}
				mesh.Mesh.RemoveTriangles(index, count, sub);
				OK.Trigger();
			}
			catch
			{
				Failed.Trigger();
			}
		}
	}
}