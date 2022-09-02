using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine
{
    public class ExtendedLightWizard : WorldLightSourcesWizard
    {
		[SyncMethod]
		private void SetIntensity(IButton button, ButtonEventData eventData)
		{
			base.ForeachLight(delegate (Light l)
			{
				l.Intensity.Value *= _intensityField.Target.ParsedValue;
			});
		}

		// This method is private, can we store a reference to it at runtime?
		private void ForeachLight(Action<Light> process)
		{
			string tag = _tag.Target.TargetString;
			foreach (Light componentsInChild in (Root.Target ?? base.World.RootSlot).GetComponentsInChildren(delegate (Light l)
			{
				if (!ProcessDisabled.Value && (!l.Enabled || !l.Slot.IsActive))
				{
					return false;
				}
				return (string.IsNullOrEmpty(tag) || !(l.Slot.Tag != tag)) && l.LightType.Value switch
				{
					LightType.Point => ProcessPointLights.Value,
					LightType.Directional => ProcessDirectionalLights.Value,
					LightType.Spot => ProcessSpotLights.Value,
					_ => false,
				};
			}))
			{
				process(componentsInChild);
			}
		}

	}
}
