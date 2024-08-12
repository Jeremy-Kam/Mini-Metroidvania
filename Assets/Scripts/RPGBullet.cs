using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBullet : MonoBehaviour
{
    [SerializeField] private float bulletForce;
    [SerializeField] private int damage;
    [SerializeField] private Vector2 knockback;

    [SerializeField] private int areaDamage;
    [SerializeField] private float areaKnockback;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletRange;
    [SerializeField] float hitboxSize;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask switchLayer;

    [SerializeField] private string bulletSoundEffect;
    [SerializeField] private GameObject impactEffect;

    private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Debug.Log("Oh no, no player Prefab found for bullet");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);

        if (transform.rotation.y < 0)
        {
            knockback.x *= -1;
        }
    }

    private void Update()
    {
        // Debug.Log("Player: " + player.transform.position + " Bullet: " + transform.position + " Distance " + Vector2.Distance(player.transform.position, transform.position));
        if (Math.Abs(Vector2.Distance(player.transform.position, transform.position)) > bulletRange)
        {
            Destroy(gameObject);
        }

        rb.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hitValidObject = false;
        // Platform
        if (collision.gameObject.layer == 7)
        {
            BreakableTile bt = collision.GetComponent<BreakableTile>();
            if (bt != null)
            {
                bt.TakeDamage(damage);
            }
            FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
            hitValidObject = true;
        }
        // Enemy
        if (collision.gameObject.layer == 9)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                enemy.TakeDamage(damage, knockback);
            }
            hitValidObject = true;
        }
        // Switch
        if (collision.gameObject.layer == 11)
        {
            Switch s = collision.GetComponent<Switch>();
            if (s != null)
            {
                FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                s.ToggleSwitch();
            }
            hitValidObject = true;
        }

        if(!hitValidObject)
        {
            return;
        }

        // AoE damage
        List<Collider2D> hits = new List<Collider2D>();

        hits.AddRange(Physics2D.OverlapCircleAll(transform.position, hitboxSize, enemyLayer));
        hits.AddRange(Physics2D.OverlapCircleAll(transform.position, hitboxSize, platformLayer));
        hits.AddRange(Physics2D.OverlapCircleAll(transform.position, hitboxSize, switchLayer));

        foreach (Collider2D hit in hits)
        {
            // Platform
            if (hit.gameObject.layer == 7)
            {
                BreakableTile bt = hit.GetComponent<BreakableTile>();
                if (bt != null)
                {
                    FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                    bt.TakeDamage(areaDamage);
                }
            }

            // Enemy
            if (hit.gameObject.layer == 9)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                    enemy.TakeDirectionalDamage(areaDamage, areaKnockback, this.transform);
                }                
            }

            // Switch
            if (hit.gameObject.layer == 11)
            {
                Switch s = hit.GetComponent<Switch>();
                if (s != null)
                {
                    FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                    s.ToggleSwitch();
                }
            }
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, hitboxSize);
    }
}
