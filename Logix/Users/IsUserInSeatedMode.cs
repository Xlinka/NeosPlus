using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Users
{
    [Category("LogiX/Users")]
    [NodeName("Is User In Seated Mode")]
    public class IsUserInSeatedMode : LogixOperator<bool>
    {
        public readonly Input<User> User;

        public override bool Content
        {
            get
            {
                var user = User.EvaluateRaw();

                try
                {
                    return user.InputInterface.SeatedMode;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
