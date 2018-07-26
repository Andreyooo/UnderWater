using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShootingWeapon : MonoBehaviour {
    public PlayerController player;
    public PlayerStats playerStats;
    public GameObject chargingBar;
    public GameObject chargingBarOutline;
    public GameObject crosshair;

    public AudioClip weaponSwitchSound1;
    public AudioClip weaponSwitchSound2;
    public AudioClip chargeSound;

    private CameraManager cam;

    private Weapon weapon;
    private int currentWeapon;
    private List<Weapon> loadOut = new List<Weapon>();

    private float chargeLevel = 0;
    private float chargeSpeed = 0.5f;
    private float chargeLimit = 1;
    private float colorChangingRangeGreen = 150;

    private SpriteRenderer weaponSR;
    public SpriteRenderer jumpSR;
    private SpriteRenderer chargingBarSR;
    private SpriteRenderer chargingBarOutlineSR;
    private SpriteRenderer crosshairSR;

    private bool active = false;
    private bool critActive = false;
    private bool rotationEnabled = true;
    public bool powerJumpMode;
    public bool canShoot = true;
    public bool canJump = true;
    private bool critAnimationPlayed = false;

    void Awake()
    {
        weaponSR = GetComponent<SpriteRenderer>();
        chargingBarSR = chargingBar.GetComponent<SpriteRenderer>();
        chargingBarOutlineSR = chargingBarOutline.GetComponent<SpriteRenderer>();
        crosshairSR = crosshair.GetComponent<SpriteRenderer>();

        SetLoadOut();
    }

    void Update()
    {
        if (active)
        {
            if (powerJumpMode)
            {
                if (rotationEnabled)
                {
                    Rotate();
                }
                PowerJump();
            } else
            {
                //rotate when not in shooting mode
                if (rotationEnabled)
                {
                    Rotate();
                    WeaponSwitching();
                }

                if (Input.GetButtonDown("Fire1") && canShoot && !EventSystem.current.IsPointerOverGameObject())
                {
                    crosshairSR.enabled = false;
                    rotationEnabled = false;
                    chargingBarSR.enabled = true;
                    chargingBarOutlineSR.enabled = true;
                    SoundManager.PlayAudioClip(chargeSound);
                }

                //charging Bulletpower
                if (Input.GetButton("Fire1") && canShoot && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (chargeLevel < chargeLimit)
                    {
                        chargeLevel += Time.deltaTime * chargeSpeed;
                        chargingBar.transform.localScale = new Vector3(chargeLevel, chargeLevel, 1);
                        byte greenValue = (byte)(235 - colorChangingRangeGreen * Mathf.Pow(chargeLevel, 3));
                        chargingBarSR.color = new Color32(235, greenValue, 0, 255);
                    }
                    else
                    {
                        ReleaseProjectile();
                        canShoot = false;
                    }
                }

                if (Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject())
                {
                    SoundManager.StopAudioClip();
                    if (canShoot)
                    {
                        ReleaseProjectile();
                    }
                    else
                    {
                        canShoot = true;
                    }
                }
            }
        }
    }

    //Release Projectile
    private void ReleaseProjectile()
    {
        chargingBarSR.enabled = false;
        chargingBarOutlineSR.enabled = false;
        chargingBarSR.color = new Color32(235, 235, 0, 255);
        chargingBar.transform.localScale = new Vector3(0, 0, 1);
        CheckLoadOut();
        StartCoroutine(Shoot());
        chargeLevel = 0;
    }

    //Powerjump
    public void PowerJump()
    {
        if (Input.GetButtonDown("Fire1") && canJump && !EventSystem.current.IsPointerOverGameObject())
        {
            crosshairSR.enabled = false;
            rotationEnabled = false;
            chargingBarSR.enabled = true;
            chargingBarOutlineSR.enabled = true;
            SoundManager.PlayAudioClip(chargeSound);
            rotationEnabled = false;
        }

        //charging Jumppower
        if (Input.GetButton("Fire1") && canJump && !EventSystem.current.IsPointerOverGameObject())
        {
            if (chargeLevel < chargeLimit)
            {
                chargeLevel += Time.deltaTime * chargeSpeed;
                chargingBar.transform.localScale = new Vector3(chargeLevel, chargeLevel, 1);
                byte greenValue = (byte)(235 - colorChangingRangeGreen * Mathf.Pow(chargeLevel, 3));
                chargingBarSR.color = new Color32(235, greenValue, 0, 255);
            }
            else
            {
                Jumping();
                canJump = false;
            }
        }

        if (Input.GetButtonUp("Fire1") && canJump && !EventSystem.current.IsPointerOverGameObject())
        {
            SoundManager.StopAudioClip();
            if (canJump)
            {
                Jumping();
                canJump = false;
            }
        }
    }

    private void Jumping()
    {
        chargingBarSR.enabled = false;
        chargingBarOutlineSR.enabled = false;
        chargingBarSR.color = new Color32(235, 235, 0, 255);
        chargingBar.transform.localScale = new Vector3(0, 0, 1);
        Rigidbody2D playerRB2D = GameManager.instance.currentPlayer.GetComponent<Rigidbody2D>();
        playerRB2D.isKinematic = false;
        playerRB2D.mass = 1f;
        Transform jumpPoint = transform.Find("FirepointArrow");
        playerStats.PlayJumpSound();
        playerRB2D.AddForce(jumpPoint.forward * 17 * chargeLevel, ForceMode2D.Impulse);
        player.powerjumped = true;
        chargeLevel = 0;
        powerJumpMode = false;
        rotationEnabled = true;
        player.canMove = false;
        active = false;
    }

    //Shoot Bullets
    private IEnumerator Shoot()
    {
        active = false;
        float currentChargelevel = chargeLevel;
        if (!playerStats.spreadShot && !playerStats.doubleShot)
        {
            int tempCritMultiplier = 1;
            if (playerStats.CalculateCrit())
            {
                tempCritMultiplier = playerStats.critMultiplier;
                Debug.Log("Heavy Crit Incoming!!!");
                playerStats.crit.transform.position = transform.Find(weapon.projectile.firepoint).position;
                playerStats.crit.Play();
                SoundManager.PlayAudioClip(playerStats.critSound);
                yield return new WaitForSeconds(1.1f);
                critActive = true;
            }
            Projectile projectile = Instantiate(weapon.projectile);
            //Crit apply
            projectile.damage = projectile.damage * tempCritMultiplier;
            projectile.poisonActive = playerStats.poisonActive;
            projectile.critActive = critActive;

            projectile.dischargeDmg = playerStats.DischargeDamage();
            projectile.fpnt = transform.Find(projectile.firepoint);
            projectile.transform.position = projectile.fpnt.position;
            projectile.transform.rotation = gameObject.transform.rotation;

            projectile.GetComponent<Rigidbody2D>().AddForce(projectile.fpnt.forward * projectile.bulletSpeed * currentChargelevel, ForceMode2D.Impulse);
            weapon.Fired();
            StartCoroutine(GameManager.instance.HasFired(projectile));
            cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
            cam.proj = projectile;
        } else
        {
            if (playerStats.spreadShot)
            {
                SpreadShot();
            }
            if (playerStats.doubleShot)
            {
                StartCoroutine(DoubleShot());
            }
        }
        rotationEnabled = true;
    }

    //Level3 SpreadShotSkill
    private void SpreadShot()
    {
        Projectile projectile1 = Instantiate(weapon.projectile);
        Projectile projectile2 = Instantiate(weapon.projectile);
        Projectile projectile3 = Instantiate(weapon.projectile);

        projectile1.fpnt = transform.Find(projectile1.firepoint);
        projectile2.fpnt = transform.Find(projectile2.firepoint);
        projectile3.fpnt = transform.Find(projectile3.firepoint);

        Physics2D.IgnoreCollision(projectile1.GetComponent<Collider2D>(), projectile2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(projectile1.GetComponent<Collider2D>(), projectile3.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(projectile2.GetComponent<Collider2D>(), projectile3.GetComponent<Collider2D>());

        projectile1.transform.position = projectile1.fpnt.position;
        projectile2.transform.position = projectile2.fpnt.position;
        projectile3.transform.position = projectile3.fpnt.position;

        projectile1.transform.rotation = gameObject.transform.rotation;
        projectile2.transform.rotation = gameObject.transform.rotation;
        projectile3.transform.rotation = gameObject.transform.rotation;

        projectile2.mainProjectile = false;
        projectile3.mainProjectile = false;

        projectile1.damage = Mathf.RoundToInt(projectile1.damage * 0.66f);
        projectile2.damage = Mathf.RoundToInt(projectile2.damage * 0.66f);
        projectile3.damage = Mathf.RoundToInt(projectile3.damage * 0.66f);

        projectile1.GetComponent<Rigidbody2D>().AddForce(projectile1.fpnt.forward * projectile1.bulletSpeed * chargeLevel, ForceMode2D.Impulse);
        projectile2.fpnt.forward = Quaternion.Euler(0, 0, -5) * projectile2.fpnt.forward;
        projectile2.GetComponent<Rigidbody2D>().AddForce(projectile2.fpnt.forward * projectile2.bulletSpeed * chargeLevel, ForceMode2D.Impulse);
        projectile3.fpnt.forward = Quaternion.Euler(0, 0, 10) * projectile3.fpnt.forward;
        projectile3.GetComponent<Rigidbody2D>().AddForce(projectile3.fpnt.forward * projectile3.bulletSpeed * chargeLevel, ForceMode2D.Impulse);

        weapon.Fired();
        StartCoroutine(GameManager.instance.HasFired(projectile1));
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        cam.proj = projectile1;
    }

    private IEnumerator EnableProjectileCollidor(Projectile proj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        proj.GetComponent<Collider2D>().enabled = true;
    }

    //Level3 DoubleShotSkill
    private IEnumerator DoubleShot()
    {
        float currentChargelevel = chargeLevel;
        Projectile projectile1 = Instantiate(weapon.projectile);
        projectile1.fpnt = transform.Find(projectile1.firepoint);
        projectile1.transform.position = projectile1.fpnt.position;
        projectile1.transform.rotation = gameObject.transform.rotation;
        projectile1.mainProjectile = false;
        projectile1.PlayReleaseSound();
        projectile1.GetComponent<Rigidbody2D>().AddForce(projectile1.fpnt.forward * projectile1.bulletSpeed * currentChargelevel, ForceMode2D.Impulse);
        weapon.Fired();

        yield return new WaitForSeconds(0.3f);
        Projectile projectile2 = Instantiate(weapon.projectile);
        Physics2D.IgnoreCollision(projectile1.GetComponent<Collider2D>(), projectile2.GetComponent<Collider2D>());
        projectile2.fpnt = transform.Find(projectile2.firepoint);
        projectile2.transform.position = projectile2.fpnt.position;
        projectile2.transform.rotation = gameObject.transform.rotation;
        projectile2.GetComponent<Rigidbody2D>().AddForce(projectile2.fpnt.forward * projectile2.bulletSpeed * currentChargelevel, ForceMode2D.Impulse);
        StartCoroutine(GameManager.instance.HasFired(projectile2));
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        cam.proj = projectile2;
    }

    //Weapon Rotation
    private void Rotate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if ((rotation_z > 90f && rotation_z <= 180 || rotation_z < -90 && rotation_z > -180) && player.flipped == false)
        {
            player.Flip();
        }
        if (rotation_z <= 90f && rotation_z >= -90 && player.flipped == true)
        {
            player.Flip();
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    //Weapon Switcing
    private void WeaponSwitching()
    {
        if (Input.GetKeyDown("e"))
        {
            SoundManager.PlayAudioClip(weaponSwitchSound1);
            if (!(currentWeapon+1 > loadOut.Count-1))
            {
                SetWeapon(currentWeapon + 1);
            } else
            {
                SetWeapon(0);
            }
        }
        if (Input.GetKeyDown("q"))
        {
            SoundManager.PlayAudioClip(weaponSwitchSound2);
            if (!(currentWeapon-1 < 0))
            {
                SetWeapon(currentWeapon - 1);
            }
            else
            {
                SetWeapon(loadOut.Count-1);
            }
        }
    }

    //Sets player Active for Aiming Mode
    public void SetActive(bool bo)
    {
        //weapon mode
        active = bo;

        //SpriteRendererComponents
        if (!powerJumpMode)
        {
            weaponSR.enabled = bo;
        } else
        {
            weaponSR.enabled = false;
        }
        crosshairSR.enabled = bo;
    }

    //Sets Weapon
    private void SetWeapon(int i)
    {
        weapon = loadOut[i];
        currentWeapon = i;
        weaponSR.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
    }

    //Adds Weapons and sets first added Weapon as Default.
    private void SetLoadOut()
    {
        loadOut.Add(Resources.Load<Weapon>("Prefabs/Weapons/Bow"));
        loadOut.Add(Resources.Load<Weapon>("Prefabs/Weapons/Bazooka"));
        loadOut.Add(Resources.Load<Weapon>("Prefabs/Weapons/Grenade(Weapon)"));
        SetWeapon(0);
    }

    //Checks for Weapons with 0 Ammo to remove them from LoadOut.
    private void CheckLoadOut()
    {
        foreach (Weapon wep in loadOut)
        {
            if (wep.ammo <= 0)
            {
                loadOut.Remove(wep);
                currentWeapon--;
            }
        }
    }
}