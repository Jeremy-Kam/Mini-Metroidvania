using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private static ProgressManager ProgressManagerInstance;

    // Format is uniqueID, bool
    private static Dictionary<string, bool> doorList = new Dictionary<string, bool>();
    private static Dictionary<string, bool> switchList = new Dictionary<string, bool>();
    private static Dictionary<string, bool> breakableWallList = new Dictionary<string, bool>();

    // Doors handled by enemyManager will not appear or be registered into the doorList

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (ProgressManagerInstance == null)
        {
            ProgressManagerInstance = this;
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

    // Register is when starting
    public void RegisterDoor(string uniqueID, bool isOpen)
    {
        if (doorList.ContainsKey(uniqueID))
        {
            UpdateDoor(uniqueID);
        }
        else
        {
            doorList.Add(uniqueID, isOpen);
            UpdateDoor(uniqueID);
        }
    }
    public void RegisterSwitch(string uniqueID, bool isOpen)
    {
        if (switchList.ContainsKey(uniqueID))
        {
            UpdateSwitch(uniqueID);
        }
        else
        {
            switchList.Add(uniqueID, isOpen);
            UpdateSwitch(uniqueID);
        }
    }

    // Should always start off not broken
    public void RegisterBreakableWall(string uniqueID)
    {
        if (breakableWallList.ContainsKey(uniqueID))
        {
            UpdateBreakableWall(uniqueID);
        }
        else
        {
            breakableWallList.Add(uniqueID, false);
            UpdateBreakableWall(uniqueID);
        }
    }

    // These methods refresh them by updating their state based on the lists
    public void UpdateDoor(string uniqueID)
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].GetComponent<Door>().uniqueID == uniqueID)
            {
                doors[i].GetComponent<Door>().UpdateState(doorList[uniqueID]);
                break;
            }
        }
    }

    public void UpdateSwitch(string uniqueID)
    {
        GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");

        for(int i = 0; i < switches.Length; i++)
        {
            if (switches[i].GetComponent<Switch>().uniqueID == uniqueID)
            {
                // Possibly unnecessary
                switches[i].GetComponent<Switch>().UpdateState(switchList[uniqueID]);


                for(int j = 0; j < switches[i].GetComponent<Switch>().connectedDoors.Length; j++)
                {
                    // Changes the state of all the doors
                    doorList[switches[i].GetComponent<Switch>().connectedDoors[j].uniqueID] = switchList[uniqueID];

                    // Then do update door
                    UpdateDoor(switches[i].GetComponent<Switch>().connectedDoors[j].uniqueID);
                }

                break;
            }
        }

    }

    public void UpdateBreakableWall(string uniqueID)
    {
        GameObject[] bts = GameObject.FindGameObjectsWithTag("BreakableWall");

        for (int i = 0; i < bts.Length; i++)
        {
            if (bts[i].GetComponent<BreakableTile>().uniqueID == uniqueID)
            {
                bts[i].GetComponent<BreakableTile>().UpdateState(breakableWallList[uniqueID]);
                break;
            }
        }
    }

    // These methods update the lists
    public void UpdateSwitchList(string uniqueID, bool isOpen)
    {
        if (switchList.ContainsKey(uniqueID))
        {
            switchList[uniqueID] = isOpen;
        }
        else
        {
            Debug.Log("This switch is not yet registered. Please register this switch");
        }
    }

    public void UpdateDoorList(string uniqueID, bool isOpen)
    {
        if (doorList.ContainsKey(uniqueID))
        {
            doorList[uniqueID] = isOpen;
        }
        else
        {
            Debug.Log("This door is not yet registered. Please register this door");
        }
    }

    public void UpdateBreakableWallList(string uniqueID, bool isBroken)
    {
        if (breakableWallList.ContainsKey(uniqueID))
        {
            breakableWallList[uniqueID] = isBroken;
        }
        else
        {
            Debug.Log("This breakable tile is not yet registered. Please register this breakable tile");
        }
    }
}
