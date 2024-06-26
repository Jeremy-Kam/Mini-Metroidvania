using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [SerializeField] private PowerUp powerUp;
    [SerializeField] private string soundName;
    [SerializeField] private GameEvent pickupEvent;

    private void Start()
    {
        if(powerUp.GetValue())
        {
            Destroy(gameObject);
        }
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
                powerUp.SetValue(true);
                FindObjectOfType<AudioManager>().Play(soundName);
                if(pickupEvent)
                {
                    pickupEvent.Raise();
                }
            }
            Destroy(gameObject);
        }

    }
}
