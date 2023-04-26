using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;

public class GhostEnemy : MonoBehaviour
{
    public Transform target;

    public float speed;
    public float nextWaypointDistance;

    public Transform enemyGFX;
    public float playerDetectionSize;

    public int attackDamage;
    public float attackRange;
    public float attackPower;

    Pathfinding.Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;

        }
    }

    void FixedUpdate()
    {
        if(PlayerWithinRange())
        {
            Attack();
            return;
        }
        if ((path == null) || (currentWaypoint >= path.vectorPath.Count) || (!SeePlayer()))
        {
            return;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * rb.mass * rb.drag * speed;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, GetComponent<Enemy>().playerLayer);
        if(hit != null)
        {
            Vector2 direction = new Vector2(hit.transform.position.x - transform.position.x, hit.transform.position.y - transform.position.y);
            direction = direction.normalized;

            Vector2 knockback = direction * attackPower;

            hit.GetComponent<Player>().TakeDamage(attackDamage, knockback);
            knockback = knockback * -1;
            rb.AddForce(knockback, ForceMode2D.Impulse);
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

    private bool PlayerWithinRange()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, GetComponent<Enemy>().playerLayer);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
