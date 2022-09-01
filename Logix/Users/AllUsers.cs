using System.Collections.ObjectModel;
using System.Linq;

namespace FrooxEngine.LogiX.Users
{
    [NodeName("All Users")]
    [Category("LogiX/Users", "LogiX/Collections")]
    public class AllUsers : LogixOperator<ReadOnlyCollection<User>>
    {
        public override ReadOnlyCollection<User> Content => new ReadOnlyCollection<User>(World.AllUsers.ToArray());
    }
}