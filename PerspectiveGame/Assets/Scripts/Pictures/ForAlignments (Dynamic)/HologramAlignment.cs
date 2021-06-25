using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

public class HologramAlignment : MonoBehaviour
{
    
    public GameObject TargetObject;
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
            Destroy(this.gameObject);
        }
        

        
    }

    bool Checkalignment()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("hologram"))
            {
                Vector3 LMPOs = child.GetComponent<AssignLandmark>().LMPosition;
                Debug.Log(child.transform.position);
                if (Math.Abs(child.transform.position.x - LMPOs.x) > 1)
                    return false;
                if (Math.Abs(child.transform.position.y - LMPOs.y) > 1)
                    return false;
                if (Math.Abs(child.transform.position.z - LMPOs.z) > 1)
                    return false;
            }
            

        }
        return true;
    }
}
