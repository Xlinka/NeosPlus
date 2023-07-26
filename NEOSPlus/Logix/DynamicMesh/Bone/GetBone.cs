using BaseX;
using FrooxEngine.LogiX;

// Rad was here
namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/Mesh/Bone")]
    public class GetBoneBinding : LogixOperator<BoneBinding>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;

        public override BoneBinding Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                return mesh?.Asset?.Data == null
                    ? new BoneBinding()
                    : mesh.Asset.Data.RawBoneBindings[Index.Evaluate()];
            }
        }
    }
}