using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // private static GameManager GameManagerInstance;

    [SerializeField] private HP PlayerHP;
    [SerializeField] private SpawnLocationIndex spawnIndex;

    [SerializeField] private Animator transition;
    [SerializeField] float transitionTime = 0.5f;

    [SerializeField] private string levelToLoadName;

    private bool gameHasEnded = false;

    /*
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (GameManagerInstance == null)
        {
            GameManagerInstance = this;
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }
    */

    public void EndGame()
    {
        if(!gameHasEnded)
        {
            gameHasEnded = true;
            // Debug.Log("Game Over");
            LoadNextLevel();
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(levelToLoadName));
    }

    IEnumerator LoadLevel(string nameOfLevel)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            nameOfLevel = "Start";

            // Regenerate Player to Max Health
            PlayerHP.SetValue(PlayerHP.GetMaxValue());

            // Spawn Index of the start position of the game
            spawnIndex.setValue(0);
        }

        // Debug.Log(nameOfLevel);
        SceneManager.LoadScene(nameOfLevel);
    }

}
