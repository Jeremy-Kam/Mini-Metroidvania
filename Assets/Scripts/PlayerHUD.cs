using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private HP PlayerHealth;
    [SerializeField] private IntVariable gunIndex;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private GunUI[] gunUI;

    private void Start()
    {
        UpdateHearts();
        UpdateGunUI();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < (PlayerHealth.GetValue() / 2))
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i < ((PlayerHealth.GetValue() + 1) / 2))
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < (PlayerHealth.GetMaxValue() / 2))
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void UpdateGunUI()
    {
        Debug.Log("Changed Weapon: " + gunIndex.GetValue());
        gunUI[gunIndex.GetValue() - 1].playGunSelect();
    }
}
