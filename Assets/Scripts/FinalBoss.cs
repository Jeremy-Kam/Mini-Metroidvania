using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    [SerializeField] float shootingSpeedScalingFactor;
    [SerializeField] private Vector2 knockback;
    [SerializeField] private float playerDetectionDeadzone;

    [SerializeField] private float areaAttackDeadzoneSize;
    [SerializeField] private float areaAttackStartup;
    [SerializeField] private int areaAttackDamage;
    [SerializeField] private Vector2 areaAttackKnockback;

    // If the boss does not get close enough to the player to proc an attack normally for a certain amount of time, it will just attack
    [SerializeField] private float randomAttackRate;
    [SerializeField] private float nextAttackTime; // This is the amount of seconds before the final boss will just attack

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject smallProjectilePrefab;

    // 0 is idle, 1 is swinging, 2 is shooting, 3 is shooting transition, 4 is area attacking
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

    // For the shooting attack
    private void MoveSlow()
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
                GetComponent<Enemy>().rb2D.velocity = new Vector2((enemySpeed * shootingSpeedScalingFactor) * -1, GetComponent<Enemy>().rb2D.velocity.y);
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
                GetComponent<Enemy>().rb2D.velocity = new Vector2((enemySpeed * shootingSpeedScalingFactor), GetComponent<Enemy>().rb2D.velocity.y);
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
                Attack();
            }
            else if (SeePlayer())
            {
                
                Move();
                // If we move for too long without attacking, then randomly attack
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                }
            }
        } else if (state == 1)
        {
            FacePlayer();
        } else if (state == 2)
        {
            MoveSlow();
        } else if (state == 3)
        {
            // Stand still
            // Used for area attack as well
            GetComponent<Enemy>().rb2D.velocity = new Vector2(0, GetComponent<Enemy>().rb2D.velocity.y);
        }
    }

    private void Attack()
    {
        int randomNumber = UnityEngine.Random.Range(0, 3); // Should generate 0, 1, 2

        if (randomNumber == 0)
        {
            SwingAttack();
        }
        else if(randomNumber == 1)
        {
            ShootAttack();
        } else if (randomNumber == 2)
        {
            AreaAttack();
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

    private void ShootAttack()
    {
        state = 3; // Stand still
        animator.SetTrigger("Shoot");
    }

    private void Shoot()
    {
        state = 2; // Move slow
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        for(int i = 0; i < 10; ++i)
        {
            Instantiate(smallProjectilePrefab, firePoint.position, firePoint.rotation);
            FindObjectOfType<AudioManager>().Play("enemyBulletShoot2");
            yield return new WaitForSeconds(0.1f);
        }

        animator.SetTrigger("StopShoot");
        state = 3; // Stand still
    }

    private void AreaAttack()
    {
        state = 3; // Stand still
        animator.SetTrigger("AreaAttack");
    }

    private void Area()
    {
        state = 3; // Keep standing still
        StartCoroutine(AreaAttacking());
    }

    private IEnumerator AreaAttacking()
    {
        // GetComponentInChildren<AreaAttack>().gameObject.SetActive(true);

        // index 6 is the area attack sprite
        this.transform.GetChild(6).gameObject.SetActive(true);

        yield return new WaitForSeconds(areaAttackStartup);

        HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();
        List<Collider2D> hits = new List<Collider2D>();

        FindObjectOfType<AudioManager>().Play("areaAttack");

        hits.AddRange(Physics2D.OverlapCircleAll(transform.position, areaAttackDeadzoneSize, GetComponent<Enemy>().playerLayer));

        foreach (Collider2D hit in hits)
        {
            uniqueHits.Add(hit);
        }

        // Outside the safe space
        if(uniqueHits.ToArray().Length == 0)
        {
            GetComponent<Enemy>().player.GetComponent<Player>().TakeDamage(areaAttackDamage, areaAttackKnockback);
        }

        // GetComponentInChildren<AreaAttack>().gameObject.SetActive(false);

        this.transform.GetChild(6).gameObject.SetActive(false);

        animator.SetTrigger("StopAreaAttack");
        state = 3; // Stand still
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 heading = firePoint.position - GetComponent<Enemy>().player.transform.position;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance; // This is now the normalized direction.

        return direction;
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
        nextAttackTime = Time.time + (1f / randomAttackRate);
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
        Gizmos.DrawWireSphere(transform.position, areaAttackDeadzoneSize);

        // Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
    }
}
