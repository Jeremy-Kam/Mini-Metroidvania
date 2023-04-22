using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletRange;

    private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        } else
        {
            Debug.Log("Oh no, no player Prefab found for bullet");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);

        if(transform.rotation.y < 0)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Platform
        if (collision.gameObject.layer == 7)
        {
            BreakableTile bt = collision.GetComponent<BreakableTile>();
            if(bt != null)
            {
                FindObjectOfType<AudioManager>().Play("pistolHit");
                bt.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        
        // Enemy
        if(collision.gameObject.layer == 9)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                FindObjectOfType<AudioManager>().Play("pistolHit");
                enemy.TakeDamage(damage, knockback);

            }
            Destroy(gameObject);
        }

        // Switch
        if(collision.gameObject.layer == 11)
        {
            Switch s = collision.GetComponent<Switch>();
            if (s != null)
            {
                FindObjectOfType<AudioManager>().Play("pistolHit");
                s.ToggleSwitch();
            }
            Destroy(gameObject);
        }
    }
}
