using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;
using NEOSPlus.Shaders;

[Category(new string[] {"Assets/Materials/NeosPlus"})]
public class UnlitDisplacementMaterial : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.UnlitDisplacement;
    [DefaultValue("white")] public readonly Sync<Texture2D> MainTex;
    [DefaultValue("gray")] public readonly Sync<Texture2D> DisplacementTex;
    [DefaultValue(0.2f)] public readonly Sync<float> Displacement;
    [DefaultValue(4f)] public readonly Sync<float> Tess;
    [DefaultValue(1f)] public readonly Sync<float> Scale;

    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }

    protected override void UpdateKeywords(ShaderKeywords keywords)
    {
    }
    private static MaterialProperty _MainTex = new MaterialProperty("_MainTex");
    private static MaterialProperty _Displacement = new MaterialProperty("_Displacement");
    private static MaterialProperty _DisplacementTex = new MaterialProperty("_DisplacementTex");
    private static MaterialProperty _Tess = new MaterialProperty("_Tess");
    private static MaterialProperty _Scale = new MaterialProperty("_Scale");


    protected override void UpdateMaterial(Material material)
    {
        material.SetTexture(_MainTex, MainTex.Value);
        material.SetFloat(_Displacement, Displacement.Value);
        material.SetTexture(_DisplacementTex, DisplacementTex.Value);
        material.SetFloat(_Tess, Tess.Value);
        material.SetFloat(_Scale, Scale.Value);
    }
}
