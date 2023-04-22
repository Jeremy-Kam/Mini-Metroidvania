using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    public string uniqueId;
    public bool pickedUp;

    private HeartContainerManager hcm;

    private void Start()
    {
        hcm = GameObject.FindObjectOfType<HeartContainerManager>();
        hcm.RegisterHeart(uniqueId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.name);

        // Player Layer
        if (collision.gameObject.layer == 6)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.IncreaseMaxHealth();
                pickedUp = false;
                hcm.DeactivateHeart(uniqueId);
            }
            Destroy(gameObject);
        }

    }
}
