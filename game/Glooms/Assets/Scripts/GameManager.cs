using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    //Stuff
	private int currentShots = 1;
    public bool projectileDestroyed = false;
    public bool playersSpawned = false;

    //Camera
    private CameraManager cam;

    //Announcer
    private GameObject announcer;

    //Players
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

    //Cards(Buttons)
    public Button level3Card1;
    public Button level3Card2;

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
        //level3Card1 = GameObject.Find("Level3Card1").GetComponent<Button>();
        //level3Card1.onClick.AddListener((UnityEngine.Events.UnityAction)this.AimingModeActive);
        //level3Card2 = GameObject.Find("Level3Card2").GetComponent<Button>();
        announcer = GameObject.Find("Announcer");
        StartCoroutine(SetupGame());
	}

    private IEnumerator SetupGame()
    {
        yield return new WaitUntil(() => playersSpawned);
        Cursor.visible = true;
        announcer.SetActive(false);
        PreparePlayers();
        StartCoroutine(SwitchPlayer());
    }

    private IEnumerator SwitchPlayer()
    {
        if (fractions[currentFraction] == vikings)
        {
            currentPlayer = fractions[currentFraction][vikingIndex];
            vikingIndex++;
            if(vikingIndex > vikings.Count-1)
            {
                vikingIndex = 0;
            }
        }
        if (fractions[currentFraction] == nerds)
        {
            currentPlayer = fractions[currentFraction][nerdIndex];
            nerdIndex++;
            if (nerdIndex > nerds.Count - 1)
            {
                nerdIndex = 0;
            }
        }
        if (fractions[currentFraction] == bandits)
        {
            currentPlayer = fractions[currentFraction][banditIndex];
            banditIndex++;
            if (banditIndex > bandits.Count - 1)
            {
                banditIndex = 0;
            }
        }
        SoundManager.PlayAudioClip(switchPlayerSound);
        cam.fullscreen = false;
        cam.player = currentPlayer;
        cam.transPlayer = true;
        yield return new WaitUntil(() => !cam.transPlayer);
        currentPlayer.GetComponent<PlayerController>().SetActive();
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
            }
        }
        CheckLivingPlayers();
        StartCoroutine(SwitchPlayer());
    }

    private void CheckLivingPlayers()
    {
        for (int i = 0; i < fractions.Count; i++)
        {
            for (int j = 0; j < fractions[i].Count; j++)
            {
                if (!fractions[i][j].GetComponent<PolygonCollider2D>().enabled)
                {
                    if (fractions[i] == vikings)
                    {
                        if (fractions[i].IndexOf(fractions[i][j]) < vikingIndex)
                        {
                            vikingIndex--;
                        }
                    }
                    if (fractions[i] == nerds)
                    {
                        if (fractions[i].IndexOf(fractions[i][j]) < nerdIndex)
                        {
                            nerdIndex--;
                        }
                    }
                    if (fractions[i] == bandits)
                    {
                        if (fractions[i].IndexOf(fractions[i][j]) < banditIndex)
                        {
                            banditIndex--;
                        }
                    }
                    Debug.Log(fractions[i][j].GetComponent<PlayerStats>().fraction + " aus der SpielerListe gelöscht");
                    fractions[i].Remove(fractions[i][j]);
                }
            }
            if (fractions[i].Count == 0)
            {
                Debug.Log("Check");
                if (fractions.IndexOf(fractions[i]) <= currentFraction)
                {
                    currentFraction--;
                }
                fractions.Remove(fractions[i]);
            }
        }

        currentFraction++;
        if (currentFraction > fractions.Count-1)
        {
            currentFraction = 0;
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

    //LevelUpPercs
    public void LevelUp()
    {
        PlayerStats playerStats = currentPlayer.GetComponent<PlayerStats>();
        Debug.Log(playerStats.level);
        if (playerStats.level == 2)
        {
            level3Card1.gameObject.SetActive(true);
            level3Card2.gameObject.SetActive(true);
            Debug.Log(level3Card1.enabled);
        }
        if (playerStats.level == 3)
        {
            level3Card1.enabled = true;
            level3Card2.enabled = true;
        }
    }
}
