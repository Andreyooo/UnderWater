using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBulletScript : Projectile {
    public GameObject explosionPrefab;

    private Rigidbody2D rb2D;
    private bool exploded = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 v = rb2D.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collider){
         DestroyProjectileAfterTime(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!exploded)
        {
            exploded = true;
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            explosion.GetComponent<BazookaExplosionScript>().damage = damage;
            SoundManager.PlayAudioClip(hitSound);
            DestroyProjectileAfterTime(0);
        }
    }
}
