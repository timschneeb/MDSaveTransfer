using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values;

    
// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class String : IValue // TypeDefIndex: 15043
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private string m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = (string)value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Integer : IValue // TypeDefIndex: 15044
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private int m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = (int)value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Float : IValue // TypeDefIndex: 15045
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private float m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = (float)value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Boolen : IValue // TypeDefIndex: 15046
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private bool m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = (bool)value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Vector2 : IValue // TypeDefIndex: 15047
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Vector2 m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Vector2; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Vector3 : IValue // TypeDefIndex: 15048
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Vector3 m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Vector3; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Vector4 : IValue // TypeDefIndex: 15049
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Vector4 m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Vector4; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Rect : IValue // TypeDefIndex: 15050
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Rect m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Rect; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Bounds : IValue // TypeDefIndex: 15051
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Bounds m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Bounds; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Color : IValue // TypeDefIndex: 15052
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Color m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Color; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class LayerMask : IValue // TypeDefIndex: 15053
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private LayerMask m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as LayerMask; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Quaternion : IValue // TypeDefIndex: 15054
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Quaternion m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Quaternion; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Gradient : IValue // TypeDefIndex: 15055
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Gradient m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Gradient; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class AnimationCurve : IValue // TypeDefIndex: 15056
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private AnimationCurve m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as AnimationCurve; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Enum : IValue // TypeDefIndex: 15057
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Enum m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Enum; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Object : IValue // TypeDefIndex: 15058
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Object m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Object; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class GameObject : IValue // TypeDefIndex: 15059
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private GameObject m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as GameObject; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Transform : IValue // TypeDefIndex: 15060
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Transform m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Transform; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class AudioClip : IValue // TypeDefIndex: 15061
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private AudioClip m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as AudioClip; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Texture2D : IValue // TypeDefIndex: 15062
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Texture2D m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Texture2D; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Image : IValue // TypeDefIndex: 15063
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private Image m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value as Image; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class Ref : IValue // TypeDefIndex: 15065
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private object m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value; }
}

// Namespace: Assets.Scripts.PeroTools.Nice.Values
public class List : IValue // TypeDefIndex: 15066
{
	// Fields
	[SerializeField] // RVA: 0x1122D0 Offset: 0x1116D0 VA: 0x1801122D0
	private object m_Result; // 0x10

	// Properties
	public object Result { get => m_Result; set => m_Result = value; }
}

