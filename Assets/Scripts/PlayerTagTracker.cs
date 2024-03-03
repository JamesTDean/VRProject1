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
    private InputData inputData;
    private GameManager myGameManagerScript;
    private PhotonView myView;
    [SerializeField] private TMP_Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        chaser = false;
        lives = 3;
        inputData = GameObject.Find("XR Origin (XR Rig)").GetComponent<InputData>();
        myGameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        myView = GetComponent<PhotonView>();
        myGameManagerScript.playerID = myView.ViewID;
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

            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool press))
            {
                if (press)
                {
                    chaser = true;
                }
            }
            //debugText.SetText(chaser.ToString());
        }
    }

    public void TriggerTag(bool tagged)
    {
        myView.RPC("TriggerTagRPC", RpcTarget.All, tagged);
    }

    [PunRPC]
    void TriggerTagRPC(bool tagged)
    {
        if (tagged)
        {
            //teleport new chaser away
            transform.position = new Vector3(0, 3f, 0);

            //make new person chaser
            chaser = true;
            lives -= 1;
        }
        else
        {
            chaser = false;
        }
    }
}
