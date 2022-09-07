﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
/// credit
/// art
/// 
namespace FrooxEngine.LogiX.Assets
{
	[Category(new string[] { "LogiX/Mesh/Operations" })]
	public class GetMesh : LogixOperator<IAssetProvider<Mesh>>
	{
		public readonly Input<Slot> Root;

		public override IAssetProvider<Mesh> Content
		{
			get
			{
				return Root.Evaluate().GetComponent<IAssetProvider<Mesh>>();
			}
		}

	}
}
