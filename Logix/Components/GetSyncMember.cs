using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;

namespace FrooxEngine.LogiX.Components
{
    [NodeName("Get Sync Member")]
    [Category("LogiX/Components")]
    [GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
    public class GetSyncMember<T> : LogixOperator<IValue<T>>
    {
        public readonly Input<Component> Component;
        public readonly Input<int> Index;

        public override IValue<T> Content
        {
            get
            {
                var comp = Component.EvaluateRaw();
                if (comp.GetSyncMember(Index.EvaluateRaw()) is IValue<T> field)
                    return field;
                return null;
            }
        }
    }
}