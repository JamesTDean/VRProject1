using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using TMPro;

public class PlayerTagTracker : MonoBehaviourPunCallbacks
{
    public bool chaser;
    public int lives;
    //public bool isHost;

    private GameObject world;
    private InputData inputData;
    private GameManager myGameManagerScript;
    private PhotonView myView;
    [SerializeField] private TMP_Text debugText;

    // Start is called before the first frame update
    void Awake()
    {
        chaser = false;
        lives = 3;
        world = GameObject.Find("World");
        inputData = GameObject.Find("XR Origin (XR Rig)").GetComponent<InputData>();
        myGameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        myView = GetComponent<PhotonView>();
        if (myView.IsMine)
        {
            myGameManagerScript.playerID = myView.ViewID;
            myGameManagerScript.AssignPlayer(gameObject);
            //myGameManagerScript.AssignHost();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            if (chaser)
            {
                gameObject.tag = "chaser";
            }
            else
            {
                gameObject.tag = "runner";
            }

            /*
            //this is only for debugging
            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool press))
            {
                if (press)
                {
                    chaser = true;
                }
            }
            debugText.SetText(chaser.ToString());*/
        }
    }

    public void TriggerTag(bool tagged)
    {
        myView.RPC("TriggerTagRPC", RpcTarget.All, tagged);
    }

    [PunRPC]
    void TriggerTagRPC(bool tagged)
    {
        if (myView.IsMine)
        {
            if (tagged)
            {
                //teleport new chaser away
                Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                transform.position = world.transform.position + (world.transform.localScale.x / 2 + 3) * spawnDirection.normalized;

                //make new person chaser
                chaser = true;
                lives -= 1;
            }
            else
            {
                chaser = false;
            }

            myGameManagerScript.CheckForWinner();
        }
    }

    public void MoveAtStart(bool isChaser)
    {
        myView.RPC("MoveAtStartRPC", RpcTarget.All, isChaser);
    }

    [PunRPC]
    void MoveAtStartRPC(bool isChaser)
    {
        Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        transform.position = world.transform.position + (world.transform.localScale.x / 2 + 3) * spawnDirection.normalized;
        if (isChaser)
        {
            chaser = true;
        }
    }

    //this function isn't working when future players aren't in the scene yet. not sure why
    public void SetHost()
    {
        myView.RPC("SetHostRPC", RpcTarget.All);
    }

    [PunRPC]
    void SetHostRPC()
    {
        if (myView.IsMine)
        {
            //isHost = true;
        }
    }
}
