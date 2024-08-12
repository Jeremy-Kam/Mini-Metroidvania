using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    // the UI/Image component
    [SerializeField] private Image imageCanvas;
    // the fake SpriteRenderer
    [SerializeField] private SpriteRenderer fakeRenderer;
    // the Animator
    [SerializeField] private Animator animator;


    void Start()
    {
        // avoid the SpriteRenderer to be rendered
        fakeRenderer.enabled = false;
        // animator = gameObject.AddComponent<Animator>();

        // set the controller
        // animator.runtimeAnimatorController = controller;
    }

    void Update()
    {
        if(!animator)
        {
            Debug.Log("No animator, resetting");
            return;
        }
        // if a controller is running, set the sprite
        if (animator.runtimeAnimatorController)
        {
            imageCanvas.sprite = fakeRenderer.sprite;
        }
    }

    public void playGunSelect()
    {
        animator.SetTrigger("GunSelect");
        // Debug.Log("Play animation");
    }
}
