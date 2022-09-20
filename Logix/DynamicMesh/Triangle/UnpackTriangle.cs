using BaseX;

namespace FrooxEngine.LogiX.Operators
{
	[Category("LogiX/Mesh/Triangle")]
	public class UnpackTriangle : LogixNode
	{
		public readonly Input<Triangle> Triangle;

		public readonly Output<Vertex> Point0;
		public readonly Output<int> Point0Index;

		public readonly Output<Vertex> Point1;
		public readonly Output<int> Point1Index;

		public readonly Output<Vertex> Point2;
		public readonly Output<int> Point2Index;

		protected override void OnEvaluate()
		{
			var tri = Triangle.Evaluate();
			Point0.Value = tri.Vertex0;
			Point1.Value = tri.Vertex1;
			Point2.Value = tri.Vertex2;
			Point0Index.Value = tri.Vertex0Index;
			Point1Index.Value = tri.Vertex1Index;
			Point2Index.Value = tri.Vertex2Index;
		}

		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)Point0).NotifyChange();
			((IOutputElement)Point0Index).NotifyChange();

			((IOutputElement)Point1).NotifyChange();
			((IOutputElement)Point1Index).NotifyChange();

			((IOutputElement)Point2).NotifyChange();
			((IOutputElement)Point2Index).NotifyChange();
		}
	}
}