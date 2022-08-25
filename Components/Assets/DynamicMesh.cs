using BaseX;
using System;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace FrooxEngine
{

    [Category("Assets/Procedural Meshes")]
    public class DynamicMesh : ProceduralMesh, ICustomInspector
    {
        public readonly Sync<bool> Colors;
        public readonly Sync<bool> Normals;
        protected override void UpdateMeshData(MeshX meshx)
        {
            uploadHint[MeshUploadHint.Flag.Colors] = Colors;
            uploadHint[MeshUploadHint.Flag.Normals] = Normals;

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
    }
}
