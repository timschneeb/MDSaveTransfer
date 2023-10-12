using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables;

// Namespace: Assets.Scripts.PeroTools.Nice.Variables
public class Constance : IVariable, IValue // TypeDefIndex: 15028
{
    // Fields
    [SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
    private IValue m_Value; // 0x10

    // Properties
    public object Result { get => m_Value; set => m_Value = (IValue)value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Variables
public class Enum : IVariable, IValue // TypeDefIndex: 15029
{
    // Fields
    [SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
    private string m_Key; // 0x10
    [SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
    private List<IVariable> m_Params; // 0x18

    // Properties
    public object Result { get; set; }
}