
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseX;
using FrooxEngine;

namespace NEOSPlus
{
    class NeosplusGridSpace
    {
        // GridSpace World Preset
        [WorldPreset("NeosPlusGrid", 0)]
        public static void GridSpace(World w)
        {
            // Create and configure user spawn and avatar
            Slot controllerSlot = w.AddSlot("Controllers");
            controllerSlot.AttachComponent<SimpleUserSpawn>();
            controllerSlot.AttachComponent<CommonAvatarBuilder>();

            // Add clipboard importer
            w.AddSlot("Clipboard Importer").AttachComponent<ClipboardImporter>();

            // Set up lighting
            Slot lightSlot = w.AddSlot("Light");
            lightSlot.GlobalPosition = float3.Up * 2f;
            lightSlot.GlobalRotation = floatQ.Euler(15f, 180f, 180f);
            Light light = lightSlot.AttachComponent<Light>();
            light.LightType.Value = LightType.Directional;
            light.Intensity.Value = 0.8f;
            light.Color.Value = new color(1f, 1f, 0.98f);
            light.ShadowType.Value = ShadowType.Hard; //less heavy than soft lighting
            light.ShadowMapResolution.Value = 1024; //who needs 8k light sources

            // Set up skybox
            Projection360Material skyboxMaterial = w.AddSlot("Skybox").AttachSkybox<Projection360Material>();
            skyboxMaterial.SetupForSkybox();
            skyboxMaterial.Exposure.Value = 0.8f;
            skyboxMaterial.Texture.Target = skyboxMaterial.Slot.AttachTexture(NeosPlusAssets.NeosplusGridSkybox);

            // Set up ground
            Slot groundSlot = w.AddSlot("Ground");
            BoxCollider groundCollider = groundSlot.AttachComponent<BoxCollider>();
            Sync<float3> groundSize = groundCollider.Size;
            groundSize.Value = float2.One.xy_ * 1000f;
            groundCollider.SetCharacterCollider();
            AttachedModel<GridMesh, PBS_Metallic> groundModel = groundSlot.AttachMesh<GridMesh, PBS_Metallic>();
            StaticTexture2D gridTexture = groundSlot.AttachTexture(NeosPlusAssets.NeosplusGridFloor);
            gridTexture.FilterMode.Value = TextureFilterMode.Anisotropic;
            groundModel.material.AlbedoTexture.Target = gridTexture;
            groundSlot.GlobalRotation = floatQ.LookRotation(float3.Down, float3.Forward);
            Sync<float2> meshSize = groundModel.mesh.Size;
            meshSize.Value = float2.One * 1000f;
            groundModel.material.TextureScale.Value = float2.One * 1000f;

            // Set up spawn area
            Slot spawnAreaSlot = w.AddSlot("SpawnArea");
            spawnAreaSlot.GlobalPosition = new float3(0f, 0.01f);
            CommonSpawnArea spawnArea = spawnAreaSlot.AttachComponent<CommonSpawnArea>();
            CirclePointGenerator spawnCircle = spawnAreaSlot.AttachComponent<CirclePointGenerator>();
            spawnArea.SpawnPointGenerator.Target = spawnCircle;
            spawnCircle.Radius.Value = 4f;

            // Set up spawn area visuals
            Slot spawnVisualSlot = spawnAreaSlot.AddSlot("Visual");
            NeosGlowCircle glowCircle = spawnVisualSlot.AttachComponent<NeosGlowCircle>();
            glowCircle.Radius.DriveFrom(spawnCircle.Radius);
            glowCircle.Height.Value = 0.2f;
            glowCircle.Color.Value = new color(0.2f, 0.2f, 0.2f, 2f);
            spawnVisualSlot.GetComponentInChildren<CylinderMesh>().Sides.Value = 32;

            // Set up material dialog
            Slot materialDialogSlot = w.AddSlot("Material Dialog");
            WorkerInspector.Create(materialDialogSlot, groundModel.material);
            materialDialogSlot.GlobalPosition = float3.Up * 1.4f + float3.Forward * 4f;

            // Set up sky dialog
            Slot skyDialogSlot = w.AddSlot("SkyDialog");
            WorkerInspector.Create(skyDialogSlot, skyboxMaterial);
            skyDialogSlot.GlobalPosition = float3.Up * 1.4f + float3.Forward * 4f + float3.Right * 0.5f;
        }
    }
}