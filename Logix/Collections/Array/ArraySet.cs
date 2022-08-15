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
	[NodeName("Array Set")]
	[Category(new string[] { "LogiX/Collections/Array" })]

	public class ArraySet<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Input<ArrayX<T>> List;

		public readonly Input<int> Index;

		public readonly Input<T> Value;

		public readonly Impulse Set;

        public readonly Impulse Fail;

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (this.List.IsConnected)
			{
				return null;
			}
			Type overload;
			overload = LogixHelper.GetMatchingOverload(this.GetOverloadName(), connectingTypes);
			if (overload != null)
			{
				return overload;
			}
			if (connectingTypes.inputs.TryGetValue("List", out var type))
			{
				return typeof(ArraySet<>).MakeGenericType(type.GetGenericArguments()[0]);
			}
			return null;
		}

		[ImpulseTarget]
		public void Add()
		{
			IList<T> _listobj;
            _listobj = List.Evaluate();
            if (_listobj != null)
            {
				try
				{
					_listobj[Index.Evaluate()] = Value.Evaluate();
					this.Set.Trigger();
				}
				catch 
				{
					this.Fail.Trigger();
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
			text = typeof(T).Name;
			uIBuilder.Text(in text);
		}
		protected override void NotifyOutputsOfChange()
		{
		}
	}

}
