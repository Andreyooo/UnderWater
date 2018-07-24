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
    public Text announcer;

    //Players
    public List<GameObject> vikings;
    private int vikingIndex = 0;
    public List<GameObject> nerds;
    private int nerdIndex = 0;
    public List<GameObject> bandits;
    private int banditIndex = 0;
    private List<List<GameObject>> fractions = new List<List<GameObject>>();
    public List<string> playerTurnOrder;

    private GameObject currentPlayer;
    private int currentFraction = 0;

    //Cards(Buttons)
    public Button redPath;
    public Button bluePath;
    public Button yellowPath;
    public Button firstCard;
    public Button secondCard;

    public Sprite redCard1;
    public Sprite redCard2;
    public Sprite blueCard1;
    public Sprite blueCard2;
    public Sprite yellowCard1;
    public Sprite yellowCard2;

    private Image redPathImage;
    private Image bluePathImage;
    private Image yellowPathImage;
    private Image firstCardImage;
    private Image secondCardImage;

    public bool percChosen = false;

    //Win
    public GameObject win;
    public AudioClip winTheme;

    //FX
    public AudioClip switchPlayerSound;
    public AudioClip lockInSound;
    public AudioClip buttonClickSound;

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
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        announcer = GameObject.Find("Announcer").GetComponent<Text>();

        //Cards
        redPathImage = redPath.GetComponent<Image>();
        bluePathImage = bluePath.GetComponent<Image>();
        yellowPathImage = yellowPath.GetComponent<Image>();
        firstCardImage = firstCard.GetComponent<Image>();
        secondCardImage = secondCard.GetComponent<Image>();

        redPath.onClick.AddListener((UnityEngine.Events.UnityAction)this.Red);
        bluePath.onClick.AddListener((UnityEngine.Events.UnityAction)this.Blue);
        yellowPath.onClick.AddListener((UnityEngine.Events.UnityAction)this.Yellow);
        firstCard.onClick.AddListener((UnityEngine.Events.UnityAction)this.First);
        secondCard.onClick.AddListener((UnityEngine.Events.UnityAction)this.Second);
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(SetupGame());
	}

    private IEnumerator SetupGame()
    {
        yield return new WaitUntil(() => playersSpawned);
        Cursor.visible = true;
        announcer.gameObject.SetActive(false);
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
        StartCoroutine(currentPlayer.GetComponent<PlayerController>().SetActive());
        percChosen = false;
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
    public IEnumerator LevelUp()
    {
        PlayerStats playerStats = currentPlayer.GetComponent<PlayerStats>();
        redPath.gameObject.SetActive(true);
        bluePath.gameObject.SetActive(true);
        yellowPath.gameObject.SetActive(true);

        redPathImage.canvasRenderer.SetAlpha(0f);
        bluePathImage.canvasRenderer.SetAlpha(0f);
        yellowPathImage.canvasRenderer.SetAlpha(0f);

        redPathImage.CrossFadeAlpha(1f, 0.2f, false);
        bluePathImage.CrossFadeAlpha(1f, 0.2f, false);
        yellowPathImage.CrossFadeAlpha(1f, 0.2f, false);

        yield return new WaitForSeconds(0.2f);
        redPath.enabled = true;
        bluePath.enabled = true;
        yellowPath.enabled = true;
    }

    private IEnumerator RedPath()
    {
        SoundManager.PlayAudioClip(buttonClickSound);
        StartCoroutine(DisablePaths());
        yield return new WaitForSeconds(0.11f);

        firstCard.gameObject.SetActive(true);
        secondCard.gameObject.SetActive(true);

        firstCardImage.canvasRenderer.SetAlpha(0f);
        secondCardImage.canvasRenderer.SetAlpha(0f);

        firstCardImage.CrossFadeAlpha(1f, 0.2f, false);
        secondCardImage.CrossFadeAlpha(1f, 0.2f, false);

        yield return new WaitForSeconds(0.2f);
        firstCard.enabled = true;
        secondCard.enabled = true;
    }

    private IEnumerator BluePath()
    {
        SoundManager.PlayAudioClip(buttonClickSound);
        StartCoroutine(DisablePaths());
        yield return new WaitForSeconds(0.11f);

        firstCard.gameObject.SetActive(true);
        secondCard.gameObject.SetActive(true);

        firstCardImage.canvasRenderer.SetAlpha(0f);
        secondCardImage.canvasRenderer.SetAlpha(0f);

        firstCardImage.CrossFadeAlpha(1f, 0.2f, false);
        secondCardImage.CrossFadeAlpha(1f, 0.2f, false);

        yield return new WaitForSeconds(0.2f);
        firstCard.enabled = true;
        secondCard.enabled = true;
    }

    private IEnumerator YellowPath()
    {
        SoundManager.PlayAudioClip(buttonClickSound);
        StartCoroutine(DisablePaths());
        yield return new WaitForSeconds(0.11f);

        firstCard.gameObject.SetActive(true);
        secondCard.gameObject.SetActive(true);

        firstCardImage.canvasRenderer.SetAlpha(0f);
        secondCardImage.canvasRenderer.SetAlpha(0f);

        firstCardImage.CrossFadeAlpha(1f, 0.2f, false);
        secondCardImage.CrossFadeAlpha(1f, 0.2f, false);

        yield return new WaitForSeconds(0.2f);
        firstCard.enabled = true;
        secondCard.enabled = true;
    }

    private IEnumerator FirstCard()
    {
        SoundManager.PlayAudioClip(lockInSound);
        StartCoroutine(DisableCards());
        yield return new WaitForSeconds(0.11f);
        currentPlayer.GetComponent<PlayerStats>().spreadShot = true;
    }

    private IEnumerator SecondCard()
    {
        SoundManager.PlayAudioClip(lockInSound);
        StartCoroutine(DisableCards());
        yield return new WaitForSeconds(0.11f);
        currentPlayer.GetComponent<PlayerStats>().doubleShot = true;
    }

    private IEnumerator DisablePaths()
    {
        redPath.enabled = false;
        bluePath.enabled = false;
        yellowPath.enabled = false;

        redPathImage.CrossFadeAlpha(0f, 0.10f, false);
        bluePathImage.CrossFadeAlpha(0f, 0.10f, false);
        yellowPathImage.CrossFadeAlpha(0f, 0.10f, false);
        yield return new WaitForSeconds(0.10f);
        redPath.gameObject.SetActive(false);
        bluePath.gameObject.SetActive(false);
        yellowPath.gameObject.SetActive(false);
    }

    private IEnumerator DisableCards()
    {
        firstCard.enabled = false;
        secondCard.enabled = false;

        firstCardImage.CrossFadeAlpha(0f, 0.10f, false);
        secondCardImage.CrossFadeAlpha(0f, 0.10f, false);
        yield return new WaitForSeconds(0.10f);
        firstCard.gameObject.SetActive(false);
        secondCard.gameObject.SetActive(false);
        percChosen = true;
    }

    //Wrappers
    private void Red()
    {
        StartCoroutine(RedPath());
    }

    private void Blue()
    {
        StartCoroutine(BluePath());
    }

    private void Yellow()
    {
        StartCoroutine(YellowPath());
    }

    private void First()
    {
        StartCoroutine(FirstCard());
    }

    private void Second()
    {
        StartCoroutine(SecondCard());
    }
}
