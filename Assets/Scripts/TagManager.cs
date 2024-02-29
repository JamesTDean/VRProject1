using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TagManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private PlayerTagTracker myPlayerTagTrackerScript;
    private PhotonView myView;

    // Start is called before the first frame update
    void Awake()
    {
        /*
        var photonViews = FindObjectsOfType<PhotonView>();
        foreach (var view in photonViews)
        {
            Debug.Log(view);
            Debug.Log(view.gameObject);
            players.Add(view.gameObject);
        }*/
        myView = GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var photonViews = FindObjectsOfType<PhotonView>();

        //remove all players if more players in list than scene
        if (photonViews.Length < players.Count)
        {
            players = new List<GameObject>();
        }

        //add new players to list
        foreach (var view in photonViews)
        {
            var player = view.gameObject;
            if (!players.Contains(player))
            {
                players.Add(player);
                //Debug.Log("Added " + player.name + " to the list.");
            }
            else
            {
                //Debug.Log(player.name + " is already in the list.");
            }
        }*/
    }

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
