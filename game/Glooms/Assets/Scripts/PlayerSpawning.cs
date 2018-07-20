using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour {
    public SoundManager soundmanager;

    public GameObject vikingPrefab;
    public GameObject nerdPrefab;
    public GameObject banditPrefab;

    public AudioClip part1;
    public AudioClip part2;
    public AudioClip part3; 

    private List<GameObject> players;
    private bool started = false;

    public bool playerSpawning = false;

    private Vector3 mousePos;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        soundmanager.PlayMusic(part1);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ActionMusic());
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            mousePos.y = 10;
            GameObject newPlayer = Instantiate(vikingPrefab);
            newPlayer.transform.position = mousePos;
            GameManager.instance.players.Add(newPlayer);
        }
        if (GameManager.instance.players.Count == 9)
        {
            GameManager.instance.playersSpawned = true;
            this.enabled = false;
        }
	}

    private IEnumerator ActionMusic()
    {
        if (!started)
        {
            started = true;
            soundmanager.PlayMusic(part2);
            yield return new WaitForSeconds(soundmanager.musicSource.clip.length);
            soundmanager.PlayMusic(part3);
        }
    }
}
