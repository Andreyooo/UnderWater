using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShootingWeapon : MonoBehaviour {
    public PlayerController player;
    public GameObject bulletPrefab;
    public GameObject chargingBar;
    public GameObject chargingBarOutline;
    public Transform firepoint;

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
            }

            //charging Bulletpower
            if (Input.GetButton("Fire1") && canShoot && !EventSystem.current.IsPointerOverGameObject())
            {
                if (chargeLevel < chargeLimit)
                {
                    chargingBarSR.enabled = true;
                    chargingBarOutlineSR.enabled = true;

                    chargeLevel += Time.deltaTime * chargeSpeed;
                    chargingBar.transform.localScale = new Vector3(chargeLevel, chargeLevel, 1);
                    byte greenValue = (byte)(235 - colorChangingRangeGreen * Mathf.Pow(chargeLevel, 7));
                    chargingBarSR.color = new Color32(235, greenValue, 0, 255);
                    rotationEnabled = false;
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
        Shoot();
        chargeLevel = 0;
        rotationEnabled = true;
        SoundManager.PlaySound("arrowShot");

    }

    //Shoot Bullets
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firepoint.transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.forward * bulletSpeed * chargeLevel, ForceMode2D.Impulse);
        bullet.GetComponent<BulletScript>().DestroyProjectileAfterTime(lifeTime);
        //GameManager.instance.HasFired(bullet);
        StartCoroutine(GameManager.instance.HasFired(bullet));
    }

    //Weapon Rotation
    private void Rotate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if ((rotation_z > 90f && rotation_z <= 180 || rotation_z < -90 && rotation_z > -180) && directionRight == true)
        {
            directionRight = false;
            player.FlipX(true);
            this.FlipY(true);
        }
        if (rotation_z <= 90f && rotation_z >= -90 && directionRight == false)
        {
            directionRight = true;
            player.FlipX(false);
            this.FlipY(false);
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    public void SetActive(bool bo)
    {
        active = bo;
        if (bo == true)
        {
            weaponSR.enabled = true;
        } else
        {
            weaponSR.enabled = false;
        }
    }

    //flipping Weapon
    public void FlipY(bool bo)
    {
        weaponSR.flipY = bo;
    }
}