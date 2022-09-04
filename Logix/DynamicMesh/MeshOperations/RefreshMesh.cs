using FrooxEngine.LogiX;
/// credit
/// faloan
/// 
namespace FrooxEngine
{
    [Category("LogiX/Mesh/Operations")]
    public class RefreshMesh : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Impulse OnRefresh;

        [ImpulseTarget]
        public void DoRefresh()
        {
            DynamicMesh.Evaluate()?.RefreshMesh();
            OnRefresh.Trigger();
        }
    }
}
