/* Written by Dennis Lavrinov */
/* PlayerHealth.cs */
using UnityEngine;
using System.Collections;


public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    float currentHealth = 0;

    public string fraction;
    public string classPath = "";

    //StrikerPath
    public float damageMultiplier = 1;
    public float damageTakenMultiplier = 1;

    //GuardianPath
    public int shieldRegen = 0;
    public int maxShield = 0;
    public int currentShield = 0;
    public int healthRegen = 0;
    public bool discharge = false;

    //HunterPath
    public float critChance = 0;
    public int critMultiplier = 1;
    public float lifesteal = 0;
    public int lifeStealedThisTurn = 0;
    public int poisoned = 0;
    public int poisonedTurns = 0;
    public bool poisonActive = false;

    public bool finishedTurn;
    public bool playerHealed = false;
    public bool poisonDamageTaken = false;

    //ExpStuff
    public int maxExp = 4;
    public int experience = 0;
    public int turnExperience = 0;
    public int level = 1;
    private bool maxLevel = false;
    public GameObject expGain;
    public GameObject levelUp;
    public GameObject level2AuraRed;
    public GameObject level2AuraBlue;
    public GameObject level2AuraYellow;
    public GameObject level3Aura;
    public AudioClip levelUpSound;
    private ParticleSystem expGainPS;
    private ParticleSystem levelUpPS;
    private ParticleSystem level2AuraRedPS;
    private ParticleSystem level2AuraBluePS;
    private ParticleSystem level2AuraYellowPS;
    private ParticleSystem level3AuraPS;

    public ParticleSystem crit;
    public AudioClip critSound;
    public ParticleSystem poisonPS;
    public AudioClip bubbling;
    public ParticleSystem heal;
    public AudioClip healing;
    public ParticleSystem dischargePS;
    public AudioClip dischargeSound;

    public bool spreadShot = false;
    public bool doubleShot = false;

    //Sounds
    public string playerHitSound;
    public string playerJumpSound;
    public string playerDeathSound;
    public string playerJoySound;
    public string playerMissSound;
    public string playerStartSound;
    public string playerLevelUpSound;
    public AudioClip expGainSound;

    //UI
    public SimpleHealthBar healthBar;
    public SimpleHealthBar shieldBar;
    public SimpleHealthBar expBar;
    public GameObject poisonedImg;

    //etc
    public GameObject blood, deadHead, deadBody, deadLeftLeg, deadLeftHand, deadRightLeg, deadRightHand;

    private void Awake()
    {
        SoundManager.PlaySound(playerJumpSound);
        expGainPS = expGain.GetComponent<ParticleSystem>();
        levelUpPS = levelUp.GetComponent<ParticleSystem>();
        level2AuraRedPS = level2AuraRed.GetComponent<ParticleSystem>();
        level2AuraBluePS = level2AuraBlue.GetComponent<ParticleSystem>();
        level2AuraYellowPS = level2AuraYellow.GetComponent<ParticleSystem>();
        level3AuraPS = level3Aura.GetComponent<ParticleSystem>();
    }

    void Start()
    {
        // Set the current health to max values.
        currentHealth = maxHealth;
        healthBar.UpdateBar(currentHealth, maxHealth);
        shieldBar.UpdateBar(currentShield, 15);
        expBar.UpdateBar(experience, maxExp);
    }

    //-------------------------Health/Shield-Functions------------------------

    public void TakeDamage(int damage)
    {
        damage = Mathf.RoundToInt(damage * damageTakenMultiplier);
        Debug.Log("Damage Taken: " + damage);
        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield > 0)
            {
                damage = 0;
            }
            else
            {
                damage = -currentShield;
                currentShield = 0;
            }
            shieldBar.UpdateBar(currentShield, 15);
        }
        currentHealth -= damage;
        if (currentHealth > 0) SoundManager.PlaySound(playerHitSound);
        if (currentHealth <= 0)
        {
            // Set the current health to zero.
            currentHealth = 0;

            // Run the Death function since the player has died.
            Death();
        }
        healthBar.UpdateBar(currentHealth, maxHealth);
    }

    public void Death()
    {
        SoundManager.PlaySound(playerDeathSound);

        //Animation
        Instantiate(blood, transform.position, Quaternion.identity);
        Instantiate(deadBody, transform.position, Quaternion.identity);
        Instantiate(deadHead, transform.position, Quaternion.identity);
        Instantiate(deadLeftLeg, transform.position, Quaternion.identity);
        Instantiate(deadRightLeg, transform.position, Quaternion.identity);
        Instantiate(deadLeftHand, transform.position, Quaternion.identity);
        Instantiate(deadRightHand, transform.position, Quaternion.identity);

        //Deactivate Player
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        transform.Find("Canvas").gameObject.SetActive(false);
        level2AuraRed.SetActive(false);
        level2AuraBlue.SetActive(false);
        level2AuraYellow.SetActive(false);
        level3Aura.SetActive(false);
        expBar.gameObject.SetActive(false);
        poisonPS.gameObject.SetActive(false);
        dischargePS.gameObject.SetActive(false);

        Invoke("DeactivatePlayer", 30);
    }

    //---------------------------RPG-Functions-------------------------
    public void ExpGain(int exp)
    {
        finishedTurn = false;
        turnExperience += exp;
    }

    public IEnumerator AddExperience()
    {
        if (!maxLevel)
        {
            Vector3 animationPosition = gameObject.transform.position;
            animationPosition.y += 0.8f;
            while (turnExperience > 0 && !maxLevel)
            {
                SoundManager.PlayAudioClip(expGainSound);
                experience++;
                turnExperience--;
                expGainPS.Play();
                expBar.UpdateBar(experience, maxExp);
                yield return new WaitUntil(() => !expGainPS.IsAlive());

                //LevelUP
                if (experience == maxExp)
                {
                    yield return new WaitForSeconds(0.3f);
                    level++;
                    experience = 0;
                    maxExp++;
                    levelUpPS.Play();
                    SoundManager.PlayAudioClip(levelUpSound);
                    yield return new WaitForSeconds(0.3f);
                    SoundManager.PlaySound(playerLevelUpSound);
                    yield return new WaitUntil(() => !levelUpPS.IsAlive());
                    if (level == 2)
                    {
                        maxHealth += 40;
                        currentHealth += 40;
                        healthBar.UpdateBar(currentHealth, maxHealth);
                        StartCoroutine(GameManager.instance.LevelUp());
                        yield return new WaitUntil(() => GameManager.instance.percChosen);
                        GameManager.instance.percChosen = false;
                        if (classPath == "Striker")
                        {
                            Debug.Log("Red");
                            level2AuraRedPS.Play();
                            level2AuraRed.GetComponent<AudioSource>().Play();
                        }
                        if (classPath == "Guardian")
                        {
                            Debug.Log("Blue");
                            level2AuraBluePS.Play();
                            level2AuraBlue.GetComponent<AudioSource>().Play();
                        }
                        if (classPath == "Hunter")
                        {
                            Debug.Log("Yellow");
                            level2AuraYellowPS.Play();
                            level2AuraYellow.GetComponent<AudioSource>().Play();
                        }
                    }

                    if (level == 3)
                    {
                        maxHealth += 10;
                        currentHealth += 10;
                        healthBar.UpdateBar(currentHealth, maxHealth);
                        StartCoroutine(GameManager.instance.LevelUp());
                        maxLevel = true;
                        yield return new WaitUntil(() => GameManager.instance.percChosen);
                        yield return new WaitForSeconds(1);
                        SoundManager.PlayAudioClip(GameManager.instance.superSayin);
                        level3AuraPS.Play();
                        yield return new WaitForSeconds(1f);
                        GameManager.instance.percChosen = false;
                    }
                    expBar.UpdateBar(experience, maxExp);
                    yield return new WaitForSeconds(2);
                }
            }
        }
        //Let´s the camera switch to the next Player
        Invoke("EndTurn", 1);
    }

    private void EndTurn()
    {
        finishedTurn = true;
    }

    //-----------------------------Tools----------------------------
    private void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public void PlayJumpSound()
    {
        SoundManager.PlaySound(playerJumpSound);
    }

    public IEnumerator Utilities()
    {
        StartCoroutine(HealPlayer(healthRegen));
        RegenShield();
        yield return new WaitUntil(() => playerHealed);
        playerHealed = false;
        StartCoroutine(TakePoisonDamge());
    }

    public IEnumerator HealPlayer(int amount)
    {
        if (currentHealth < maxHealth && amount > 0)
        {
            // Increase the current health by healthRegen-Value
            currentHealth += amount;

            // If the current health is greater than max, then set it to max.
            if (currentHealth > maxHealth) currentHealth = maxHealth;

            heal.Play();
            SoundManager.PlayAudioClip(healing);
            // Update the Simple Health Bar with the new Health values.
            healthBar.UpdateBar(currentHealth, maxHealth);
        }
        if (heal.IsAlive())
        {
            yield return new WaitUntil(() => !heal.IsAlive());
            yield return new WaitForSeconds(0.2f);
        }
        playerHealed = true;
    }

    public void RegenShield()
    {
        Debug.Log(currentShield + "    " + maxShield);
        // Increase the current shield by shieldRegen-Value
        currentShield += shieldRegen;

        // If the current shield is greater than max, then set it to max.
        if (currentShield > maxShield)
            currentShield = maxShield;
        // Update the Simple Health Bar with the new Shield values.
        shieldBar.UpdateBar(currentShield, 15);
    }

    public IEnumerator TakePoisonDamge()
    {
        if (poisonedTurns > 0 && poisoned > 0)
        {
            poisonPS.Play();
            SoundManager.PlayAudioClip(bubbling);
            SoundManager.PlaySound(playerHitSound);
            currentHealth -= poisoned;
            healthBar.UpdateBar(currentHealth, maxHealth);
            poisonedTurns--;
        }
        if (poisonedTurns == 0 || poisoned == 0)
        {
            poisonedImg.SetActive(false);
            poisoned = 0;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 1;
            healthBar.UpdateBar(currentHealth, maxHealth);
            poisonedImg.SetActive(false);
            poisoned = 0;
            poisonedTurns = 0;
        } else
        {
            if (poisonPS.IsAlive())
            {
                yield return new WaitUntil(() => !poisonPS.IsAlive());
                yield return new WaitForSeconds(0.2f);
            }
        }
        poisonDamageTaken = true;
    }

    public void Poisoned(int poisonDmg, int poisonDmgTurns)
    {
        if (poisonDmg > 0)
        {
            poisonPS.Play();
            SoundManager.PlayAudioClip(bubbling);
            if (poisonDmg > poisoned) poisoned = poisonDmg;
            poisonedTurns = poisonDmgTurns;
            poisonedImg.SetActive(true);
        }
    }

    public int DischargeDamage()
    {
        if (discharge)
        {
            return currentShield;
        } else
        {
            return 0;
        }
    }

    public void Discharged()
    {
        dischargePS.Play();
        SoundManager.PlayAudioClip(dischargeSound);
    }

    public void Lifesteal(){
        StartCoroutine(HealPlayer(lifeStealedThisTurn));
        lifeStealedThisTurn = 0;
    }

    public bool CalculateCrit(){
        float randValue = Random.value;
        return randValue <= critChance;
    }

    
}
