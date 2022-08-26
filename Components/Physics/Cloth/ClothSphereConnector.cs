namespace FrooxEngine
{
    public class ClothSphereConnector : UnityBetterComponentConnector<ClothSphereCollider, UnityEngine.SphereCollider>
    {
        public override void ApplyChanges()
        {
            unityComponent.radius = Owner.Radius.Value;
        }
    }
}
