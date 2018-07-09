using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    public int damage;
    public float bulletSpeed;
    public string firepoint;
    public Transform fpnt;
    public AudioClip releaseSound;
    public AudioClip hitSound;
    [SerializeField]
    private float lifeTime;
    private void Start()
    {
        SoundManager.PlayAudioClip(releaseSound);
        DestroyProjectileAfterTime(lifeTime);
    }

    public void DestroyProjectileAfterTime(float delay)
    {
        Invoke("ExecuteProjectileDestruction", delay);
    }

    private void ExecuteProjectileDestruction()
    {
        Invoke("ProjectileDestroyed", 0.1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    private void ProjectileDestroyed()
    {
        GameManager.instance.projectileDestroyed = true;
        Destroy(gameObject);
    }

}
