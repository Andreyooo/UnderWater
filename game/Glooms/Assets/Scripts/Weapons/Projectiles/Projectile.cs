using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    public int damage;
    public int expGain;
    public float bulletSpeed;
    public string firepoint;
    public Transform fpnt;
    public AudioClip releaseSound;
    public AudioClip hitSound;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float afterLifeTime;
    private void Start()
    {
        SoundManager.PlayAudioClip(releaseSound);
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
        GameManager.instance.projectileDestroyed = true;
        Destroy(gameObject);
    }

}
