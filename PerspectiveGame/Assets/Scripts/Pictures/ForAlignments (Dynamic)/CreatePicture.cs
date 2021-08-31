using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePicture : MonoBehaviour
{
    [Header("Only turn on this script to take picture of a specific stage object and create picture object")]


    [Header("PictureCreation")]
    public string Actiontype = "none";
    public GameObject Player;
    public Camera playerCamera;
    public GameObject PicturePrefab;
    public GameObject HoldingPosition;
    public List<GameObject> Landmarks = new List<GameObject>();

    //To Check if player has alrd taken a picture
    private bool created = false;
    private int HG_count = 1;
    private string folderPath = "";
    private GameObject pic;
    void Awake()
    {
        playerCamera = Player.transform.Find("Camera").GetComponent<Camera>();
        folderPath = System.IO.Directory.GetCurrentDirectory() + $"/Assets/TargetObjects/" + this.gameObject.name + "/";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            takepic();             
    }
    private void LateUpdate() ///Had to create the picture gameobject in LateUpdate as screenshot only runs at the end of the update
    {
        if (System.IO.File.Exists(folderPath+ $"/Screenshot_" +this.gameObject.name + ".png") && created == false) // Ensure the screenshot is created.
        {
            Debug.Log(created);
            GameObject newpic = Instantiate(PicturePrefab, HoldingPosition.transform.position, HoldingPosition.transform.rotation);

            newpic.GetComponent<PictureController>().TargetObject = this.gameObject;
            newpic.GetComponent<PictureController>().Actiontype = Actiontype;
            newpic.GetComponent<PictureController>().player = Player;

            if(Actiontype == "create")
            {
                newpic.GetComponent<PictureController>().TargetPosition = this.gameObject.transform.position;
                newpic.GetComponent<PictureController>().TargetRotation = this.gameObject.transform.rotation;
            }
            else if(Actiontype == "rotate")
                newpic.GetComponent<PictureController>().TargetRotation = this.gameObject.transform.localRotation;

            foreach (GameObject LM in Landmarks)
            {
                if (LM.GetComponent("AssignHolograms") as AssignHolograms != null)
                {
                    GameObject HG = LM.GetComponent<AssignHolograms>().hologram;
                    GameObject newLMHologram = Instantiate(HG, LM.transform.position, LM.transform.rotation);
                    newLMHologram.name = "hologram" + HG_count.ToString();
                    newLMHologram.GetComponent<AssignLandmark>().Landmark = LM;
                    newLMHologram.GetComponent<AssignLandmark>().LMPosition = LM.transform.position;
                    newLMHologram.transform.parent = newpic.transform;
                    HG_count += 1;
                }

            }
            created = true;
           
        }
        
      
    }
    void takepic() // Take screenshot and save the picture at the designated path
    {      
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);
        playerCamera.GetComponent<ScreenshotController>().TakeScreenshot(this.gameObject.name);
    }
}
