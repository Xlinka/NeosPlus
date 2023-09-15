using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Math/Constants")]
    [NodeName("Cheese")]
    [HiddenNode]
    public class Cheese : LogixOperator<string>
    {
        public override string Content => "Cheese";
        public override color NodeBackground => color.FromHexCode("#FBDB65");
    }
}