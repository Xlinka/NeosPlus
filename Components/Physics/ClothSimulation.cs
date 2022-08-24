using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/Cloth" })]
    public class Cloth : MeshRenderer
    {
        [HideInInspector]
        public readonly Sync<bool> ShouldReset;
        public readonly Sync<bool> ClothEnabled;
        public readonly Sync<float> StretchingStiffness;
        public readonly Sync<float> BendingStiffness;
        public readonly Sync<bool> UseTethers;
        public readonly Sync<bool> UseGravity;
        public readonly Sync<float> Damping;
        public readonly Sync<float3> ExternalAcceleration;
        public readonly Sync<float3> RandomAcceleration;
        public readonly Sync<float> WorldVelocityScale;
        public readonly Sync<float> WorldAccelerationScale;
        public readonly Sync<bool> UseContinuousCollision;
        public readonly Sync<float> UseVirtualParticles;
        public readonly Sync<float> SolverFrequency;
        public readonly Sync<float> SleepThreshold;

        public readonly SyncList<ClothSpherePair> ClothSpherePairColliders;
        public readonly SyncRefList<ClothCapsuleCollider> ClothCapsuleColliders;
        public readonly SyncFieldList<float3> VirtualParticleWeights;

        protected override void OnAttach()
        {
            base.OnAttach();
            ClothEnabled.Value = true;
            StretchingStiffness.Value = 1f;
            BendingStiffness.Value = 0f;
            UseTethers.Value = true;
            UseGravity.Value = false;
            Damping.Value = 0f;
            ExternalAcceleration.Value = float3.Zero;
            RandomAcceleration.Value = float3.Zero;
            WorldVelocityScale.Value = 0.5f;
            WorldAccelerationScale.Value = 1f;
            UseContinuousCollision.Value = true;
            UseVirtualParticles.Value = 1f;
            SolverFrequency.Value = 120f;
            SleepThreshold.Value = 0.1f;
        }

        public override void BuildInspectorUI(UIBuilder ui)
        {
            base.BuildInspectorUI(ui);
            ui.Button("Reset Cloth Simulation".AsLocaleKey(), Reset);
        }

        [ImpulseTarget]
        public void Reset()
        {
            ShouldReset.Value = true;
        }

        [SyncMethod]
        public void Reset(IButton button, ButtonEventData eventData)
        {
            Reset();
        }

        public class ClothSpherePair : SyncObject
        {
            public readonly SyncRef<ClothSphereCollider> a;
            public readonly SyncRef<ClothSphereCollider> b;
        }
    }
}


