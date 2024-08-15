using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorController;

    public string uniqueID;
    private bool isOpen;

    private ProgressManager pm;

    private void Start()
    {
        EnemyManager enemyM = GameObject.FindObjectOfType<EnemyManager>();
        if(enemyM == null)
        {
            pm = GameObject.FindObjectOfType<ProgressManager>();
            pm.RegisterDoor(uniqueID, isOpen);
            UpdateState(isOpen);
        }
    }

    public void UpdateState(bool isOpen)
    {
        this.isOpen = isOpen;

        // For when non progress manager objects change door status
        // pm.UpdateDoorList(uniqueID, isOpen);

        // Animation
        if (doorController != null)
        {
            if (isOpen)
            {
                doorController.SetBool("shouldClose", false);
                GetComponent<Collider2D>().enabled = false;
            } else
            {
                doorController.SetBool("shouldClose", true);
                GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
