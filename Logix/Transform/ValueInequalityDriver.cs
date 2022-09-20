using System.Collections.Generic;
using BaseX;
using FrooxEngine;

[Category(new string[] {"Transform/Drivers"})]
[GenericTypes(GenericTypes.Group.NeosPrimitives)]
public class ValueInequalityDriver<T> : Component
{
    public readonly RelayRef<IField<T>> TargetValue;

    public readonly Sync<T> Reference;

    public readonly FieldDrive<bool> Target;

    public readonly Sync<bool> UseApproximateComparison;

    public static bool IsValidGenericType => Coder<T>.IsNeosPrimitive;

    protected override void OnAwake()
    {
        base.OnAwake();
        UseApproximateComparison.Value = true;
    }

    protected override void OnChanges()
    {
        if (Target.IsLinkValid)
        {
            T val = default(T);
            if (TargetValue.Target != null)
            {
                val = TargetValue.Target.Value;
            }

            bool value = (!UseApproximateComparison.Value || !Coder<T>.SupportsApproximateComparison)
                ? EqualityComparer<T>.Default.Equals(val, Reference.Value)
                : Coder<T>.Approximately(val, Reference.Value);
            Target.Target.Value = !value;
        }
    }
}