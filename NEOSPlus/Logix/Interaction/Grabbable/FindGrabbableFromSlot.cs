namespace FrooxEngine.LogiX.Interaction
{
	[Category("LogiX/Interaction/Grabbable")]
	[NodeName("Find Grabbable")]
	public class FindGrabbableFromSlot : LogixNode
	{
		public readonly Input<Slot> Instance;

		public readonly Output<IGrabbable> Grabbable;

		protected override void OnEvaluate()
		{
			Slot slot = Instance.EvaluateRaw();
			if (slot == null)
			{
				Grabbable.Value = null;
			}
			else
			{
				Grabbable.Value = slot.GetComponentInParents<Grabbable>();
			}
		}
	}
}