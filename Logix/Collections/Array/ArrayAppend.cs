using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;
namespace FrooxEngine.Logix.Collections.Array
{
	[NodeName("Array Append")]
	[Category(new string[] { "LogiX/Collections/Array" })]
	public class ArrayAppend<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Input<ArrayX<T>> To;

		public readonly Input<ArrayX<T>> From;

		public readonly Impulse Added;

        [ImpulseTarget]
        public void Add()
        {
            ArrayX<T> _listobj;
            _listobj = To.Evaluate();
            if (_listobj != null)
            {
               _listobj.AppendArray(From.Evaluate());
                this.Added.Trigger();
            }
        }

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (this.To.IsConnected)
			{
				return null;
			}
			Type overload;
			overload = LogixHelper.GetMatchingOverload(this.GetOverloadName(), connectingTypes);
			if (overload != null)
			{
				return overload;
			}
			if (connectingTypes.inputs.TryGetValue("To", out var type))
			{
				return typeof(ArrayAppend<>).MakeGenericType(type.GetGenericArguments()[0]);
			}
			return null;
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
		protected override void NotifyOutputsOfChange()
		{
		}
	}

}
