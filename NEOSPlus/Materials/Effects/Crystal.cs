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
    public readonly Sync<float> Metallic;
    public readonly Sync<float> Gloss;
    public readonly Sync<float> Repetition;
    public readonly Sync<float> ColorLoop;
    public readonly Sync<float> Width;
    public readonly Sync<float> ColorLevel;
    public readonly Sync<float> PastelColor;
    public readonly Sync<float> Distortion;
    public readonly Sync<float> ChromaticAberration;
    public readonly Sync<int> LightCompletion;
    public readonly Sync<float> Cutoff;

    private static MaterialProperty _BaseColorTextureProp = new MaterialProperty("_BaseColorTexture");
    private static MaterialProperty _BaseColorProp = new MaterialProperty("_BaseColor");
    private static MaterialProperty _SurfaceColorTextureProp = new MaterialProperty("_SurfaceColorTexture");
    private static MaterialProperty _SurfaceColorProp = new MaterialProperty("_SurfaceColor");
    private static MaterialProperty _NormalTextureProp = new MaterialProperty("_NormalTexture");
    private static MaterialProperty _AlphaTextureProp = new MaterialProperty("_AlphaTexture");
    private static MaterialProperty _MetallicProp = new MaterialProperty("_Metallic");
    private static MaterialProperty _GlossProp = new MaterialProperty("_Gloss");
    private static MaterialProperty _RepetitionProp = new MaterialProperty("_Repetition");
    private static MaterialProperty _ColorLoopProp = new MaterialProperty("_ColorLoop");
    private static MaterialProperty _WidthProp = new MaterialProperty("_Width");
    private static MaterialProperty _ColorLevelProp = new MaterialProperty("_ColorLevel");
    private static MaterialProperty _PastelColorProp = new MaterialProperty("_PastelColor");
    private static MaterialProperty _DistortionProp = new MaterialProperty("_Distortion");
    private static MaterialProperty _ChromaticAberrationProp = new MaterialProperty("_ChromaticAberration");
    private static MaterialProperty _LightCompletionProp = new MaterialProperty("_LightCompletion");
    private static MaterialProperty _CutoffProp = new MaterialProperty("_Cutoff");

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
        //add default values when i wakeup - xlinka
    }

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateTexture(_BaseColorTextureProp, BaseColorTexture);
        material.UpdateColor(_BaseColorProp, BaseColor);
        material.UpdateTexture(_SurfaceColorTextureProp, SurfaceColorTexture);
        material.UpdateColor(_SurfaceColorProp, SurfaceColor);
        material.UpdateTexture(_NormalTextureProp, NormalTexture);
        material.UpdateTexture(_AlphaTextureProp, AlphaTexture);
        material.UpdateFloat(_MetallicProp, Metallic);
        material.UpdateFloat(_GlossProp, Gloss);
        material.UpdateFloat(_RepetitionProp, Repetition);
        material.UpdateFloat(_ColorLoopProp, ColorLoop);
        material.UpdateFloat(_WidthProp, Width);
        material.UpdateFloat(_ColorLevelProp, ColorLevel);
        material.UpdateFloat(_PastelColorProp, PastelColor);
        material.UpdateFloat(_DistortionProp, Distortion);
        material.UpdateFloat(_ChromaticAberrationProp, ChromaticAberration);
        material.UpdateInt(_LightCompletionProp, LightCompletion);
        material.UpdateFloat(_CutoffProp, Cutoff);
    }
}

