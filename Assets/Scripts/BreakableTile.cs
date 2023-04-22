using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : MonoBehaviour
{
    public string uniqueID;
    public int health;
    private bool isBroken = false;

    private ProgressManager pm;

    private void Start()
    {
        pm = GameObject.FindObjectOfType<ProgressManager>();
        pm.RegisterBreakableWall(uniqueID);
        UpdateState(isBroken);
    }


    public void UpdateState(bool isBroken)
    {
        this.isBroken = isBroken;

        if(this.isBroken == true)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        pm.UpdateBreakableWallList(uniqueID, true);
        pm.UpdateBreakableWall(uniqueID);
        
        Destroy(gameObject);
    }
}
