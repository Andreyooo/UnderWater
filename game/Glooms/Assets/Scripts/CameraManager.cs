using System.Collections;
using UnityEngine;



public class CameraManager : MonoBehaviour
{
    //Invoked when a button is pressed.

    public Projectile proj = null;
    private Vector3 originalCamPos;
    private float originalZoom;
    private void Start()
    {
        originalCamPos = gameObject.transform.position;
        originalZoom = gameObject.GetComponent<Camera>().orthographicSize;

    }

    private void FixedUpdate()
    {
        if (proj != null)
        {
            CameraFollow(proj);
        }
        else
        {
            ResetCamera();
        }
    }

    private void CameraFollow(Projectile projectile)
    {
        gameObject.transform.position = projectile.transform.position + new Vector3(0, 0, -5);
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 10;
    }

    private void ResetCamera()
    {
        gameObject.transform.position = originalCamPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom;
    }
}
