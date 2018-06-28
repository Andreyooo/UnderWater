using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaExplosionScript : MonoBehaviour {
    public float delay = 0;
    public int damage;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, gameObject.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player(Clone)")
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
