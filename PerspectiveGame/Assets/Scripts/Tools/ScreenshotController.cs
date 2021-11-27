using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UI;
    private string filename;
    private string folderPath;
    private string objectname;
    private int S_width;
    private int S_Height;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Screenshot() 
    {
        yield return new WaitForEndOfFrame();
       
        Texture2D texture = new Texture2D(S_width, S_Height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect((Screen.width-S_width)/2, (Screen.height - S_Height) / 2, ((Screen.width - S_width) / 2) + S_width, ((Screen.height - S_Height) / 2)+ S_Height), 0, 0);
        texture.Apply();

        byte[] byteArray = texture.EncodeToPNG();
        
        System.IO.File.WriteAllBytes(folderPath + $"/Screenshot_" + objectname + ".png", byteArray);

        
        Destroy(texture);
        UI.SetActive(true);
    }

    public void TakeScreenshot(string Objectname, float widthratio, float heightratio) 
    {
        S_width = (int)((widthratio / 16) * Screen.width);
        S_Height = (int)((heightratio / 9) * Screen.height);
        objectname = Objectname;
        folderPath = System.IO.Directory.GetCurrentDirectory() + $"/Assets/Resources/TargetObjects/" + objectname + "/";
        filename = "Screenshot_" + objectname + ".png";
        System.IO.Directory.CreateDirectory(folderPath);
        UI.SetActive(false);
        StartCoroutine("Screenshot");
    }
}
