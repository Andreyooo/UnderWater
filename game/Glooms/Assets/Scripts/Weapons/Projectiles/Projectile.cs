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
        Invoke("ExecuteProjectileDestruction", delay + 0.1f);
    }

    private void ExecuteProjectileDestruction()
    {
        GameManager.instance.projectileDestroyed = true;
        Destroy(gameObject);
    }
}
