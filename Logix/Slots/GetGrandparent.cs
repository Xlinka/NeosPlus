using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Slots
{
	[Category("LogiX/Slots")]
	[NodeName("Get Grandparent")]
	public class GetGrandparent : LogixOperator<Slot>
	{
		public readonly Input<Slot> Instance;
		public readonly Input<int> Grandparent;

		public override Slot Content
		{
			get
			{
				var instance = Instance.EvaluateRaw();
				var grandparent = Grandparent.EvaluateRaw();
				if (instance == null || grandparent < 0)
					return null;
				for (var i = 0; i < grandparent; i++)
				{
					if (instance.Parent != null)
						instance = instance.Parent;
					else return null;
				}
				return instance;
			}
		}

	}
}
