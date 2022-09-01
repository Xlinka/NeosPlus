using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
// Rad was here
namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class GetBoneBinding : LogixOperator<BoneBinding>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;

        public override BoneBinding Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                if(mesh?.Asset?.Data == null)
                {
                    return new BoneBinding();
                }
                return mesh.Asset.Data.RawBoneBindings[Index.Evaluate()];
            }
        }
    }
}