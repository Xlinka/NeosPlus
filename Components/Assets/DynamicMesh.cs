using System;
using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace FrooxEngine
{

	[Category("Assets/Procedural Meshes")]
	public class DynamicMesh : ProceduralMesh, ICustomInspector
	{
		public readonly Sync<bool> Normals;
		public readonly Sync<bool> Tangents;
		public readonly Sync<bool> Colors;
		public readonly Sync<bool> UV0s;
		public readonly Sync<bool> UV1s;
		public readonly Sync<bool> UV2s;
		public readonly Sync<bool> UV3s;
		public readonly Sync<bool> UV4s;
		public readonly Sync<bool> UV5s;
		public readonly Sync<bool> UV6s;
		public readonly Sync<bool> UV7s;
		public readonly Sync<bool> BindPoses;
		public readonly Sync<bool> BoneWeights;

		protected override void OnAttach()
		{
			Normals.Value = true;
			Tangents.Value = true;
			Colors.Value = true;
			UV0s.Value = true;
			UV1s.Value = false;
			UV2s.Value = false;
			UV3s.Value = false;
			UV4s.Value = false;
			UV5s.Value = false;
			UV6s.Value = false;
			UV7s.Value = false;
			BindPoses.Value = false;
			BoneWeights.Value = false;
		}

		protected override void UpdateMeshData(MeshX meshx)
		{
			uploadHint[MeshUploadHint.Flag.Normals] = Normals;
			uploadHint[MeshUploadHint.Flag.Tangents] = Tangents;
			uploadHint[MeshUploadHint.Flag.Colors] = Colors;
			uploadHint[MeshUploadHint.Flag.UV0s] = UV0s;
			uploadHint[MeshUploadHint.Flag.UV1s] = UV1s;
			uploadHint[MeshUploadHint.Flag.UV2s] = UV2s;
			uploadHint[MeshUploadHint.Flag.UV3s] = UV3s;
			uploadHint[MeshUploadHint.Flag.UV4s] = UV4s;
			uploadHint[MeshUploadHint.Flag.UV5s] = UV5s;
			uploadHint[MeshUploadHint.Flag.UV6s] = UV6s;
			uploadHint[MeshUploadHint.Flag.UV7s] = UV7s;
			uploadHint[MeshUploadHint.Flag.BindPoses] = BindPoses;
			uploadHint[MeshUploadHint.Flag.BoneWeights] = BoneWeights;
			meshx.HasNormals = Normals;
			meshx.HasTangents = Tangents;
			meshx.HasColors = Colors;
			meshx.HasUV0s = UV0s;
			meshx.HasUV1s = UV1s;
			meshx.HasUV2s = UV2s;
			meshx.HasUV3s = UV3s;
			meshx.SetHasUV(4, UV4s);
			meshx.SetHasUV(5, UV5s);
			meshx.SetHasUV(6, UV6s);
			meshx.SetHasUV(7, UV7s);
			meshx.HasBoneBindings = BoneWeights;
		}
		public MeshX Mesh => meshx;

		protected override void ClearMeshData()
		{
			meshx?.Clear();
		}

		public new void BuildInspectorUI(UIBuilder ui)
		{
			base.BuildInspectorUI(ui);
			ui.Button("Refresh Mesh", OnRefreshMesh);
			ui.Button("Clear Mesh", OnClearMesh);
		}

		[SyncMethod]
		private void OnClearMesh(IButton button, ButtonEventData eventData)
		{
			button.Enabled = false;
			ClearMesh();
		}
		[ImpulseTarget]
		public void ClearMesh()
		{
			Mesh?.Clear();
			Mesh?.ClearBones();
			Mesh?.ClearSubmeshes();
		}

		[SyncMethod]
		private void OnRefreshMesh(IButton button, ButtonEventData eventData)
		{
			button.Enabled = false;
			RefreshMesh();
		}
		[ImpulseTarget]
		public void RefreshMesh()
		{
			MarkChangeDirty();
		}
	}
}
