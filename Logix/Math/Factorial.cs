﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Factorial")]
    [Category(new string[] { "LogiX/Operators" })]

    public sealed class Factorial : LogixOperator<int>
    {
        public readonly Input<int> input;
        public readonly Output<int> output;

        public override int Content
        {
            get
            {
                int fact = 1;
                for (var i = 1; i <= input.EvaluateRaw(); i++)
                {
                    fact *= i;
                }
                return fact;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}