using FrooxEngine.CommonAvatar;

namespace FrooxEngine.LogiX.Assets
{
    [Category("LogiX/Mesh/Operations")]
    public class GetMesh : LogixOperator<Mesh>
    {
        public readonly Input<Slot> Root;
        public override Mesh Content
        {
            get
            {
                var slot = Root.Evaluate();
                var component = slot.GetComponent<Mesh>();
                if (component == null) return null;
                var assets = slot.World.AssetsSlot;
                if (slot.FindParent(i => assets == i) != null || slot.IsAvatarProtected())
                    return null;
                return component;
            }
        }
    }
}