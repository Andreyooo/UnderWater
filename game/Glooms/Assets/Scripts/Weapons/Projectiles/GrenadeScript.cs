using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : Projectile {
    public GameObject explosionPrefab;

    private Rigidbody2D rb2D;
    private bool triggered = false;

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
        Debug.Log("Explosion erstellt");
        explosion.transform.position = transform.position;
        explosion.GetComponent<GrenadeExplosionScript>().damage = damage;
        DestroyProjectileAfterTime(0);


        /*Vector2 explosionPos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100 / 100);

        for (int i = 0; i < colliders.Length; i++)
        {
            // TODO: two calls for getcomponent is bad
            DestructibleSprite destructibleSpriteScript = colliders[i].GetComponent<DestructibleSprite>();
            if (destructibleSpriteScript)
                destructibleSpriteScript.ApplyDamage(explosionPos, 100);
        }*/
    }
}
