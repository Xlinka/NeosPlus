namespace FrooxEngine
{
    [Category(new string[] {"Physics/Cloth"})]
    public class ClothCapsuleCollider : ClothCollider
    {
        public readonly Sync<float> Height;

        protected override void OnAttach()
        {
            base.OnAttach();
            Radius.Value = 0.5f;
            Height.Value = 1.0f;
        }
    }
}