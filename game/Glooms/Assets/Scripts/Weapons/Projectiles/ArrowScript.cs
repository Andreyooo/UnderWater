using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : Projectile {
    private Rigidbody2D rb2D;
    private bool inAir = true;
    private bool hit = false;
    [SerializeField]
    private float destroyDelay;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.freezeRotation = true;
    }

    private void Update()
    {
        if (inAir)
        {
            Vector2 v = rb2D.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
         DestroyProjectileAfterTime(2.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hit)
        {
            hit = true;
            SoundManager.PlayAudioClip(hitSound);
            inAir = false;
            rb2D.isKinematic = true;
            rb2D.velocity = Vector2.zero;
            gameObject.GetComponent<Collider2D>().enabled = false;
            DestroyProjectileAfterTime(destroyDelay);
            if (collision.gameObject.tag == "Player")
            {
                var hit = collision.gameObject;
                var playerStats = hit.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                    PlayerStats currentPlayerStats = GameManager.instance.currentPlayer.GetComponent<PlayerStats>();
                    if (critActive) StartCoroutine(GameManager.instance.ShakeCamera());
                    if (currentPlayerStats.discharge)
                    {
                        playerStats.Discharged();
                        currentPlayerStats.currentShield = 0;
                        currentPlayerStats.shieldBar.UpdateBar(0, 15);
                    }
                    if (critActive) StartCoroutine(GameManager.instance.ShakeCamera());
                    if (poisonActive)
                    {
                        playerStats.Poisoned(poison, 3);
                    }
                    if (hit != GameManager.instance.currentPlayer)
                    {
                        GameManager.instance.CurrentPlayerGetsExp(expGain);
                    }
                    GameManager.instance.CurrentPlayerStealsLife(Mathf.RoundToInt(damage * playerStats.lifesteal));
                }
            }
        }
    }
}
