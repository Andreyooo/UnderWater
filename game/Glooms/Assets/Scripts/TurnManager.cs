using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	public int shotsPerTurn = 1;
	private int currentShots;
    private Player p1;
    private Player p2;



    public int currentPlayer = 1;
	public int previousPlayer;
	public GameObject[] player1Objects;
    public GameObject[] player2Objects;


    // Use this for initialization
    void Start () {
		currentShots = shotsPerTurn;
        ReallySwitchPlayer();
	}

	public void HasFired(){
		currentShots--;
		if(currentShots <= 0){
			previousPlayer = currentPlayer;
			currentShots = shotsPerTurn;
            currentPlayer++;
            if (currentPlayer > 2)
            {
                currentPlayer = 1;
            }
            ReallySwitchPlayer();
        }
	}

    public void ReallySwitchPlayer()
    {
        if (currentPlayer == 1)
        {
            player1Objects[0].transform.Find("ShootingWeapon").gameObject.SetActive(true);
            player1Objects[0].GetComponent<Player>().enabled = true;

            player2Objects[0].transform.Find("ShootingWeapon").gameObject.SetActive(false);
            player2Objects[0].GetComponent<Player>().enabled = false;

        }

        if (currentPlayer == 2)
        {
            player2Objects[0].transform.Find("ShootingWeapon").gameObject.SetActive(true);
            player2Objects[0].GetComponent<Player>().enabled = true;


            player1Objects[0].transform.Find("ShootingWeapon").gameObject.SetActive(false);
            player1Objects[0].GetComponent<Player>().enabled = false;

        }
    }
}
