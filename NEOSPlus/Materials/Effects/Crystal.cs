using System;
using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]
public class CrystalMaterial : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.Crystal;

    public readonly AssetRef<ITexture2D> BaseColorTexture;
    public readonly Sync<color> BaseColor;
    public readonly AssetRef<ITexture2D> SurfaceColorTexture;
    public readonly Sync<color> SurfaceColor;
    public readonly AssetRef<ITexture2D> NormalTexture;
    public readonly AssetRef<ITexture2D> AlphaTexture;

    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Metallic;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Gloss;
    [Range(1f, 20f, "0.00")]
    public readonly Sync<float> Repetition;
    [Range(1f, 5f, "0.00")]
    public readonly Sync<float> ColorLoop;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Width;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> ColorLevel;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> PastelColor;
    [Range(0f, 10f, "0.00")]
    public readonly Sync<float> Distortion;
    [Range(-1f, 1f, "0.00")]
    public readonly Sync<float> ChromaticAberration;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<int> LightCompletion;
    [Range(0f, 1f, "0.00")]
    // needs to be a bool but how the hell do i do a material update bool i need to find a example of this.
    public readonly Sync<float> Cutoff; //supposed to be hidden but eh

    private static MaterialProperty _BaseColortex = new MaterialProperty("_BaseColortex");
    private static MaterialProperty _BasaColor = new MaterialProperty("_BasaColor");
    private static MaterialProperty _SurfaceClolrtex = new MaterialProperty("_SurfaceClolrtex");
    private static MaterialProperty _SurfaceClolr = new MaterialProperty("SurfaceClolr");
    private static MaterialProperty _Normal = new MaterialProperty("_Normal");
    private static MaterialProperty _alpha = new MaterialProperty("_alpha");
    private static MaterialProperty _Metallic = new MaterialProperty("_Metallic");
    private static MaterialProperty _GlossProp = new MaterialProperty("_Gloss");
    private static MaterialProperty _Repetition = new MaterialProperty("_Repetition");
    private static MaterialProperty _ColorLoop = new MaterialProperty("_ColorLoop");
    private static MaterialProperty _WidthProp = new MaterialProperty("_Width");
    private static MaterialProperty _ColoeLevel = new MaterialProperty("_ColoeLevel");
    private static MaterialProperty _PasutelColor = new MaterialProperty("_PasutelColor");
    private static MaterialProperty _Distortion = new MaterialProperty("_Distortion");
    private static MaterialProperty _ChromaticAberration = new MaterialProperty("_ChromaticAberration");
    private static MaterialProperty _LightCompletion = new MaterialProperty("_LightCompletion");
    private static MaterialProperty _Cutoff = new MaterialProperty("_Cutoff");

    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;

    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }
    protected override void UpdateKeywords(ShaderKeywords keywords) { }
    protected override void OnAttach()
    {
        base.OnAttach();
        Metallic.Value = 0.5f;
        Gloss.Value = 0.9f;
        Repetition.Value = 10f;
        ColorLoop.Value = 1f;
        Width.Value = 0.5f;
        ColorLevel.Value = 0.5f;
        PastelColor.Value = 0.3f;
        Distortion.Value = 5f;
        ChromaticAberration.Value = 0f;
        LightCompletion.Value = 1;
        Cutoff.Value = 0.5f;
        BaseColor.Value = new color(1f);
        SurfaceColor.Value = new color(1f);

    }

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateTexture(_BaseColortex, BaseColorTexture);
        material.UpdateColor(_BasaColor, BaseColor);
        material.UpdateTexture(_SurfaceClolrtex, SurfaceColorTexture);
        material.UpdateColor(_SurfaceClolr, SurfaceColor);
        material.UpdateTexture(_Normal, NormalTexture);
        material.UpdateTexture(_alpha, AlphaTexture);
        material.UpdateFloat(_Metallic, Metallic);
        material.UpdateFloat(_GlossProp, Gloss);
        material.UpdateFloat(_Repetition, Repetition);
        material.UpdateFloat(_ColorLoop, ColorLoop);
        material.UpdateFloat(_WidthProp, Width);
        material.UpdateFloat(_ColoeLevel, ColorLevel);
        material.UpdateFloat(_PasutelColor, PastelColor);
        material.UpdateFloat(_Distortion, Distortion);
        material.UpdateFloat(_ChromaticAberration, ChromaticAberration);
        material.UpdateInt(_LightCompletion, LightCompletion);
        material.UpdateFloat(_Cutoff, Cutoff);

        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int)RenderQueue == -1) renderQueue = 3000;
        material.SetRenderQueue(renderQueue);
    }
}

