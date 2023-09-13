using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]
public class ParallaxMaterialOrb : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => new Uri("Custom/ParallaxOcclusionAlpha"); //new url so i can archive the old one.

    public readonly Sync<color> Color;
    public readonly AssetRef<ITexture2D> MainTex;
    public readonly Sync<float> TextureScale;
    public readonly AssetRef<ITexture2D> NormalMap;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> NormalScale;
    public readonly AssetRef<ITexture2D> ParallaxMap;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Parallax;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Glossiness;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Metallic;
    [Range(2f, 100f, "0")]
    public readonly Sync<float> ParallaxMinSamples;
    [Range(2f, 100f, "0")]
    public readonly Sync<float> ParallaxMaxSamples;
    [Range(0f, 1f, "0.00")] // New Alpha Cutoff property
    public readonly Sync<float> AlphaCutoff;

    private static MaterialProperty _Color = new MaterialProperty("_Color");
    private static MaterialProperty _MainTex = new MaterialProperty("_MainTex");
    private static MaterialProperty _TextureScale = new MaterialProperty("_TextureScale");
    private static MaterialProperty _NormalMap = new MaterialProperty("_BumpMap");
    private static MaterialProperty _NormalScale = new MaterialProperty("_BumpScale");
    private static MaterialProperty _ParallaxMap = new MaterialProperty("_ParallaxMap");
    private static MaterialProperty _Parallax = new MaterialProperty("_Parallax");
    private static MaterialProperty _Glossiness = new MaterialProperty("_Glossiness");
    private static MaterialProperty _Metallic = new MaterialProperty("_Metallic");
    private static MaterialProperty _ParallaxMinSamples = new MaterialProperty("_ParallaxMinSamples");
    private static MaterialProperty _ParallaxMaxSamples = new MaterialProperty("_ParallaxMaxSamples");
    private static MaterialProperty _AlphaCutoff = new MaterialProperty("_AlphaCutoff"); // New Alpha Cutoff property

    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;
    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }
    protected override void UpdateMaterial(Material material)
    {
        material.UpdateColor(_Color, Color);
        material.UpdateTexture(_MainTex, MainTex);
        material.UpdateFloat(_TextureScale, TextureScale);
        material.UpdateTexture(_NormalMap, NormalMap);
        material.UpdateFloat(_NormalScale, NormalScale);
        material.UpdateTexture(_ParallaxMap, ParallaxMap);
        material.UpdateFloat(_Parallax, Parallax);
        material.UpdateFloat(_Glossiness, Glossiness);
        material.UpdateFloat(_Metallic, Metallic);
        material.UpdateFloat(_ParallaxMinSamples, ParallaxMinSamples);
        material.UpdateFloat(_ParallaxMaxSamples, ParallaxMaxSamples);
        material.UpdateFloat(_AlphaCutoff, AlphaCutoff); // Update Alpha Cutoff

        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int)RenderQueue == -1) renderQueue = 2000;
        material.SetRenderQueue(renderQueue);
    }
    protected override void UpdateKeywords(ShaderKeywords keywords) { }
    protected override void OnAttach()
    {
        base.OnAttach();
        Color.Value = new color(1);
        NormalScale.Value = 1;
        Parallax.Value = 0.05f;
        Glossiness.Value = 0.5f;
        Metallic.Value = 0;
        ParallaxMinSamples.Value = 4;
        ParallaxMaxSamples.Value = 20;
        AlphaCutoff.Value = 0.5f; // Default Alpha Cutoff
    }
}
