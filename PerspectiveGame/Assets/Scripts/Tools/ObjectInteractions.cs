using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Interaction")]
    public string type;
    public Quaternion TargetRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyInteraction()
    {
        if (type == "rotate")
            this.transform.localRotation = TargetRotation;
    }
}
