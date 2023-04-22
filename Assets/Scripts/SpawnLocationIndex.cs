using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Index", menuName = "Spawn Index")]
public class SpawnLocationIndex : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int initialValue;
    [SerializeField] private int runtimeValue;

    // To stop it from unloading when switching scenes
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public void OnAfterDeserialize()
    {
        runtimeValue = initialValue;
    }

    public void OnBeforeSerialize() { }

    public int getValue()
    {
        return runtimeValue;
    }

    public void setValue(int value)
    {
        runtimeValue = value;
    }
}
