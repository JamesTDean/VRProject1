using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class gameover : MonoBehaviour
{
    private GameManager myGameManagerScript;
    private PlayerTagTracker myPlayerTagTracker;
    private PhotonView myView;

    void Start()
    {
        myGameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject myPlayer = PhotonView.Find(myGameManagerScript.playerID).gameObject;
        myPlayerTagTracker = myPlayer.GetComponent<PlayerTagTracker>();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        myGameManagerScript.updateHealth(3);
        myGameManagerScript.timeRemaining = 5;
    }


}
