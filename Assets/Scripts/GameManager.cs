using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver, heart0, heart1, heart2, heart3;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI health_text;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI chaserText;
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject winnerText;

    public int health;
    public int playerID;
    public float timeRemaining = 120;
    public GameObject myPlayer;

    [SerializeField] private PlayerTagTracker myPlayerTagTrackerScript;
    private InputData inputData;
    private PhotonView myView;
    [SerializeField] private bool gameStarted;
    private bool isWinner;

    // Start is called before the first frame update
    void Start()
    {
        //health = myPlayerTagTrackerScript.lives;
        gameStarted = false;
        heart0.gameObject.SetActive(true);
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        myView = GetComponent<PhotonView>();
        inputData = GameObject.Find("XR Origin (XR Rig)").GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myPlayer != null)
        {
            health = myPlayerTagTrackerScript.lives;
        }

        if(gameStarted == false)
        {
            health = 100;
        }

        if(isWinner == true)
        {
            health = 50;
        }

        if (timeRemaining > 0 && gameStarted)
        {
            timeRemaining -= Time.deltaTime;
            updateTime(timeRemaining); 
        }
        else if(timeRemaining <= 0 && gameStarted)
        {
            ResetTimer();
            var photonViews = FindObjectsOfType<PhotonView>();
            foreach (var view in photonViews)
            {
                if (view.gameObject.name == "NetworkPlayer(Clone)")
                {
                    var playerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                    if (playerScript.chaser)
                    {
                        playerScript.lives -= 1;
                    }
                }    
            }
        }

        if (inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool press) && !gameStarted)
        {
            if (press)
            {
                StartGame();
            }
        }


        string chaserTextString = "Runner";
        if (myPlayer != null)
        {   
            if (myPlayerTagTrackerScript.chaser)
            {
                chaserTextString = "Chaser";
            }
        }
        
        switch (health)
        {
            case 100:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(false);
                health_text.gameObject.SetActive(false);
                chaserText.gameObject.SetActive(false);
                startText.SetActive(true);
                winnerText.SetActive(false);
                break;
            case 50:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(false);
                health_text.gameObject.SetActive(false);
                chaserText.gameObject.SetActive(false);
                startText.SetActive(false);
                winnerText.SetActive(true);
                break;
            case 3:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                health_text.gameObject.SetActive(true);
                chaserText.gameObject.SetActive(true);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                chaserText.text = "You are a: " + chaserTextString;
                break;
            case 2:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                health_text.gameObject.SetActive(true);
                chaserText.gameObject.SetActive(true);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                chaserText.text = "You are a: " + chaserTextString;
                break;
            case 1:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                health_text.gameObject.SetActive(true);
                chaserText.gameObject.SetActive(true);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                chaserText.text = "You are a: " + chaserTextString;
                break;
            case 0:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                //gameOver.gameObject.SetActive(true);
                timer.gameObject.SetActive(false);
                health_text.gameObject.SetActive(false);
                chaserText.gameObject.SetActive(false);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                chaserText.text = "You are a: " + chaserTextString;
                //Time.timeScale = 0;
                //restartButton.gameObject.SetActive(true);
                break;

            default:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                //gameOver.gameObject.SetActive(true);
                //Time.timeScale = 0;
                //restartButton.gameObject.SetActive(true);
                health_text.text = "Lives: " + health;
                break;

        }
    }

    public void AssignPlayer(GameObject player)
    {
        PhotonView playerView = player.GetComponent<PhotonView>();
        myPlayer = player;
        myPlayerTagTrackerScript = myPlayer.GetComponent<PlayerTagTracker>();
    }

    private void StartGame()
    {
        if (PhotonView.Find(playerID).IsMine)
        {
            myView.RPC("StartGameRPC", RpcTarget.All);
            var photonViews = FindObjectsOfType<PhotonView>();
            int playersInScene = 0;

            foreach (var view in photonViews)
            {
                if (view.gameObject.name == "NetworkPlayer(Clone)")
                {
                    playersInScene++;
                }
            }

            int chaserIndex = Random.Range(0, playersInScene);
            int counter = 0;

            Debug.Log(chaserIndex);

            foreach (var view in photonViews)
            {
                if (view.gameObject.name == "NetworkPlayer(Clone)")
                {
                    bool isChaser = false;
                    if (counter == chaserIndex)
                    {
                        isChaser = true;
                    }
                    counter++;
                    PlayerTagTracker anyPlayerTagTracker = view.gameObject.GetComponent<PlayerTagTracker>();
                    anyPlayerTagTracker.MoveAtStart(isChaser);
                }
            }
        }
    }

    [PunRPC]
    void StartGameRPC()
    {
        gameStarted = true;
    }

    public void CheckForWinner()
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        int playersInScene = 0;

        foreach (var view in photonViews)
        {
            if (view.gameObject.name == "NetworkPlayer(Clone)")
            {
                playersInScene++;
            }
        }

        int playersAlive = 0;

        foreach (var view in photonViews)
        {
            if (view.gameObject.name == "NetworkPlayer(Clone)")
            {
                PlayerTagTracker anyPlayTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                if(anyPlayTagTrackerScript.lives > 0)
                {
                    playersAlive++;
                }
            }
        }

        bool myPlayerAlive = (myPlayerTagTrackerScript.lives > 0);

        if(playersAlive == 1 && myPlayerAlive)
        {
            isWinner = true;
        }
    }

    public void updateHealth(int num)
    {
        myView.RPC("updateHealthRPC", RpcTarget.All, num);
    }

    [PunRPC]
    void updateHealthRPC(int num)
    {
        if (myView.IsMine)
        {
            if (myPlayer != null)
            {
                myPlayerTagTrackerScript.lives = num;
            }
        }
        
    }

    public void ResetTimer()
    {
        myView.RPC("ResetTimerRPC", RpcTarget.All);
    }

    [PunRPC]
    void ResetTimerRPC()
    {
        timeRemaining = 120f;
        updateTime(timeRemaining);
    }

    private void updateTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        timer.text = "Time Remaining: " + string.Format("{0:00}:{1:00}", mins, sec);
    }

    
}
