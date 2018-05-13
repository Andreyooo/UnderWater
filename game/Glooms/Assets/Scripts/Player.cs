using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private float pos;
    private void Start()
    {
        anim = GetComponent<Animator>();
        pos = transform.position.x;
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        transform.Translate(x, 0, 0);
        if(x != pos)
        {
            pos = x;
            anim.SetTrigger("Walk");
        } else
        {
            anim.SetTrigger("Idle");
        }
    }
}
