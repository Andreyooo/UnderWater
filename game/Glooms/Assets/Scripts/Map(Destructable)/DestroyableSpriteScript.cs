using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableSpriteScript : MonoBehaviour {

    private void Awake()
    {
        gameObject.GetComponent<Explodable>().explode();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gameObject.GetComponent<Explodable>().explode();
    }
}
