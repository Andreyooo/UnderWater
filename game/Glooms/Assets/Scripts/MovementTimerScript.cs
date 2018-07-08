using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementTimerScript : MonoBehaviour {

	Image timerBar;
	public float maxTime = 5f;
	float timeLeft;
	// Use this for initialization
	void Start () {
		timerBar = GetComponent<Image>();
		timeLeft = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			timerBar.fillAmount = timeLeft/maxTime;
		} 
	}

	public void SetActive () {
		timeLeft = maxTime;
	}

	public void SetPassive () {
		timeLeft = 0;
		timerBar.fillAmount = timeLeft/maxTime;
	}
}
