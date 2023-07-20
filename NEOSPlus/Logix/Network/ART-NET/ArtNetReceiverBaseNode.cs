using FrooxEngine;
using FrooxEngine.LogiX;

public abstract class ArtNetReceiverBaseNode : LogixNode
{
    public readonly Input<ArtNetReceiver> Receiver;

    private ArtNetReceiver _registered;

    protected override void OnChanges()
    {
        base.OnChanges();
        ArtNetReceiver artNetReceiver = Receiver.Evaluate();
        if (artNetReceiver != _registered)
        {
            Unregister();
            if (artNetReceiver != null)
            {
                Register(artNetReceiver);
            }
            _registered = artNetReceiver;
        }
    }

    protected abstract void Register(ArtNetReceiver receiver);

    protected abstract void Unregister(ArtNetReceiver receiver);

    private void Unregister()
    {
        if (_registered != null)
        {
            Unregister(_registered);
        }
        _registered = null;
    }

    protected override void OnDispose()
    {
        Unregister();
        base.OnDispose();
    }
}