using BaseX;

namespace FrooxEngine;

[Category("Data")]
[GenericTypes(GenericTypes.Group.NeosPrimitives)]
public class ValueFieldList<T> : Component
{
    public readonly SyncFieldList<T> Value;
    public static bool IsValidGenericType => Coder<T>.IsNeosPrimitive;
}