using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReset : MonoBehaviour
{
    [SerializeField] private SpawnLocationIndex sli;
    [SerializeField] private int spawnIndexChange;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.tag == "Player")
        {
            sli.setValue(spawnIndexChange);
        }

    }
}
