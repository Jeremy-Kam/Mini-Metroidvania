using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyManagerCompletion", menuName = "EnemyManagerCompletion")]
public class EnemyManagerCompletion : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private bool initialCompleted;
    [SerializeField] private bool runtimeCompleted;

    // To stop it from unloading when switching scenes
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public void OnAfterDeserialize()
    {
        runtimeCompleted = initialCompleted;
    }

    public void OnBeforeSerialize() { }

    public bool GetValue()
    {
        return runtimeCompleted;
    }

    public void SetValue(bool value)
    {
        runtimeCompleted = value;
    }
}