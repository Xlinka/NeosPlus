using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Math;
//01I Adds a integer output of bool
[Category(new string[] { "LogiX/Operators" })]
[NodeName("0 1 I")]
public class ZeroOneI : LogixOperator<int>
{
	public readonly Input<bool> Boolean;

	public override int Content
	{
		get
		{
			if (!Boolean.EvaluateRaw(def: false))
			{
				return 0;
			}
			return 1;
		}
	}

	public static ZeroOneI __New()
	{
		return new ZeroOneI();
	}

	protected override void NotifyOutputsOfChange()
	{
		((IOutputElement)this).NotifyChange();
	}
}