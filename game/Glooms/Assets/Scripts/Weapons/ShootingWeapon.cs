using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShootingWeapon : MonoBehaviour {
    public PlayerController player;
    public GameObject chargingBar;
    public GameObject chargingBarOutline;
    public AudioClip weaponSwitchSound1;
    public AudioClip weaponSwitchSound2;

    private Weapon weapon;
    private int currentWeapon;
    private List<Weapon> loadOut = new List<Weapon>();

    private float bulletSpeed = 22;
    private float lifeTime = 10;
    private float chargeLevel = 0;
    private float chargeSpeed = 0.7f;
    private float chargeLimit = 1;
    private float colorChangingRangeGreen = 200;

    private SpriteRenderer weaponSR;
    private SpriteRenderer chargingBarSR;
    private SpriteRenderer chargingBarOutlineSR;
    private bool active = false;
    private bool directionRight = true;
    private bool rotationEnabled = true;
    private bool canShoot = true;

    void Awake()
    {
        weaponSR = GetComponent<SpriteRenderer>();
        SetLoadOut();
        chargingBarSR = chargingBar.GetComponent<SpriteRenderer>();
        chargingBarOutlineSR = chargingBarOutline.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (active)
        {
            //rotate when not in shooting mode
            if (rotationEnabled)
            {
                Rotate();
                WeaponSwitching();
            }

            //charging Bulletpower
            if (Input.GetButton("Fire1") && canShoot && !EventSystem.current.IsPointerOverGameObject())
            {
                if (chargeLevel < chargeLimit)
                {
                    rotationEnabled = false;
                    chargingBarSR.enabled = true;
                    chargingBarOutlineSR.enabled = true;

                    chargeLevel += Time.deltaTime * chargeSpeed;
                    chargingBar.transform.localScale = new Vector3(chargeLevel, chargeLevel, 1);
                    byte greenValue = (byte)(235 - colorChangingRangeGreen * Mathf.Pow(chargeLevel, 7));
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

    //Release Projectile
    private void ReleaseProjectile()
    {
        chargingBarSR.enabled = false;
        chargingBarOutlineSR.enabled = false;
        chargingBarSR.color = new Color32(235, 235, 0, 255);
        chargingBar.transform.localScale = new Vector3(0, 0, 1);
        CheckLoadOut();
        Shoot();
        chargeLevel = 0;
        rotationEnabled = true;
    }

    //Shoot Bullets
    private void Shoot()
    {
        Projectile projectile = Instantiate(weapon.projectile);
        projectile.fpnt = transform.Find(projectile.firepoint);
        projectile.transform.position = projectile.fpnt.position;
        projectile.transform.rotation = gameObject.transform.rotation;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.fpnt.forward * projectile.bulletSpeed * chargeLevel, ForceMode2D.Impulse);
        weapon.Fired();
        StartCoroutine(GameManager.instance.HasFired(projectile));
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
        active = bo;
        if (bo == true)
        {
            //CheckLoadOut();
            weaponSR.enabled = true;
        } else
        {
            weaponSR.enabled = false;
        }
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

    //flipping Weapon VERALTET************************
    /*public void FlipWeapon()
    {
        player.Flip();
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        newScale.y *= -1;
        transform.localScale = newScale;
    }*/
}