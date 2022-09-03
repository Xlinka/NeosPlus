using System;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Input" })]
    [GenericTypes(GenericTypes.Group.CommonEnums)]
    public class ParseEnum<E> : LogixOperator<E> where E : Enum, IConvertible
    {
        public readonly Input<string> Canidate;
        public readonly Input<bool> IgnoreCase;
        public readonly Output<bool> IsParsed;

        public override E Content
        {
            get
            {
                E testEnum;
                IsParsed.Value = EnumTryParse(Canidate.EvaluateRaw(), IgnoreCase.EvaluateRaw(), out testEnum);
                return testEnum;
            }
        }

        // E is not a nullable type, so we can't use Enum.TryParse. Right?
        private static bool EnumTryParse<T>(string input, bool ignoreCase, out T theEnum)
        {
            StringComparison sc = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            foreach (string en in Enum.GetNames(typeof(T)))
            {
                if (en.Equals(input, sc))
                {
                    theEnum = (T)Enum.Parse(typeof(T), input, ignoreCase);
                    return true;
                }
            }
            theEnum = default(T);
            return false;
        }
    }
}
