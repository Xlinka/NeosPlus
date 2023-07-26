using System;
using FrooxEngine;
using FrooxEngine.LogiX;

[Category(new string[] { "LogiX/Network/ART-NET" })]
public class ArtNetReceiverConnect : LogixNode
{
    public readonly Input<ArtNetReceiver> Receiver;
    public readonly Input<Uri> URL;
    public readonly Input<User> HandlingUser;
    public readonly Impulse OnConnectStart;

    [ImpulseTarget]
    public void Connect()
    {
        ArtNetReceiver artNetReceiver = Receiver.Evaluate();
        if (artNetReceiver != null)
        {
            Uri value = URL.Evaluate(artNetReceiver.URL.Value);
            User target = HandlingUser.Evaluate(base.LocalUser);
            artNetReceiver.URL.Value = value;
            artNetReceiver.HandlingUser.Target = target;
            OnConnectStart.Trigger();
        }
    }
}