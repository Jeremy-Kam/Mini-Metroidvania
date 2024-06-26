using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;

    public void playGunSelect()
    {
        gunAnimator.SetTrigger("GunSelect");
        Debug.Log("Play animation");
    }
}
