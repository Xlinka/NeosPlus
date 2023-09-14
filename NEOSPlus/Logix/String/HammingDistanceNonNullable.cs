using System;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.String
{
    [Category("LogiX/NeosPlus/String")]
    [NodeName("Hamming Distance NonNullable")]
    public class HammingDistanceNonNullable : LogixOperator<int>
    {
        public readonly Input<string> String1;
        public readonly Input<string> String2;

        public override int Content
        {
            get
            {
                var string1 = String1.EvaluateRaw();
                var string2 = String2.EvaluateRaw();
                var length1 = string1?.Length;
                var length2 = string2?.Length;
                if (string1 is null || string2 is null || length1 != length2)
                    return -1;
                var count = 0;
                for (var i = 0; i < length1; i++)
                    if (string1[i] != string2[i])
                        count++;
                return count;
            }
        }
    }
}