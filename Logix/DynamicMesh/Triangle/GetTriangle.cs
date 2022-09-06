using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh/Triangle" })]
    public class GetTriangle : LogixOperator<Triangle>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;
        public readonly Input<int> Submesh;

        public override Triangle Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                if(mesh?.Asset == null)
                {
                    return new Triangle();
                }
                return mesh.Asset.Data.GetTriangle(Index.Evaluate(), Submesh.Evaluate());
            }
        }
        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}