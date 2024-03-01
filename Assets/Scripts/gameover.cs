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
        Time.timeScale = 1;
        
        SceneManager.LoadScene(0);
        myPlayerTagTracker.lives = 3;
        myGameManagerScript.timeRemaining = 5;
    }


}
