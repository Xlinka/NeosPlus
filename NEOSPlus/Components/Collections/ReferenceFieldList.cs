using BaseX;

namespace FrooxEngine;

[Category("NeosPlus/Data")]
public class ReferenceFieldList<T> : Component where T : class, IWorldElement
{
    public readonly SyncRefList<T> Value;
}