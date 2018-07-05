using System.Collections;
using UnityEngine;



public class CameraManager : MonoBehaviour {
//Invoked when a button is pressed.

	public GameObject cam;
    public void SetParent(GameObject newParent)
    {
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        cam.transform.parent = newParent.transform;

        //Display the parent's name in the console.
        Debug.Log("Cam's Parent: " + cam.transform.parent.name);

    }

    public void DetachFromParent()
    {
        // Detaches the transform from its parent.
        transform.parent = null;
    }
}
