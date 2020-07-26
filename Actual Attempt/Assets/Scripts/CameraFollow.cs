using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   

    public Transform followTransform;
    public BoxCollider2D mapBounds;

    public float xMin, xMax, yMin, yMax;
    public float camX, camY;
    public float xResize = 2f;
    public float smoothSpeed = 0.5f;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;
    private Vector3 smoothPos;


    private void Start()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        cameraRatio = (xMax + camOrthsize) / xResize;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + cameraRatio, xMax - cameraRatio); 
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        this.transform.position = smoothPos;
        
    }
}
