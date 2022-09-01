using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;

[NodeName("Field Register")]
[Category("LogiX/Components")]
[GenericTypes(GenericTypes.Group.NeosPrimitivesAndEnums, typeof(RefID))]
public class FieldRegister<T> : LogixOperator<IValue<T>>, IValue<IValue<T>>, IChangeable, IWorldElement
{
	public readonly SyncRef<IValue<T>> Target;

	public override IValue<T> Content => Target.Target;

	protected override string Label => typeof(IValue<T>).Name;

	IValue<T> IValue<IValue<T>>.Value
	{
		get
		{
			return Content;
		}
		set
		{
			Target.Target = value;
		}
	}
}
