using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0SpawnScript : MonoBehaviour
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
        EnemySpawn Boss0 = em.GetEnemy("Boss0");
        Enemy Boss0E = Instantiate(Boss0.enemy, Boss0.spawnPosition, Boss0.spawnRotation);
        Boss0E.SetUniqueID("Boss0");
        // Debug.Log("Enemy Spawned");

        yield return new WaitForSeconds(2f);

        EnemySpawn Ghost0 = em.GetEnemy("Ghost0");
        Enemy Ghost0E = Instantiate(Ghost0.enemy, Ghost0.spawnPosition, Ghost0.spawnRotation);
        Ghost0E.SetUniqueID("Ghost0");

        yield return new WaitForSeconds(2f);

        EnemySpawn Ghost1 = em.GetEnemy("Ghost1");
        Enemy Ghost1E = Instantiate(Ghost1.enemy, Ghost1.spawnPosition, Ghost1.spawnRotation);
        Ghost1E.SetUniqueID("Ghost1");




        while (em.GetEnemy("Boss0").isDead == false)
        {
            yield return new WaitForSeconds(2f);
            if(em.GetEnemy("Ghost0").isDead)
            {
                em.GetEnemy("Ghost0").isDead = false;
                Ghost0E = Instantiate(Ghost0.enemy, Ghost0.spawnPosition, Ghost0.spawnRotation);
                Ghost0E.SetUniqueID("Ghost0");
            } else if(em.GetEnemy("Ghost1").isDead)
            {
                em.GetEnemy("Ghost1").isDead = false;
                Ghost1E = Instantiate(Ghost1.enemy, Ghost1.spawnPosition, Ghost1.spawnRotation);
                Ghost1E.SetUniqueID("Ghost1");
            }

            yield return new WaitForSeconds(2f);
        }

        
    }
}
