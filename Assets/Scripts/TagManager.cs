using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TagManager : MonoBehaviourPunCallbacks
{
    private PlayerTagTracker myPlayerTagTrackerScript;

    public void TagOccured(int newChaserID)
    {
        var photonViews = FindObjectsOfType<PhotonView>();

        foreach (var view in photonViews)
        {
            Debug.Log(view.gameObject.name); 
            if (view.gameObject.name == "NetworkPlayer(Clone)")
            {
                bool tagged = (view.ViewID == newChaserID);
                Debug.Log(tagged);
                PlayerTagTracker myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                myPlayerTagTrackerScript.TriggerTag(tagged);
            }
        }
    }
}
