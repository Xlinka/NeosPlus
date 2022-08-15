using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;
using BaseX;

namespace FrooxEngine.Logix.Collections.Utils
{
	[Category(new string[] { "LogiX/BetterActions" })]
	[NodeName("->")]
	[NodeDefaultType(typeof(BetterReferenceTarget<IWorldElement>))]
	public class BetterReferenceTarget<T> : LogixOperator<T> where T : class, IWorldElement
	{
		public readonly Input<ISyncRef> Reference;

		public override T Content
		{
			get
			{
				ISyncRef syncRef = Reference.EvaluateRaw();
				if (syncRef == null)
				{
					return null;
				}
				return (T)syncRef.Target;
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			return null;
		}


		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)this).NotifyChange();
		}
	}
}
