
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseX;
using FrooxEngine;


/* This is a world template for making your own worlds from the create new session tab 
 * Good for demo components or features or to use custom materials for world presets.
 * uncomment the world preset and then change the name from NeosPlusGrid to whatever you want the preset to be named 
 * Then Build the dll the preset then will show up in the create new session tab.
 */

namespace NEOSPlus
{
    class TemplateWorld
    {
        // GridSpace World Preset
        //[WorldPreset("NeosPlusGrid", 0)] // Uncomment this to use this
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

            // Set up skybox
            ProceduralSkyMaterial proceduralSkyMaterial = w.AddSlot("Skybox").AttachSkybox<ProceduralSkyMaterial>();
            proceduralSkyMaterial.Exposure.Value = 0.8f;
            proceduralSkyMaterial.Sun.Target = light;

            // Set up ground
            Slot groundSlot = w.AddSlot("Ground");
            BoxCollider groundCollider = groundSlot.AttachComponent<BoxCollider>();
            Sync<float3> groundSize = groundCollider.Size;
            groundSize.Value = float2.One.x_y * 1000f;
            groundCollider.SetCharacterCollider();
            AttachedModel<QuadMesh, PBS_Metallic> groundModel = groundSlot.AttachMesh<QuadMesh, PBS_Metallic>();
            StaticTexture2D gridTexture = groundSlot.AttachTexture(NeosAssets.Graphics.Patterns.DevGridDark);
            gridTexture.FilterMode.Value = TextureFilterMode.Anisotropic;
            groundModel.material.AlbedoTexture.Target = gridTexture;
            groundModel.mesh.Facing = float3.Up;
            Sync<float2> meshSize = groundModel.mesh.Size;
            meshSize.Value = float2.One * 1000f;
            groundModel.mesh.ScaleUVWithSize.Value = true;

            // Set up spawn area
            Slot spawnAreaSlot = w.AddSlot("SpawnArea");
            spawnAreaSlot.GlobalPosition = new float3(0f, 0.01f);
            CommonSpawnArea spawnArea = spawnAreaSlot.AttachComponent<CommonSpawnArea>();
            CirclePointGenerator spawnCircle = spawnAreaSlot.AttachComponent<CirclePointGenerator>();
            spawnArea.SpawnPointGenerator.Target = spawnCircle;
            spawnCircle.Radius.Value = 4f;
        }
    }
}