namespace Assets.Scripts.PeroTools.Nice.Interface;

public interface IData // TypeDefIndex: 15067
{
    // Properties
    public abstract Dictionary<string, IVariable> Fields { get; }
    public abstract IVariable Item { get; set; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Interface
public interface IValue // TypeDefIndex: 15070
{
    // Properties
    public abstract object Result { get; set; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Interface
public interface IVariable : IValue // TypeDefIndex: 15071
{}