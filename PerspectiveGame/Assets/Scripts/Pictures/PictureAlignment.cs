using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureAlignment : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject Player;
    public GameObject TargetObject;

    [Header("PictureCoordinates")]
    public float PosX;
    public float PosY;
    public float PosZ;

    [Header("picture rotation")]
    public float RotX;
    public float RotY;
    public float RotZ;
    

    [Header("targetCoordinates")]
    public float TPosX;
    public float TPosY;
    public float TPosZ;

    [Header("target rotation")]
    public float TRotX;
    public float TRotY;
    public float TRotZ;

    private int counts = 0;
    private void Awake()
    {
        playerCamera = Player.transform.Find("Camera").GetComponent<Camera>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counts == 0) { 
        if(Checkalignment() == true)
        {
            Debug.Log("aligned");
            Instantiate(TargetObject, new Vector3(TPosX, TPosY, TPosZ), Quaternion.Euler(new Vector3(TRotX, TRotY, TRotZ)));
             counts = 1;
            Destroy(this.gameObject);
        }
        }

    }

    bool Checkalignment()
    {
        if (!(Player.transform.position.x <= PosX+2 && Player.transform.position.x >= PosX-2))
            return false;
        if (!(Player.transform.position.y <= PosY + 2 && Player.transform.position.y >= PosY - 2))
            return false;
        if (!(Player.transform.position.z <= PosZ + 2 && Player.transform.position.z >= PosZ - 2))
            return false;
        if (!((Player.transform.rotation.eulerAngles.y - 360) <= RotY + 2 && (Player.transform.rotation.eulerAngles.y - 360) >= RotY - 2)   )
            return false;
        if (!(playerCamera.transform.eulerAngles.x <= RotX + 2 && playerCamera.transform.eulerAngles.x >= RotX - 2))
            return false;
        if (!(playerCamera.transform.eulerAngles.z != RotZ + 2 && playerCamera.transform.eulerAngles.z >= RotZ - 2))
            return false;

        return true;
    }
}
