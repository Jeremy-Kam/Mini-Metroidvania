using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BoolVariable", menuName = "BoolVariable")]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField] private bool InitialValue;
	[SerializeField] private bool RuntimeValue;

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
	}

	public void OnBeforeSerialize() { }

	public bool GetValue()
	{
		return RuntimeValue;
	}

	public void SetValue(bool value)
	{
		RuntimeValue = value;
	}

	
}
