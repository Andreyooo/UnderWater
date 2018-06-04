using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private Rigidbody2D rb2D;
    private bool inAir = true;
    public int dmg = 1;

    private void OnCollisionEnter(Collision collision)
    {
        inAir = false;
        Debug.Log("hit");
        //Debug.Log(collision.gameObject.name);
        Destroy(transform.parent.gameObject);
        if (collision.gameObject.name == "Player2")
        {
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
