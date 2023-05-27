using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 17.84475f;
    public float xLimits;
    public float ylimits;
    public bool ZoomInButtonisPressed;
    public bool ZoomOutButtonisPressed;
    public float increament;
    private void Start()
    {
        Camera.main.orthographicSize = zoomOutMax;
    }
    private static readonly float PanSpeed = 40f;
   

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                //wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved && Camera.main.orthographicSize != zoomOutMax && !ZoomInButtonisPressed && !ZoomOutButtonisPressed)
                {
                    PanCamera(touch.position);
                }
                break;

        }
    }

    

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed,0 );

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -xLimits, xLimits);
        pos.y = Mathf.Clamp(transform.position.y, -ylimits, ylimits);
        pos.z = -10;
        transform.position = pos;
        Debug.Log(transform.position);
        // Cache the position
        lastPanPosition = newPanPosition;
    }

    

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
    void zoomOut(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + increment, zoomOutMin, zoomOutMax);
    }
    public void ZoominButtonIsPressed()
    {
        ZoomInButtonisPressed = true;
        StartCoroutine(ZoomIn());
    }
    public void ZoominButtonIsRelease()
    {
        ZoomInButtonisPressed = false;
        StopCoroutine(ZoomIn());
    }
    IEnumerator ZoomIn()
    {
        while (ZoomInButtonisPressed)
        {
            if(Camera.main.orthographicSize!= zoomOutMin)
            {
                increament += 0.1f;
                zoom(increament);

            }
            yield return new WaitForSeconds(0.5f);

        }
    }
    public void ZoomOutyButtonIsPressed()
    {
        ZoomOutButtonisPressed = true;
        StartCoroutine(ZoomOut());
    }
    public void ZoomOutyButtonIsRelease()
    {
        ZoomOutButtonisPressed = false;
        StopCoroutine(ZoomOut());
    }
    IEnumerator ZoomOut()
    {
        while (ZoomOutButtonisPressed)
        {
            if (Camera.main.orthographicSize != zoomOutMax && increament>=0)
            {
                increament -= 0.1f;
                if (increament > 0)
                {
                    zoomOut(increament);

                } 
                else
                {
                    increament = 0;
                    Camera.main.orthographicSize = zoomOutMax;
                }

            }
           
            yield return new WaitForSeconds(0.5f);

        }
    }
}