using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : Projectile {
    public GameObject explosionPrefab;

    private Rigidbody2D rb2D;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collider){
         DestroyProjectileAfterTime(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered)
        {
            triggered = true;
            Invoke("Explode", 3);
        }
        SoundManager.PlayAudioClip(hitSound);
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.GetComponent<GrenadeExplosionScript>().damage = damage;
        DestroyProjectileAfterTime(0);
    }
}
