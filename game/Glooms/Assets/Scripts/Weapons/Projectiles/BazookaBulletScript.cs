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
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag !="Explosion" && collider.gameObject.tag != "Destroying")
        {
            DestroyProjectileAfterTime(0);
        } else
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider.gameObject.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!exploded)  
        {
            exploded = true;
            GameObject explosion = Instantiate(explosionPrefab);
            BazookaExplosionScript explScript = explosion.GetComponent<BazookaExplosionScript>();
            explScript.damage = damage;
            explScript.poison = poison;
            explScript.poisonActive = poisonActive;
            explosion.transform.position = transform.position;
            SoundManager.PlayAudioClip(hitSound);
            DestroyProjectileAfterTime(0);
        }
    }
}
