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
	[NodeName("Array Length")]
	[Category(new string[] { "LogiX/Collections/Array" })]
	public class ArrayCount<T> : LogixNode, IChangeable, IWorldElement 
	{
        public readonly Output<int> Count;

        public readonly Input<IList<T>> A;

        protected override void OnEvaluate()
        {
			IList<T> _listobj;
            _listobj = A.Evaluate();
            if (_listobj != null)
            {
                this.Count.Value = _listobj.Count;
                NotifyOutputsOfChange();
            }
        }

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (this.A.IsConnected)
			{
				return null;
			}
			Type overload;
			overload = LogixHelper.GetMatchingOverload(this.GetOverloadName(), connectingTypes);
			if (overload != null)
			{
				return overload;
			}
			if (connectingTypes.inputs.TryGetValue("A", out var type))
			{
				return typeof(ArrayCount<>).MakeGenericType(type.GetGenericArguments()[0]);
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
			((IOutputElement)this.Count).NotifyChange();
		}
	}

}
