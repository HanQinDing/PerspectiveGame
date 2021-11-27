using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPartsController : MonoBehaviour
{
    public List<GameObject> parts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfAllAlgined()
    {
        foreach (GameObject child in parts)
        {
            if (!child.GetComponent<PartController>().inplace)
                return false;
        }
        return true;
    }
}
