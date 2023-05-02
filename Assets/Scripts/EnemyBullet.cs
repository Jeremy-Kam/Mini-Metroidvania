using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);

        if (transform.rotation.y < 0)
        {
            knockback.x *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Platform
        if (collision.gameObject.layer == 7)
        {
            BreakableTile bt = collision.GetComponent<BreakableTile>();
            if (bt != null)
            {
                // FindObjectOfType<AudioManager>().Play("pistolHit");
                bt.TakeDamage(damage);
            }
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        // Player
        if (collision.gameObject.layer == 6)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // FindObjectOfType<AudioManager>().Play("pistolHit");
                player.TakeDamage(damage, knockback);

            }
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        // Switch
        if (collision.gameObject.layer == 11)
        {
            Switch s = collision.GetComponent<Switch>();
            if (s != null)
            {
                // FindObjectOfType<AudioManager>().Play("pistolHit");
                s.ToggleSwitch();
            }
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
