namespace FrooxEngine
{
    [Category(new string[] { "Physics/Cloth" })]
    public class ClothSphereCollider : ImplementableComponent
    {
        public readonly Sync<float> Radius;

        protected override void OnAttach()
        {
            base.OnAttach();
            Radius.Value = 0.5f;
        }
    }
}
