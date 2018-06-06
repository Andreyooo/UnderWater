using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    public float maxSpeed = 0.1f;
    public float jumpTakeOffSpeed = 5;
    public bool aimingMode;
    public bool movingMode;


    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void HandlePlayer()
    {
        if (movingMode)
        {
            Moving();
        }
        if (aimingMode)
        {
            animator.SetTrigger("Aim");
        }
    }
    private void Moving()
    {
        //Animation
        if (!inAir)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Idle");
            }
        }
        else
        {
            animator.SetTrigger("Offground");
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        //Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        //Flip player when moving
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        targetVelocity = move * maxSpeed;
    }

    //Set To Aiming Mode
    public void AimingModeActive()
    {
        movingMode = false;
        aimingMode = true;
        animator.SetTrigger("Aim");
        transform.Find("ShootingWeapon").gameObject.SetActive(true);
    }

    //Set To Aiming Mode
    public void MovingModeActive()
    {
        aimingMode = false;
        movingMode = true;
        transform.Find("ShootingWeapon").gameObject.SetActive(false);
    }

    //Flip Character
    public void FlipX(bool bo)
    {
        spriteRenderer.flipX = bo;
    }
}
