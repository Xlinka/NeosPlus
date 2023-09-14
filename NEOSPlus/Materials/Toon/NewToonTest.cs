using BaseX;
using FrooxEngine;


//FORK of the unity urp toon lit shader by nilocat on linka github https://github.com/Xlinka/UnityURPToonLitShader
[Category(new string[] { "Assets/Materials/NeosPlus/Toon" })]
public class NiloToon : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.NiloToonFork;

    public readonly Sync<float> IsFace;
    public readonly AssetRef<ITexture2D> BaseMap;
    public readonly Sync<Color> BaseColor;
    public readonly Sync<float> UseAlphaClipping;
    public readonly Sync<float> Cutoff;
    public readonly Sync<float> UseEmission;
    public readonly Sync<Color> EmissionColor;  
    public readonly Sync<float> EmissionMulByBaseColor;
    public readonly AssetRef<ITexture2D> EmissionMap;
    public readonly Sync<Vector> EmissionMapChannelMask;
    public readonly Sync<float> UseOcclusion;   
    public readonly Sync<float> OcclusionStrength;
    public readonly AssetRef<ITexture2D> OcclusionMap; 
    public readonly Sync<Vector> OcclusionMapChannelMask;
    public readonly Sync<float> OcclusionRemapStart;
    public readonly Sync<float> OcclusionRemapEnd;
    public readonly Sync<Color> IndirectLightMinColor;
    public readonly Sync<float> IndirectLightMultiplier;
    public readonly Sync<float> DirectLightMultiplier;
    public readonly Sync<float> CelShadeMidPoint;    
    public readonly Sync<float> CelShadeSoftness;
    public readonly Sync<float> MainLightIgnoreCelShade;
    public readonly Sync<float> AdditionalLightIgnoreCelShade;
    public readonly Sync<float> ReceiveShadowMappingAmount;
    public readonly Sync<float> ReceiveShadowMappingPosOffset;
    public readonly Sync<Color> ShadowMapColor;
    public readonly Sync<float> OutlineWidth;
    public readonly Sync<Color> OutlineColor;
    public readonly Sync<float> OutlineZOffset;
    public readonly AssetRef<ITexture2D> OutlineZOffsetMaskTex;
    public readonly Sync<float> OutlineZOffsetMaskRemapStart;
    public readonly Sync<float> OutlineZOffsetMaskRemapEnd;

    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;
    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }
    private static MaterialProperty IsFace = new MaterialProperty("_IsFace");
    private static MaterialProperty BaseMap = new MaterialProperty("_BaseMap");
    private static MaterialProperty BaseColor = new MaterialProperty("_BaseColor");
    private static MaterialProperty UseAlphaClipping = new MaterialProperty("_UseAlphaClipping");
    private static MaterialProperty Cutoff = new MaterialProperty("_Cutoff");
    private static MaterialProperty UseEmission = new MaterialProperty("_UseEmission");
    private static MaterialProperty EmissionColor = new MaterialProperty("_EmissionColor");
    private static MaterialProperty EmissionMulByBaseColor = new MaterialProperty("_EmissionMulByBaseColor");
    private static MaterialProperty EmissionMap = new MaterialProperty("_EmissionMap");
    private static MaterialProperty EmissionMapChannelMask = new MaterialProperty("_EmissionMapChannelMask");
    private static MaterialProperty UseOcclusion = new MaterialProperty("_UseOcclusion");
    private static MaterialProperty OcclusionStrength = new MaterialProperty("_OcclusionStrength");
    private static MaterialProperty OcclusionMap = new MaterialProperty("_OcclusionMap");
    private static MaterialProperty OcclusionMapChannelMask = new MaterialProperty("_OcclusionMapChannelMask");
    private static MaterialProperty OcclusionRemapStart = new MaterialProperty("_OcclusionRemapStart");
    private static MaterialProperty OcclusionRemapEnd = new MaterialProperty("_OcclusionRemapEnd");
    private static MaterialProperty IndirectLightMinColor = new MaterialProperty("_IndirectLightMinColor");
    private static MaterialProperty IndirectLightMultiplier = new MaterialProperty("_IndirectLightMultiplier");
    private static MaterialProperty DirectLightMultiplier = new MaterialProperty("_DirectLightMultiplier");
    private static MaterialProperty CelShadeMidPoint = new MaterialProperty("_CelShadeMidPoint");
    private static MaterialProperty CelShadeSoftness = new MaterialProperty("_CelShadeSoftness");
    private static MaterialProperty MainLightIgnoreCelShade = new MaterialProperty("_MainLightIgnoreCelShade");
    private static MaterialProperty AdditionalLightIgnoreCelShade = new MaterialProperty("_AdditionalLightIgnoreCelShade");
    private static MaterialProperty ReceiveShadowMappingAmount = new MaterialProperty("_ReceiveShadowMappingAmount");
    private static MaterialProperty ReceiveShadowMappingPosOffset = new MaterialProperty("_ReceiveShadowMappingPosOffset");
    private static MaterialProperty ShadowMapColor = new MaterialProperty("_ShadowMapColor");
    private static MaterialProperty OutlineWidth = new MaterialProperty("_OutlineWidth");
    private static MaterialProperty OutlineColor = new MaterialProperty("_OutlineColor");
    private static MaterialProperty OutlineZOffset = new MaterialProperty("_OutlineZOffset");
    private static MaterialProperty OutlineZOffsetMaskTex = new MaterialProperty("_OutlineZOffsetMaskTex");
    private static MaterialProperty OutlineZOffsetMaskRemapStart = new MaterialProperty("_OutlineZOffsetMaskRemapStart");
    private static MaterialProperty OutlineZOffsetMaskRemapEnd = new MaterialProperty("_OutlineZOffsetMaskRemapEnd");
    

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateFloat(IsFace, _IsFace);
        material.UpdateTexture(BaseMap, _BaseMap);
        material.UpdateColor(BaseColor, _BaseColor);
        material.UpdateFloat(UseAlphaClipping, _UseAlphaClipping);
        material.UpdateFloat(Cutoff, _Cutoff);
        material.UpdateFloat(UseEmission, _UseEmission);
        material.UpdateColor(EmissionColor, _EmissionColor);
        material.UpdateFloat(EmissionMulByBaseColor, _EmissionMulByBaseColor);
        material.UpdateTexture(EmissionMap, _EmissionMap);
        material.UpdateVector(EmissionMapChannelMask, _EmissionMapChannelMask);
        material.UpdateFloat(UseOcclusion, _UseOcclusion);
        material.UpdateFloat(OcclusionStrength, _OcclusionStrength);
        material.UpdateTexture(OcclusionMap, _OcclusionMap);
        material.UpdateVector(OcclusionMapChannelMask, _OcclusionMapChannelMask);
        material.UpdateFloat(OcclusionRemapStart, _OcclusionRemapStart);
        material.UpdateFloat(OcclusionRemapEnd, _OcclusionRemapEnd);
        material.UpdateColor(IndirectLightMinColor, _IndirectLightMinColor);
        material.UpdateFloat(IndirectLightMultiplier, _IndirectLightMultiplier);
        material.UpdateFloat(DirectLightMultiplier, _DirectLightMultiplier);
        material.UpdateFloat(CelShadeMidPoint, _CelShadeMidPoint);
        material.UpdateFloat(CelShadeSoftness, _CelShadeSoftness);
        material.UpdateFloat(MainLightIgnoreCelShade, _MainLightIgnoreCelShade);
        material.UpdateFloat(AdditionalLightIgnoreCelShade, _AdditionalLightIgnoreCelShade);
        material.UpdateFloat(ReceiveShadowMappingAmount, _ReceiveShadowMappingAmount);
        material.UpdateFloat(ReceiveShadowMappingPosOffset, _ReceiveShadowMappingPosOffset);
        material.UpdateColor(ShadowMapColor, _ShadowMapColor);
        material.UpdateFloat(OutlineWidth, _OutlineWidth);
        material.UpdateColor(OutlineColor, _OutlineColor);
        material.UpdateFloat(OutlineZOffset, _OutlineZOffset);
        material.UpdateTexture(OutlineZOffsetMaskTex, _OutlineZOffsetMaskTex);
        material.UpdateFloat(OutlineZOffsetMaskRemapStart, _OutlineZOffsetMaskRemapStart);
        material.UpdateFloat(OutlineZOffsetMaskRemapEnd, _OutlineZOffsetMaskRemapEnd);
        
        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if (RenderQueue.Value == -1) renderQueue = 2300;
        material.SetRenderQueue(renderQueue);
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        IsFace.Value = 0.0f;
        Cutoff.Value = 0.5f;
        UseEmission.Value = 0.0f;
        UseOcclusion.Value = 0.0f;
        OutlineWidth.Value = 1.0f;
        OutlineZOffset.Value = 0.0001f;
        RenderQueue.Value = -1;
    }

    protected override void UpdateKeywords(ShaderKeywords keywords) { }
}
