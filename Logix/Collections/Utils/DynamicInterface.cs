using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Objs;
using System.Reflection;
using BaseX;
using System.Linq.Expressions;

namespace FrooxEngine.Logix.Collections.Utils
{
	public interface CrazyPart
    {
		void SetValue(object element);
    }
	public class Holder<T> : Output<T>, CrazyPart
	{
        public void SetValue(object element)
        {
			try
			{
				Value = (T)element;
            }
            catch { }
        }
    }

	[NodeName("Dynamic Interface")]
	[Category(new string[] { "LogiX/", "AbcFastGrab" })]
	[NodeDefaultType(typeof(DynamicInterface<Spinner>))]
	public class DynamicInterface<T> : LogixNode, IChangeable, IWorldElement where T: Worker
	{
		public readonly Input<T> data;

		[HideInInspector]
		public readonly SyncList<SyncVar> Outputs;

		protected override void OnGenerateVisual(Slot root)
		{
			base.OnGenerateVisual(root);
			var canvas = root[0].GetComponent<Canvas>();
			canvas.Size.Value = new float2(140f, canvas.Size.Value.y);
			var img = root[0][0];
			img[img.ChildrenCount - 1].Destroy();
			List<FieldInfo> info = new List<FieldInfo>(from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
								   where typeof(IWorldElement).IsAssignableFrom(l.FieldType)
								   select l);
			var index = 0;
			img.ForeachChild((slot) =>
			{
				var outproxy = slot.GetComponent<OutputProxy>();
				if (outproxy != null)
				{
					var rect = slot.GetComponent<RectTransform>();
					rect.OffsetMin.Value = new float2(12f, rect.OffsetMin.Value.y);
					rect.OffsetMax.Value = new float2(140f, rect.OffsetMax.Value.y);
					var text = slot.AddSlot("Text").AttachComponent<Text>();
					text.Content.Value = info[index].Name;
					text.AutoSizeMin.Value = 0f;
					text.AutoSizeMax.Value = 40f;
					text.HorizontalAutoSize.Value = true;
					text.VerticalAutoSize.Value = true;
					text.Align = Alignment.MiddleCenter;
					var collider = slot.GetComponent<BoxCollider>();
					collider.Size.Value = new float3(0.128f, collider.Size.Value.yz);
					//this is half of what is above this line
					slot[0].LocalPosition = new float3(0.064f, slot[0].LocalPosition.yz);
					index++;
				}
			});
		}


