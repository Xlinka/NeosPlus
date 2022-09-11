using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Assets/Procedural Meshes" })]
    public class SierpinskiPyramidMesh : ProceduralMesh
    {
        [Range(1, 8)]
		public readonly Sync<int> Subdivisions;
		private SierpinskiPyramid pyramid;
		private int _subdivisions;

		protected override void OnAwake()
		{
			base.OnAwake();
			Subdivisions.Value = 1;
		}

		protected override void PrepareAssetUpdateData()
		{
			_subdivisions = Subdivisions.Value;
		}

		protected override void ClearMeshData()
		{
			pyramid = null;
		}

		protected override void UpdateMeshData(MeshX meshx)
		{
			bool value = false;
			if (pyramid == null || pyramid.Subdivisions != _subdivisions)
			{
				pyramid?.Remove();
				pyramid = new SierpinskiPyramid(meshx, _subdivisions);
				value = true;
			}
			pyramid.Subdivisions = Subdivisions.Value;
			pyramid.Update();
			uploadHint[MeshUploadHint.Flag.Geometry] = value;
		}
	}
}
