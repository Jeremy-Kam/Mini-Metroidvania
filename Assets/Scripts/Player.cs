using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Each "heart" counts as two health
    [SerializeField] private HP PlayerHP;
    [SerializeField] private IntVariable gunIndex;
    [SerializeField] private GameEvent playerChangeGun;
    [SerializeField] private GameEvent playerChangeHealth;
    [SerializeField] private PowerUp wallJumpPower;
    [SerializeField] private PowerUp dashPower;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float wallSlidingSpeed;

    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float groundDeceleration;
    [SerializeField] private float airDeceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForceX;
    [SerializeField] private float wallJumpForceY;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float stopDashTime;

    private int lastWallTouched; // -1 is left, 0 is no wall, 1 is right

    private Vector2 currentVelocity;
    private float currentAcceleration;

    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [SerializeField] private float wallCoyoteTime;
    private float wallCoyoteTimeCounter;

    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;

    private float originalGravity;
    private float originalDrag;

    private bool isLeavingLevel = false;
    private bool isInHitstun = false;

    [SerializeField] private LayerMask p_WhatIsGround;
    [SerializeField] private Transform firePoint;

    private Rigidbody2D rb2D;
    private CapsuleCollider2D capC2D;
    private bool facingRight = true;

    [SerializeField] private float invincibleTime;
    private bool isInvincible = false;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        capC2D = GetComponent<CapsuleCollider2D>();

        originalGravity = rb2D.gravityScale;
        originalDrag = rb2D.drag;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeavingLevel || isInHitstun)
        {
            return;
        }
        
        if(isDashing)
        {
            if(isNextToLeftWall() || isNextToRightWall())
            {
                StopCoroutine("Dash");
                rb2D.drag = originalDrag;
                rb2D.gravityScale = originalGravity;
                // rb2D.velocity = Vector2.zero;
                isDashing = false;
                canDash = true;

                // Debug.Log("Drag reset to: " + rb2D.drag);
            } else
            {
                return;
            }
        }
        
        Move();
        CheckGunSwitch();

        // Debug.Log("Drag 2: " + rb2D.drag);

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            lastWallTouched = 0;
            canDash = true;
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (isNextToLeftWall() || isNextToRightWall())
        {
            wallCoyoteTimeCounter = wallCoyoteTime;
            lastWallTouched = (isNextToLeftWall()) ? -1 : 1;
            canDash = true;
        } else
        {
            wallCoyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        } else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Essentially pretends I'm grounded here
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            // Perhaps add a modifier to the jumpForce that will test if the player is on a moving platform
            // It will make the player jump higher when the platform is moving up, and vice versa
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            FindObjectOfType<AudioManager>().Play("playerJump");

        } else
        {
            // Wall Jumping
            // Only check if we have wallJumpPower at the end for performance
            if(jumpBufferCounter > 0f && wallCoyoteTimeCounter > 0f && wallJumpPower.GetValue())
            {
                // Debug.Log("Jumped");
                rb2D.velocity = new Vector2(wallJumpForceX * lastWallTouched * -1, wallJumpForceY);
                jumpBufferCounter = 0f;
                // Debug.Log(rb2D.drag);

                FindObjectOfType<AudioManager>().Play("playerJump");

            } else if(((isNextToLeftWall() && Input.GetKey(KeyCode.A)) || (isNextToRightWall() && Input.GetKey(KeyCode.D))) && rb2D.velocity.y < 0f) // Sliding if holding towards wall and falling
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, wallSlidingSpeed * -1);
                
            }
        }

        // Jumping further by holding down button
        if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        // Capped Fall Speed
        if (rb2D.velocity.y < 0f && rb2D.velocity.y < (maxFallSpeed * -1))
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, (maxFallSpeed * -1));
        }

        if(canDash && Input.GetButtonDown("Fire2") && dashPower.GetValue())
        {
            StartCoroutine("Dash");
        }
    }

    // Moves character
    private void Move()
    {
        float dir = 0.0f;
        if (Input.GetKey(KeyCode.A)) dir--;
        if (Input.GetKey(KeyCode.D)) dir++;

        if (dir == 0) // No input --> Just stop
        {
            currentAcceleration = (isGrounded()) ? groundDeceleration : airDeceleration;
        } else if (dir < 0 ^ currentVelocity.x < 0) // Skidding from turning around
        {
            currentAcceleration = (isGrounded()) ? groundAcceleration + groundDeceleration : airAcceleration + airDeceleration;
        } else // Starting to run
        {
            currentAcceleration = (isGrounded()) ? groundAcceleration : airAcceleration;
        }

        // This modifier changes the velocity when the character is on a moving platform
        float modifier = 0;
        if (isGrounded())
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(capC2D.bounds.center, Vector2.Scale(capC2D.bounds.size, new Vector2(0.95f, 0.95f)), 0f, Vector2.down, groundCheckDistance, p_WhatIsGround);
            if (raycastHit.collider.tag == "MovingPlatform")
            {
                // currentVelocity += raycastHit.rigidbody.velocity;
                modifier = raycastHit.rigidbody.velocity.x;
            }
        }


        currentVelocity = rb2D.velocity;
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, dir * maxSpeed + modifier, currentAcceleration);

        
        rb2D.velocity = currentVelocity;
        // Debug.Log(rb2D.velocity);


        // Debug.Log(rb2D.velocity);

        // Input is moving character right and is facing left
        if (dir == 1 && !facingRight)
        {
            Flip();
        } 
        // Input is moving character left and is facing right
        else if (dir == -1 && facingRight)
        {
            Flip();
        }


        // Debug.Log(dir);
    }

    private bool isGrounded()
    {
        // The size is downscaled because of weird interactions when pressing against a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(capC2D.bounds.center, Vector2.Scale(capC2D.bounds.size, new Vector2(0.95f, 0.95f)), 0f, Vector2.down, groundCheckDistance, p_WhatIsGround);
        
        
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }

        // Debug.DrawRay(capC2D.bounds.center + new Vector3(capC2D.bounds.extents.x, 0), Vector2.down * (capC2D.bounds.extents.y + groundCheckDistance), rayColor);
        // Debug.DrawRay(capC2D.bounds.center - new Vector3(capC2D.bounds.extents.x, 0), Vector2.down * (capC2D.bounds.extents.y + groundCheckDistance), rayColor);
        // Debug.DrawRay(capC2D.bounds.center - new Vector3(0, capC2D.bounds.extents.y), Vector2.right * capC2D.bounds.extents.x, rayColor);

        

        // Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

    private bool isNextToLeftWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capC2D.bounds.center, Vector2.Scale(capC2D.bounds.size, new Vector2(0.95f, 0.95f)), 0f, Vector2.left, wallCheckDistance, p_WhatIsGround);

        // Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
    }

    private bool isNextToRightWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capC2D.bounds.center, Vector2.Scale(capC2D.bounds.size, new Vector2(0.95f, 0.95f)), 0f, Vector2.right, wallCheckDistance, p_WhatIsGround);

        // Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
    }

    // When leaving a level, we want to remove control and have the character move out while the level is fading
    public void EndLevel(Vector2 direction)
    {
        StartCoroutine(MoveInDirection(direction));
    }

    // Will return true if player dies from spikes, and false if not
    public bool HitSpikes(int damage)
    {
        TakeDamage(damage, Vector2.zero);
        if (PlayerHP.GetValue() <= 1) // Will die from the spikes
        {
            return true;
        }
        StartCoroutine(MoveInDirection(Vector2.up));
        return false;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        if(isInvincible)
        {
            FindObjectOfType<AudioManager>().Play("invincibleBlip");
            return;
        }

        PlayerHP.SetValue(PlayerHP.GetValue() - damage);
        playerChangeHealth.Raise();

        if(damage <= 2)
        {
            FindObjectOfType<AudioManager>().Play("playerHit");
        } else if (damage <= 4)
        {
            FindObjectOfType<AudioManager>().Play("criticalHit");
        } else
        {
            FindObjectOfType<AudioManager>().Play("explosionHit");
        }

        StartCoroutine(Stun(CalculateHitStun(knockback)));
        if(isDashing)
        {
            StopCoroutine("Dash");
            rb2D.drag = originalDrag;
            rb2D.gravityScale = originalGravity;
            isDashing = false;
            canDash = true;
        }

        rb2D.velocity = Vector2.zero;

        // Debug.Log("Hitsun: " + CalculateHitStun(knockback));

        rb2D.AddForce(knockback, ForceMode2D.Impulse);

        // Debug.Log(PlayerHP.GetValue());

        if (PlayerHP.GetValue() <= 0)
        {
            Die();
        }

        StartCoroutine(setInvincible());
    }

    public void HealHealth(int heal)
    {
        PlayerHP.SetValue(PlayerHP.GetValue() + heal);

        FindObjectOfType<AudioManager>().Play("heartPickup");

        if (PlayerHP.GetValue() > PlayerHP.GetMaxValue())
        {
            PlayerHP.SetValue(PlayerHP.GetMaxValue());
        }

        playerChangeHealth.Raise();
    }
    
    public void IncreaseMaxHealth()
    {
        PlayerHP.SetMaxValue(PlayerHP.GetMaxValue() + 2);

        FindObjectOfType<AudioManager>().Play("heartContainerPickup");

        // Heal to full
        PlayerHP.SetValue(PlayerHP.GetMaxValue());
        playerChangeHealth.Raise();
    }
    private void Die()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(gm)
        {
            gm.EndGame();
            // Do a Death animation
        } else
        {
            Debug.LogWarning("Game Manager Not Found, Make sure it transfered between stages");
        }
        
    }

    private float CalculateHitStun(Vector2 knockback)
    {
        return (float)Math.Sqrt((knockback.x * knockback.x) + (knockback.y * knockback.y)) / 65;
    }

    private void CheckGunSwitch()
    {
        // Uses KeyDown instead of just Key as it would return true every frame. This only returns true the frame it is pressed, and needs to be released before it can return true again.
        // This code kinda sucks, but I don't want to make it better
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunIndex.SetValue(1);
            playerChangeGun.Raise();
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunIndex.SetValue(2);
            playerChangeGun.Raise();
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunIndex.SetValue(3);
            playerChangeGun.Raise();
        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gunIndex.SetValue(4);
            playerChangeGun.Raise();
        }
    }

    private IEnumerator Dash()
    {
        FindObjectOfType<AudioManager>().Play("playerDash");
        canDash = false;
        isDashing = true;
        originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;
        
        rb2D.velocity = new Vector2(((facingRight) ? 1 : -1) * dashingSpeed, 0f);

        yield return new WaitForSeconds(dashingTime);

        originalDrag = rb2D.drag;
        rb2D.drag = 10f;

        yield return new WaitForSeconds(stopDashTime);

        rb2D.drag = originalDrag;
        rb2D.gravityScale = originalGravity;
        isDashing = false;
    }

    private IEnumerator Stun(float stunTime)
    {
        isInHitstun = true;
        yield return new WaitForSeconds(stunTime);
        isInHitstun = false;
    }

    private IEnumerator MoveInDirection(Vector2 direction)
    {
        isLeavingLevel = true;
        
        if(direction == Vector2.up)
        {
            rb2D.gravityScale = 0;
        }
        
        rb2D.velocity = direction * maxSpeed;

        yield return null;
    }

    private IEnumerator setInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
