using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;

//01I Adds a integer output of bool
namespace FrooxEngine.LogiX.Operators
{
    [Category("LogiX/NeosPlus/Operators")]
    [NodeName("0 1 I")]
    public class ZeroOneI : LogixOperator<int>
    {
        public readonly Input<bool> Boolean;
        public override int Content => Boolean.EvaluateRaw() ? 1 : 0;
    }
}