using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public enum PictureSwitchState
    {
        PutupNew,        //Player is holding up a new picture
        Putdown,         //Player has chose to stop holding a picture
        PutdownPrevious //Player is keeping its current picture and swtiching to a new picture

    }


    [Header("ObjReference")]
    public List<GameObject> Pictures = new List<GameObject>();
    public GameObject player;
    public GameObject Pictureholder;
    public GameObject crosshair;


    [Header("For Switching of Picture")]
    public GameObject HoldingPosition;
    public GameObject FiveSeven_HoldingPosition;
    public GameObject KeepingPosition;
    public float Switchdelay = 1f; // adjust how the speed of the transition

    public bool holdpicture = false;
    public bool Isswitching = false;
    private int CurrentPicureIndex = 0;
    private int PreviousPicureIndex = 0;
    

    Ray ray;
    RaycastHit hit;
    Camera playerCamera;
    PictureSwitchState SwitchState;
    GameObject targetpositon;
    float switchingStartTIme;
   



    void Start()
    {
        SwitchState = PictureSwitchState.Putdown;
    }

    // Update is called once per frame
    void Update()
    {       
        if (!Isswitching) 
        { 
            HoldPicture();
            SwitchPictureIndex();
            //PicturePositioning();
        }
    }
    private void LateUpdate()
    {
        if (Isswitching)
            UpdateSwitchPicture();
    }

    void UpdateSwitchPicture() //This will proccess the movements of the pictures (From Holding Position to Keeping Position vice versa)
    {
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
                currentpicture.transform.position = KeepingPosition.transform.position;
                currentpicture.transform.parent = KeepingPosition.transform;
                currentpicture.SetActive(false);
                holdpicture = false;
                Isswitching = false;
                //crosshair.SetActive(true);
            }
            else if (SwitchState == PictureSwitchState.PutdownPrevious)
            {

                previouspicture.transform.position = KeepingPosition.transform.position;
                previouspicture.transform.parent = KeepingPosition.transform;
                previouspicture.SetActive(false);
                currentpicture.SetActive(true);
                string Picture_ratio = currentpicture.transform.GetComponent<PictureController>().PictureRatio;
                if (Picture_ratio == "5x7")
                    targetpositon = FiveSeven_HoldingPosition;
                else
                    targetpositon = HoldingPosition;

                switchingStartTIme = Time.time;
                SwitchState = PictureSwitchState.PutupNew;
               
            }
            else
            {
                
                currentpicture.transform.parent = targetpositon.transform;
                currentpicture.transform.position = targetpositon.transform.position;
                currentpicture.transform.parent = targetpositon.transform;
                holdpicture = true;
                Isswitching = false;
                //crosshair.SetActive(false);
            }
                                          
        }

        else
        {
            if (SwitchState == PictureSwitchState.Putdown)
                currentpicture.transform.position = Vector3.Lerp(targetpositon.transform.position, KeepingPosition.transform.position, switchingTimeFactor);
            if (SwitchState == PictureSwitchState.PutdownPrevious)
                previouspicture.transform.position = Vector3.Lerp(targetpositon.transform.position, KeepingPosition.transform.position, switchingTimeFactor);
            if (SwitchState == PictureSwitchState.PutupNew)
                {
                currentpicture.SetActive(true);
                currentpicture.transform.position = Vector3.Lerp(KeepingPosition.transform.position, targetpositon.transform.position, switchingTimeFactor);
            }
        } 
        
    }
    
    public void PickupPicture(GameObject PicutreObject)
    {           

                if (!Pictures.Contains(PicutreObject))
                {
                    Pictures.Add(PicutreObject);
                    //The picture object's scale will change when we set its parent. Hence we need to record its original scale beforehand and use it to set the picture object's scale back.
                    Vector3 originalscale = new Vector3(PicutreObject.transform.localScale.x * (1/player.transform.localScale.x), PicutreObject.transform.localScale.y * (1 / player.transform.localScale.y), PicutreObject.transform.localScale.z * (1 / player.transform.localScale.z));
                    
                    if (Pictures.Count == 1) // Player did not have any picture beforehand
                    {
                            string Picture_ratio = PicutreObject.transform.GetComponent<PictureController>().PictureRatio;
                            if(Picture_ratio == "5x7")
                                targetpositon = FiveSeven_HoldingPosition;
                            else
                                targetpositon = HoldingPosition;
                            PicutreObject.transform.rotation = targetpositon.transform.rotation;
                            PicutreObject.transform.parent = targetpositon.transform;
                            PicutreObject.transform.position = targetpositon.transform.position;
                            //PicutreObject.transform.localPosition = HoldingPosition.transform.localPosition;
                            holdpicture = true;
                            //crosshair.SetActive(false); 
                    }

                    else // Player had at least 1 picture with him
                    {
                            
                            PicutreObject.transform.position = KeepingPosition.transform.position;
                            PicutreObject.transform.rotation = KeepingPosition.transform.rotation;
                            PicutreObject.transform.parent = KeepingPosition.transform;
                            PicutreObject.SetActive(false);
                     }
                    
                    PicutreObject.transform.localScale = originalscale;  // This is where we use set the picture object back to its original scale.
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
