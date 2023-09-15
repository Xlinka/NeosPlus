using System.Collections.ObjectModel;
using System.Linq;

namespace FrooxEngine.LogiX.Users
{
    [NodeName("All Users")]
    [Category("LogiX/NeosPlus/Users", "LogiX/NeosPlus/Collections")]
    public class AllUsers : LogixOperator<ReadOnlyCollection<User>>
    {
        public override ReadOnlyCollection<User> Content => new(World.AllUsers.ToArray());
    }
}