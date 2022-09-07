using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;
[Category(new string[] { "Assets/Materials/NeosPlus" })]
public class TestMaterial : SingleShaderMaterialProvider
{
	protected override Uri ShaderURL => NeosAssets.Shaders.NeosPBSColorSplatSpecular; //Temp testing using the NeosPBS shader
	public readonly Sync<float> Scale;
	public readonly Sync<float3> Offset;
	[DefaultValue(-1)]
	public readonly Sync<int> RenderQueue;
	private static PropertyState _propertyInitializationState;
	private static MaterialProperty _Scale = new MaterialProperty("_Scale");
	private static MaterialProperty _Offset = new MaterialProperty("_Offset");
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
		material.UpdateFloat(_Scale, Scale);
		material.UpdateFloat3(_Offset, Offset);
		if (!RenderQueue.GetWasChangedAndClear()) return;
		var renderQueue = RenderQueue.Value;
		if ((int)RenderQueue == -1) renderQueue = 2600;
		material.SetRenderQueue(renderQueue);
	}
}