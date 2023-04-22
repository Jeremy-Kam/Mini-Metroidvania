using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainerManager : MonoBehaviour
{
    private static HeartContainerManager heartContainerManagerInstance;
    private static Dictionary<string, bool> heartContainerList = new Dictionary<string, bool>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (heartContainerManagerInstance == null)
        {
            heartContainerManagerInstance = this;
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

    public void RegisterHeart(string uniqueId)
    {
        if (heartContainerList.ContainsKey(uniqueId))
        {
            if (heartContainerList[uniqueId])
            {
                FindAndDestroy(uniqueId);
            }
        }
        else
        {
            heartContainerList.Add(uniqueId, false);
        }
    }

    public void DeactivateHeart(string uniqueId)
    {
        heartContainerList[uniqueId] = true;
    }

    private void FindAndDestroy(string uniqueId)
    {
        GameObject heartContainer = GameObject.FindGameObjectWithTag("Heart Container");

        if (heartContainer)
        {
            Destroy(heartContainer);
        }
    }
}
