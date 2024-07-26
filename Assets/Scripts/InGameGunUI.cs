using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameGunUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] gunSprites;
    [SerializeField] private PowerUp[] gunsAcquired;
    [SerializeField] private IntVariable gunIndex;

    private void Awake()
    {
        ChangeGun();
    }

    // 0 is pistol, 1 is magnum, 2 is shotgun, 3 is RPG
    public void ChangeGun()
    {
        if(gunsAcquired[gunIndex.GetValue() - 1].GetValue())
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = gunSprites[gunIndex.GetValue() - 1];
        } else
        {
            spriteRenderer.enabled = false;
        }
    }

}
