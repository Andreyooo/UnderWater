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

    private float smooth;

    private float originalZoom;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private bool fullscreen;

    public bool transPlayer;

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
        smooth = 0.125f;
        transPlayer = false;
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
                if(transPlayer){
                    transitionPlayer(player);
                }
                else{
                    FollowPlayer(player);
                }
            }
            else
            {
                ResetCamera();
            }
        }
    }


    private void FullScreenToggle()
    {
        fullscreen = !fullscreen;
    }

    private void FollowProjectile(Projectile projectile)
    {
        fullscreen = false;
        player = null;

        Vector3 finalPos = gameObject.transform.position;
        newPos = projectile.transform.position;

        if (newPos.x > minX && newPos.x < maxX)
        {
            finalPos.x = newPos.x;
        }
        if (newPos.y > minY && newPos.y < maxY)
        {
            finalPos.y = newPos.y;
        }
        finalPos.z = -5f;

        gameObject.transform.position = finalPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 9;
    }

    private void FollowPlayer(GameObject play)
    {
        player = play;
        Vector3 finalPos = gameObject.transform.position;
        newPos = player.transform.position;

        if (newPos.x > minX && newPos.x < maxX)
        {
            finalPos.x = newPos.x;
        }
        if (newPos.y > minY && newPos.y < maxY)
        {
            finalPos.y = newPos.y;
        }
        finalPos.z = -5f;

        gameObject.transform.position = finalPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 9;
    }

    private void transitionPlayer(GameObject play){
        player = play;
        Vector3 finalPos = gameObject.transform.position;
        newPos = player.transform.position;

        if (newPos.x > minX && newPos.x < maxX)
        {
            finalPos.x = newPos.x;
        }
        if (newPos.y > minY && newPos.y < maxY)
        {
            finalPos.y = newPos.y;
        }
        finalPos.z = -5f;

        Vector3 smoothedPosition = Vector3.Lerp(gameObject.transform.position, finalPos, smooth);
        gameObject.transform.position = smoothedPosition;
        if(gameObject.transform.position == finalPos) transPlayer = false;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - 9;

    }
    private void ResetCamera()
    {
        gameObject.transform.position = originalCamPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom;
    }
}
