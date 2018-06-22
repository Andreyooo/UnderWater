using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour {

    private Rigidbody2D rb2D;
    private bool inAir = true;
    public int dmg = 10;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.freezeRotation = true;
    }

    private void Start()
    {
        		SoundManager.PlaySound("rocket");

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

    //Destroys bullet after time
    public void DestroyProjectileAfterTime(float delay)
    {
        Invoke("ExecuteProjectileDestruction", delay);
    }

    private void ExecuteProjectileDestruction()
    {
        GameManager.instance.projectileDestroyed = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.PlaySound("explosion");
        inAir = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        //Debug.Log(collision.gameObject.name);
        DestroyProjectileAfterTime(1);
        if (collision.gameObject.name == "Player(Clone)")
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(dmg);
            }
        }

    }
}
