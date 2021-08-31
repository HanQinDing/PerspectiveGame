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
        int ScreenWidth1 = Screen.width;
        float ScreenWidth2  =ScreenWidth1 / 3.2f;

        Texture2D texture = new Texture2D(254, 357, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(685, 278, 939,635), 0, 0);
        texture.Apply();

        byte[] byteArray = texture.EncodeToPNG();
        
        System.IO.File.WriteAllBytes(folderPath + $"/Screenshot_" + objectname + ".png", byteArray);
        Debug.Log("Screenshot");
        
        Destroy(texture);
        UI.SetActive(true);
    }

    public void TakeScreenshot(string Objectname) 
    {
        objectname = Objectname;
        folderPath = System.IO.Directory.GetCurrentDirectory() + $"/Assets/TargetObjects/" + objectname + "/";
        filename = "Screenshot_" + objectname + ".png";
        System.IO.Directory.CreateDirectory(folderPath);
        UI.SetActive(false);
        StartCoroutine("Screenshot");
    }
}
