using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PowerUp", menuName = "PowerUp")]
public class PowerUp : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private bool initialValue;
    [SerializeField] private bool value;

    // To stop it from unloading when switching scenes
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }

    public void OnBeforeSerialize() { }

    public bool GetValue()
    {
        return value;
    }

    public void SetValue(bool value)
    {
        this.value = value;
    }
}