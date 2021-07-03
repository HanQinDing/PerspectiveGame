using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

public class PictureController : MonoBehaviour
{
    
    public GameObject TargetObject;
    public GameObject crosshair;
    public GameObject player;
    public Vector3 TargetPosition;
    public Quaternion TargetRotation;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Checkalignment())
        {
            Instantiate(TargetObject, TargetPosition, TargetRotation);
            PictureManager PM = player.transform.GetComponent<PictureManager>();
            PM.Pictures.Remove(this.gameObject);
            PM.holdpicture = false;
            Destroy(this.gameObject);
            crosshair.SetActive(true);
        }
        

        
    }

    bool Checkalignment()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("hologram"))
            {
                Vector3 LMPOs = child.GetComponent<AssignLandmark>().LMPosition;
                //Debug.Log(child.transform.position);
                if (Math.Abs(child.transform.position.x - LMPOs.x) > 1)
                    return false;
                if (Math.Abs(child.transform.position.y - LMPOs.y) > 1)
                    return false;
                if (Math.Abs(child.transform.position.z - LMPOs.z) >    1)
                    return false;
            }
            

        }
        return true;
    }
}
