using UnityEngine;
using UnityNeos;

namespace FrooxEngine
{
    public class ClothConnector : ComponentConnector<Cloth>
    {
        private GameObject go;
		private UnityEngine.Cloth UnityCloth { get; set; }
        private UnityEngine.SkinnedMeshRenderer SkinnedMeshRenderer { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            go = new GameObject("");
            go.transform.SetParent(attachedGameObject.transform, worldPositionStays: false);
            go.layer = attachedGameObject.layer;
            UnityCloth = go.AddComponent<UnityEngine.Cloth>(); // this attaches a skinned mesh renderer to the component
            SkinnedMeshRenderer = go.GetComponent<UnityEngine.SkinnedMeshRenderer>();
        }

        public override void ApplyChanges()
        {
            SkinnedMeshRenderer.sharedMesh = Helper.GetUnity(Owner.Mesh.Value.Asset);

            UnityCloth.stretchingStiffness = Owner.StretchingStiffness;
            UnityCloth.bendingStiffness = Owner.BendingStiffness;
            UnityCloth.useTethers = Owner.UseTethers;
            UnityCloth.useGravity = Owner.UseGravity;
            UnityCloth.damping = Owner.Damping;
            UnityCloth.externalAcceleration = Conversions.ToUnity(Owner.ExternalAcceleration);
            UnityCloth.randomAcceleration = Conversions.ToUnity(Owner.RandomAcceleration);
            UnityCloth.worldVelocityScale = Owner.WorldVelocityScale;
            UnityCloth.worldAccelerationScale = Owner.WorldAccelerationScale;
            UnityCloth.sleepThreshold = Owner.SleepThreshold;
            UnityCloth.enableContinuousCollision = Owner.UseContinuousCollision;
            UnityCloth.useVirtualParticles = Owner.UseVirtualParticles;
            UnityCloth.clothSolverFrequency = Owner.SolverFrequency;
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool)go)
                Object.Destroy(go);
            go = null;
            base.Destroy(destroyingWorld);
        }
    }
}
