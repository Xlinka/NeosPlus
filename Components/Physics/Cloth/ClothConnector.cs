using System;
using System.Linq;
using UnityEngine;
using UnityNeos;

namespace FrooxEngine
{
    public class ClothConnector : MeshRendererConnectorBase<Cloth, UnityEngine.SkinnedMeshRenderer>
    {
        private UnityEngine.Cloth UnityCloth { get; set; }
        private ClothSkinningCoefficient[] coefficients = Array.Empty<ClothSkinningCoefficient>();
        private ClothSphereColliderPair[] colliderPairs = Array.Empty<ClothSphereColliderPair>();
        
        protected override bool UseMeshFilter => false;

        protected override void OnAttachRenderer()
        {
            base.OnAttachRenderer();
            UnityCloth = MeshRenderer.gameObject.AddComponent<UnityEngine.Cloth>();        
        }
        public override void ApplyChanges()
        {
            bool flag = false;
            if (Owner.ShouldReset.DirectValue && MeshRenderer != null)
            {
                if (MeshRenderer != null && MeshRenderer.gameObject)
                {
                    UnityEngine.Object.Destroy(MeshRenderer.gameObject);
                }
                MeshRenderer = null;
                OnCleanupRenderer();
                Owner.ShouldReset.DirectValue = false;
                flag = true;
            }
            if (flag)
            {
                Owner.MaterialsChanged = true;
            }
            base.ApplyChanges();
            if (flag && Owner.Mesh.Asset != null)
            {
                AssignMesh(MeshRenderer, Owner.Mesh.Asset.GetUnity());  
            }
            if (UnityCloth is null)
                return;
            SetupCloth();
        }

        protected override void OnCleanupRenderer()
        {
            base.OnCleanupRenderer();
            UnityEngine.Object.Destroy(UnityCloth);
            UnityCloth = null;
        }

        protected override void AssignMesh(UnityEngine.SkinnedMeshRenderer renderer, UnityEngine.Mesh mesh)
        {
            renderer.sharedMesh = mesh;
        }

        private void SetupCloth()
        {
            if (Owner.ClothEnabled.WasChanged) UnityCloth.enabled = Owner.ClothEnabled;
            
            if (Owner.StretchingStiffness.WasChanged) UnityCloth.stretchingStiffness = Owner.StretchingStiffness;
            
            if (Owner.BendingStiffness.WasChanged) UnityCloth.bendingStiffness = Owner.BendingStiffness;
            
            if (Owner.UseTethers.WasChanged) UnityCloth.useTethers = Owner.UseTethers;
            
            if (Owner.UseGravity.WasChanged) UnityCloth.useGravity = Owner.UseGravity;
            
            if (Owner.Damping.WasChanged) UnityCloth.damping = Owner.Damping;
            
            if (Owner.ExternalAcceleration.WasChanged) UnityCloth.externalAcceleration = Conversions.ToUnity(Owner.ExternalAcceleration);
            
            if (Owner.RandomAcceleration.WasChanged) UnityCloth.randomAcceleration = Conversions.ToUnity(Owner.RandomAcceleration);
            
            if (Owner.WorldVelocityScale.WasChanged) UnityCloth.worldVelocityScale = Owner.WorldVelocityScale;
            
            if (Owner.WorldAccelerationScale.WasChanged) UnityCloth.worldAccelerationScale = Owner.WorldAccelerationScale;
            
            if (Owner.SleepThreshold.WasChanged) UnityCloth.sleepThreshold = Owner.SleepThreshold;

            if (Owner.UseContinuousCollision.WasChanged) UnityCloth.enableContinuousCollision = Owner.UseContinuousCollision;
            
            if (Owner.UseVirtualParticles.WasChanged) UnityCloth.useVirtualParticles = Owner.UseVirtualParticles;
            
            if (Owner.SolverFrequency.WasChanged) UnityCloth.clothSolverFrequency = Owner.SolverFrequency;
            
            var spheres = Owner.ClothColliders.Where(s => s is ClothSphereCollider).ToArray();

            bool reload = false;
            if (colliderPairs.Length != spheres.Length)
            {
                Array.Resize(ref colliderPairs, spheres.Length);
                reload = true;
            }

            for (int i = 0; i < colliderPairs.Length; i++)
            {
                var Collider = spheres[i];
                if (Owner.ClothColliders.WasChanged)
                {
                    reload = true;
                    var unityCollider = ((ClothSphereConnector)Collider.Connector).unityComponent;
                    colliderPairs[i].first = unityCollider;
                    colliderPairs[i].second = unityCollider;
                }
            }

            if (reload)
            {
                UnityCloth.sphereColliders = colliderPairs;
            }

            UnityCloth.capsuleColliders = Owner.ClothColliders
                .Where(x => x.Connector is ClothCapsuleConnector)
                .Select(x => ((ClothCapsuleConnector)x.Connector).unityComponent)
                .ToArray();

            CreateClothSkinningCoefficients();
        }

        private void CreateClothSkinningCoefficients()
        {
            if (Owner.PinningCoefficients.Count == 0)
            {
                UnityCloth.coefficients = Array.Empty<ClothSkinningCoefficient>();
                return;
            }
            
            if (coefficients.Length != Owner.PinningCoefficients.Count)
            {
                Array.Resize(ref coefficients, MeshRenderer.sharedMesh.vertexCount);
            }

            for (int i = 0; i < coefficients.Length; i++)
            {
                coefficients[i].maxDistance = float.MaxValue;
                coefficients[i].collisionSphereDistance = 0f;
            }

            foreach (var elem in Owner.PinningCoefficients)
            {
                coefficients[elem.VertexIndex.Value].maxDistance = elem.MaxDistance;
                coefficients[elem.VertexIndex.Value].collisionSphereDistance = elem.CollisionSphereDistance;
            }

            UnityCloth.coefficients = coefficients;
        }
    }
}
