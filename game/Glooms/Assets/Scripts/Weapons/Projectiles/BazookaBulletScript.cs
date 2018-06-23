using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBulletScript : Projectile {
    private Rigidbody2D rb2D;
    private bool adjusted = false;
    private float accelerationBoost = 0.3f;
    private float accelerationDecrease = 0.001f;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (fpnt.forward.x < 0 && !adjusted)
        {
            accelerationBoost *= -1;
            accelerationDecrease *= -1;
            adjusted = true;
        }

        if (accelerationBoost > 0)
        {
            rb2D.AddForce(fpnt.forward * accelerationBoost * rb2D.velocity.x/4, ForceMode2D.Impulse);
            accelerationBoost -= accelerationDecrease;
            accelerationDecrease += 0.002f;
        } 
        Debug.Log(rb2D.velocity);
        Vector2 v = rb2D.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.PlayAudioClip(hitSound);
        DestroyProjectileAfterTime(0);
        if (collision.gameObject.name == "Player(Clone)")
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

    }
}
