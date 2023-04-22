using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public string uniqueID;
    public Enemy enemy;

    // Don't want for a enemy to start dead
    [HideInInspector]
    public bool isDead;

    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

}
