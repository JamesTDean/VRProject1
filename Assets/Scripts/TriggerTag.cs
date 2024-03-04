using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriggerTag : MonoBehaviour
{
    TagManager myTagManagerScript;

    private void Start()
    {
        myTagManagerScript = GameObject.Find("TagManager").GetComponentInParent<TagManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            if (collision.transform.parent.tag == "runner")
            {
                if (gameObject.transform.parent.tag == "chaser")
                {
                    GameObject taggedPlayer = collision.transform.parent.gameObject;
                    PhotonView taggedView = taggedPlayer.GetComponent<PhotonView>();
                    myTagManagerScript.TagOccured(taggedView.ViewID);
                    
                }
            }
        }
        catch
        {
            //Debug.Log("Collision object does not have a parent.");
        }
            
    }
}
