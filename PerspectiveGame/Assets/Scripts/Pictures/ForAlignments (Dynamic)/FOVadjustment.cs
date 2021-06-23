using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVadjustment : MonoBehaviour
{
    public Camera playerCamera2;
    public float targetscale = 0.35f;
    public float fov;
    private float fullscreenscale = 0.433f;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera2 = GetComponent<Camera>();
        fov = playerCamera2.fieldOfView;
        playerCamera2.fieldOfView = (targetscale / fullscreenscale) * fov;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
