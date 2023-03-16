using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Assets/Procedural Meshes" })]
    public class MobiusStripMesh : ProceduralMesh
    {
        [Range(3, 50)] public readonly Sync<int> Sides;
        [Range(3, 50)] public readonly Sync<int> Resolution;
        public readonly Sync<float> Width;

        private MobiusStrip strip;
        private int _sides;
        private int _resolution;
        private float _width;

        protected override void OnAwake()
        {
            base.OnAwake();
            Sides.Value = 10;
            Resolution.Value = 10;
            Width.Value = 0.5f;
        }

        protected override void PrepareAssetUpdateData()
        {
            _sides = Sides.Value;
            _resolution = Resolution.Value;
            _width = Width.Value;
        }

        protected override void ClearMeshData()
        {
            strip = null;
        }

        protected override void UpdateMeshData(MeshX meshx)
        {
            bool value = false;
            if (strip == null || strip.Sides != _sides || strip.Resolution != _resolution || MathX.Abs(strip.Width - _width) > MathX.FLOAT_EPSILON)
            {
                strip?.Remove();
                strip = new MobiusStrip(meshx, _sides, _resolution, _width);
                value = true;
            }

            strip.Sides = _sides;
            strip.Resolution = _resolution;
            strip.Width = _width;
            strip.Update();
            uploadHint[MeshUploadHint.Flag.Geometry] = value;
        }
    }
}