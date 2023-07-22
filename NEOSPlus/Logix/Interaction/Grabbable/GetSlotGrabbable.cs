namespace FrooxEngine.LogiX.Interaction
{
	[Category("LogiX/Interaction/Grabbable")]
	[NodeName("Get Slot Grabbable")]
	public class GetSlotGrabbable : LogixNode
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
				Grabbable.Value = slot.GetComponent<Grabbable>();
			}
		}
	}
}