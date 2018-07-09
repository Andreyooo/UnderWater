using System.Collections;
using UnityEngine;



public class CameraManager : MonoBehaviour {
//Invoked when a button is pressed.

    public Projectile proj = null;
    private void Start() {
    }

    private void Update(){
        if(proj != null){
            CameraFollow(proj);
        }
        else{
            ResetCamera();
        }
    }

    private void CameraFollow(Projectile projectile){
        gameObject.transform.position = projectile.transform.position + new Vector3(0,0,-5);
    }

    private void ResetCamera(){
        gameObject.transform.position = new Vector3(0,0,-99);
    }
}
