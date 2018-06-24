using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenu : MonoBehaviour {

	public Sprite[] frames; 
	private float fps = 14;

	private float index = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () { 
		index = Time.time * fps; 
		index = index % frames.Length; 
		GetComponent<SpriteRenderer>().sprite = frames[(int)index]; 
	}
}
