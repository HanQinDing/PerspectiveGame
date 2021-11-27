using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour
{
    public Vector3 TargetPosition;
    public Quaternion TargetRotation;
    public bool inplace = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckifInPlace(bool Pos, bool Rot)
    {
        //Debug.Log(this.gameObject.transform.localRotation);
        //Debug.Log(TargetRotation);
        if (Pos  && !Rot) //Only Need to be in the right position
            inplace = (this.gameObject.transform.localPosition == TargetPosition);

        else if(Rot && !Pos) //Only need to be in the right rotation
            inplace = (this.gameObject.transform.localRotation == TargetRotation);

        else // Need to be in the right position with the right position
             inplace = (this.gameObject.transform.localPosition == TargetPosition && this.gameObject.transform.localRotation == TargetRotation);
    }
}
