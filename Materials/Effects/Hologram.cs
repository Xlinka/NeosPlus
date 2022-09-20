using System;
using BaseX;
using CloudX.Shared;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] {"Assets/Materials/NeosPlus/Effects"})]
public class Hologram : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.FragmentHologram;

    // Main
    public readonly AssetRef<ITexture2D> MainTexture;

    public readonly Sync<color> MainColor;

    // Repeating ramp
    public readonly AssetRef<ITexture2D> RampTexture;
    public readonly Sync<float> RampScale;

    public readonly Sync<float> ScrollSpeed;

    // Fresnel
    public readonly Sync<color> FresnelColor;
    public readonly Sync<float> FresnelStrength;
    public readonly Sync<float> FresnelPower;

    [DefaultValue(-1)] public readonly Sync<int> RenderQueue;
    private static PropertyState _propertyInitializationState;

    // Main
    private static MaterialProperty _MainColor = new MaterialProperty("_MainColor");

    private static MaterialProperty _MainTexture = new MaterialProperty("_MainTexture");

    // Repeating ramp
    private static MaterialProperty _RampTexture = new MaterialProperty("_RampTexture");
    private static MaterialProperty _RampScale = new MaterialProperty("_RampScale");

    private static MaterialProperty _ScrollSpeed = new MaterialProperty("_ScrollSpeed");

    // Fresnel
    private static MaterialProperty _FresnelColor = new MaterialProperty("_FresnelColor");
    private static MaterialProperty _FresnelStrength = new MaterialProperty("_FresnelStrength");
    private static MaterialProperty _FresnelPower = new MaterialProperty("_FresnelPower");

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }

    protected override void UpdateMaterial(Material material)
    {
        // Main
        material.UpdateTexture(_MainTexture, MainTexture);
        material.UpdateColor(_MainColor, MainColor);
        // Repeating ramp
        material.UpdateTexture(_RampTexture, RampTexture);
        material.UpdateFloat(_RampScale, RampScale);
        material.UpdateFloat(_ScrollSpeed, ScrollSpeed);
        // Fresnel
        material.UpdateColor(_FresnelColor, FresnelColor);
        material.UpdateFloat(_FresnelStrength, FresnelStrength);
        material.UpdateFloat(_FresnelPower, FresnelPower);

        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int) RenderQueue == -1) renderQueue = 2600;
        material.SetRenderQueue(renderQueue);
    }

    protected override void UpdateKeywords(ShaderKeywords keywords)
    {
    }
}