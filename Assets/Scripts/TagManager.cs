using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TagManager : MonoBehaviourPunCallbacks
{
    private PlayerTagTracker myPlayerTagTrackerScript;
    [SerializeField] private GameManager myGameManagerScript;

    public void TagOccured(int newChaserID)
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        GameObject newChaser = PhotonView.Find(newChaserID).gameObject;
        myPlayerTagTrackerScript = newChaser.GetComponent<PlayerTagTracker>();

        //check if new chaser is out of lives
        if(myPlayerTagTrackerScript.lives == 0)
        {
            List<GameObject> playerObjects = new List<GameObject>();
            int playersAlive = 0;

            foreach (var view in photonViews)
            {
                if (view.gameObject.name == "NetworkPlayer(Clone)")
                {
                    PlayerTagTracker myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                    if (myPlayerTagTrackerScript.lives > 0)
                    {
                        playerObjects.Add(view.gameObject);
                        playersAlive++;
                    }
                }
            }

            int newAliveChaserIndex = Random.Range(0, playersAlive);

            GameObject newAliveChaser = playerObjects[newAliveChaserIndex];
            newChaserID = newAliveChaser.GetComponent<PhotonView>().ViewID;
            
        }

        foreach (var view in photonViews)
        {
            Debug.Log(view.gameObject.name); 
            if (view.gameObject.name == "NetworkPlayer(Clone)")
            {
                bool tagged = (view.ViewID == newChaserID);
                Debug.Log(tagged);
                myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                myPlayerTagTrackerScript.TriggerTag(tagged);
            }
        }

        myGameManagerScript.ResetTimer();
    }
}
