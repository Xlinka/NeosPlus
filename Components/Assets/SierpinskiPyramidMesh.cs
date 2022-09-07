using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Assets/Procedural Meshes" })]
    public class SierpinskiPyramidMesh : ProceduralMesh
    {
        [Range(1, 6)]
		public readonly Sync<int> Subdivisions;
		private int _res;
		private SierpinskiPyramid pyramid;

		protected override void OnAwake()
		{
			base.OnAwake();
			Subdivisions.Value = 1;
			_res = 1;
		}

		protected override void ClearMeshData()
		{
			pyramid = null;
		}

		protected override void UpdateMeshData(MeshX meshx)
		{
			bool value = false;
			if (pyramid == null)
			{
				pyramid?.Remove();
				pyramid = new SierpinskiPyramid(meshx, _res);
				value = true;
			}
			pyramid.Update();
			uploadHint[MeshUploadHint.Flag.Geometry] = value;
		}

		protected override void PrepareAssetUpdateData()
		{
			int a = Subdivisions.Value;
			_res = a;
		}
	}
}
