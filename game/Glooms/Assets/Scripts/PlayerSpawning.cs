using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawning : MonoBehaviour {
    public SoundManager soundmanager;

    public GameObject vikingPrefab;
    public GameObject nerdPrefab;
    public GameObject banditPrefab;
    private Text announcer;
    private GameObject newPlayer;
    private List<string> playerSpawnOrder = new List<string>{ "Viking", "Nerd", "Bandit"};

    public AudioClip part1;
    public AudioClip part2;
    public AudioClip part3; 

    private List<GameObject> players;
    private bool started = false;
    private bool executed = false;
    private CameraManager cam;

    public bool playerSpawning = false;
    public bool playerInAir = false;

    private Vector3 mousePos;

    // Use this for initialization
    void Start () {
        announcer = GameObject.Find("Announcer").GetComponent<Text>();
        Cursor.visible = false;
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        soundmanager.PlayMusic(part1);
        RandomiseSpawnOrder();
        AnnounceCurrentPlayer();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && !playerInAir && !executed)
        {
            playerInAir = true;
            StartCoroutine(ActionMusic());
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            mousePos.y = 10;
            if (playerSpawnOrder[0].Equals("Viking")) {
                newPlayer = Instantiate(vikingPrefab);
                newPlayer.GetComponent<PlayerStats>().fraction = "Viking";
                GameManager.instance.vikings.Add(newPlayer);
            }
            if (playerSpawnOrder[0].Equals("Nerd")) {
                newPlayer = Instantiate(nerdPrefab);
                newPlayer.GetComponent<PlayerStats>().fraction = "Nerd";
                GameManager.instance.nerds.Add(newPlayer);
            }
            if (playerSpawnOrder[0].Equals("Bandit")) {
                newPlayer = Instantiate(banditPrefab);
                newPlayer.GetComponent<PlayerStats>().fraction = "Bandit";
                GameManager.instance.bandits.Add(newPlayer);
            }
            newPlayer.transform.position = mousePos;
            cam.player = newPlayer;
            cam.fullscreen = false;
            cam.transPlayer = true;
            playerSpawnOrder.RemoveAt(0);

            //für schnellere Playtests
            newPlayer.GetComponent<PlayerController>().gravityModifier = 10;
        }
        if (playerSpawnOrder.Count == 0 && !executed)
        {
            executed = true;
            StartCoroutine(EndSpawnPhase(newPlayer));
        }
	}

    private IEnumerator EndSpawnPhase(GameObject player)
    {
        yield return new WaitUntil(() => player.GetComponent<PlayerController>().grounded);
        GameManager.instance.playersSpawned = true;
        GameObject.Find("SpawnSpot").SetActive(false);
        this.enabled = false;
    }

    public void AnnounceCurrentPlayer()
    {
        if (playerSpawnOrder.Count != 0)
        {
            announcer.text = playerSpawnOrder[0] + " is Spawning";
        }
    }

    private void RandomiseSpawnOrder()
    {
        for (int i = 0; i < playerSpawnOrder.Count; i++)
        {
            string temp = playerSpawnOrder[i];
            int randomIndex = Random.Range(i, playerSpawnOrder.Count);
            playerSpawnOrder[i] = playerSpawnOrder[randomIndex];
            playerSpawnOrder[randomIndex] = temp;
        }
        GameManager.instance.playerTurnOrder.Add(playerSpawnOrder[2]);
        GameManager.instance.playerTurnOrder.Add(playerSpawnOrder[1]);
        GameManager.instance.playerTurnOrder.Add(playerSpawnOrder[0]);
        playerSpawnOrder.Add(playerSpawnOrder[0]);
        playerSpawnOrder.Add(playerSpawnOrder[1]);
        playerSpawnOrder.Add(playerSpawnOrder[2]);
        playerSpawnOrder.Add(playerSpawnOrder[0]);
        playerSpawnOrder.Add(playerSpawnOrder[1]);
        playerSpawnOrder.Add(playerSpawnOrder[2]);
        for (int i = 0; i < GameManager.instance.playerTurnOrder.Count; i++)
        {
            Debug.Log(GameManager.instance.playerTurnOrder[i]);
        }
    }

    private IEnumerator ActionMusic()
    {
        if (!started)
        {
            started = true;
            soundmanager.PlayMusic(part2);
            yield return new WaitForSeconds(soundmanager.musicSource.clip.length + 0.3f);
            soundmanager.PlayMusic(part3);
        }
    }
}
