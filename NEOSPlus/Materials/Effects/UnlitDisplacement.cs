using System;
using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]
public class DisplacementMaterial : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.UnlitDisplacement;

    public readonly AssetRef<ITexture2D> MainTexture;
    public readonly Sync<float> Scale;
    public readonly AssetRef<ITexture2D> DisplacementTexture;
    [Range(0f, 1f, "0.00")]
    public readonly Sync<float> Displacement;
    [Range(1f, 32f, "0")]
    public readonly Sync<float> Tessellation;
    
    [DefaultValue(-1)]
    public readonly Sync<int> RenderQueue;

    private static MaterialProperty _MainTex = new MaterialProperty("_MainTex");
    private static MaterialProperty _DisplaceTexture = new MaterialProperty("_DisplacementTex");
    private static MaterialProperty _Displacement = new MaterialProperty("_Displacement");
    private static MaterialProperty _Tess = new MaterialProperty("_Tess");
    private static MaterialProperty _Scale = new MaterialProperty("_Scale");

    private static PropertyState _propertyInitializationState;
    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }

    protected override void UpdateMaterial(Material material)
    {
        material.UpdateTexture(_MainTex, MainTexture);
        material.UpdateTexture(_DisplaceTexture, DisplacementTexture);
        material.UpdateFloat(_Displacement, Displacement);
        material.UpdateFloat(_Tess, Tessellation);
        material.UpdateFloat(_Scale, Scale);

        if (RenderQueue.Value != -1)
            material.SetRenderQueue(RenderQueue.Value);
        else
            material.SetRenderQueue(2000);

        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int)RenderQueue == -1) renderQueue = 2600;
        material.SetRenderQueue(renderQueue);
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        Displacement.Value = 0.2f;
        Tessellation.Value = 1.0f;
        Scale.Value = 1.0f;
        RenderQueue.Value = -1;
    }

    protected override void UpdateKeywords(ShaderKeywords keywords)
    {
        // No additional keywords needed
    }
}
