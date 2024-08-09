using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShot : MonoBehaviour
{
    [SerializeField] private int[] damage;
    [SerializeField] Transform[] attackPoints;
    [SerializeField] private Vector2[] knockback;
    [SerializeField] float[] hitboxSize;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask switchLayer;

    [SerializeField] private string bulletSoundEffect;

    [SerializeField] private GameObject impactEffect;

    // Start is called before the first frame update
    void Awake()
    {
        if (transform.rotation.y < 0)
        {
            for(int i = 0; i < knockback.Length; ++i)
            {
                knockback[i].x *= -1;
            }
        }
    }

    private void Start()
    {
        HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();

        for (int i = 0; i < attackPoints.Length; ++i)
        {
            HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();
            List<Collider2D> hits = new List<Collider2D>();

            hits.AddRange(Physics2D.OverlapCircleAll(attackPoints[i].position, hitboxSize[i], enemyLayer));
            hits.AddRange(Physics2D.OverlapCircleAll(attackPoints[i].position, hitboxSize[i], platformLayer));
            hits.AddRange(Physics2D.OverlapCircleAll(attackPoints[i].position, hitboxSize[i], switchLayer));

            foreach (Collider2D hit in hits)
            {
                uniqueHits.Add(hit);
            }

            foreach (Collider2D hit in uniqueHits)
            {
                if(alreadyHit.Contains(hit))
                {
                    continue;
                }
                // Platform
                if (hit.gameObject.layer == 7)
                {
                    BreakableTile bt = hit.GetComponent<BreakableTile>();
                    if (bt != null)
                    {
                        FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                        bt.TakeDamage(damage[i]);
                        // Debug.Log("Shotgun did: " + damage[i] + " damage");
                        alreadyHit.Add(hit);
                        Instantiate(impactEffect, hit.transform.position, hit.transform.rotation);
                    }
                }

                // Enemy
                if (hit.gameObject.layer == 9)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        FindObjectOfType<AudioManager>().Play(bulletSoundEffect);
                        enemy.TakeDamage(damage[i], knockback[i]);
                        // Debug.Log("Shotgun did: " + damage[i] + " damage");
                        alreadyHit.Add(hit);
                        Instantiate(impactEffect, hit.transform.position, hit.transform.rotation);
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
                        alreadyHit.Add(hit);
                        Instantiate(impactEffect, hit.transform.position, hit.transform.rotation);
                    }
                }
            }

        }
        
        
        
    }

    public void animationEnded()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
        {
            return;
        }
        for (int i = 0; i < attackPoints.Length; ++i)
        {
            Gizmos.DrawWireSphere(attackPoints[i].position, hitboxSize[i]);
        }
    }
}
