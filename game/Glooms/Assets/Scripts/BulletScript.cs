using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private Rigidbody2D rb2D;
    private bool inAir = true;
    public int dmg = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inAir = false;
       // Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player2")
        {
            Debug.Log("PLAYER HIT!!!!");
            Destroy(transform.parent.gameObject);
        }
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (inAir)
        {
            Vector2 v = rb2D.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
