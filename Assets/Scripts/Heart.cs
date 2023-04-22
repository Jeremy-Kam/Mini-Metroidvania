using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private int heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.name);

        // Player Layer
        if (collision.gameObject.layer == 6)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.HealHealth(heal);

            }
            Destroy(gameObject);
        }

    }
}
