using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    //Stuff
    public int shotsPerTurn = 1;
	private int currentShots;
    public bool projectileDestroyed = false;
    public bool playersSpawned = false;

    //Camera
    private CameraManager cam;

    //Players
    public GameObject playerPrefab;
    public GameObject nerdPrefab;
    public GameObject vikingPrefab;

    public List<GameObject> players;
    private GameObject previousPlayer = null;
    private int currentPlayer = 1;


    //Win
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
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        /*float[] xPos = { -24f, -1, 23f };
        for(int i=0; i < 3; i++)
        {
            GameObject aPlayer = Instantiate(playerPrefab);
            aPlayer.transform.position = new Vector3(xPos[i], 10, 1);
            players.Add(aPlayer);
            if (aPlayer.transform.position.x > 0)
            {
                aPlayer.GetComponent<PlayerController>().Flip();
            }
        }
        StartGame();*/
        StartCoroutine(SetupGame());
	}

    private IEnumerator SetupGame()
    {
        yield return new WaitUntil(() => playersSpawned);
        //StartGame();
    }

    private void StartGame()
    {
        currentShots = shotsPerTurn;

        //StartingPlayer
        players[currentPlayer].GetComponent<PlayerController>().SetActive();
        cam.player = players[currentPlayer];
        cam.transPlayer = true;
    }

	public IEnumerator HasFired(Projectile projectile){
		currentShots--;
        if (currentShots <= 0){
            players[currentPlayer].GetComponent<PlayerController>().SetPassive();
            Debug.Log("Start Waiting");
            yield return new WaitUntil(() => projectileDestroyed);
            Debug.Log("Stop Waiting");
            projectileDestroyed = false;
            StartCoroutine(FinishPlayerTurn());
        }
	}

    private void SwitchPlayer()
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

        if (currentPlayer >= 0)
        {
            previousPlayer = GetCurrentPlayer();
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
        GetCurrentPlayer().GetComponent<PlayerController>().SetActive();
        SoundManager.PlayAudioClip(switchPlayerSound);
        cam.player = GetCurrentPlayer();
        cam.transPlayer = true;

        if (players.Count <= 1)
        {
            win.SetActive(true);
            SoundManager.PlayAudioClip(winTheme);
            return;
        }
    }

    private IEnumerator FinishPlayerTurn()
    {
        if (players[currentPlayer].GetComponent<PolygonCollider2D>().enabled)
        {
            cam.player = GetCurrentPlayer();
            cam.transPlayer = true;
            yield return new WaitUntil(() => !cam.transPlayer);

            PlayerStats CurrentPlayerStats = GetCurrentPlayer().GetComponent<PlayerStats>();
            if (CurrentPlayerStats.turnExperience > 0)
            {
                StartCoroutine(CurrentPlayerStats.AddExperience());
                yield return new WaitUntil(() => CurrentPlayerStats.finishedTurn);
                Debug.Log("Experience: " + CurrentPlayerStats.experience);
            }
        }
        SwitchPlayer();
    }

    public void CurrentPlayerGetsExp(int exp)
    {
        GetCurrentPlayer().GetComponent<PlayerStats>().ExpGain(exp);
    }

    public GameObject GetCurrentPlayer()
    {
        return players[currentPlayer];
    }
}
