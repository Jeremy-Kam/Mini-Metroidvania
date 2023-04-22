using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new IntVariable", menuName = "IntVariable")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public int InitialValue;
	public int RuntimeValue;

    // To stop it from unloading when switching scenes
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public int GetValue()
	{
		return RuntimeValue;
	}

	public void SetValue(int value)
	{
		RuntimeValue = value;
	}

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
	}

	public void OnBeforeSerialize() { }
}