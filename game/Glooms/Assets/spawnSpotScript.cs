using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnSpotScript : MonoBehaviour {
    private Vector3 newPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        newPos.y = 10;
        gameObject.transform.position = newPos;
    }
}