		protected override void OnAttach()
        {
			base.OnAttach();
			foreach (var item in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
			{
				if (typeof(IWorldElement).IsAssignableFrom(item.FieldType))
				{
					var putput = Outputs.Add();
					putput.ElementType = typeof(Holder<>).MakeGenericType(item.FieldType);
				}
			}			
		}

		protected override void OnInputChange()
		{
			var date = data.Evaluate();
			if (date is null)
			{
				return;
			}
			int index = 0;
			foreach (var item in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
			{
				if (typeof(IWorldElement).IsAssignableFrom(item.FieldType))
				{
					var putput = Outputs[index];
					((CrazyPart)putput.Element).SetValue((IWorldElement)item.GetValue(date));
					index++;
				}
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("data", out var value))
			{
					if (value == null)
					{
						return null;
					}
				if (!typeof(Worker).IsAssignableFrom(value))
				{
					return null;
				}
				if (typeof(LogixNode).IsAssignableFrom(value))
				{
					return null;
				}
				var newslot = Slot.Parent.AddSlot("ValueGrab");
				newslot.AttachComponent(typeof(DynamicInterface<>).MakeGenericType(value));
				newslot.AttachComponent<Grabbable>();
				newslot.CopyTransform(Slot);
				Slot heldSlotReference = newslot;
				if (heldSlotReference != null && heldSlotReference != base.World.RootSlot)
				{
					heldSlotReference.GetComponentsInChildren<LogixNode>().ForEach(delegate (LogixNode n)
					{
						n.GenerateVisual();
					});
				}
				Slot.Destroy(true);
			}
			return null;
		}
	}


	[Category(new string[] { "LogiX/References" })]
	[NodeName("& - ValueGrab")]
	[NodeDefaultType(typeof(ValueGrab<float>))]
	public class ValueGrab<T> : LogixOperator<T> 
	{
		public readonly Input<IValue<T>> Reference;

		public override T Content
		{
			get
			{
				IValue<T> syncRef = Reference.EvaluateRaw();
				if (syncRef == null)
				{
					return (T)typeof(T).GetDefaultValue();
				}
				return syncRef.Value;
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("Reference", out var value))
			{
				Type type = value.EnumerateInterfacesRecursively().FirstOrDefault((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValue<>));
				if (type != null)
				{
					return typeof(ValueGrab<>).MakeGenericType(type.GetGenericArguments()[0]);
				}
				return null;
			}
			return null;
		}

		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)this).NotifyChange();
		}
	}

	//Crashes game and dosn't work
	//[NodeName("ConStructor")]
	//[Category(new string[] { "LogiX/", "AbcFastGrab" })]
	//[NodeDefaultType(typeof(ConStructor<RefID>))]
	//public class ConStructor<T> : LogixNode, IChangeable, IWorldElement where T : struct
	//{
	//	public readonly Input<T> inData;
	//	public readonly Output<T> outData;

	//	private T _value = default;

	//	[HideInInspector]
	//	public readonly SyncList<SyncVar> Inputs;

	//	protected override void OnGenerateVisual(Slot root)
	//	{
	//		base.OnGenerateVisual(root);
	//		var canvas = root[0].GetComponent<Canvas>();
	//		canvas.Size.Value = new float2(140f, canvas.Size.Value.y);
	//		var img = root[0][0];
	//		img[img.ChildrenCount - 1].Destroy();
	//		List<FieldInfo> info = new List<FieldInfo>(from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
	//												   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
	//												   select l);
	//		var index = 0;
	//		bool skipFirst = false;
	//		img.ForeachChild((slot) =>
	//		{
	//			if (skipFirst)
	//			{
	//				var outproxy = slot.GetComponent<InputProxy>();
	//				if (outproxy != null)
	//				{
	//					var rect = slot.GetComponent<RectTransform>();
	//					rect.OffsetMin.Value = new float2(0f, rect.OffsetMin.Value.y);
	//					rect.OffsetMax.Value = new float2(140f, rect.OffsetMax.Value.y);
	//					var text = slot.AddSlot("Text").AttachComponent<Text>();
	//					text.Content.Value = info[index].Name;
	//					text.AutoSizeMin.Value = 0f;
	//					text.AutoSizeMax.Value = 40f;
	//					text.HorizontalAutoSize.Value = true;
	//					text.VerticalAutoSize.Value = true;
	//					text.Align = Alignment.MiddleCenter;
	//					var collider = slot.GetComponent<BoxCollider>();
	//					collider.Size.Value = new float3(0.128f, collider.Size.Value.yz);
	//					//this is half of what is above this line
	//					slot[0].LocalPosition = new float3(-0.064f, slot[0].LocalPosition.yz);
	//					index++;
	//				}
 //               }
 //               else
 //               {
	//				skipFirst = true;
 //               }
	//		});
	//	}

	//	protected override void OnAttach()
	//	{
	//		base.OnAttach();
	//		var info = from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
	//				   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
	//				   select l;
	//		foreach (var item in info)
	//		{
	//			var putput = Inputs.Add();
	//			putput.ElementType = typeof(Input<>).MakeGenericType(item.FieldType);
	//		}
	//	}

	//	protected override void OnInputChange()
	//	{
 //           if (inData.IsConnected)
 //           {
	//			_value = inData.EvaluateRaw();
 //           }
 //           else
 //           {
	//			_value = (T)typeof(T).GetDefaultValue();
	//		}
	//		int index = 0;
	//		var info = from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
	//				   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
	//				   select l;
	//		foreach (var item in info)
	//		{
	//			var putput = Inputs[index];
	//			if (putput != null) {
	//				if (((IInputElement)putput.Element).IsConnected)
	//				{
	//					MethodInfo method = putput.ElementType.GetMethod("EvaluateRaw", item.FieldType.GenericTypeArguments);
	//					item.SetValue(_value, method.Invoke(putput.Element, new object[1] { item.FieldType.GetDefaultValue() }));
	//				}
	//			}
	//			index++;
	//		}
	//		outData.Value = _value;
	//	}

	//	protected override Type FindOverload(NodeTypes connectingTypes)
	//	{
	//		if (connectingTypes.inputs.TryGetValue("inData", out var value))
	//		{
	//			if (!value.IsValueType)
	//			{
	//				return null;
	//			}
	//			var newslot = Slot.Parent.AddSlot("ValueGrab");
	//			newslot.AttachComponent(typeof(ConStructor<>).MakeGenericType(value));
	//			newslot.AttachComponent<Grabbable>();
	//			newslot.CopyTransform(Slot);
	//			Slot heldSlotReference = newslot;
	//			if (heldSlotReference != null && heldSlotReference != base.World.RootSlot)
	//			{
	//				heldSlotReference.GetComponentsInChildren<LogixNode>().ForEach(delegate (LogixNode n)
	//				{
	//					n.GenerateVisual();
	//				});
	//			}
	//			Slot.Destroy(true);
	//		}
	//		return null;
	//	}
	//}

	[NodeName("DeStructor")]
	[Category(new string[] { "LogiX/", "AbcFastGrab" })]
	[NodeDefaultType(typeof(DeStructor<RefID>))]
	public class DeStructor<T> : LogixNode, IChangeable, IWorldElement where T : struct
	{
		public readonly Input<T> data;

		[HideInInspector]
		public readonly SyncList<SyncVar> Outputs;

		protected override void OnGenerateVisual(Slot root)
		{
			base.OnGenerateVisual(root);
			var canvas = root[0].GetComponent<Canvas>();
			canvas.Size.Value = new float2(140f, canvas.Size.Value.y);
			var img = root[0][0];
			img[img.ChildrenCount - 1].Destroy();
			List<FieldInfo> info = new List<FieldInfo>(from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
													   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
													   select l);
			var index = 0;
			img.ForeachChild((slot) =>
			{
				var outproxy = slot.GetComponent<OutputProxy>();
				if (outproxy != null)
				{
					var rect = slot.GetComponent<RectTransform>();
					rect.OffsetMin.Value = new float2(12f, rect.OffsetMin.Value.y);
					rect.OffsetMax.Value = new float2(140f, rect.OffsetMax.Value.y);
					var text = slot.AddSlot("Text").AttachComponent<Text>();
					text.Content.Value = info[index].Name;
					text.AutoSizeMin.Value = 0f;
					text.AutoSizeMax.Value = 40f;
					text.HorizontalAutoSize.Value = true;
					text.VerticalAutoSize.Value = true;
					text.Align = Alignment.MiddleCenter;
					var collider = slot.GetComponent<BoxCollider>();
					collider.Size.Value = new float3(0.128f, collider.Size.Value.yz);
					//this is half of what is above this line
					slot[0].LocalPosition = new float3(0.064f, slot[0].LocalPosition.yz);
					index++;
				}
			});
		}


		protected override void OnAttach()
		{
			base.OnAttach();
			var info = from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
					   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
					   select l;
			foreach (var item in info)
			{
				var putput = Outputs.Add();
				putput.ElementType = typeof(Holder<>).MakeGenericType(item.FieldType);
			}
		}

		protected override void OnInputChange()
		{
			var date = data.Evaluate();
			int index = 0;
			var info = from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
					   where l.FieldType.IsPrimitive || l.FieldType == typeof(string) || l.FieldType == typeof(Uri) || l.FieldType == typeof(Type) || l.FieldType == typeof(decimal) || l.FieldType == typeof(RefID)
					   select l;
			foreach (var item in info)
			{
				var putput = Outputs[index];
				((CrazyPart)putput.Element).SetValue(item.GetValue(date));
				index++;
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("data", out var value))
			{
				if (!value.IsValueType)
				{
					return null;
				}
				var newslot = Slot.Parent.AddSlot("ValueGrab");
				newslot.AttachComponent(typeof(DeStructor<>).MakeGenericType(value));
				newslot.AttachComponent<Grabbable>();
				newslot.CopyTransform(Slot);
				Slot heldSlotReference = newslot;
				if (heldSlotReference != null && heldSlotReference != base.World.RootSlot)
				{
					heldSlotReference.GetComponentsInChildren<LogixNode>().ForEach(delegate (LogixNode n)
					{
						n.GenerateVisual();
					});
				}
				Slot.Destroy(true);
			}
			return null;
		}
	}

}
