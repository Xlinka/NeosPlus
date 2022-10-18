using System;
using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]
public class Hologram : SingleShaderMaterialProvider
{
	protected override Uri ShaderURL => ShaderInjection.HologramV2;

	// Main
	public readonly AssetRef<ITexture2D> MainTexture;
	public readonly Sync<color> MainColor;
	public readonly AssetRef<ITexture2D> EmissionTexture;
	public readonly Sync<color> EmissionColor;
	// Repeating ramp
	public readonly AssetRef<ITexture2D> RampTexture;
	public readonly Sync<float> Scale;
	public readonly Sync<float> ScrollSpeed;
	// Fresnel
	public readonly Sync<color> FresnelColor;
	public readonly Sync<float> FresnelExponent;

	[DefaultValue(-1)]
	public readonly Sync<int> RenderQueue;
	private static PropertyState _propertyInitializationState;

	// Main
	private static MaterialProperty _MainTexture = new("_MainTexture");
	private static MaterialProperty _MainColor = new("_MainColor");
	private static MaterialProperty _EmissionTexture = new("_EmissionTexture");
	private static MaterialProperty _EmissionColor = new("_EmissionColor");
	// Repeating ramp
	private static MaterialProperty _RampTexture = new("_RampTexture");
	private static MaterialProperty _Scale = new("_Scale");
	private static MaterialProperty _ScrollSpeed = new("_ScrollSpeed");
	// Fresnel
	private static MaterialProperty _FresnelColor = new("_FresnelColor");
	private static MaterialProperty _FresnelExponent = new("_FresnelExponent");

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
		material.UpdateTexture(_EmissionTexture, EmissionTexture);
		material.UpdateColor(_EmissionColor, EmissionColor);
		// Repeating ramp
		material.UpdateTexture(_RampTexture, RampTexture);
		material.UpdateFloat(_Scale, Scale);
		material.UpdateFloat(_ScrollSpeed, ScrollSpeed);
		// Fresnel
		material.UpdateColor(_FresnelColor, FresnelColor);
		material.UpdateFloat(_FresnelExponent, FresnelExponent);

		if (!RenderQueue.GetWasChangedAndClear()) return;
		var renderQueue = RenderQueue.Value;
		if ((int)RenderQueue == -1) renderQueue = 2600;
		material.SetRenderQueue(renderQueue);
	}

	protected override void UpdateKeywords(ShaderKeywords keywords) { }
	protected override void OnAttach()
	{
		base.OnAttach();
		MainColor.Value = new color(1f);
		Scale.Value = 10;
		ScrollSpeed.Value = 10;
		FresnelColor.Value = new color(1f);
		FresnelExponent.Value = 1;

		RampTexture.Target = World.GetSharedComponentOrCreate("HoloV2_DefaultRampTexture", delegate (StaticTexture2D tex)
		{
			tex.URL.Value = new Uri("neosdb:///e8d0be4b3d5f364eee47461fa10213fc2152b47c6ccc8a07099250f9ea945549.png");
			tex.Uncompressed.Value = false;
		}, 1);
	}
}