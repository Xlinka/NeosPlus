using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
	[Category("LogiX/Mesh/Vertex")]
	public class SetVertex : LogixNode
	{
		public readonly Input<Vertex> Vertex;

		public readonly Input<float3> Position;
		public readonly Input<float3> Normal;
		public readonly Input<float3> Tangent;
		public readonly Input<float4> Tangent4;
		public readonly Input<color> Color;
		public readonly Input<BoneBinding> BoneBinding;
		public readonly Input<float2> UV0;
		public readonly Input<float2> UV1;
		public readonly Input<float2> UV2;
		public readonly Input<float2> UV3;
		public readonly Impulse OK;
		public readonly Impulse Failed;

		[ImpulseTarget]
		public void Process()
		{
			try
			{
				var vert = Vertex.Evaluate();
				if (Position.IsConnected) vert.Position = Position.Evaluate();
				if (Normal.IsConnected && vert.Mesh.HasNormals) vert.Normal = Normal.Evaluate();
				if (Tangent.IsConnected && vert.Mesh.HasTangents) vert.Tangent = Tangent.Evaluate();
				if (Tangent4.IsConnected && vert.Mesh.HasTangents) vert.Tangent4 = Tangent4.Evaluate();
				if (Color.IsConnected && vert.Mesh.HasColors) vert.Color = Color.Evaluate();
				if (BoneBinding.IsConnected && vert.Mesh.HasBoneBindings) vert.BoneBinding = BoneBinding.Evaluate();
				if (UV0.IsConnected && vert.Mesh.HasUV0s) vert.UV0 = UV0.Evaluate();
				if (UV1.IsConnected && vert.Mesh.HasUV1s) vert.UV1 = UV1.Evaluate();
				if (UV2.IsConnected && vert.Mesh.HasUV2s) vert.UV2 = UV2.Evaluate();
				if (UV3.IsConnected && vert.Mesh.HasUV3s) vert.UV3 = UV3.Evaluate();
				OK.Trigger();
			}
			catch
			{
				Failed.Trigger();
			}
		}
	}
}