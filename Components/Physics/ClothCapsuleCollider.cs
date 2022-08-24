namespace FrooxEngine
{
    [Category(new string[] { "Physics/Cloth" })]
    public class ClothCapsuleCollider : ImplementableComponent
    {
        public readonly Sync<float> Height;
        public readonly Sync<float> Radius;

        protected override void OnAttach()
        {
            base.OnAttach();
            Radius.Value = 0.5f;
            Height.Value = 1.0f;
        }
    }
}
