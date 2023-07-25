using System.Linq;
using FrooxEngine;

[Category(new string[] { "Transform/Drivers" })]
[GenericTypes(GenericTypes.Group.Primitives)]
public class MultiValueArithmeticDriver<T> : Component
{
    public enum ArithmeticMode
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public readonly FieldDrive<T> Target;

    public readonly Sync<ArithmeticMode> Mode;

    public readonly SyncList<Sync<T>> Values;

    protected override void OnChanges()
    {
        if (!Target.IsLinkValid)
        {
            return;
        }
        if (Values.Contains(Target.Target))
        {
            Target.ReleaseLink();
            return;
        }
        T value = Values[0].Value;
        switch (Mode.Value)
        {
            case ArithmeticMode.Addition:
                foreach (Sync<T> sync in Values.Skip(1))
                {
                    value += (dynamic)sync.Value;
                }
                break;
            case ArithmeticMode.Subtraction:
                foreach (Sync<T> sync in Values.Skip(1))
                {
                    value -= (dynamic)sync.Value;
                }
                break;
            case ArithmeticMode.Multiplication:
                foreach (Sync<T> sync in Values.Skip(1))
                {
                    value *= (dynamic)sync.Value;
                }
                break;
            case ArithmeticMode.Division:
                foreach (Sync<T> sync in Values.Skip(1))
                {
                    value /= (dynamic)sync.Value;
                }
                break;
        }
        Target.Target.Value = value;
    }
}