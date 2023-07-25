﻿using System.Linq;
using BaseX;
using FrooxEngine;

[Category(new string[] { "Transform/Drivers" })]
[GenericTypes(GenericTypes.Group.Primitives)]
public class MultiValueArithmeticDriver<T> : Component
{
    public static bool IsValidGenericType => Coder<T>.SupportsAddSub;

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
        if (Values.Count == 0)
        {
            return;
        }
        if (Values.Contains(Target.Target))
        {
            // don't let the component drive itself, don't want a feedback loop
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