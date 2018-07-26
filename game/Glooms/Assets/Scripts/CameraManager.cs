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
    private int zoomCorrector = 13;
    private float smooth;

    private float originalZoom;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public bool fullscreen;
    public bool transPlayer;

    private void Start()
    {
        originalCamPos = gameObject.transform.position;
        originalZoom = gameObject.GetComponent<Camera>().orthographicSize;
        mapButton = GameObject.Find("Map Button").GetComponent<Button>();
        mapButton.onClick.AddListener((UnityEngine.Events.UnityAction)this.FullScreenToggle);
        fullscreen = false;
        minX = -19f;
        maxX = 19f;
        minY = -10f;
        maxY = 10f;
        smooth = 0.1f;
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
                if (player != null)
                {
                    if (transPlayer)
                    {
                        TransitionPlayer(player);
                    }
                    else
                    {
                        FollowPlayer(player);
                    }
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
        float currentZoom = gameObject.GetComponent<Camera>().orthographicSize;
        fullscreen = false;
        player = null;

        Vector3 finalPos = gameObject.transform.position;
        newPos = projectile.transform.position;
        float smoothedZoom = Mathf.Lerp(currentZoom, originalZoom - zoomCorrector, smooth);

        finalPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        finalPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        finalPos.z = -5f;


        gameObject.transform.position = finalPos;
        //gameObject.GetComponent<Camera>().orthographicSize = smoothedZoom;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - zoomCorrector;
    }

    private void FollowPlayer(GameObject play)
    {
        player = play;
        Vector3 finalPos = gameObject.transform.position;
        newPos = player.transform.position;
        
        finalPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        finalPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        finalPos.z = -5f;

        gameObject.transform.position = finalPos;
        gameObject.GetComponent<Camera>().orthographicSize = originalZoom - zoomCorrector;
    }

    private void TransitionPlayer(GameObject play)
    {
        float currentZoom = gameObject.GetComponent<Camera>().orthographicSize;
        player = play;
        Vector3 finalPos = gameObject.transform.position;
        newPos = player.transform.position;

        finalPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        finalPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        finalPos.z = -5f;

        Vector3 smoothedPosition = Vector3.Lerp(gameObject.transform.position, finalPos, smooth);
        float smoothedZoom = Mathf.Lerp(currentZoom, originalZoom - zoomCorrector, smooth);

        gameObject.transform.position = smoothedPosition;
        if (Mathf.Abs(gameObject.transform.position.x - finalPos.x) < 0.01)
        {
            transPlayer = false; 
        }
        gameObject.GetComponent<Camera>().orthographicSize = smoothedZoom;
    }

    private void ResetCamera()
    {
        float currentZoom = gameObject.GetComponent<Camera>().orthographicSize;
        Vector3 smoothedPosition = Vector3.Lerp(gameObject.transform.position, originalCamPos, smooth);
       // gameObject.transform.position = originalCamPos;
        float smoothedZoom = Mathf.Lerp(currentZoom, originalZoom, smooth);
       // gameObject.GetComponent<Camera>().orthographicSize = originalZoom;
        gameObject.transform.position = smoothedPosition;
        gameObject.GetComponent<Camera>().orthographicSize = smoothedZoom;
        transPlayer = true;
    }
}
