using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePieceScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Destroying")
        {
            Destroy(gameObject);
        }
        Debug.Log("Test");
    }
}
