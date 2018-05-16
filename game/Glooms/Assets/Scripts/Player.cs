using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private float pos;
    private SpriteRenderer characterSR;

    private void Start()
    {
        anim = GetComponent<Animator>();
        characterSR = GetComponent<SpriteRenderer>();
        pos = transform.position.x;
    }

    void Update()
    {
        Moving();
    }

    //Movement+Animation
    private void Moving()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 1f;
        transform.Translate(x, 0, 0);
        if (x != pos)
        {
            pos = x;
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.SetTrigger("Idle");
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
}
