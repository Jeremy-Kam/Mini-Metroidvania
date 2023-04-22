using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new HP", menuName = "HP")]
public class HP : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField] private int InitialHealth;
	[SerializeField] private int health;

	[SerializeField] private int InitialMaxHealth;
	[SerializeField] private int maxHealth;

	// To stop it from unloading when switching scenes
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }


    public void OnAfterDeserialize()
	{
		health = InitialHealth;
		maxHealth = InitialMaxHealth;
	}

	public void OnBeforeSerialize() { }

	public int GetValue()
	{
		return health;
	}

	public void SetValue(int value)
	{
		health = value;
	}

	public int GetMaxValue()
    {
		return maxHealth;
    }

	public void SetMaxValue(int value)
    {
		maxHealth = value;
    }
}
