using BaseX;
using System;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using System.Threading.Tasks;
using CodeX; 
namespace FrooxEngine
{
    //pain and suffering sponsored by faolan(rad)
    [Category("Assets/Procedural Meshes")]
    public class DynamicMeshEX : ProceduralMesh, ICustomInspector, IAssetRequester
    {
        public readonly Sync<bool> Normals;
        public readonly Sync<bool> Tangents;
        public readonly Sync<bool> Colors;
        public readonly Sync<bool> UV0s;
        public readonly Sync<bool> UV1s;
        public readonly Sync<bool> UV2s;
        public readonly Sync<bool> UV3s;
        public readonly Sync<bool> UV4s;
        public readonly Sync<bool> UV5s;
        public readonly Sync<bool> UV6s;
        public readonly Sync<bool> UV7s;
        public readonly Sync<bool> BindPoses;
        public readonly Sync<bool> BoneWeights;

        [HideInInspector,NonDrivable]
        public readonly Sync<string> meshResync;
        protected override void OnAttach()
        {
            Normals.Value = true;
            Tangents.Value = true;
            Colors.Value = true;
            UV0s.Value = true;
            UV1s.Value = false;
            UV2s.Value = false;
            UV3s.Value = false;
            UV4s.Value = false;
            UV5s.Value = false;
            UV6s.Value = false;
            UV7s.Value = false;
            BindPoses.Value = false;
            BoneWeights.Value = false;
        }

        protected override void UpdateMeshData(MeshX meshx)
        {
            uploadHint[MeshUploadHint.Flag.Normals] = Normals;
            uploadHint[MeshUploadHint.Flag.Tangents] = Tangents;
            uploadHint[MeshUploadHint.Flag.Colors] = Colors;
            uploadHint[MeshUploadHint.Flag.UV0s] = UV0s;
            uploadHint[MeshUploadHint.Flag.UV1s] = UV1s;
            uploadHint[MeshUploadHint.Flag.UV2s] = UV2s;
            uploadHint[MeshUploadHint.Flag.UV3s] = UV3s;
            uploadHint[MeshUploadHint.Flag.UV4s] = UV4s;
            uploadHint[MeshUploadHint.Flag.UV5s] = UV5s;
            uploadHint[MeshUploadHint.Flag.UV6s] = UV6s;
            uploadHint[MeshUploadHint.Flag.UV7s] = UV7s;
            uploadHint[MeshUploadHint.Flag.BindPoses] = BindPoses;
            uploadHint[MeshUploadHint.Flag.BoneWeights] = BoneWeights;
            meshx.HasNormals = Normals;
            meshx.HasTangents = Tangents;
            meshx.HasColors = Colors;
            meshx.HasUV0s = UV0s;
            meshx.HasUV1s = UV1s;
            meshx.HasUV2s = UV2s;
            meshx.HasUV3s = UV3s;
            meshx.SetHasUV(4, UV4s);
            meshx.SetHasUV(5, UV5s);
            meshx.SetHasUV(6, UV6s);
            meshx.SetHasUV(7, UV7s);
            meshx.HasBoneBindings = BoneWeights;
        }
        public MeshX Mesh => meshx;
        
        protected override void ClearMeshData()
        {
            meshx?.Clear();
        }

        public new void BuildInspectorUI(UIBuilder ui)
        {
            base.BuildInspectorUI(ui);
            ui.Button("Refresh Mesh", OnRefreshMesh);
            ui.Button("Clear Mesh", OnClearMesh);
            ui.Button("Syncronize Mesh", OnSyncronizeMesh);
        }

        [SyncMethod]
        private void OnClearMesh(IButton button, ButtonEventData eventData)
        {
            button.Enabled = false;
            ClearMesh();
        }
        [ImpulseTarget]
        public void ClearMesh()
        {
            Mesh?.Clear();
            Mesh?.ClearBones();
            Mesh?.ClearSubmeshes();
        }

        [SyncMethod]
        private void OnRefreshMesh(IButton button, ButtonEventData eventData)
        {
            button.Enabled = false;
            RefreshMesh();
        }
        [ImpulseTarget]
        public void RefreshMesh()
        {
            MarkChangeDirty();
        }

        [SyncMethod]
        private void OnSyncronizeMesh(IButton button, ButtonEventData eventData)
        {
            button.Enabled = false;
            SyncronizeMesh();
        }
        [ImpulseTarget]
        public void SyncronizeMesh()
        {

            StartTask(SyncronizeMeshAsync);
        }
        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            if (!string.IsNullOrEmpty(meshResync))
            {
                if (!_isprocecing)
                {
                    StartTask(MeshReciever);
                }
            }
        }
        bool _isprocecing;
        private async Task MeshReciever()
        {
            _isprocecing = true;
            var uri = new Uri(meshResync);
            if (uri.Scheme == "local")
            {
                World.AssetManager.RegisterLocalAsset(uri);
            }
            await default(ToBackground);
            if (IsDisposed) { _isprocecing = false; return; }
            MeshMetadata metadata = await Engine.AssetManager.RequestMetadata<MeshMetadata>(uri).ConfigureAwait(false);
            if (IsDisposed) { _isprocecing = false; return; }
            MeshVariantDescriptor meshvariant = new MeshVariantDescriptor(true);
            Engine.AssetManager.RequestAsset<Mesh>(uri, meshvariant, this, metadata);
            _isprocecing = false;
        }
        public async Task SyncronizeMeshAsync()
        {
            AssetLoader<Mesh> loader = null;
            if(AssetReferenceCount == 0)
            {
                loader = this.ForceLoad();
            }
            while(Asset?.Data == null)
            {
                await default(NextUpdate);
            }
            var bakelock = new object();
            await Asset.RequestReadLock(bakelock);
            await default(ToBackground);
            Uri uri;
            try
            {
                uri = await Engine.LocalDB.SaveAssetAsync(Asset.Data).ConfigureAwait(false);
            }
            finally
            {
                Asset.ReleaseReadLock(bakelock);
            }
            await default(ToWorld);
            meshResync.Value = uri.ToString();
            loader?.Destroy();
        }

        public void AssignAsset(Asset asset)
        {
            
        }

        public void AssetLoadStateUpdated(Asset asset)
        {
            if(asset.AssetURL != new Uri(meshResync.Value))
            {
                return;
            }
            if(asset.LoadState == AssetLoadState.FullyLoaded)
            {
                if(asset is Mesh mesh) { loadedmesh(mesh); }
                meshResync.DirectValue = null;
            }
        }
        private void loadedmesh(Mesh mesh)
        {
            ClearMesh();
            Mesh.Append(mesh.Data);
            mesh.Unload();
        }
    }
}
