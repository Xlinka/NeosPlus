
using BaseX;
using FrooxEngine;
using System;

namespace NEOSPlus.Shaders
{
	[ImplementableClass(true)]
	internal class ExecutionHook
	{
		#pragma warning disable CS0169
		private static Type? __connectorType; 
		private static Type? __connectorTypes; 
		#pragma warning restore CS0169
		
		static ExecutionHook()
		{
			try
			{
				Engine.Current.OnReady += () =>
				{
					ShaderInjection.AppendShaders();
				};
			}
			catch (Exception e) 
			{
				UniLog.Log($"Thrown Exception \n{e}");
			}
		}

		private static DummyConnector InstantiateConnector()
		{
			return new DummyConnector();
		}

		private class DummyConnector : IConnector
		{
			public IImplementable? Owner { get; private set; }
			public void ApplyChanges() { }
			public void AssignOwner(IImplementable owner) => Owner = owner;
			public void Destroy(bool destroyingWorld) { }
			public void Initialize() { }
			public void RemoveOwner() => Owner = null;
		}
	}
}