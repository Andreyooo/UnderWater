using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    //Invoked when a button is pressed.

    public Projectile proj = null;
    public Button mapButton;
    public GameObject player = null;
    private Vector3 originalCamPos;
    private Vector3 newPos;

    private float originalZoom;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private bool fullscreen;

    private void Start()
    {
        originalCamPos = gameObject.transform.position;
        originalZoom = gameObject.GetComponent<Camera>().orthographicSize;
        mapButton = GameObject.Find("Map Button").GetComponent<Button>();
        mapButton.onClick.AddListener((UnityEngine.Events.UnityAction)this.FullScreenToggle);
        fullscreen = false;
        minX = -16f;
        maxX = 16f;
        minY = -10f;
        maxY = 10f;
    }

    private void FixedUpdate()
    {
        if (proj != null)
        {
            FollowProjectile(proj);
        }
        else
        {
            if (!fullscreen)
            {
                FollowPlayer(player);
            }
            else
            {
               ResetCamera();
            }
        }
    }

   
    private void FullScreenToggle(){
        fullscreen = !fullscreen;
    }
    private void FollowProjectile(Projectile projectile)
    {
        fullscreen = false;
        player = null;
        newPos = projectile.transform.position + new Vector3(0, 0, -5);
         if(newPos.x > minX && newPos.x <maxX && newPos.y>minY && newPos.y<maxY) {
             gameObject.transform.position = newPos;
         }
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 9;
    }

    private void FollowPlayer(GameObject play)
    {
        player = play;
        newPos = player.transform.position + new Vector3(0, 0, -5);
        if(newPos.x > minX && newPos.x <maxX && newPos.y>minY && newPos.y<maxY) {
             gameObject.transform.position = newPos;
         }
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 9;
    }

    private void ResetCamera()
    {
        gameObject.transform.position = originalCamPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom;
    }
}
