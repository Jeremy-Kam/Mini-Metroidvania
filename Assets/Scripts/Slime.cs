using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : MonoBehaviour
{
    public Animator animator;
    public float jumpForceX;
    public float jumpForceY;

    private bool isJumping = false;
    private bool isFalling = false;

    public float groundCheckDistance;
    public LayerMask p_WhatIsGround;

    public Transform[] attackPoints; // Index 0 is detecting point

    public Transform airHitPoint;
    public float airHitboxSize;
    public Vector2 airKnockback;
    public int airAttackDamage;

    public float hitboxSize;
    public float playerDetectionSize;
    public int landingAttackDamage;
    public Vector2 knockback;

    // Update is called once per frame
    void Update()
    {
        if(isJumping)
        {
            if(isFalling)
            {
                if (isGrounded())
                {
                    // Debug.Log("Land");
                    animator.SetBool("isLanding", true);
                    
                    isJumping = false;
                    isFalling = false;
                    animator.SetBool("isJumping", isJumping);
                    animator.SetBool("isFalling", isFalling);
                }
            } else if(GetComponent<Enemy>().rb2D.velocity.y < 0)
            {
                isFalling = true;
                animator.SetBool("isFalling", isFalling);
            }

            HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();
            List<Collider2D> hits = new List<Collider2D>();
            
            hits.AddRange(Physics2D.OverlapCircleAll(airHitPoint.position, airHitboxSize, GetComponent<Enemy>().playerLayer));
            
            foreach (Collider2D hit in hits)
            {
                uniqueHits.Add(hit);
            }

            foreach (Collider2D hit in uniqueHits)
            {
                Vector2 temp = airKnockback;
                if (hit.transform.position.x < transform.position.x)
                {
                    temp.x *= -1;
                }
                hit.GetComponent<Player>().TakeDamage(airAttackDamage, temp);
            }
        } else
        {
            if(SeePlayer())
            {
                if ((GetComponent<Enemy>().player.transform.position.x < transform.position.x && GetComponent<Enemy>().facingRight) || 
                    (GetComponent<Enemy>().player.transform.position.x > transform.position.x && !GetComponent<Enemy>().facingRight))
                {
                    GetComponent<Enemy>().Flip();
                }

                if(isGrounded())
                {
                    isJumping = true;
                    animator.SetBool("isJumping", isJumping);
                }
                
            }
        }
    }

    private void playLandingSound()
    {
        FindObjectOfType<AudioManager>().Play("slimeLand");
    }

    private void Land()
    {
        HashSet<Collider2D> uniqueHits = new HashSet<Collider2D>();
        List<Collider2D> hits = new List<Collider2D>();

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

            if(hit.transform.position.x < transform.position.x)
            {
                temp.x *= -1;
            }
            hit.GetComponent<Player>().TakeDamage(landingAttackDamage, temp);
        }
    }


    private void Jump()
    {
        GetComponent<Enemy>().rb2D.velocity = new Vector2((GetComponent<Enemy>().facingRight) ? jumpForceX : (jumpForceX * -1), jumpForceY);
        FindObjectOfType<AudioManager>().Play("slimeJump");
    }

    private void StopLanding()
    {
        animator.SetBool("isLanding", false);
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

    private bool isGrounded()
    {
        // The size is downscaled because of weird interactions when pressing against a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetComponent<Enemy>().cap2D.bounds.center, Vector2.Scale(GetComponent<Enemy>().cap2D.bounds.size, new Vector2(0.95f, 0.95f)), 0f, Vector2.down, groundCheckDistance, p_WhatIsGround);

        /*
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(capC2D.bounds.center + new Vector3(capC2D.bounds.extents.x, 0), Vector2.down * (capC2D.bounds.extents.y + groundCheckDistance), rayColor);
        Debug.DrawRay(capC2D.bounds.center - new Vector3(capC2D.bounds.extents.x, 0), Vector2.down * (capC2D.bounds.extents.y + groundCheckDistance), rayColor);
        Debug.DrawRay(capC2D.bounds.center - new Vector3(0, capC2D.bounds.extents.y), Vector2.right * capC2D.bounds.extents.x, rayColor);

        Debug.Log("Is this working?");

        */

        // Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
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

        Gizmos.DrawWireSphere(airHitPoint.position, airHitboxSize);
        Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
    }

}
