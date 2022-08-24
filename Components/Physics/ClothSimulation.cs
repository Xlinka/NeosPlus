using BaseX;
using UnityNeos;

namespace FrooxEngine
{
    [Category(new string[] { "Rendering" })]
    public class Cloth : ImplementableComponent, IWorker, IWorldElement
    {
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
        public readonly Sync<AssetRef<Mesh>> Mesh;

        // public readonly SyncList<float> CapsuleColliders;
        // public readonly SyncList<float> SphereColliders;
        // public readonly SyncList<float> VirtualParticleWeights;

        protected override void OnAwake()
        {
            base.OnAwake();
            var grid = Slot.AttachComponent<GridMesh>();
            var render = Slot.AttachComponent<SkinnedMeshRenderer>();
            // render.Mesh.Asset = Mesh.Value.Asset;
            var mat = Slot.AttachComponent<PBS_Metallic>();
            render.Mesh.Target = grid;
            render.Materials.Add(mat);
            grid.Size.Value = new float2(10, 10);

            Slot.WorldTransformChanged += Slot_WorldTransformChanged;
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

        protected override void OnDispose()
        {
            Slot.WorldTransformChanged -= Slot_WorldTransformChanged;
            base.OnDispose();
        }

        private void Slot_WorldTransformChanged(Slot slot)
        {
            MarkChangeDirty();
        }
    }
}


