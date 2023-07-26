using System;
using BaseX;
using FrooxEngine;
using NEOSPlus.Shaders;
//Someone Else Can figure this out
//[Category(new string[] { "Assets/Materials/NeosPlus/Effects" })]    
public class WireFrame : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.WireFrame;

    public readonly Sync<color> Color;
    public readonly Sync<color> Color2;

    private static MaterialProperty _WireframeColor = new MaterialProperty("_WireframeColor");
    private static MaterialProperty _BackgroundColor = new MaterialProperty("_BackgroundColor");

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
        material.UpdateColor(_WireframeColor, Color);
        material.UpdateColor(_BackgroundColor, Color2);

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
    }
}