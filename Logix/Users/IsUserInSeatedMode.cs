using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Users
{
    [Category("LogiX/Users")]
    [NodeName("Is User In Seated Mode")]
    public class IsUserInSeatedMode : LogixOperator<bool>
    {
        public readonly Input<User> User;
        public override bool Content => User.EvaluateRaw()?.InputInterface.SeatedMode ?? false;
    }
}
