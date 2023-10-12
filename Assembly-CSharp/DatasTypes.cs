using System.Reflection;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Datas;

// Namespace: Assets.Scripts.PeroTools.Nice.Datas
public class Data : IData, IValue // TypeDefIndex: 15097
{
    // Fields
    [SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
    private string m_Uid; // 0x10
    [SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
    private Dictionary<string, IVariable> m_Fields; // 0x18

    // Properties
    public string Uid
    {
        get => m_Uid;
        set => m_Uid = value;
    }
    
    public Dictionary<string, IVariable> Fields
    {
        get => m_Fields;
        set => m_Fields = value;
    }

    public IVariable Item { get; set; } = null!;
    public object Result { get; set; } = null!;
}