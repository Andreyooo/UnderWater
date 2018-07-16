﻿/* Written by Dennis Lavrinov */
/* PlayerHealth.cs */
using UnityEngine;
using System.Collections;


public class PlayerStats : MonoBehaviour {
	public int maxHealth = 25;
	float currentHealth = 0;
    public int experience = 0;
    public int turnExperience = 0;

    public bool finishedTurn;

    public GameObject ExpGainPrefab;

    //Sounds
    private int soundVar;
    private string playerHitSound;
    private string playerJumpSound;
    private string playerDeathSound;
    public AudioClip expGainSound;

	public SimpleHealthBar healthBar;
    public GameObject blood, deadHead, deadBody, deadLeftLeg, deadLeftHand, deadRightLeg, deadRightHand;

	void Start ()
	{
		// Set the current health to max values.
		currentHealth = maxHealth;

        soundVar = Random.Range(0, 3);
        if (soundVar == 0)
        {
            playerHitSound = "ouch";
            playerJumpSound = "jump";
            playerDeathSound = "death";
        }
        if (soundVar == 1)
        {
            playerHitSound = "hmpf";
            playerJumpSound = "jump";
            playerDeathSound = "death";
        }
        if (soundVar == 2)
        {
            playerHitSound = "argh";
            playerJumpSound = "Viking Jump";
            playerDeathSound = "Viking Death";
        }

        healthBar.UpdateBar( currentHealth, maxHealth );
	}

    //-------------------------Health-Functions------------------------
    public void HealPlayer ()
    {
        // Increase the current health by 25%.
        currentHealth += ( maxHealth / 4 );

        // If the current health is greater than max, then set it to max.
        if( currentHealth > maxHealth )
            currentHealth = maxHealth;

        // Update the Simple Health Bar with the new Health values.
        healthBar.UpdateBar( currentHealth, maxHealth );
    }

    public void TakeDamage ( int damage )
	{
		currentHealth -= damage;
        if (currentHealth > 0) SoundManager.PlaySound(playerHitSound);
        if ( currentHealth <= 0 )
		{
		    // Set the current health to zero.
			currentHealth = 0;

			// Run the Death function since the player has died.
			Death();
		}
		healthBar.UpdateBar( currentHealth, maxHealth );
    }

    public void Death ()
	{
        SoundManager.PlaySound(playerDeathSound);
        
        //Animation
        Instantiate(blood, transform.position, Quaternion.identity);
        Instantiate(deadBody, transform.position, Quaternion.identity);
        Instantiate(deadHead, transform.position, Quaternion.identity);
        Instantiate(deadLeftLeg, transform.position, Quaternion.identity);
        Instantiate(deadRightLeg, transform.position, Quaternion.identity);
        Instantiate(deadLeftHand, transform.position, Quaternion.identity);
        Instantiate(deadRightHand, transform.position, Quaternion.identity);
        StartCoroutine("ShakeCamera");

        //Deactivate Player
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        Invoke("DeactivatePlayer", 11);
    }

    //---------------------------RPG-Functions-------------------------
    public void ExpGain(int exp)
    {
        finishedTurn = false;
        turnExperience += exp;
        Debug.Log("TurnExperience: " + turnExperience);
    }

    public IEnumerator AddExperience()
    {
        Vector3 animationPosition = gameObject.transform.position;
        animationPosition.y += 0.8f;
        while (turnExperience > 0)
        {
            SoundManager.PlayAudioClip(expGainSound);
            experience++;
            turnExperience--;
            GameObject expAnimation = Instantiate(ExpGainPrefab);
            expAnimation.transform.position = animationPosition;
            yield return new WaitUntil(() => !expAnimation.GetComponent<ParticleSystem>().IsAlive());
        }

        //Let´s the camera switch to the next Player
        Invoke("Test", 1);
    }

    private void Test()
    {
        finishedTurn = true;
    }

    //-----------------------------Tools----------------------------
    private void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public void PlayJumpSound()
    {
        SoundManager.PlaySound(playerJumpSound);
    }

	IEnumerator ShakeCamera ()
	{
		// Store the original position of the camera.
		Vector2 origPos = Camera.main.transform.position;
        for( float t = 0.0f; t < 0.1f; t += Time.deltaTime * 2.0f )
        {
			// Create a temporary vector2 with the camera's original position modified by a random distance from the origin.
			Vector2 tempVec = origPos + Random.insideUnitCircle/5;

			// Apply the temporary vector.
			Camera.main.transform.position = tempVec;

			// Yield until next frame.
			yield return null;
		}

		// Return back to the original position.
		Camera.main.transform.position = origPos;
	}
}
