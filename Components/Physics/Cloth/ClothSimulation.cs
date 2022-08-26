using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using System;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/Cloth" })]
    public class Cloth : MeshRenderer // Need to extend MeshRenderer to avoid unity attaching its own.
    {
        [HideInInspector] // Needed to sync the reset across all users
        public readonly Sync<bool> ShouldReset; 
        public readonly Sync<bool> AddCollidersOnSpawn;
        public readonly Sync<bool> RemoveCollidersOnDestroy;

        // Need to seperate the cloth enabled state from the mesh rendering
        public readonly Sync<bool> ClothEnabled; 

        // Unity's default cloth component properties
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

        public readonly SyncRefList<ClothCollider> ClothColliders;
        public readonly SyncFieldList<float3> VirtualParticleWeights;
        public readonly SyncFieldList<float2> Coefficients;

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

            AddCollidersOnSpawn.Value = true;
            RemoveCollidersOnDestroy.Value = true;
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (World.IsAuthority)
            {
                foreach (var slot in World.RootSlot.GetAllChildren()) OnSlotAdded(slot);
                ShouldReset.Value = false;
                // World.SlotAdded += OnSlotAdded;

                ClothColliders.ElementsAdded += (a, b, c) => ClothColliders.CleanList();
                ClothColliders.ElementsRemoved += (a, b, c) => ClothColliders.CleanList();
                ClothColliders.Changed += (a) => ClothColliders.CleanList();
            }
        }

        private void OnSlotAdded(Slot slot) 
        {
            if (AddCollidersOnSpawn.Value)
            {
                var sphere = slot.GetComponent<ClothSphereCollider>();
                if (sphere != null)
                    ClothColliders.AddUnique(sphere);

                var capsule = slot.GetComponent<ClothCapsuleCollider>();
                if (capsule != null)
                    ClothColliders.AddUnique(capsule);
            }
        }

        public override void BuildInspectorUI(UIBuilder ui)
        {
            base.BuildInspectorUI(ui);
            ui.Button("Add all user colliders".AsLocaleKey()).SetUpActionTrigger(SetupUserColliders);
            ui.Button("Remove all user colliders".AsLocaleKey()).SetUpActionTrigger(RemoveUserColliders);
            ui.Button("Remove all VirtualParticleWeights".AsLocaleKey()).SetUpActionTrigger(ClearVirtualParticleWeights);
            ui.Button("Remove all ClothSpherePairColliders".AsLocaleKey()).SetUpActionTrigger(ClearClothSpherePairColliders);
            ui.Button("Remove all ClothCapsuleColliders".AsLocaleKey()).SetUpActionTrigger(ClearClothCapsuleColliders);
            ui.Button("Remove all Coefficients".AsLocaleKey()).SetUpActionTrigger(ClearCoefficients);
            ui.Button("Reset cloth simulation".AsLocaleKey()).SetUpActionTrigger(Reset);
        }

        [ImpulseTarget]
        public void SetupUserColliders()
        {
            foreach (var user in Engine.Current.WorldManager.FocusedWorld.AllUsers)
            {
                foreach (var sphere in user.Root.Slot.GetComponentsInChildren<ClothSphereCollider>())
                    ClothColliders.AddUnique(sphere);

                foreach (var capsule in user.Root.Slot.GetComponentsInChildren<ClothCapsuleCollider>())
                    ClothColliders.AddUnique(capsule);
            }   
        }
        
        [ImpulseTarget] public void RemoveUserColliders() => ClothColliders.Clear();
        [ImpulseTarget] public void ClearVirtualParticleWeights() => VirtualParticleWeights.Clear();
        [ImpulseTarget] public void ClearClothSpherePairColliders() => ClothColliders.Clear();
        [ImpulseTarget] public void ClearClothCapsuleColliders() => ClothColliders.Clear();
        [ImpulseTarget] public void ClearCoefficients() => Coefficients.Clear();
        [ImpulseTarget] public void Reset() => ShouldReset.Value = true;
    }
}


