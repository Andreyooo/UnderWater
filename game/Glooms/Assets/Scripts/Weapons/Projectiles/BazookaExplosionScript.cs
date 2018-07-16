using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaExplosionScript : MonoBehaviour {
    public float delay = 0;
    public int damage;
    public int expGain;
    private bool over = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player(Clone)" && !over)
        {
            Invoke("ExplosionHappened", 0.05f);
            var hit = collision.gameObject;
            var playerStats = hit.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                GameManager.instance.CurrentPlayerGetsExp(expGain);
            }
        }
    }

    //Um mehrmaliges triggern des selben Spielers zu verhindern
    private void ExplosionHappened()
    {
        over = true;
    }
}
