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
    public float picutrezposition = 1.11f;

    private float fullscreenscale = 0.433f;
    private bool check = false;
    private int count = 0;
    private int HG_count = 1;
    private float fov;
    void Awake()
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
        if (Input.GetKeyDown(KeyCode.P))
            takepic();

        if (System.IO.File.Exists($"ScreenShotbridge.png"))
        {
            if(count == 0)
            {
                check = true;
                count = 1;
            }
        }
        
        
    }
    private void LateUpdate()
    {
        if(check == true) {
         playerCamera.fieldOfView = fov;
        GameObject newpic = Instantiate(pictureobj, pictureexample.transform.position, pictureexample.transform.rotation);
         //   newpic.transform.position = playerCamera.WorldToScreenPoint(newpic);
        newpic.transform.parent = playerCamera.transform;
        //newpic.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        foreach (GameObject LM in Landmarks)
        {
            if (LM.GetComponent("AssignHolograms") as AssignHolograms != null)
            {
                GameObject HG = LM.GetComponent<AssignHolograms>().hologram;
                GameObject newLMHologram = Instantiate(HG, LM.transform.position, LM.transform.rotation);
                newLMHologram.name = "hologram" + HG_count.ToString();
                newLMHologram.GetComponent<AssignLandmark>().Landmark = LM;
                newLMHologram.GetComponent<AssignLandmark>().LMPosition = LM.transform.position;
                //newLMHologram.transform.position = Vector3.Lerp(newLMHologram.transform.position, newpic.transform.position, 0.5f);
                //newLMHologram.transform.position = Lerpwithoutclamp(newpic.transform.position, newLMHologram.transform.position, GettingFraction(fullscreenscale, 0.35f));
                newLMHologram.transform.parent = newpic.transform;
                //GettingFraction(fullscreenscale, 0.35f)
                 HG_count += 1;
            }

        }
            check = false;
        }
    }

    /*float GettingFraction(float Fullscale, float currentscale)
    {
        float percentage = ((fullscreenscale - currentscale) / fullscreenscale) * 100;
        return ((percentage + 100) / 100);

        //((fullscreenscale - 0.35f) / fullscreenscale) + fullscreenscale)/ fullscreenscale
    }
    
    Vector3 Lerpwithoutclamp(Vector3 A, Vector3 B, float t)
    {
        return A + (B - A) * t;
    }*/

    void takepic()
    {
        fov = playerCamera.fieldOfView;
        playerCamera.fieldOfView = (targetscale / fullscreenscale) * fov;
        ScreenCapture.CaptureScreenshot($"ScreenShotbridge.png", Supersize);
        
    }
}
