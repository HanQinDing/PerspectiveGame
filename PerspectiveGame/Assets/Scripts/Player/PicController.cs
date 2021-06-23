using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicController : MonoBehaviour
{
    public GameObject pic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pic.activeSelf)
                pic.SetActive(false);
            else
                pic.SetActive(true);
        }
    }
}
