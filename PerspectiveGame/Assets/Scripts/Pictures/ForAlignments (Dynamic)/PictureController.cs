using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

public class PictureController : MonoBehaviour
{   
    // 0.83 z local position
    
    public GameObject TargetObject;
    public GameObject crosshair;
    public Vector3 TargetPosition;
    public Quaternion TargetRotation;
    public string Actiontype = "none"; // to indicate what action to take when picture is aligned (e.g. create/rotate the targetobject)
    public string PictureRatio = "none"; // to indicate what action to take when picture is aligned (e.g. create/rotate the targetobject)
    public float leeway = 1f; // the amount of leeway given to the player. the picture can be slightly off its target location to make it easier for player 
    


    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (Checkalignment())
        {
            if (Actiontype == "create")
                Instantiate(TargetObject, TargetPosition, TargetRotation);
            else if (Actiontype == "rotate") 
                TargetObject.transform.rotation = TargetRotation;



            PictureManager PM = player.transform.GetComponent<PictureManager>();
            PM.Pictures.Remove(this.gameObject);
            PM.holdpicture = false;
            crosshair.SetActive(true);
            Debug.Log("active");
            Destroy(this.gameObject);   
            
        }
        */





    }

    bool Checkalignment()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("hologram"))
            {
                Vector3 LMPOs = child.GetComponent<AssignLandmark>().LMPosition;
                //Debug.Log(child.transform.position);
                if (Math.Abs(child.transform.position.x - LMPOs.x) > leeway)
                    return false;
                if (Math.Abs(child.transform.position.y - LMPOs.y) > leeway)
                    return false;
                if (Math.Abs(child.transform.position.z - LMPOs.z) > leeway)
                    return false;
            }
            

        }
        return true;
    }


}
