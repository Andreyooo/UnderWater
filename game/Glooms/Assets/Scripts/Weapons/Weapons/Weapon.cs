using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public Projectile projectile;
    public int ammo;

    public void Fired()
    {
        ammo--;
    }
}
