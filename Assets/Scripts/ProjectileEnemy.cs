using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform[] attackPoints; // Index 0 is detecting point
    [SerializeField] float playerDetectionSize;
    [SerializeField] Vector2 playerShootSize;
    [SerializeField] float enemySpeed;
    [SerializeField] private float playerDetectionDeadzone;

    [SerializeField] private EnemyWeapon ew;

    private bool isAttacking = false;


    private void Update()
    {
        if(!isAttacking)
        {
            if (SeePlayer())
            {
                if (IsWithinShootRange())
                {
                    Shoot();
                }
                else
                {
                    Move();
                }
            }
        }

        animator.SetFloat("speed", GetComponent<Enemy>().rb2D.velocity.x);
    }

    // Same as sword enemy
    private void Move()
    {
        // Debug.Log("Moving");
        if (GetComponent<Enemy>().player == null)
        {
            return;
        }

        // Debug.Log("Actually moving");

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

    private bool IsWithinShootRange()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(transform.position, playerShootSize, 0f, GetComponent<Enemy>().playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            if (hit)
            {
                GetComponent<Enemy>().player = hit;
                return true;
            }
        }

        // If not within shoot does not guarentee that it is not within seeing distance
        return false;
    }

    private void Shoot()
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

                isAttacking = true;
                animator.SetTrigger("shoot");
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

                isAttacking = true;
                animator.SetTrigger("shoot");
            }
        }
        else
        {
            GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);
        }
    }

    public void FireGun()
    {
        FindObjectOfType<AudioManager>().Play("enemyBulletShoot2");
        ew.Shoot();
    }

    public void EndFireGun()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
        // I am not drawing the detection box
    }
}
