using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category("LogiX/Mesh/Bone")]
    public class StripEmptyBones : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Impulse OK;
        public readonly Impulse Failed;
        [OldName("amount")]
        public readonly Output<int> Count;
        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;
                }
                Count.Value = mesh.Mesh.StripEmptyBones();
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}