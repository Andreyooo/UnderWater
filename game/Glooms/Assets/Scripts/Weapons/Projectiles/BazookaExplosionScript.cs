using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaExplosionScript : MonoBehaviour {
    public float delay = 0;
    public int damage;
    public int expGain;
    public int poison;
    public int poisonTurns = 3;
    public bool poisonActive = false;
    private bool over = false;
    public bool critActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !over)
        {
            Invoke("ExplosionHappened", 0.05f);
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
                if (poisonActive)
                {
                    playerStats.Poisoned(poison, poisonTurns);
                }
                if (hit != GameManager.instance.currentPlayer)
                {
                    GameManager.instance.CurrentPlayerGetsExp(expGain);
                }
                GameManager.instance.CurrentPlayerStealsLife(Mathf.RoundToInt(damage * playerStats.lifesteal));
            }
        }
    }

    //Um mehrmaliges triggern des selben Spielers zu verhindern
    private void ExplosionHappened()
    {
        over = true;
    }
}
