using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Assets/Procedural Meshes" })]
    public class MobuisStripMesh : ProceduralMesh
    {
		public readonly Sync<int> Resolution;
		private int _res;
		private MobiusStrip strip;
		protected override void OnAwake()
		{
			base.OnAwake();
			Resolution.Value = 100;
			_res = 100;
		}

		protected override void ClearMeshData()
        {
            strip = null;
        }

		protected override void UpdateMeshData(MeshX meshx)
		{
			bool value = false;
			if (strip == null)
			{
				strip?.Remove();
				strip = new MobiusStrip(meshx, _res);
				value = true;
			}
			strip.planeResolution = _res;
			strip.Update();
			uploadHint[MeshUploadHint.Flag.Geometry] = value;
		}

		protected override void PrepareAssetUpdateData()
		{
			int a = Resolution.Value;
			_res = a;
		}
	}
}
