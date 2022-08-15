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
	[NodeName("Array Remove At")]
	[Category(new string[] { "LogiX/Collections/Array" })]

	public class ArrayRemoveAt<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Input<IList<T>> List;

        public readonly Input<T> AddedValue;

		public readonly Input<int> Index;


		public readonly Impulse Removed;

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
				return typeof(ArrayRemoveAt<>).MakeGenericType(type.GetGenericArguments()[0]);
			}
			return null;
		}


		[ImpulseTarget]
		public void Remove()
		{
			IList<T> _listobj;
            _listobj = List.Evaluate();
			if (_listobj != null)
			{
				_listobj.RemoveAt(Index.Evaluate());
				this.Removed.Trigger();
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
