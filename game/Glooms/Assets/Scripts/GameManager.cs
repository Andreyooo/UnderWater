using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    //Stuff
	private int currentShots = 1;
    public bool projectileDestroyed = false;
    public bool playersSpawned = false;

    //Camera
    private CameraManager cam;

    //Players
    public GameObject playerPrefab;
    public GameObject nerdPrefab;
    public GameObject vikingPrefab;

    public List<GameObject> vikings;
    private int vikingIndex = 0;
    public List<GameObject> nerds;
    private int nerdIndex = 0;
    public List<GameObject> bandits;
    private int banditIndex = 0;
    private List<List<GameObject>> fractions = new List<List<GameObject>>();
    public List<string> playerTurnOrder;

    private GameObject previousPlayer = null;
    private GameObject currentPlayer;
    private int currentPlayerIndex = 0;
    private int currentFraction = 0;


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
        StartCoroutine(SetupGame());
	}

    private IEnumerator SetupGame()
    {
        yield return new WaitUntil(() => playersSpawned);
        Cursor.visible = true;
        PreparePlayers();
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        if (fractions[currentFraction] == vikings)
        {
            currentPlayer = fractions[currentFraction][vikingIndex];
        }
        if (fractions[currentFraction] == nerds)
        {
            currentPlayer = fractions[currentFraction][nerdIndex];
        }
        if (fractions[currentFraction] == bandits)
        {
            currentPlayer = fractions[currentFraction][banditIndex];
        }
        currentPlayer.GetComponent<PlayerController>().SetActive();
        SoundManager.PlayAudioClip(switchPlayerSound);
        cam.player = currentPlayer;
        cam.transPlayer = true;
    }

	public IEnumerator HasFired(Projectile projectile){
		currentShots--;
        if (currentShots <= 0){
            currentPlayer.GetComponent<PlayerController>().SetPassive();
            Debug.Log("Start Waiting");
            yield return new WaitUntil(() => projectileDestroyed);
            Debug.Log("Stop Waiting");
            projectileDestroyed = false;
            StartCoroutine(FinishPlayerTurn());
        }
	}

    private IEnumerator FinishPlayerTurn()
    {
        if (currentPlayer.GetComponent<PolygonCollider2D>().enabled)
        {
            cam.player = currentPlayer;
            cam.transPlayer = true;
            yield return new WaitUntil(() => !cam.transPlayer);

            PlayerStats CurrentPlayerStats = currentPlayer.GetComponent<PlayerStats>();
            if (CurrentPlayerStats.turnExperience > 0)
            {
                StartCoroutine(CurrentPlayerStats.AddExperience());
                yield return new WaitUntil(() => CurrentPlayerStats.finishedTurn);
                Debug.Log("Experience: " + CurrentPlayerStats.experience);
            }
        }
        CheckLivingPlayers();
        SwitchPlayer();
    }

    private void CheckLivingPlayers()
    {
        foreach (List<GameObject> fraction in fractions)
        {
            foreach (GameObject player in fraction)
            {
                if (!player.GetComponent<PolygonCollider2D>().enabled)
                {
                    fraction.Remove(player);
                    Debug.Log(player.GetComponent<PlayerStats>().fraction + " aus der SpielerListe gelöscht");
                }
            }
            if (fraction.Count == 0)
            {
                //if (fractions.IndexOf(fraction) <
                fractions.Remove(fraction);
            }
        }
        if (fractions.Count <= 1)
        {
            win.SetActive(true);
            SoundManager.PlayAudioClip(winTheme);
            return;
        }
    }

    public void PreparePlayers()
    {
        RandomizeList(vikings);
        RandomizeList(nerds);
        RandomizeList(bandits);
        //PlayerOrder
        foreach (string fraction in playerTurnOrder)
        {
            if (fraction.Equals("Viking"))
            {
                fractions.Add(vikings);
            }
            if (fraction.Equals("Nerd"))
            {
                fractions.Add(nerds);
            }
            if (fraction.Equals("Bandit"))
            {
                fractions.Add(bandits);
            }
        }

        foreach (List<GameObject> fraction in fractions)
        {
            foreach (GameObject player in fraction)
            {
                player.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }

    public void CurrentPlayerGetsExp(int exp)
    {
        currentPlayer.GetComponent<PlayerStats>().ExpGain(exp);
    }

    private void RandomizeList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
