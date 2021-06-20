using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public int Supersize = 2;
    public int _shotIndex = 0;
    public Camera playerCamera;
    public GameObject bridge;
    // Start is called before the first frame update
    private void Awake()
    {
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot($"ScreenShot{_shotIndex}.png", Supersize);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("PosX: " + transform.position.x);
            Debug.Log("PosY: " + transform.position.y);
            Debug.Log("PosZ: " + transform.position.z);
            Debug.Log("RotY: " + (transform.rotation.eulerAngles.y-360));
            Debug.Log("RotX: " + playerCamera.transform.eulerAngles.x);
            Debug.Log("bridge: " + bridge.transform.position);
        }
    }
}
