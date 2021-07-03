using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePicture : MonoBehaviour
{
    //FULLSCREEN SCALE OF THE PICTURE IS 0.433F

    [Header("Phototaking")]
    public int Supersize = 2;
    public int _shotIndex = 0;
    

    [Header("PictureCreation")]
    public GameObject Player;
    public Camera playerCamera;
    public GameObject pictureobj;
    public GameObject pictureexample;
    public List<GameObject> Landmarks = new List<GameObject>();
    public float targetscale = 0.35f;
    public float picutrezposition = 0.933f;
    public float picscale = 0.5f;
    // 1st try : 1.11f

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
    private void LateUpdate()                                                                               //Had to create the object in Late Update as screenshot only runs at the end of the update
    {
        if (System.IO.File.Exists(folderPath+ $"/Screenshot_" +this.gameObject.name + ".png") & created == false) // Ensure the screenshot is created.
        {
            Debug.Log("create");

            GameObject newpic = Instantiate(pictureobj, pictureexample.transform.position, pictureexample.transform.rotation);
            newpic.transform.parent = playerCamera.transform;
            newpic.transform.localScale = new Vector3(picscale, picscale, picscale);            
            newpic.GetComponent<PictureController>().TargetObject = this.gameObject;
            newpic.GetComponent<PictureController>().TargetPosition = this.gameObject.transform.position;
            newpic.GetComponent<PictureController>().TargetRotation = this.gameObject.transform.rotation;


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
        var screenshotName ="Screenshot_" +this.gameObject.name + ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
        
    }
}
