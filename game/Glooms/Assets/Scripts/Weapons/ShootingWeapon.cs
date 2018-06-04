using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : MonoBehaviour {
    public PlayerController player;
    public GameObject bulletPrefab;
    public GameObject chargingBar;
    public GameObject chargingBarOutline;
    public Transform firepoint;
    public TurnManager turnmanager;

    private float bulletSpeed = 30;
    private float lifeTime = 10;
    private float chargeLevel = 0;
    private float chargeSpeed = 0.7f;
    private float chargeLimit = 1;
    private float colorChangingRangeGreen = 200;

    private SpriteRenderer weaponSR;
    private SpriteRenderer chargingBarSR;
    private SpriteRenderer chargingBarOutlineSR;
    private bool directionRight = true;
    private bool rotationEnabled = true;
    private bool canShoot = true;


    void Start()
    {
        weaponSR = GetComponent<SpriteRenderer>();
        chargingBarSR = chargingBar.GetComponent<SpriteRenderer>();
        chargingBarOutlineSR = chargingBarOutline.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //rotate when not in shooting mode
        if(rotationEnabled)
        {
            Rotate();
        }

        //charging Bulletpower
        if (Input.GetButton("Fire1") && canShoot)
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
            } else
            {
                ReleaseProjectile();
                canShoot = false;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if(canShoot)
            {
                ReleaseProjectile();
            } else
            {
                canShoot = true;
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
    }

    //Shoot Bullets
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firepoint.transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.forward * bulletSpeed * chargeLevel, ForceMode2D.Impulse);
        Debug.Log(firepoint.forward * bulletSpeed * chargeLevel);
        StartCoroutine(DestroyBulletAfterTime(bullet, lifeTime));
        //turnmanager.HasFired();
    }

    //Destroys bullet after time
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
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

    //flipping Weapon
    public void FlipY(bool bo)
    {
        weaponSR.flipY = bo;
    }
}
