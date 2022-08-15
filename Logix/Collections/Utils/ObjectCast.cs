using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;

namespace FrooxEngine.Logix.Collections.Utils
{
	[NodeName("Object Cast")]
	[Category(new string[] { "LogiX/Components" ,"AbcFastGrab"})]
	[NodeDefaultType(typeof(ObjectCast<Spinner>))]
	public class ObjectCast<T> : LogixOperator<T>, IChangeable, IWorldElement
	{
		public readonly Input<object> In;

        public override T Content => (T)In.EvaluateRaw();
	}
}
