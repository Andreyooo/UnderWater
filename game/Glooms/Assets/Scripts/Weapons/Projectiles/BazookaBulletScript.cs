using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBulletScript : Projectile {
    public GameObject explosionPrefab;

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
            Debug.Log("adjusted");
            adjusted = true;
        }

        if (accelerationBoost > 0 && adjusted == false)
        {
            rb2D.AddForce(fpnt.forward * accelerationBoost * rb2D.velocity.x/4, ForceMode2D.Impulse);
            accelerationBoost -= accelerationDecrease;
            accelerationDecrease += 0.0015f;
        }
        if (accelerationBoost < 0 && adjusted == true)
        {
            rb2D.AddForce(fpnt.forward * accelerationBoost * rb2D.velocity.x / 4, ForceMode2D.Impulse);
            accelerationBoost -= accelerationDecrease;
            accelerationDecrease -= 0.0015f;
        }

        Vector2 v = rb2D.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<BazookaExplosionScript>().damage = damage;
        SoundManager.PlayAudioClip(hitSound);
        DestroyProjectileAfterTime(0);
    }
}
