using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

public class HologramAlignment : MonoBehaviour
{
    
    public GameObject TargetObject;

    [Header("targetCoordinates")]
    public float TPosX;
    public float TPosY;
    public float TPosZ;

    [Header("target rotation")]
    public float TRotX;
    public float TRotY;
    public float TRotZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Checkalignment())
        {
            Instantiate(TargetObject, new Vector3(TPosX, TPosY, TPosZ), Quaternion.Euler(new Vector3(TRotX, TRotY, TRotZ)));
            Destroy(this.gameObject);
            
            
            //Debug.Log("OKOKOK");
        }
        

        
    }

    bool Checkalignment()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("hologram"))
            {
                Vector3 LMPOs = child.GetComponent<AssignLandmark>().LMPosition;
                if (Math.Abs(child.transform.position.x - LMPOs.x) > 0.5)
                    return false;
                if (Math.Abs(child.transform.position.y - LMPOs.y) > 0.5)
                    return false;
                if (Math.Abs(child.transform.position.z - LMPOs.z) > 0.5)
                    return false;
            }
            

        }
        return true;
    }
}
