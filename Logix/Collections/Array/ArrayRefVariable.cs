using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;


namespace FrooxEngine.Logix.Collections.Variables
{
	[NodeName("Array Reference Variable")]
	[Category(new string[] { "LogiX/Variables" , "LogiX/Collections" })]
	[GenericTypes(new Type[]{
	typeof(Slot),
	typeof(User)
})]
	public class ArrayRefVariable<T> : LogixOperator<ArrayX<T>>, IValue<ArrayX<T>>, IChangeable, IWorldElement where T : class, IWorldElement
	{
		public readonly RefArrayX<T> Value;

		public override ArrayX<T> Content => this.Value;
		ArrayX<T> IValue<ArrayX<T>>.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				if (value != null)
				{
					this.Value.Copy(value);
				}
				else
				{
					this.Value.Clear();
				}
			}
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
			text =  typeof(T).Name;
			uIBuilder.Text(in text);
		}
		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)this).NotifyChange();
		}
	}

}
