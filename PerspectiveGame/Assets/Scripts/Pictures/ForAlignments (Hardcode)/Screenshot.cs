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

    [Header("targetCoordinates")]
    public float TPosX;
    public float TPosY;
    public float TPosZ;

    [Header("target rotation")]
    public float TRotX;
    public float TRotY;
    public float TRotZ;
    private void Awake()
    {
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
    }
    void Start()
    {

        Instantiate(bridge, new Vector3(TPosX, TPosY, TPosZ), Quaternion.Euler(new Vector3(TRotX, TRotY, TRotZ)));
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
            Debug.Log("PosX: " + transform.position.x + "\n PosY: " + transform.position.y + "\n PosZ: " + transform.position.z + "\n RotY: " + (transform.rotation.eulerAngles.y - 360) + "\n RotX: " + playerCamera.transform.eulerAngles.x);
            Debug.Log("bridge: " + bridge.transform.position);
        }
    }
}
