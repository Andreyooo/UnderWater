using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosionScript : MonoBehaviour {
    public float delay = 0;
    public int damage;
    public AudioClip expl;
    private bool over = false;

    // Use this for initialization
    void Start()
    {
        SoundManager.PlayAudioClip(expl);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player(Clone)" && !over)
        {
            Invoke("ExplosionHappened", 0.001f);
            var hit = collision.gameObject;
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    //Um mehrmaliges triggern des selben Spielers zu verhindern
    private void ExplosionHappened()
    {
        over = true;
    }
}
