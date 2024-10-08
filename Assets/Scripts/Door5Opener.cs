using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door5Opener : MonoBehaviour
{
    [SerializeField] private PowerUp dash;

    // Start is called before the first frame update
    void Start()
    {
        // If the player has dash, then the door should be open for the rest of the game
        if(dash.GetValue())
        {
            FindObjectOfType<ProgressManager>().UpdateDoorList("Door5", true);
            // Debug.Log("Opened");
        }
    }
}
