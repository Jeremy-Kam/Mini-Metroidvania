using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss0 : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform[] attackPoints; // Index 0 is detecting point
    [SerializeField] float hitboxSize;
    [SerializeField] float playerDetectionSize;
    [SerializeField] int attackDamage;
    [SerializeField] float enemySpeed;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private float playerDetectionDeadzone;

    [SerializeField] private GameObject sheild;

    private bool isAttacking = false;

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

            // Debug.Log("Case 1");
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

            // Debug.Log("Case 2");
        }
        else
        {
            GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);

            // Debug.Log("Case 3");
        }
        // Debug.Log("Move: " + GetComponent<Enemy>().rb2D.velocity.x);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAttacking)
        {
            if (detectedPlayer())
            {
                // Debug.Log("Attack");
                sheild.SetActive(false);
                Attack();
            }
            else if (SeePlayer())
            {
                // Debug.Log("Move");
                Move();
            }
        }

        animator.SetFloat("Speed", Math.Abs(GetComponent<Enemy>().rb2D.velocity.x));
        // Debug.Log(GetComponent<Enemy>().rb2D.velocity.x);
    }

    private void Attack()
    {
        isAttacking = true;
        GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);
        animator.SetTrigger("Swing");
    }
    private void Swing()
    {
        HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();

        List<Collider2D> hits = new List<Collider2D>();

        FindObjectOfType<AudioManager>().Play("Boss0Swing");

        foreach (Transform hitboxPos in attackPoints)
        {
            hits.AddRange(Physics2D.OverlapCircleAll(hitboxPos.position, hitboxSize, GetComponent<Enemy>().playerLayer));
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
    }



    private bool detectedPlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoints[0].position, hitboxSize, GetComponent<Enemy>().playerLayer);
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

    private void endAttacking()
    {
        isAttacking = false;
        sheild.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
        {
            return;
        }
        foreach (Transform hitboxPos in attackPoints)
        {
            Gizmos.DrawWireSphere(hitboxPos.position, hitboxSize);
        }

        Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
    }
}
