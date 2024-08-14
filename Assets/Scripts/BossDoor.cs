using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] private Animator doorController;
    [SerializeField] private Collider2D doorCollider;

    private bool isOpen;

    // Door should always start open
    private void Start()
    {
        UpdateState(true);
    }

    public void UpdateState(bool isOpen)
    {
        bool wasOpen = this.isOpen;
        this.isOpen = isOpen;

        // Fpr when non progress manager objects change door status
        // pm.UpdateDoorList(uniqueID, isOpen);

        // Animation
        if (doorController != null)
        {
            if (isOpen)
            {
                doorController.SetBool("shouldClose", false);
                doorCollider.enabled = false;
            }
            else
            {
                doorController.SetBool("shouldClose", true);
                doorCollider.enabled = true;
                if(wasOpen)
                {
                    FindObjectOfType<AudioManager>().Play("doorClosing");
                }
            }
        }
    }
}
