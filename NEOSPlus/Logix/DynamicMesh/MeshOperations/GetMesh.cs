using FrooxEngine.CommonAvatar;

/// credit
/// art
/// 
namespace FrooxEngine.LogiX.Assets
{
    [Category("LogiX/Mesh/Operations")]
    public class GetMesh : LogixOperator<IAssetProvider<Mesh>>
    {
        public readonly Input<Slot> Root;

        public override IAssetProvider<Mesh> Content
        {
            get
            {
                var slot = Root.Evaluate();
                var component = slot.GetComponent<IAssetProvider<Mesh>>();
                if (component == null) return null;
                var assets = slot.World.AssetsSlot;
                if (slot.FindParent(i => assets == i) != null || slot.IsAvatarProtected())
                    return null;
                return component;
            }
        }
    }
}