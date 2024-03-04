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
    [SerializeField] private List<Material> myMaterials;
    //public bool isHost;

    private GameObject world;
    private InputData inputData;
    private GameManager myGameManagerScript;
    private PhotonView myView;
    //[SerializeField] private TMP_Text debugText;
    private MeshRenderer myMesh;

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
        myMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            if (chaser)
            {
                gameObject.tag = "chaser";
                ChangeMaterial(0);
            }
            else
            {
                gameObject.tag = "runner";
                ChangeMaterial(1);
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

    private void ChangeMaterial(int index)
    {
        myView.RPC("ChangeMaterialRPC", RpcTarget.All, index);
    }

    [PunRPC]
    void ChangeMaterialRPC(int index)
    {
        if (myView.IsMine)
        {
            var children = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in children)
            {
                if(mesh.gameObject.name != "Hat")
                {
                    mesh.material = myMaterials[index];
                }
            }
        }
    }
}
