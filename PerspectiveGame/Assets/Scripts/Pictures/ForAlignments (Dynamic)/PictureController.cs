using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using UnityEngine;

public class PictureController : MonoBehaviour
{
    // 0.83 z local position
    [Header("MainAttributes")]
    public GameObject player;
    public GameObject TargetObject;
    public GameObject ParentObject;
    public List<string> ObjectstoCreate = new List<string>();
    public List<string> ObjectstoDestroy = new List<string>();
    public List<Vector3> ObjectstoCreate_Position = new List<Vector3>();
    public List<Quaternion> ObjectstoCreate_Rotation = new List<Quaternion>();

    public Vector3 TargetPosition;
    public Quaternion TargetRotation;
    public string Actiontype = "none";      // to indicate what action to take when picture is aligned (e.g. create/rotate the targetobject)
    public string PictureRatio = "none";
    public string PictureType = "static";
    public float leeway = 1f;               // the amount of leeway given to the player. the picture can be slightly off its target location to make it easier for player 
    public bool aligned = false;
    public bool destroy = false;



    [Header("Static")]
    public Vector3 PlayerTargetPosition;
    public Vector3 PlayerTargetRotation;
    public Vector3 StaticPosition;
    public Quaternion StaticRotation;
    public List<GameObject> Parts;

    [Header("Dynamic")]
    public GameObject crosshair;




    void Start()
    {
        if(PictureType == "static") {
        this.gameObject.transform.position = StaticPosition;
        this.gameObject.transform.rotation = StaticRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (PictureType == "Dynamic")
        {
            Checkalignment();
        }
        if (aligned)
        {
           // Debug.Log("Aligned");
            if (Actiontype == "create")
            {
                foreach (string GO in ObjectstoCreate)
                {
                    GameObject newObject = ParentObject.transform.Find(GO).gameObject;
                    newObject.SetActive(true);
                }
            }
                
            else if (Actiontype == "rotate") 
                TargetObject.transform.localRotation = TargetRotation;

            if (destroy)
            {
                foreach (string d in ObjectstoDestroy)
                {
                    Destroy(ParentObject.transform.Find(d).gameObject);
                }
            }
               
            PictureManager PM = player.transform.GetComponent<PictureManager>();
            PM.Pictures.Remove(this.gameObject);
            if(PictureType == "Dynamic")
            {
                PM.holdpicture = false;
                crosshair.SetActive(true);
                //Debug.Log("active");
            }
            Destroy(this.gameObject);   
            
        }
        
        



    }

    bool Checkalignment()
    {
            foreach (Transform child in transform)
            {
                if (child.name.Contains("Reference"))
                {
                    Vector3 TargetPosition = child.GetComponent<ReferenceObject>().TargetPosition;
                    if (Math.Abs(child.transform.position.x - TargetPosition.x) > leeway)
                        return false;
                    if (Math.Abs(child.transform.position.y - TargetPosition.y) > leeway)
                        return false;
                    if (Math.Abs(child.transform.position.z - TargetPosition.z) > leeway)
                        return false;
                }
            }
        aligned = true;
        return true;
    }


}
