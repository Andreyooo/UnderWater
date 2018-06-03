using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour {

    public int maxHealth = 25;
    public int health = 25;
    public SimpleHealthBar healthBar;

    void TakeDamage(int damage)
    {
        health -= damage;

        // Now is where you will want to update the Simple Health Bar. Only AFTER the value has been modified.
        healthBar.UpdateBar(health, maxHealth);
    }

}
