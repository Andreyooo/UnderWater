using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Player player;
    private SpriteRenderer weaponSR;
    private bool directionRight = true;

    private void Start()
    {
        weaponSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Rotate();
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
