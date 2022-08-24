using FrooxEngine;

namespace NEOSPlus.Components.Physics
{
    public class ClothSphereConnector : UnityBetterComponentConnector<ClothSphereCollider, UnityEngine.SphereCollider>
    {
        public override void ApplyChanges()
        {
            unityComponent.radius = Owner.Radius.Value;
        }
    }
}
