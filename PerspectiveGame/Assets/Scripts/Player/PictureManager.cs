using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public enum PictureSwitchState
    {
        PutupNew,
        Putdown,
        PutdownPrevious,

    }


    [Header("ObjReference")]
    public List<GameObject> Pictures = new List<GameObject>();
    public GameObject player;
    public GameObject Pictureholder;
    public GameObject crosshair;

    [Header("For Switching of Picture")]
    public GameObject HoldingPosition;
    public GameObject KeepingPosition;
    public float Switchdelay = 1f; // adjust how the speed of the transition

    public bool holdpicture = false;
    private bool Isswitching = false;
    private int CurrentPicureIndex = 0;
    private int PreviousPicureIndex = 0;
    

    Ray ray;
    RaycastHit hit;
    Camera playerCamera;
    PictureSwitchState SwitchState;
    float switchingStartTIme;



    void Start()
    {
        SwitchState = PictureSwitchState.Putdown;
    }

    // Update is called once per frame
    void Update()
    {
        
        PickupPicture();
        if (!Isswitching)
            HoldPicture();
            SwitchPictureIndex();
    }
    private void LateUpdate()
    {
        if (Isswitching)
            UpdateSwitchPicture();
    }

    void UpdateSwitchPicture() {

        GameObject currentpicture = Pictures[CurrentPicureIndex];
        GameObject previouspicture = Pictures[PreviousPicureIndex];
        float switchingTimeFactor = 0f;
        
        if (Switchdelay == 0f)
            switchingTimeFactor = 1f;
        else
            switchingTimeFactor = Mathf.Clamp01((Time.time - switchingStartTIme) / Switchdelay);

           
        if (switchingTimeFactor >= 1f)
        {
            if (SwitchState == PictureSwitchState.Putdown)
            {
                currentpicture.SetActive(false);
                holdpicture = false;
                Isswitching = false;
                crosshair.SetActive(true);
            }
            else if (SwitchState == PictureSwitchState.PutdownPrevious)
            {
                previouspicture.SetActive(false);
                currentpicture.SetActive(true);
               
                switchingStartTIme = Time.time;
                SwitchState = PictureSwitchState.PutupNew;
               
            }
            else
            {
                holdpicture = true;
                Isswitching = false;
                crosshair.SetActive(false);
            }
                
            
                
        }

        else
        {
            if (SwitchState == PictureSwitchState.Putdown)
                currentpicture.transform.localPosition = Vector3.Lerp(HoldingPosition.transform.localPosition, KeepingPosition.transform.localPosition, switchingTimeFactor);
            if (SwitchState == PictureSwitchState.PutdownPrevious)
                previouspicture.transform.localPosition = Vector3.Lerp(HoldingPosition.transform.localPosition, KeepingPosition.transform.localPosition, switchingTimeFactor);
            if (SwitchState == PictureSwitchState.PutupNew)
                {
                currentpicture.SetActive(true);
                currentpicture.transform.localPosition = Vector3.Lerp(KeepingPosition.transform.localPosition, HoldingPosition.transform.localPosition, switchingTimeFactor);
            }
        } 
        
    }
    
    void PickupPicture()
    {
        playerCamera = player.transform.Find("Camera").GetComponent<Camera>();
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0) &&  hit.collider.gameObject.transform.parent != null) {
                if ( hit.collider.gameObject.transform.parent.transform.GetComponent<PictureController>() != null && !Pictures.Contains(hit.collider.gameObject.transform.parent.gameObject))
                {

                    GameObject parentObject = hit.collider.gameObject.transform.parent.gameObject;
                    Pictures.Add(parentObject);
                    parentObject.transform.parent = Pictureholder.transform;
                    if(Pictures.Count == 1)
                    {
                        parentObject.transform.localPosition = HoldingPosition.transform.localPosition;
                        parentObject.transform.localEulerAngles = HoldingPosition.transform.localEulerAngles;
                        holdpicture = true;
                        crosshair.SetActive(false); 
                    }
                    else
                    {
                        parentObject.transform.localPosition = KeepingPosition.transform.localPosition;
                        parentObject.transform.localEulerAngles = HoldingPosition.transform.localEulerAngles;
                        parentObject.SetActive(false);
                    }               
                }
            }
        }
    }

    void HoldPicture() // player can choose whether to hold the picture
    {
        
        if (Input.GetKeyDown(KeyCode.E) && Isswitching == false)
        {
            if (holdpicture == false)   // player currently not holding any picture
            {
                if (Pictures.Count != 0) // player keeps at least 1 picture
                {
  
                    GameObject currentpicture = Pictures[CurrentPicureIndex];
                    SwitchState = PictureSwitchState.PutupNew;
                    switchingStartTIme = Time.time;
                    Isswitching = true;
                } 
            }


            else
            {
                GameObject currentpicture = Pictures[CurrentPicureIndex];
                SwitchState = PictureSwitchState.Putdown;
                switchingStartTIme = Time.time;
                Isswitching = true;
            }
        }
    }

    void SwitchPictureIndex()
    {
        
        int SwitchInput = GetSwitchPictureInput();
        if(SwitchInput != 0)
        {
            PreviousPicureIndex = CurrentPicureIndex;
            if (CurrentPicureIndex + SwitchInput >= Pictures.Count)
                CurrentPicureIndex = 0;
            else if (CurrentPicureIndex + SwitchInput < 0)
                CurrentPicureIndex = Pictures.Count - 1;
            else
                CurrentPicureIndex += SwitchInput;

            if (holdpicture && CurrentPicureIndex != PreviousPicureIndex)
            {

                SwitchState = PictureSwitchState.PutdownPrevious;
                switchingStartTIme = Time.time;
                Isswitching = true;
            }
            
        }
    }
    int GetSwitchPictureInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            return -1;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            return 1;
        return 0;
    }

}
