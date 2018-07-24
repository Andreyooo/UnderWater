using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    public int damage;
    public int expGain;
    public float bulletSpeed;
    public string firepoint;
    public int poison = 0;
    public int poisonTurns = 0;
    public int lifesteal = 0;
    public int critMultiplier = 1;
    public bool mainProjectile = true;
    public Transform fpnt;
    public AudioClip releaseSound;
    public AudioClip hitSound;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float afterLifeTime;
    private void Start()
    {
        if (mainProjectile)
        {
            SoundManager.PlayAudioClip(releaseSound);
        }
        DestroyProjectileAfterTime(lifeTime);
    }

    public void DestroyProjectileAfterTime(float delay)
    {
        if (delay == 0)
        {
            ExecuteProjectileDestruction();
        } else
        {
            Invoke("ExecuteProjectileDestruction", delay);
        }
    }

    private void ExecuteProjectileDestruction()
    {
        Invoke("ProjectileDestroyed", afterLifeTime);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void ProjectileDestroyed()
    {
        if (mainProjectile)
        {
            GameManager.instance.projectileDestroyed = true;
        }
        Destroy(gameObject);
    }



}
