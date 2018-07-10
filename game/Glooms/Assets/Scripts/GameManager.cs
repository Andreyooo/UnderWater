using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int shotsPerTurn = 1;
    public bool projectileDestroyed = false;
	private int currentShots;
    private GameObject previousPlayer = null;

    private CameraManager cam;
    public int currentPlayer = 0;
    public GameObject playerPrefab;
    public List<GameObject> players;

    //win
    public GameObject win;
    public AudioClip winTheme;

    //FX
    public AudioClip switchPlayerSound;

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
    }

    // Use this for initialization
    void Start () {
        float[] xPos = { -4.4f, -1, 5.4f };
        for(int i=0; i < 3; i++)
        {
            GameObject aPlayer = Instantiate(playerPrefab);
            aPlayer.transform.position = new Vector3(xPos[i], 4, 1);
            players.Add(aPlayer);
            if (i == 2)
                aPlayer.GetComponent<PlayerController>().Flip();
        }
		currentShots = shotsPerTurn;
        SwitchPlayer();
	}

	public IEnumerator HasFired(Projectile projectile){
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
        for (int j = 0; j < players.Count; j++)
        {
            if (!players[j].GetComponent<PolygonCollider2D>().enabled)
            {
                players.RemoveAt(j);
                Debug.Log("Player " + j + " aus der SpielerListe gelöscht");
                if (currentPlayer>=j)
                {
                    currentPlayer--;
                }
                j--;
            }
            if(players.Count <= 1){
                
            }
        }

        if (players.Count <= 1)
        {
            win.SetActive(true);
            SoundManager.PlayAudioClip(winTheme);
            return;
        }

        if (currentPlayer >= 0)
        {
            previousPlayer = players[currentPlayer];
        }  
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
        SoundManager.PlayAudioClip(switchPlayerSound);
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        cam.player = players[currentPlayer];
        cam.transPlayer = true;
    }
}
