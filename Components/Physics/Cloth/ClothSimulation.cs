using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/Cloth" })]
    public class Cloth : MeshRenderer // Need to extend MeshRenderer to avoid Unity attaching its own
    {
        [HideInInspector] // Needed to sync the reset across all users
        public readonly Sync<bool> ShouldReset;

        // Needed to seperate the cloth enabled state from the mesh rendering
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
        public readonly SyncList<ClothCoefficent> PinningCoefficients;

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

        protected override void OnStart()
        {
            base.OnStart();
            if (World.IsAuthority)
            {
                ClothColliders.ElementsAdded += (a, b, c) => ClothColliders.CleanList();
                ClothColliders.ElementsRemoved += (a, b, c) => ClothColliders.CleanList();
                ClothColliders.Changed += (a) => ClothColliders.CleanList();
            }
        }

        public override void BuildInspectorUI(UIBuilder ui)
        {
            base.BuildInspectorUI(ui);
            ui.Button("Add all colliders in world".AsLocaleKey()).SetUpActionTrigger(SetupWorldColliders);
            ui.Button("Add all user colliders".AsLocaleKey()).SetUpActionTrigger(SetupUserColliders);
            ui.Button("Remove all colliders".AsLocaleKey()).SetUpActionTrigger(ClearColliders);
            ui.Button("Remove all pinning coefficients".AsLocaleKey()).SetUpActionTrigger(ClearCoefficients);
            ui.Button("Reset cloth simulation".AsLocaleKey()).SetUpActionTrigger(Reset);
        }

        [ImpulseTarget]
        public void SetupWorldColliders()
        {
            foreach (var slot in Engine.Current.WorldManager.FocusedWorld.RootSlot.GetAllChildren())
            {
                foreach (var sphere in slot.GetComponentsInChildren<ClothSphereCollider>())
                    ClothColliders.AddUnique(sphere);

                foreach (var capsule in slot.GetComponentsInChildren<ClothCapsuleCollider>())
                    ClothColliders.AddUnique(capsule);
            }
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

        [ImpulseTarget] public void ClearColliders() => ClothColliders.Clear();
        [ImpulseTarget] public void ClearCoefficients() => PinningCoefficients.Clear();
        [ImpulseTarget] public void Reset() => ShouldReset.Value = true;
    }
}


