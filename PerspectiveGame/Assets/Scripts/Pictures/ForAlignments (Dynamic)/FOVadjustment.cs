using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVadjustment : MonoBehaviour
{
    public Camera playerCamera;
    public float targetscale = 0.5f;
    public bool reduced = false;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            /*
             if (reduced) {
                 playerCamera.fieldOfView = playerCamera.fieldOfView / targetscale ;
             reduced = false;
             }
             else { 
                 playerCamera.fieldOfView = targetscale * playerCamera.fieldOfView;
                 reduced = true;
             }
             */
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y, 9.8f);
        }
    }
}
