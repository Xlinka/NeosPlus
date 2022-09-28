using BaseX;

namespace FrooxEngine;

[Category("Data")]
public class ReferenceFieldList<T> : Component where T : class, IWorldElement
{
    public readonly SyncRefList<T> Value;
}