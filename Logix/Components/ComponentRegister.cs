using FrooxEngine;
using FrooxEngine.LogiX;

[NodeName("Component Register")]
[Category("LogiX/Components")]
public class ComponentRegister : LogixOperator<Component>, IValue<Component>, IChangeable, IWorldElement
{
	public readonly SyncRef<Component> Target;

	public override Component Content => Target.Target;

	protected override string Label => typeof(Component).Name;

	Component IValue<Component>.Value
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
