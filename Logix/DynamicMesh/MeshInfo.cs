using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine
{
	[Category(new string[] { "LogiX/Mesh" })]
	public class MeshInfo : LogixNode
	{
		public readonly Input<DynamicMesh> DynamicMesh;
		public readonly Output<int> VertexCount;
		public readonly Output<int> TriangleCount;
		public readonly Output<int> FaceCount;
		public readonly Output<int> SubmeshCount;

		public readonly Output<int> BoneCount;
		protected override void OnEvaluate()
		{
			var mesh = DynamicMesh.Evaluate();
			VertexCount.Value = mesh.Mesh.VertexCount;
			TriangleCount.Value = mesh.Mesh.TotalTriangleCount;
			FaceCount.Value = mesh.Mesh.TotalFaceCount;
			SubmeshCount.Value = mesh.Mesh.SubmeshCount;
			BoneCount.Value = mesh.Mesh.BoneCount;
		}
		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			MarkChangeDirty();
		}
	}
}