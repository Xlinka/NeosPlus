using System;
using FrooxEngine;
using NEOSPlus.Shaders;

[Category(new string[] {"Assets/Materials/NeosPlus"})]
public class TestMaterial : SingleShaderMaterialProvider
{
    protected override Uri ShaderURL => ShaderInjection.NeosPlusTest;
    [DefaultValue(-1)] public readonly Sync<int> RenderQueue;
    private static PropertyState _propertyInitializationState;

    public override PropertyState PropertyInitializationState
    {
        get => _propertyInitializationState;
        protected set => _propertyInitializationState = value;
    }

    protected override void UpdateKeywords(ShaderKeywords keywords)
    {
    }

    protected override void UpdateMaterial(Material material)
    {
        if (!RenderQueue.GetWasChangedAndClear()) return;
        var renderQueue = RenderQueue.Value;
        if ((int) RenderQueue == -1) renderQueue = 2600;
        material.SetRenderQueue(renderQueue);
    }
}