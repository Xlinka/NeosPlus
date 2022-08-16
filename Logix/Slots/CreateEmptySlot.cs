using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Slots
{
    [Category("LogiX/Slots")]
    [NodeName("Create Empty Slot")]
    public class CreateEmptySlot : LogixNode
    {
        public readonly Impulse OnDone;
        public readonly Input<Slot> Parent;
        public readonly Input<string> Name;
        public readonly Input<string> Tag;
        public readonly Input<bool> Persistent;
        public readonly Input<bool> Active;
        public readonly Output<Slot> Slot;

        [ImpulseTarget]
        public void Create()
        {
            var parent = Parent.EvaluateRaw();
            var name = Name.EvaluateRaw();
            name = name == null ? "EmptySlot" : name;

            var newSlot = parent == null ? base.Slot.Parent.AddSlot(name) : parent.AddSlot(name);
            
            newSlot.Tag = Tag.EvaluateRaw();
            newSlot.ActiveSelf = Active.EvaluateRaw();
            newSlot.PersistentSelf = Persistent.EvaluateRaw();

            Slot.Value = newSlot;

            OnDone.Trigger();
        }
    }
}
