using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private SpawnPosition[] spawnLocations;
    [SerializeField] private SpawnLocationIndex spawnIndex;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        } else
        {
            player = Instantiate(playerPrefab);
        }

        Debug.Log("Spawn Index: " + spawnIndex.getValue());
        Debug.Log(spawnLocations.Length);

        // Figure out a way to know which spawn position to use
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            if (spawnLocations[i].index == spawnIndex.getValue())
            {
                player.transform.position = spawnLocations[i].transform.position;
                Debug.Log(player.transform.position);
                break;
            }
        }
        
    }
}
