using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : MonoBehaviour {
    public Player player;
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;
    public float lifeTime = 3;

    private SpriteRenderer weaponSR;
    private bool directionRight = true;

    public Transform firepoint;

    private void Awake()
    {
        if(firepoint == null)
        {
            Debug.LogError("Kein Firepoint zugewiesen.");
        }
    }

    void Start()
    {
        weaponSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        Rotate();
    }

    //Shoot Bullets
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firepoint.transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.forward * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, lifeTime));
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
