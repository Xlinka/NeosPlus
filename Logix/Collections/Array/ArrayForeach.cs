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
	[NodeName("Array Foreach")]
	[Category(new string[] { "LogiX/Collections/Array", "LogiX/Flow" })]
	public class ArrayForeach<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Output<int> Index;

		public readonly Output<T> Value;

		public readonly Input<IList<T>> List;

		public readonly Impulse LoopStart;

		public readonly Impulse LoopIteration;

		public readonly Impulse LoopEnd;
		
		[ImpulseTarget]
        public void Run()
		{
            if (!base.Enabled)
            {
                return;
            }
			this.LoopStart.Trigger();
			IList<T> _listobj;
			int count;
            _listobj = List.Evaluate();
            if (_listobj != null)
            {
				count = _listobj.Count;
				for (int i = 0; i < count; i++)
				{
					this.Value.Value = _listobj[i];
					this.Index.Value = i;
					this.LoopIteration.Trigger();
					if (base.Logix.ImpulsesBlocked)
					{
						break;
					}
				}
				this.Index.Value = 0;
				this.LoopEnd.Trigger();
            }
        }

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
				return typeof(ArrayForeach<>).MakeGenericType(type.GetGenericArguments()[0]);
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
