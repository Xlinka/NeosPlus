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
	[NodeName("Add Component")]
	[Category(new string[] { "LogiX/Components" ,"AbcFastGrab"})]
	[NodeDefaultType(typeof(AddComponent<Spinner>))]
	public class AddComponent<T> : LogixNode, IChangeable, IWorldElement where T:Component ,new()
	{
		public readonly Input<Slot> slot;

		public readonly Impulse Added;

		public readonly Output<T> Comp;

		[ImpulseTarget]
		public void Add()
		{
			if(slot.Evaluate() is null)
            {
				return;
            }
            T t = slot.Evaluate().AttachComponent<T>();
			Comp.Value = t;
			NotifyOutputsOfChange();
			Added.Trigger();
		}
		
		protected override void OnGenerateVisual(Slot root)
		{
			UIBuilder uIBuilder;
			uIBuilder = base.GenerateUI(root, 184f, 76f);
			VerticalLayout verticalLayout;
			verticalLayout = uIBuilder.VerticalLayout(4f);
			verticalLayout.PaddingLeft.Value = 8f;
			verticalLayout.PaddingRight.Value = 16f;
			uIBuilder.Style.MinHeight = 32f;
			LocaleString text;
			text = typeof(T).Name;
			uIBuilder.Text(in text);
		}
	}
}
