using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] HealthBar healthBar;
    
    [SerializeField] private IntVariable healthDrop;
    [SerializeField] private Heart heartPrefab;

    public LayerMask playerLayer;
    public bool facingRight;

    [HideInInspector]
    public Collider2D player;

    public Rigidbody2D rb2D;
    public CapsuleCollider2D cap2D;

    // Only needed when there is an enemyManager in the scene
    private string uniqueID;

    public void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Dev can decide if they face right or left to start
        if(facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        health -= damage;
        rb2D.AddForce(knockback, ForceMode2D.Impulse);
        // Debug.Log(knockback);

        healthBar.SetHealth(health);

        if(health <= 0)
        {
            Die();
        }
    }

    // Damage that needs to take into account the thing that hit it
    public void TakeDirectionalDamage(int damage, float force, Transform damageDealer)
    {
        health -= damage;

        Vector2 heading = this.transform.position - damageDealer.position;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance; // This is now the normalized direction.

        rb2D.AddForce(direction * force, ForceMode2D.Impulse);

        healthBar.SetHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }
    
    public void Flip()
    {
        if(facingRight)
        {
            transform.position = new Vector3(transform.position.x + (cap2D.offset.x * 2), transform.position.y, transform.position.z);
        } else
        {
            transform.position = new Vector3(transform.position.x - (cap2D.offset.x * 2), transform.position.y, transform.position.z);
        }

        
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void Die()
    {
        healthDrop.SetValue(healthDrop.GetValue() + 1);
        // Every four enemies killed, they'll drop one heart
        if(healthDrop.GetValue() == 4)
        {
            healthDrop.SetValue(0);
            Instantiate(heartPrefab, transform.position, transform.rotation);
        }

        // Debug.Log("This is the uniqueID: " + uniqueID);

        // Register the death with the em if it is there
        EnemyManager em = FindObjectOfType<EnemyManager>();
        if(em != null)
        {
            em.RegisterDeath(uniqueID);
            
        }

        Destroy(gameObject);
    }

    public void SetUniqueID(string ID)
    {
        uniqueID = ID;
        // Debug.Log("Yo the uniqueID is set to: " + uniqueID);
    }

    // Used when it touches a level loader
    public void UnLoad()
    {
        Destroy(gameObject);
    }

     

}
