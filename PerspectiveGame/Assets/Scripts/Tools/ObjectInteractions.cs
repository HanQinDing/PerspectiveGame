using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Interaction")]
    public bool Repeat = false;
    public bool Activatable = true;

    [Header("Rotate")]
    public Vector3 RotateDirection;

    public bool Rotate = false;

    [Header("Move")]
    public Vector3 TargetPosition;
    public bool BackForth;
    public bool Move = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyInteraction()
    {
        if (Activatable) { 
        if (Rotate)
            this.transform.Rotate(RotateDirection.x, RotateDirection.y, RotateDirection.z,Space.Self);

        if (Move)
        {
            Vector3 NewPos;
            if (BackForth)
                NewPos = this.transform.position;
            else
                NewPos = TargetPosition;
            this.transform.localPosition = TargetPosition;
            TargetPosition = NewPos;

        }
        if (this.gameObject.GetComponent<PartController>())
            this.gameObject.GetComponent<PartController>().CheckifInPlace(Move,Rotate);
        if(Repeat != true)
            {
                Activatable = false;
            }
        }

    }
}
