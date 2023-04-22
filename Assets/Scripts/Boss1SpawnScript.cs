using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SpawnScript : MonoBehaviour
{
    [SerializeField] private EnemyManager em;

    // I don't want the player reentering the box to respawn all the enemies again
    private bool alreadyStarted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !em.complete.GetValue() && !alreadyStarted)
        {
            // Spawn all enemies
            alreadyStarted = true;

            em.CloseDoors();
            StartCoroutine("SpawnEnemies");
        }
    }

    private IEnumerator SpawnEnemies()
    {
        EnemySpawn Sword0S = em.GetEnemy("Sword0");
        Enemy Sword0E = Instantiate(Sword0S.enemy, Sword0S.spawnPosition, Sword0S.spawnRotation);
        Sword0E.SetUniqueID("Sword0");
        // Debug.Log("Enemy Spawned");

        yield return new WaitForSeconds(1f);
    }
}
