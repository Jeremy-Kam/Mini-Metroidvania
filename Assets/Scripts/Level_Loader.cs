using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Loader : MonoBehaviour
{
    [SerializeField] private SpawnLocationIndex spawnIndex;

    [SerializeField] private Animator transition;
    [SerializeField] float transitionTime = 1f;

    [SerializeField] private string levelToLoadName;
    [SerializeField] private int levelToLoadIndex;
    [SerializeField] private Vector2 loadZoneDirection;

    [SerializeField] private Player player;

    void Start()
    {
        // player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGO = collision.gameObject;
        // Debug.Log(collisionGO.name);
        if(collisionGO.tag == "Player")
        {
            LoadNextLevel();
        } else if (collisionGO.tag == "Enemy")
        {
            collisionGO.GetComponent<Enemy>().UnLoad();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(levelToLoadName));
    }

    IEnumerator LoadLevel(string nameOfLevel)
    {
        transition.SetTrigger("Start");
        player.EndLevel(loadZoneDirection.normalized);

        yield return new WaitForSeconds(transitionTime);

        // Debug.Log(nameOfLevel);

        SceneManager.LoadScene(nameOfLevel);

        spawnIndex.setValue(levelToLoadIndex);
    }
}
