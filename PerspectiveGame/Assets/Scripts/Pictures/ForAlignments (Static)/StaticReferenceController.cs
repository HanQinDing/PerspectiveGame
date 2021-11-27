using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticReferenceController : MonoBehaviour
{
    public List<GameObject> StaticPictures = new List<GameObject>();
    public GameObject player;
    public float POSleeway;
    public float Angleleeway;
    private List<GameObject> ItemsToRemove = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForImages();
    }

    void CheckForImages()
    {
         foreach (GameObject child in StaticPictures){
            //Debug.Log(this.gameObject.transform.position);
            //x`Debug.Log(child.transform.position);
            //if (Vector3.Distance(this.gameObject.transform.position, child.transform.position) <= POSleeway && Vector3.Distance(this.gameObject.transform.position, child.transform.position) >= 0 && Quaternion.Angle(this.gameObject.transform.rotation, child.transform.rotation) <= Angleleeway && Quaternion.Angle(this.gameObject.transform.rotation, child.transform.rotation) >= 0)
             if (Vector3.Distance(this.gameObject.transform.position, child.transform.position) <= POSleeway && Vector3.Distance(this.gameObject.transform.position, child.transform.position) >= 0)
            {
                Debug.Log("Hit");
                GameObject Parent = child.transform.parent.gameObject;
                GameObject SPContorller = Parent.transform.Find("StaticPartController").gameObject;
                if (SPContorller.GetComponent<StaticPartsController>().CheckIfAllAlgined())
                {
                    Parent.GetComponent<PictureController>().aligned = true;
                    ItemsToRemove.Add(child);
                    player.GetComponent<PlayerController>().characterVelocity = new Vector3(0, 0, 0);
                    break;
                }
            }
        }
        foreach (GameObject item in ItemsToRemove)
        {
            StaticPictures.Remove(item);
        }
    }
}
