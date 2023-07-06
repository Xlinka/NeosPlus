using System;
using System.Numerics;
using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]
public class FlameMaterial : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.NiceFlame;

    public readonly AssetRef<ITexture2D> FlameNoise;
    public readonly AssetRef<ITexture2D> FlameWave;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> FlameHeight;
    [Range(0.01f, 1f, "0.00")]
    public readonly Sync<float> FlameTransparent;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> FlameYMask;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> FlameSpeed;
    [Range(-1f, 1f, "0.00")]
    public readonly Sync<float> MoveV;
    [Range(-1f, 1f, "0.00")]
    public readonly Sync<float> MoveU;
    [Range(2f, 50f, "0.00")]
    public readonly Sync<float> EdgeLength;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> TessPhongStrength;
    [Range(0f, 5f, "0.00")]
    public readonly Sync<float> RimFlameScale;
    [Range(0f, 0.3f, "0.00")]
    public readonly Sync<float> RimFlameIntensity;
    [Range(0f, 0.2f, "0.00")]
    public readonly Sync<float> RimFlameBias;

    public readonly Sync<color> FlameColor;

    public readonly Sync<color> FlameColorSecond;
    [Range(0.1f, 2f, "0.00")]
    public readonly Sync<float> FlameContrast;
    [Range(0f, 100f, "0.00")]
    public readonly Sync<float> BlinkContrastSpeed;
    [Range(1f, 10f, "0.00")]
    public readonly Sync<float> BlinkContrastStrength;

    public readonly AssetRef<ITexture2D> TextureFlame;

    public readonly Sync<int> GrayScaleTextureFlame;

    public readonly Sync<color> ColorTextureFlame;

    public readonly AssetRef<ITexture2D> DistortionMap;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> TextureFlameDistortion;
    public readonly Sync<float4> TextureFlameSpeed;
    public readonly Sync<int> RainbowFlame;
    public readonly Sync<int> XYRotatRainbowFlame;
    [Range(0f, 10f, "0.00")]
    public readonly Sync<float> RainbowFlameSpeed;
    [Range(0f, 10f, "0.00")]
    public readonly Sync<float> RainbowFlameScale;
    public readonly Sync<int> Noise;
    // all the int fields need to be a bool but idk how they keywords thing works.
    [Range(0f, 2f, "0.00")]
    public readonly Sync<float> NoiseFlameIntensity;
    [Range(0f, 10f, "0.00")]
    public readonly Sync<float> NoiseFlameSpeed;
    public readonly AssetRef<ITexture2D> Texcoord;
    [DefaultValue(1)]
    public readonly Sync<int> Dirty;


    private static MaterialProperty _FlameNoiseProp = new MaterialProperty("_Flamenoise");
    private static MaterialProperty _FlameWaveProp = new MaterialProperty("_FlameWave");
    private static MaterialProperty _FlameHeightProp = new MaterialProperty("_FlameHeight");
    private static MaterialProperty _FlameTransparentProp = new MaterialProperty("_FlameTransparent");
    private static MaterialProperty _FlameYMaskProp = new MaterialProperty("_FlameYMask");
    private static MaterialProperty _FlameSpeedProp = new MaterialProperty("_FlameSpeed");
    private static MaterialProperty _MoveVProp = new MaterialProperty("_MoveV");
    private static MaterialProperty _MoveUProp = new MaterialProperty("_MoveU");
    private static MaterialProperty _EdgeLengthProp = new MaterialProperty("_EdgeLength");
    private static MaterialProperty _TessPhongStrengthProp = new MaterialProperty("_TessPhongStrength");
    private static MaterialProperty _RimFlameScaleProp = new MaterialProperty("_oRimFlameScale");
    private static MaterialProperty _RimFlameIntensityProp = new MaterialProperty("_oRimFlameIntensity");
    private static MaterialProperty _RimFlameBiasProp = new MaterialProperty("_oRimFlameBias");
    private static MaterialProperty _FlameColorProp = new MaterialProperty("_FlameColor");
    private static MaterialProperty _FlameColorSecondProp = new MaterialProperty("_FlamecolorSecond");
    private static MaterialProperty _FlameContrastProp = new MaterialProperty("_zFlameContrast");
    private static MaterialProperty _BlinkContrastStrengthProp = new MaterialProperty("_zBlinkContrastStrength");
    private static MaterialProperty _BlinkContrastSpeedProp = new MaterialProperty("_BlinkContrastSpeedProp");
    private static MaterialProperty _TextureFlameProp = new MaterialProperty("_TextureFlame");
    private static MaterialProperty _GrayScaleTextureFlameProp = new MaterialProperty("_GrayScaleTextureFlame");
    private static MaterialProperty _ColorTextureFlameProp = new MaterialProperty("_ColorTextureFlame");
    private static MaterialProperty _DistortionMapProp = new MaterialProperty("_DistortionMap");
    private static MaterialProperty _TextureFlameDistortionProp = new MaterialProperty("_TextureFlameDistortion");
    private static MaterialProperty _TextureFlameSpeedProp = new MaterialProperty("_TextureFlameSpeed");
    private static MaterialProperty _RainbowFlameProp = new MaterialProperty("_RainbowFlame");
    private static MaterialProperty _XYRotatRainbowFlameProp = new MaterialProperty("_XYRotatRainbowFlame");
    private static MaterialProperty _RainbowFlameSpeedProp = new MaterialProperty("_oRainbowFlameSpeed");
    private static MaterialProperty _RainbowFlameScaleProp = new MaterialProperty("_oRainbowFlameScale");
    private static MaterialProperty _NoiseProp = new MaterialProperty("_Noise");
    private static MaterialProperty _NoiseFlameIntensityProp = new MaterialProperty("_xNoiseFlameIntensity");
    private static MaterialProperty _NoiseFlameSpeedProp = new MaterialProperty("_xNoiseFlameSpeed");
    private static MaterialProperty _TexcoordProp = new MaterialProperty("_texcoord");
    private static MaterialProperty _DirtyProp = new MaterialProperty("__dirty");

    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;

    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }
    protected override void OnAttach()
    {
        base.OnAttach();
        FlameHeight.Value = 0.069f;
        FlameTransparent.Value = 0.484f;
        FlameYMask.Value = 0.975f;
        FlameSpeed.Value = 0.24f;
        MoveV.Value = -1f;
        MoveU.Value = 1f;
        EdgeLength.Value = 1f;
        TessPhongStrength.Value = 0.001f;
        RimFlameScale.Value = 2.86f;
        RimFlameIntensity.Value = 0.3f;
        RimFlameBias.Value = 0.2f;
        FlameColor.Value = new color(1f);
        FlameColorSecond.Value = new color(1f);
        FlameContrast.Value = 1.3f;
        BlinkContrastSpeed.Value = 33f;
        BlinkContrastStrength.Value = 1.23f;

        FlameNoise.Target = World.GetSharedComponentOrCreate("FlameNoise", delegate (StaticTexture2D tex)
        {
            tex.URL.Value = new Uri("neosdb:///140c55bd97c21b9b22008d7054b492333fec67fbfc1a1b266e5b0209b647eed4.png");
            tex.Uncompressed.Value = false;
        }, 1);
        FlameWave.Target = World.GetSharedComponentOrCreate("FlameWave", delegate (StaticTexture2D tex)
        {
            tex.URL.Value = new Uri("neosdb:///783c233492f3dd00b80f8013c8d64d84a25eef27412a1f400249daee9c79cd63.tga");
            tex.Uncompressed.Value = false;
        }, 1);
        TextureFlame.Target = World.GetSharedComponentOrCreate("TextureFlame", delegate (StaticTexture2D tex)
        {
            tex.URL.Value = new Uri("neosdb:///0c646cd6c6287748df29e578522625785550586c11e59a58de9da5e1c18b78d5.png");
            tex.Uncompressed.Value = false;
        }, 1);
        DistortionMap.Target = World.GetSharedComponentOrCreate("DistortionMap", delegate (StaticTexture2D tex)
        {
            tex.URL.Value = new Uri("neosdb:///140c55bd97c21b9b22008d7054b492333fec67fbfc1a1b266e5b0209b647eed4.png");
            tex.Uncompressed.Value = false;
        }, 1);

    }

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateTexture(_FlameNoiseProp, FlameNoise);
        material.UpdateTexture(_FlameWaveProp, FlameWave);
        material.UpdateFloat(_FlameHeightProp, FlameHeight);
        material.UpdateFloat(_FlameTransparentProp, FlameTransparent);
        material.UpdateFloat(_FlameYMaskProp, FlameYMask);
        material.UpdateFloat(_FlameSpeedProp, FlameSpeed);
        material.UpdateFloat(_MoveVProp, MoveV);
        material.UpdateFloat(_MoveUProp, MoveU);
        material.UpdateFloat(_EdgeLengthProp, EdgeLength);
        material.UpdateFloat(_TessPhongStrengthProp, TessPhongStrength);
        material.UpdateFloat(_RimFlameScaleProp, RimFlameScale);
        material.UpdateFloat(_RimFlameIntensityProp, RimFlameIntensity);
        material.UpdateFloat(_RimFlameBiasProp, RimFlameBias);
        material.UpdateColor(_FlameColorProp, FlameColor);
        material.UpdateColor(_FlameColorSecondProp, FlameColorSecond);
        material.UpdateFloat(_FlameContrastProp, FlameContrast);
        material.UpdateFloat(_BlinkContrastSpeedProp, BlinkContrastSpeed);
        material.UpdateFloat(_BlinkContrastStrengthProp, BlinkContrastStrength);
        material.UpdateTexture(_TextureFlameProp, TextureFlame);
        material.UpdateInt(_GrayScaleTextureFlameProp, GrayScaleTextureFlame);
        material.UpdateColor(_ColorTextureFlameProp, ColorTextureFlame);
        material.UpdateTexture(_DistortionMapProp, DistortionMap);
        material.UpdateFloat(_TextureFlameDistortionProp, TextureFlameDistortion);
        material.UpdateFloat4(_TextureFlameSpeedProp, TextureFlameSpeed);
        material.UpdateInt(_RainbowFlameProp, RainbowFlame);
        material.UpdateInt(_XYRotatRainbowFlameProp, XYRotatRainbowFlame);
        material.UpdateFloat(_RainbowFlameSpeedProp, RainbowFlameSpeed);
        material.UpdateFloat(_RainbowFlameScaleProp, RainbowFlameScale);
        material.UpdateInt(_NoiseProp, Noise);
        material.UpdateFloat(_NoiseFlameIntensityProp, NoiseFlameIntensity);
        material.UpdateFloat(_NoiseFlameSpeedProp, NoiseFlameSpeed);
        material.UpdateTexture(_TexcoordProp, Texcoord);
        material.UpdateInt(_DirtyProp, Dirty);


        if (RenderQueue.Value != -1)
            material.SetRenderQueue(RenderQueue.Value);
        else
            material.SetRenderQueue(2000);

        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int)RenderQueue == -1) renderQueue = 2800;
        material.SetRenderQueue(renderQueue);
    }

    protected override void UpdateKeywords(ShaderKeywords keywords)
    { }

}

       