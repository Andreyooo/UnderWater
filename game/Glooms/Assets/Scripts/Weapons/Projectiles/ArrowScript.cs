﻿using System.Collections;
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
         DestroyProjectileAfterTime(0);
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
            DestroyProjectileAfterTime(destroyDelay);
            if (collision.gameObject.name == "Player(Clone)")
            {
                var hit = collision.gameObject;
                var playerStats = hit.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                    GameManager.instance.CurrentPlayerGetsExp(expGain);
                }
            }
        }
    }
}
