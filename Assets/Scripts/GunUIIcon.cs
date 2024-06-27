using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUIIcon : MonoBehaviour
{
    [SerializeField] private PowerUp gotGun;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateStatus();
    }

    // Needs to be called when we pick up a gun
    public void UpdateStatus()
    {
        // Debug.Log("Updated Gun UI: " + gotGun.GetValue());
        image.enabled = gotGun.GetValue();
    }
}
