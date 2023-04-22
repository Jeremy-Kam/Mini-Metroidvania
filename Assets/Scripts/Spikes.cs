using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    [SerializeField] private SpawnLocationIndex spawnIndex;

    [SerializeField] private Animator transition;
    [SerializeField] float transitionTime = 1f;

    [SerializeField] int spikeDamage = 2;

    [SerializeField] private string levelToLoadName;
    [SerializeField] private Vector2 loadZoneDirection;

    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGO = collision.gameObject;
        // Debug.Log(collisionGO.name);
        if (collisionGO.tag == "Player")
        {
            ReloadLevel();
        }
        else if (collisionGO.tag == "Enemy")
        {
            collisionGO.GetComponent<Enemy>().UnLoad();
        }
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevel(levelToLoadName));
    }

    IEnumerator LoadLevel(string nameOfLevel)
    {
        transition.SetTrigger("Start");
        player.HitSpikes(spikeDamage);

        yield return new WaitForSeconds(transitionTime);

        // Debug.Log(nameOfLevel);

        SceneManager.LoadScene(nameOfLevel);
    }
}
