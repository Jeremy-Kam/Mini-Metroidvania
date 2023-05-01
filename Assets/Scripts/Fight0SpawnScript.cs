using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight0SpawnScript : MonoBehaviour
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
        EnemySpawn Sword0 = em.GetEnemy("Sword0");
        Enemy Sword0E = Instantiate(Sword0.enemy, Sword0.spawnPosition, Sword0.spawnRotation);
        Sword0E.SetUniqueID("Sword0");
        // Debug.Log("Enemy Spawned");

        while (em.GetEnemy("Sword0").isDead == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        EnemySpawn Ghost0 = em.GetEnemy("Ghost0");
        Enemy Ghost0E = Instantiate(Ghost0.enemy, Ghost0.spawnPosition, Ghost0.spawnRotation);
        Ghost0E.SetUniqueID("Ghost0");
        // Debug.Log("Enemy Spawned");

        EnemySpawn Ghost1 = em.GetEnemy("Ghost1");
        Enemy Ghost1E = Instantiate(Ghost1.enemy, Ghost1.spawnPosition, Ghost1.spawnRotation);
        Ghost1E.SetUniqueID("Ghost1");
        // Debug.Log("Enemy Spawned");

        while (em.GetEnemy("Ghost0").isDead == false || em.GetEnemy("Ghost1").isDead == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        EnemySpawn Slime0 = em.GetEnemy("Slime0");
        Enemy Slime0E = Instantiate(Slime0.enemy, Slime0.spawnPosition, Slime0.spawnRotation);
        Slime0E.SetUniqueID("Slime0");
        // Debug.Log("Enemy Spawned");

        EnemySpawn Slime1 = em.GetEnemy("Slime1");
        Enemy Slime1E = Instantiate(Slime1.enemy, Slime1.spawnPosition, Slime1.spawnRotation);
        Slime1E.SetUniqueID("Slime1");
        // Debug.Log("Enemy Spawned");

        EnemySpawn Gun0 = em.GetEnemy("Gun0");
        Enemy Gun0E = Instantiate(Gun0.enemy, Gun0.spawnPosition, Gun0.spawnRotation);
        Gun0E.SetUniqueID("Gun0");
        // Debug.Log("Enemy Spawned");

        while (em.GetEnemy("Slime0").isDead == false || em.GetEnemy("Slime1").isDead == false || em.GetEnemy("Gun0").isDead == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        EnemySpawn Gun1 = em.GetEnemy("Gun1");
        Enemy Gun1E = Instantiate(Gun1.enemy, Gun1.spawnPosition, Gun1.spawnRotation);
        Gun1E.SetUniqueID("Gun1");

        EnemySpawn Gun2 = em.GetEnemy("Gun2");
        Enemy Gun2E = Instantiate(Gun2.enemy, Gun2.spawnPosition, Gun2.spawnRotation);
        Gun2E.SetUniqueID("Gun2");

        EnemySpawn Sword1 = em.GetEnemy("Sword1");
        Enemy Sword1E = Instantiate(Sword1.enemy, Sword1.spawnPosition, Sword1.spawnRotation);
        Sword1E.SetUniqueID("Sword1");

        EnemySpawn Sword2 = em.GetEnemy("Sword2");
        Enemy Sword2E = Instantiate(Sword2.enemy, Sword2.spawnPosition, Sword2.spawnRotation);
        Sword2E.SetUniqueID("Sword2");

    }
}
