using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer characterSR;
    private Rigidbody2D characterRB;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded, jump;
    private float pos;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float movementSpeed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        characterSR = GetComponent<SpriteRenderer>();
        pos = transform.position.x;
        characterRB = GetComponent<Rigidbody2D>();
    }

    //void Update()
    //{
    //    Moving();
    //}

    void FixedUpdate()
    {
        // Reihenfolge wichtig
        HandleInput();
        isGrounded = IsGrounded();
        Moving();

        ResetValues();
    }

    //Movement+Animation
    private void Moving()
    {
        var x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        //horizontal movement
        if (isGrounded || airControl)
        {
            characterRB.velocity = new Vector2(x, characterRB.velocity.y);//transform.Translate(x, 0, 0);
        }

        //jumping
        if (isGrounded && jump)
        {
            isGrounded = false;
            characterRB.AddForce(new Vector2(0, jumpSpeed));

        }

        if (x != pos && isGrounded || airControl)
        {
            pos = x;
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.SetTrigger("Idle");
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Pressed Space");
            jump = true;
        }
    }

    //isCharacterFlipped?
    public bool GetXFlip()
    {
        return characterSR.flipX;
    }

    //flipping Character
    public void FlipX(bool bo)
    {
        characterSR.flipX = bo;
    }

    //checks if Character is grounded, ///(GZIBLCHSDJKBPICUS
    private bool IsGrounded()
    {
        if (characterRB.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround );

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        //Debug.Log("Grounded");
                        return true;
                    }

                }
            }
        }
        return false;
    }

    public void ResetValues()
    {
        jump = false;
    }
}


// TODO

// soll man in der luft laufen können? evtl beheben/verhindern

// hintereinander Space ist man noch in der luft, die groundpoints ggf. höher setzen bzw den "Boden" anpassen