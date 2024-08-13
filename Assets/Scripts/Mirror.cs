using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private bool hasTriggered = false;

    [SerializeField] private Animator animator;

    private ProgressManager pm;

    private void Start()
    {
        pm = GameObject.FindObjectOfType<ProgressManager>();
        pm.UpdateMirror();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered)
        {
            return;
        }
        // Debug.Log(collision.name);

        // Player Layer
        if (collision.gameObject.layer == 6)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                hasTriggered = true;
                pm.SetMirrorTriggered(hasTriggered);
                animator.SetTrigger("PlayerDetected");
            }
        }

    }

    public void UpdateState(bool hasTriggered)
    {
        this.hasTriggered = hasTriggered;
    }
}
