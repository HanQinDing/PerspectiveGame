using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePicture : MonoBehaviour
{
    [Header("Only turn on this script to take picture of a specific stage object and create picture object")]


    [Header("PictureCreation")]
    public float S_width = 5;
    public float S_Height = 7;
    public float scale = 1;
    public GameObject Player;
    public Camera playerCamera;
    public GameObject PicturePrefab;
    public GameObject HoldingPosition;
    public string Actiontype = "none";
    public string Picturetype = "Dynamic";


    [Header("Dynamic")]
    public Material InvisbleMaterial;
    public List<GameObject> Reference = new List<GameObject>();

    [Header("Static")]
    public GameObject StaticReferenceObject;



    //To Check if player has alrd taken a picture
    public List<GameObject> ObjectstoCreate = new List<GameObject>();
    public List<GameObject> ObjectstoDestroy = new List<GameObject>();
    public bool CreateSelf = true;
    public bool destroy = false;

    public GameObject ObJ_To_Destroy;
    private bool created = false;
    private string folderPath = "";
    private GameObject pic;
    private int HG_count = 1;
    void Awake()
    {
        playerCamera = Player.transform.Find("Camera").GetComponent<Camera>();
        folderPath = System.IO.Directory.GetCurrentDirectory() + $"/Assets/Resources/TargetObjects/" + this.gameObject.name + "/";
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
            //Debug.Log(HoldingPosition.transform.position);
            GameObject newpic = Instantiate(PicturePrefab, HoldingPosition.transform.position, HoldingPosition.transform.rotation);
            
            newpic.GetComponent<PictureController>().TargetObject = this.gameObject;
            newpic.GetComponent<PictureController>().Actiontype = Actiontype;
            newpic.GetComponent<PictureController>().PictureType = Picturetype;
            newpic.GetComponent<PictureController>().PictureRatio = S_width.ToString() + "x" + S_Height.ToString();

            if (Actiontype == "create")
            {
                if (CreateSelf)
                    ObjectstoCreate.Add(this.gameObject);
                foreach (GameObject GO in ObjectstoCreate)
                {
                    newpic.GetComponent<PictureController>().ObjectstoCreate.Add(GO.name);
                    newpic.GetComponent<PictureController>().ObjectstoCreate_Position.Add(GO.transform.position);
                    newpic.GetComponent<PictureController>().ObjectstoCreate_Rotation.Add(GO.transform.rotation);
                }
            }
            else if(Actiontype == "rotate")
                newpic.GetComponent<PictureController>().TargetRotation = this.gameObject.transform.localRotation;

            if (destroy)
            {
                foreach (GameObject D in ObjectstoDestroy)
                {
                    newpic.GetComponent<PictureController>().ObjectstoDestroy.Add(D.name);
                }
            }
            if(Picturetype == "Dynamic")
            {
                newpic.name = "Dynamic_" + this.gameObject.name;
                foreach (GameObject R in Reference)
                {  
                     
                    GameObject ReferenceObj = Instantiate(R, R.transform.position, R.transform.rotation);
                    ReferenceObj.GetComponent<Renderer>().material = InvisbleMaterial;
                    ReferenceObj.AddComponent<ReferenceObject>();
                    ReferenceObj.GetComponent<ReferenceObject>().TargetPosition = R.transform.position;
                    ReferenceObj.name = "Reference" + HG_count.ToString();
                    ReferenceObj.transform.parent = newpic.transform;
                    HG_count += 1;
                }
            }
            else if(Picturetype== "static")
            {
                newpic.name = "Static_" + this.gameObject.name;
                GameObject ReferenceObj = Instantiate(StaticReferenceObject, HoldingPosition.transform.position, HoldingPosition.transform.rotation);
                ReferenceObj.transform.parent = newpic.transform;
                newpic.GetComponent<PictureController>().StaticPosition = HoldingPosition.transform.position;
                newpic.GetComponent<PictureController>().StaticRotation = HoldingPosition.transform.rotation;
                GameObject StaticPartController = new GameObject();
                StaticPartController.name = "StaticPartController";
                StaticPartController.transform.parent = newpic.transform;
                StaticPartController.transform.position = ReferenceObj.transform.position;
                StaticPartController.AddComponent<StaticPartsController>();
                playerCamera.transform.Find("PicHandler").transform.Find("5x7Reference").transform.GetComponent<StaticReferenceController>().StaticPictures.Add(ReferenceObj);
            }
            created = true;
           
        }
        
      
    }
    void takepic() // Take screenshot and save the picture at the designated path
    {      
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);
        playerCamera.GetComponent<ScreenshotController>().TakeScreenshot(this.gameObject.name, S_width/(1/scale), S_Height/(1 / scale));
    }
}
