using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementTimerScript : MonoBehaviour {

	Image timerBar;
	public float maxTime = 5f;
	public float timeLeft;
    public bool playerMoving = false;
	// Use this for initialization
	void Start () {
		timerBar = GetComponent<Image>();
		timeLeft = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0 && playerMoving) {
			timeLeft -= Time.deltaTime;
			timerBar.fillAmount = timeLeft/maxTime;
		}
      //  Debug.Log(playerMoving);
    }

	public void SetActive () {
        timerBar.fillAmount = timeLeft / maxTime;
    }

	public void SetPassive () {
        playerMoving = false;
        timeLeft = maxTime;
        timerBar.fillAmount = 0 / maxTime;
    }
}
