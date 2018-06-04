using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public int dmg = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        //Debug.Log(collision.gameObject.name);
        Destroy(transform.parent.gameObject);
        if (collision.gameObject.name == "Player2")
        {
          Destroy(transform.parent.gameObject);
        }
    }
}
