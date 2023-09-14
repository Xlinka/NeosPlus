using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "NeosPlus/Assets/Procedural Meshes" })]
    public class MengerSpongeMesh : ProceduralMesh
    {
        [Range(1, 4)] public readonly Sync<int> Subdivisions;
        private MengerSponge sponge;
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
            sponge = null;
        }

        protected override void UpdateMeshData(MeshX meshx)
        {
            bool value = false;
            if (sponge == null || sponge.Subdivisions != _subdivisions)
            {
                sponge?.Remove();
                sponge = new MengerSponge(meshx, _subdivisions);
                value = true;
            }

            sponge.Subdivisions = Subdivisions.Value;
            sponge.Update();
            uploadHint[MeshUploadHint.Flag.Geometry] = value;
        }
    }
}