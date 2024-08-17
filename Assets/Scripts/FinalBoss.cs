using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FinalBoss : MonoBehaviour
{
    [SerializeField] Animator animator;

    // These two arrays MUST have the same size or else bad things happen
    [SerializeField] Transform[] attackPoints; // Index 0 is detecting point
    [SerializeField] float[] hitboxSize;
    [SerializeField] float playerDetectionSize;

    [SerializeField] Transform firePoint;

    [SerializeField] Transform swingAttackDetectionPoint;
    [SerializeField] float swingAttackDetectionSize;

    [SerializeField] int attackDamage;
    [SerializeField] float enemySpeed;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private float playerDetectionDeadzone;

    [SerializeField] GameObject projectilePrefab;

    // 0 is idle, 1 is swinging, 2 is shooting, 3 is areaAttacking
    private int state = 0;

    private void Move()
    {
        if (GetComponent<Enemy>().player == null)
        {
            return;
        }

        if (GetComponent<Enemy>().player.transform.position.x < ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x)))
        {
            if (Math.Abs(GetComponent<Enemy>().player.transform.position.x - ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x))) > playerDetectionDeadzone)
            {
                if (GetComponent<Enemy>().facingRight)
                {
                    GetComponent<Enemy>().Flip();
                }
                GetComponent<Enemy>().rb2D.velocity = new Vector2(enemySpeed * -1, GetComponent<Enemy>().rb2D.velocity.y);
            }
        }
        else if (GetComponent<Enemy>().player.transform.position.x > ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x)))
        {
            if (Math.Abs(GetComponent<Enemy>().player.transform.position.x - ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x))) > playerDetectionDeadzone)
            {
                if (!GetComponent<Enemy>().facingRight)
                {
                    GetComponent<Enemy>().Flip();
                }
                GetComponent<Enemy>().rb2D.velocity = new Vector2(enemySpeed, GetComponent<Enemy>().rb2D.velocity.y);
            }
        }
        else
        {
            GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);
        }
    }

    // Used for the swing attack, where the boss does not move, but needs to face the player.
    private void FacePlayer()
    {
        GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);

        if (GetComponent<Enemy>().player.transform.position.x < ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x)))
        {
            if (Math.Abs(GetComponent<Enemy>().player.transform.position.x - ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x))) > playerDetectionDeadzone)
            {
                if (GetComponent<Enemy>().facingRight)
                {
                    GetComponent<Enemy>().Flip();
                }
            }
        }
        else if (GetComponent<Enemy>().player.transform.position.x > ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x)))
        {
            if (Math.Abs(GetComponent<Enemy>().player.transform.position.x - ((GetComponent<Enemy>().facingRight) ? (transform.position.x + GetComponent<Enemy>().cap2D.offset.x) : (transform.position.x - GetComponent<Enemy>().cap2D.offset.x))) > playerDetectionDeadzone)
            {
                if (!GetComponent<Enemy>().facingRight)
                {
                    GetComponent<Enemy>().Flip();
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (state == 0)
        {
            if (detectedPlayer())
            {
                SwingAttack();
            }
            else if (SeePlayer())
            {
                // Debug.Log(player);
                Move();
            }
        } else
        {
            FacePlayer();
        }
    }

    private void SwingAttack()
    {
        state = 1;
        GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);
        animator.SetTrigger("Swing");
    }

    private void Swing()
    {
        HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();

        List<Collider2D> hits = new List<Collider2D>();

        FindObjectOfType<AudioManager>().Play("finalBossSwing");

        for(int i = 0; i < attackPoints.Length; ++i)
        {
            hits.AddRange(Physics2D.OverlapCircleAll(attackPoints[i].position, hitboxSize[i], GetComponent<Enemy>().playerLayer));
        }

        foreach (Collider2D hit in hits)
        {
            uniqueHits.Add(hit);
        }

        foreach (Collider2D hit in uniqueHits)
        {
            Vector2 temp = knockback;
            if (!GetComponent<Enemy>().facingRight)
            {
                temp.x *= -1;
            }
            hit.GetComponent<Player>().TakeDamage(attackDamage, temp);
        }

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    private bool detectedPlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(swingAttackDetectionPoint.position, swingAttackDetectionSize, GetComponent<Enemy>().playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            if (hit)
            {
                return true;
            }
        }

        return false;
    }

    private bool SeePlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, playerDetectionSize, GetComponent<Enemy>().playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            if (hit)
            {
                GetComponent<Enemy>().player = hit;
                return true;
            }
        }

        GetComponent<Enemy>().player = null;
        return false;
    }

    private void backToIdle()
    {
        state = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
        {
            return;
        }
        for(int i = 0; i < attackPoints.Length; ++i)
        {
            Gizmos.DrawWireSphere(attackPoints[i].position, hitboxSize[i]);
        }

        Gizmos.DrawWireSphere(swingAttackDetectionPoint.position, swingAttackDetectionSize);

        // Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
    }

    // From Weapons.cs, to kinda get the buffer system.
    /*
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            attackBufferCounter = attackBufferTime;
        }
        else
        {
            attackBufferCounter -= Time.deltaTime;
        }

        if (attackBufferCounter > 0 && Time.time >= nextAttackTime)
        {
            Shoot();
            // The minus one is because gunIndex is 1-indexed, while the array is zero indexed
            nextAttackTime = Time.time + (1f / attackRate[gunIndex.GetValue() - 1]);
        }
    }
    */
}
