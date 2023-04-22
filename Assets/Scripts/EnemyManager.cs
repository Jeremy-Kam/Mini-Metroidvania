using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Door[] doorList;
    public EnemySpawn[] enemyList;

    public EnemyManagerCompletion complete;
    
    void Start()
    {
        OpenDoors();
    }

    public EnemySpawn GetEnemy(string uniqueID)
    {
        return Array.Find(enemyList, enemy => enemy.uniqueID == uniqueID);
    }

    public void OpenDoors()
    {
        foreach (Door d in doorList)
        {
            d.UpdateState(true);
        }
    }

    public void CloseDoors()
    {
        foreach (Door d in doorList)
        {
            d.UpdateState(false);
        }
    }

    public void RegisterDeath(string uniqueID)
    {
        EnemySpawn enemy = Array.Find(enemyList, enemy => enemy.uniqueID == uniqueID);
        enemy.isDead = true;

        if(IsCompleted())
        {
            OpenDoors();
            complete.SetValue(true);
            // Debug.Log("Level completed");
        }
    }

    public bool IsCompleted()
    {
        foreach(EnemySpawn es in enemyList)
        {
            if(!es.isDead)
            {
                return false;
            }
        }

        return true;
    }
}
