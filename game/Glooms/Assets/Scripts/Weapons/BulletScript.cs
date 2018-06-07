using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

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

    public void DestroyProjectile()
    {
        Invoke("ExecuteProjectileDestruction", 1f);
    }

    private void ExecuteProjectileDestruction()
    {
        GameManager.instance.projectileDestroyed = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.PlaySound("arrowHit");
        inAir = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        //Debug.Log(collision.gameObject.name);
        DestroyProjectile();
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
