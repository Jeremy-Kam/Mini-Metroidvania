using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public void PlayBlip()
    {
        FindObjectOfType<AudioManager>().Play("winScreenBlip");
    }
}
