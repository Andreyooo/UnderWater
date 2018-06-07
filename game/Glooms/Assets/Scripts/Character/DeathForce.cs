using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathForce : MonoBehaviour {

    Rigidbody2D rb;
    float dirX;
    float dirY;
    float torque;

	// Use this for initialization
	void Start () {

        dirX = Random.Range(-2, 2);
        dirY = Random.Range(2, 5);
        torque = Random.Range(3, 15);
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(dirX, dirY), ForceMode2D.Impulse);
        rb.AddTorque(torque, ForceMode2D.Force);

       Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
