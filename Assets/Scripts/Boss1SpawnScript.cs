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
        EnemySpawn FinalBoss = em.GetEnemy("FinalBoss");
        Enemy FinalBossE = Instantiate(FinalBoss.enemy, FinalBoss.spawnPosition, FinalBoss.spawnRotation);
        FinalBossE.SetUniqueID("FinalBoss");

        yield return new WaitForSeconds(0.1f);
        
    }
}
