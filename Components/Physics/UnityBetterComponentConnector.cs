using FrooxEngine;
using UnityEngine;
using UnityNeos;

namespace FrooxEngine
{
	public abstract class UnityBetterComponentConnector<C, U> : ComponentConnector<C> where C : ImplementableComponent where U : UnityEngine.Component
	{
		public U unityComponent { get; private set; }

		public override void Initialize()
		{
			base.Initialize();
			unityComponent = base.attachedGameObject.AddComponent<U>();
		}

		public override void Destroy(bool destroyingWorld)
		{
			if ((Object)unityComponent != (Object)null)
			{
				if (!destroyingWorld && (bool)(Object)unityComponent)
				{
					Object.Destroy(unityComponent);
				}
				unityComponent = null;
			}
			base.Destroy(destroyingWorld);
		}
	}
}
