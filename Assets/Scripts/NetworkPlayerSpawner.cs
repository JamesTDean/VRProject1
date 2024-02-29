using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerObject;
    private TagManager myTagManagerScript;

    void Start()
    {
        myTagManagerScript = GameObject.Find("TagManager").GetComponent<TagManager>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayerObject = PhotonNetwork.Instantiate("NetworkPlayer", transform.position, transform.rotation);
        //myTagManagerScript.AddPlayer(spawnedPlayerObject);
    }

    public override void OnLeftRoom()
    {
        //myTagManagerScript.RemovePlayer(spawnedPlayerObject);
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerObject);
    }
}
