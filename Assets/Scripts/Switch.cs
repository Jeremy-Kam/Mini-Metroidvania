using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Animator switchController;

    public string uniqueID;
    public Door[] connectedDoors;

    public bool isOpen = false;

    private ProgressManager pm;

    private void Start()
    {
        pm = GameObject.FindObjectOfType<ProgressManager>();
        pm.RegisterSwitch(uniqueID, isOpen);

        if (switchController.gameObject.activeSelf)
        {
            switchController.SetBool("isOpen", isOpen);
        }
    }

    public void UpdateState(bool isOpen)
    {
        this.isOpen = isOpen;
    }

    public void ToggleSwitch()
    {
        
        isOpen = (isOpen) ? false : true;

        if (switchController.gameObject.activeSelf)
        {
            switchController.SetBool("isOpen", isOpen);
        }

        pm.UpdateSwitchList(uniqueID, isOpen);

        pm.UpdateSwitch(uniqueID);
    }
}
