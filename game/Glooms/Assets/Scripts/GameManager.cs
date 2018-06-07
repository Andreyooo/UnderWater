using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int shotsPerTurn = 1;
	private int currentShots;

    public int currentPlayer = 0;
	private GameObject previousPlayer = null;
    public GameObject playerPrefab;
    public List<GameObject> players;

    void Awake () {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
            Destroy(gameObject);
            }
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        float[] xPos = { -4.8f, -1, 5.4f };
        for(int i=0; i < 3; i++)
        {
            GameObject aPlayer = Instantiate(playerPrefab);
            aPlayer.transform.position = new Vector3(xPos[i], 2, 1);
            players.Add(aPlayer);
            if (i == 2)
                aPlayer.GetComponent<PlayerController>().FlipX(true);
        }
		currentShots = shotsPerTurn;
        SwitchPlayer();
	}

	public void HasFired(){
		currentShots--;
		if(currentShots <= 0){
			previousPlayer = players[currentPlayer];
			currentShots = shotsPerTurn;
            currentPlayer++;
            if (currentPlayer > players.Count-1)
            {
                currentPlayer = 0;
            }
            SwitchPlayer();
        }
	}

    public void SwitchPlayer()
    {
        if (previousPlayer != null)
        {
            previousPlayer.GetComponent<PlayerController>().SetPassive();
        }
        players[currentPlayer].GetComponent<PlayerController>().SetActive();

        /*if (currentPlayer == 0)
        {
            player2.GetComponent<PlayerController>().SetPassive();
            player1.GetComponent<PlayerController>().SetActive();
        }

        if (currentPlayer == 1)
        {
            player1.GetComponent<PlayerController>().SetPassive();
            player2.GetComponent<PlayerController>().SetActive();
        }

        if (currentPlayer == 2)
        {
            player1.GetComponent<PlayerController>().SetPassive();
            player2.GetComponent<PlayerController>().SetActive();
        }*/
    }
}
