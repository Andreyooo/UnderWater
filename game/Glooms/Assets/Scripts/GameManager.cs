using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int shotsPerTurn = 1;
    public bool projectileDestroyed = false;
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

	public IEnumerator HasFired(GameObject projectile){
		currentShots--;
        if (currentShots <= 0){
            players[currentPlayer].GetComponent<PlayerController>().SetPassive();
            Debug.Log("Start Waiting");
            yield return new WaitUntil(() => projectileDestroyed);
            Debug.Log("Stop Waiting");
            projectileDestroyed = false;
            SwitchPlayer();
        }
	}

    public void SwitchPlayer()
    {
        for (int j = 0; j < players.Count - 1; j++)
        {
            if (!players[j].GetComponent<PolygonCollider2D>().enabled)
            {
                Debug.Log("Player " + j + " aus der SpielerListe gelöscht");
                players.RemoveAt(j);
                if (currentPlayer>=j)
                {
                    currentPlayer--;
                }
                j--;
            }
        }

        previousPlayer = players[currentPlayer];
        currentPlayer++;
        currentShots = shotsPerTurn;
        if (currentPlayer > players.Count - 1)
        {
            currentPlayer = 0;
        }
        Debug.Log("Spieler " + currentPlayer + " ist dran");
        if (previousPlayer != null && previousPlayer.activeSelf)
        {
            previousPlayer.GetComponent<PlayerController>().SetPassive();
        }
        players[currentPlayer].GetComponent<PlayerController>().SetActive();
    }
}
