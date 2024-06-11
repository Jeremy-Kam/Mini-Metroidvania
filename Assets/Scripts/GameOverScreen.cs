using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private bool canContinue = false;
    
    private void Update()
    {
        if (canContinue && Input.GetKey(KeyCode.Return))
        {
            FindObjectOfType<GameManager>().LoadNextLevel();
        }
    }

    public void PlayBlip()
    {
        FindObjectOfType<AudioManager>().Play("gameOverBlip");
    }

    public void SetCanContinueToTrue()
    {
        canContinue = true;
    }
}
