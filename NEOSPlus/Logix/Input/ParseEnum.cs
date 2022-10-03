using System;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category("LogiX/Input")]
    [GenericTypes(GenericTypes.Group.CommonEnums)]
    public class ParseEnum<E> : LogixOperator<E> where E : struct, Enum, IConvertible
    {
        public readonly Input<string> Canidate;
        public readonly Input<bool> IgnoreCase;
        public readonly Output<bool> IsParsed;

        public override E Content
        {
            get
            {
                IsParsed.Value = Enum.TryParse(Canidate.EvaluateRaw(), IgnoreCase.EvaluateRaw(), out E testEnum);
                return testEnum; // Logically equivalent to default(E), or the first item in an enumeration
            }
        }
    }
}