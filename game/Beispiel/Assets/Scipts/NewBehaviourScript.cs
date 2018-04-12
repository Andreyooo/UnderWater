using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public float paddleSpeed = 1f;
<<<<<<< HEAD
    public string dstring = "Merge this!";
=======


>>>>>>> 04d2175178ec9a8d69b69b14a8a959e3c6888c48

    private Vector3 playerPos = new Vector3 (0, -9.5f, 0);

    void Update () 
    {
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        playerPos = new Vector3 (Mathf.Clamp (xPos, -8f, 8f), -9.5f, 0f);
        transform.position = playerPos;

    }
}