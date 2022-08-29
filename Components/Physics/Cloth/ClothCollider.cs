namespace FrooxEngine
{
    public abstract class ClothCollider : ImplementableComponent
    {
        public readonly Sync<float> Radius;

        protected override void OnAttach()
        {
            base.OnAttach();
            Slot.Tag = "Cloth Collider";
        }
    }
}
