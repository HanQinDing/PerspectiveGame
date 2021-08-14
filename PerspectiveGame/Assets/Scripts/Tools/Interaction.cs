using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Interaction")]
    public string type;
    public Quaternion TargetRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyInteraction();
    }

    void ApplyInteraction()
    {
        if(type == "pickup")
        {

        }
        else if(type == "rotate")
        {
            Vector3 targetrotationVec3 = TargetRotation.eulerAngles;
            Vector3 ObjectCurrentRotationVec3 = this.transform.rotation.eulerAngles;
            this.transform.rotation = Quaternion.Euler(ObjectCurrentRotationVec3.x + targetrotationVec3.x, ObjectCurrentRotationVec3.y + targetrotationVec3.y, ObjectCurrentRotationVec3.z + targetrotationVec3.z);

        }
    }
}
