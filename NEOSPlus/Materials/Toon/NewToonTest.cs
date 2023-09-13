using BaseX;
using FrooxEngine;


//FORK of the unity urp toon lit shader by nilocat on linka github https://github.com/Xlinka/UnityURPToonLitShader
[Category(new string[] { "Assets/Materials/NeosPlus/Toon" })]
public class NiloToon : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.NiloToonFork

    public readonly Sync<float> _IsFace;
    public readonly AssetRef<ITexture2D> _BaseMap;
    public readonly Sync<Color> _BaseColor;
    public readonly Sync<float> _UseAlphaClipping;
    public readonly Sync<float> _Cutoff;
    public readonly Sync<float> _UseEmission;
    public readonly Sync<Color> _EmissionColor;  
    public readonly Sync<float> _EmissionMulByBaseColor;
    public readonly AssetRef<ITexture2D> _EmissionMap;
    public readonly Sync<Vector> _EmissionMapChannelMask;
    public readonly Sync<float> _UseOcclusion;   
    public readonly Sync<float> _OcclusionStrength;
    public readonly AssetRef<ITexture2D> _OcclusionMap; 
    public readonly Sync<Vector> _OcclusionMapChannelMask;
    public readonly Sync<float> _OcclusionRemapStart;
    public readonly Sync<float> _OcclusionRemapEnd;
    public readonly Sync<Color> _IndirectLightMinColor;
    public readonly Sync<float> _IndirectLightMultiplier;
    public readonly Sync<float> _DirectLightMultiplier;
    public readonly Sync<float> _CelShadeMidPoint;    
    public readonly Sync<float> _CelShadeSoftness;
    public readonly Sync<float> _MainLightIgnoreCelShade;
    public readonly Sync<float> _AdditionalLightIgnoreCelShade;
    public readonly Sync<float> _ReceiveShadowMappingAmount;
    public readonly Sync<float> _ReceiveShadowMappingPosOffset;
    public readonly Sync<Color> _ShadowMapColor;
    public readonly Sync<float> _OutlineWidth;
    public readonly Sync<Color> _OutlineColor;
    public readonly Sync<float> _OutlineZOffset;
    public readonly AssetRef<ITexture2D> _OutlineZOffsetMaskTex;
    public readonly Sync<float> _OutlineZOffsetMaskRemapStart;
    public readonly Sync<float> _OutlineZOffsetMaskRemapEnd;
    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;
    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }
    private static MaterialProperty _IsFace = new MaterialProperty("_IsFace");
    private static MaterialProperty _BaseMap = new MaterialProperty("_BaseMap");
    private static MaterialProperty _BaseColor = new MaterialProperty("_BaseColor");
    private static MaterialProperty _UseAlphaClipping = new MaterialProperty("_UseAlphaClipping");
    private static MaterialProperty _Cutoff = new MaterialProperty("_Cutoff");
    private static MaterialProperty _UseEmission = new MaterialProperty("_UseEmission");
    private static MaterialProperty _EmissionColor = new MaterialProperty("_EmissionColor");
    private static MaterialProperty _EmissionMulByBaseColor = new MaterialProperty("_EmissionMulByBaseColor");
    private static MaterialProperty _EmissionMap = new MaterialProperty("_EmissionMap");
    private static MaterialProperty _EmissionMapChannelMask = new MaterialProperty("_EmissionMapChannelMask");
    private static MaterialProperty _UseOcclusion = new MaterialProperty("_UseOcclusion");
    private static MaterialProperty _OcclusionStrength = new MaterialProperty("_OcclusionStrength");
    private static MaterialProperty _OcclusionMap = new MaterialProperty("_OcclusionMap");
    private static MaterialProperty _OcclusionMapChannelMask = new MaterialProperty("_OcclusionMapChannelMask");
    private static MaterialProperty _OcclusionRemapStart = new MaterialProperty("_OcclusionRemapStart");
    private static MaterialProperty _OcclusionRemapEnd = new MaterialProperty("_OcclusionRemapEnd");
    private static MaterialProperty _IndirectLightMinColor = new MaterialProperty("_IndirectLightMinColor");
    private static MaterialProperty _IndirectLightMultiplier = new MaterialProperty("_IndirectLightMultiplier");
    private static MaterialProperty _DirectLightMultiplier = new MaterialProperty("_DirectLightMultiplier");
    private static MaterialProperty _CelShadeMidPoint = new MaterialProperty("_CelShadeMidPoint");
    private static MaterialProperty _CelShadeSoftness = new MaterialProperty("_CelShadeSoftness");
    private static MaterialProperty _MainLightIgnoreCelShade = new MaterialProperty("_MainLightIgnoreCelShade");
    private static MaterialProperty _AdditionalLightIgnoreCelShade = new MaterialProperty("_AdditionalLightIgnoreCelShade");
    private static MaterialProperty _ReceiveShadowMappingAmount = new MaterialProperty("_ReceiveShadowMappingAmount");
    private static MaterialProperty _ReceiveShadowMappingPosOffset = new MaterialProperty("_ReceiveShadowMappingPosOffset");
    private static MaterialProperty _ShadowMapColor = new MaterialProperty("_ShadowMapColor");
    private static MaterialProperty _OutlineWidth = new MaterialProperty("_OutlineWidth");
    private static MaterialProperty _OutlineColor = new MaterialProperty("_OutlineColor");
    private static MaterialProperty _OutlineZOffset = new MaterialProperty("_OutlineZOffset");
    private static MaterialProperty _OutlineZOffsetMaskTex = new MaterialProperty("_OutlineZOffsetMaskTex");
    private static MaterialProperty _OutlineZOffsetMaskRemapStart = new MaterialProperty("_OutlineZOffsetMaskRemapStart");
    private static MaterialProperty _OutlineZOffsetMaskRemapEnd = new MaterialProperty("_OutlineZOffsetMaskRemapEnd");
    

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateFloat(_IsFace, _IsFace);
        material.UpdateTexture(_BaseMap, _BaseMap);
        material.UpdateColor(_BaseColor, _BaseColor);
        material.UpdateFloat(_UseAlphaClipping, _UseAlphaClipping);
        material.UpdateFloat(_Cutoff, _Cutoff);
        material.UpdateFloat(_UseEmission, _UseEmission);
        material.UpdateColor(_EmissionColor, _EmissionColor);
        material.UpdateFloat(_EmissionMulByBaseColor, _EmissionMulByBaseColor);
        material.UpdateTexture(_EmissionMap, _EmissionMap);
        material.UpdateVector(_EmissionMapChannelMask, _EmissionMapChannelMask);
        material.UpdateFloat(_UseOcclusion, _UseOcclusion);
        material.UpdateFloat(_OcclusionStrength, _OcclusionStrength);
        material.UpdateTexture(_OcclusionMap, _OcclusionMap);
        material.UpdateVector(_OcclusionMapChannelMask, _OcclusionMapChannelMask);
        material.UpdateFloat(_OcclusionRemapStart, _OcclusionRemapStart);
        material.UpdateFloat(_OcclusionRemapEnd, _OcclusionRemapEnd);
        material.UpdateColor(_IndirectLightMinColor, _IndirectLightMinColor);
        material.UpdateFloat(_IndirectLightMultiplier, _IndirectLightMultiplier);
        material.UpdateFloat(_DirectLightMultiplier, _DirectLightMultiplier);
        material.UpdateFloat(_CelShadeMidPoint, _CelShadeMidPoint);
        material.UpdateFloat(_CelShadeSoftness, _CelShadeSoftness);
        material.UpdateFloat(_MainLightIgnoreCelShade, _MainLightIgnoreCelShade);
        material.UpdateFloat(_AdditionalLightIgnoreCelShade, _AdditionalLightIgnoreCelShade);
        material.UpdateFloat(_ReceiveShadowMappingAmount, _ReceiveShadowMappingAmount);
        material.UpdateFloat(_ReceiveShadowMappingPosOffset, _ReceiveShadowMappingPosOffset);
        material.UpdateColor(_ShadowMapColor, _ShadowMapColor);
        material.UpdateFloat(_OutlineWidth, _OutlineWidth);
        material.UpdateColor(_OutlineColor, _OutlineColor);
        material.UpdateFloat(_OutlineZOffset, _OutlineZOffset);
        material.UpdateTexture(_OutlineZOffsetMaskTex, _OutlineZOffsetMaskTex);
        material.UpdateFloat(_OutlineZOffsetMaskRemapStart, _OutlineZOffsetMaskRemapStart);
        material.UpdateFloat(_OutlineZOffsetMaskRemapEnd, _OutlineZOffsetMaskRemapEnd);
        
        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if (RenderQueue.Value == -1) renderQueue = 2000;
        material.SetRenderQueue(renderQueue);
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        _IsFace.Value = 0.0f;
        _Cutoff.Value = 0.5f;
        _UseEmission.Value = 0.0f;
        _UseOcclusion.Value = 0.0f;
        _OutlineWidth.Value = 1.0f;
        _OutlineZOffset.Value = 0.0001f;
        RenderQueue.Value = -1;
    }

    protected override void UpdateKeywords(ShaderKeywords keywords) { }
}
