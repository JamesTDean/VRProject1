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
        } else
        {
            updateTime(0);
            //updateHealth(0);
        }

        if (inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool press) && !gameStarted)
        {
            if (press)
            {
                StartGame();
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
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                break;
            case 2:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                health_text.gameObject.SetActive(true);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                break;
            case 1:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
                health_text.gameObject.SetActive(true);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
                break;
            case 0:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                timer.gameObject.SetActive(false);
                health_text.gameObject.SetActive(false);
                startText.SetActive(false);
                winnerText.SetActive(false);
                health_text.text = "Lives: " + health;
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
                PlayerTagTracker myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                if(myPlayerTagTrackerScript.lives > 0)
                {
                    playersAlive++;
                }
            }
        }

        myPlayerTagTrackerScript = myPlayer.GetComponent<PlayerTagTracker>();
        bool myPlayerAlive = (myPlayerTagTrackerScript.lives > 0);

        if(playersAlive == 1 && myPlayerAlive)
        {
            isWinner = true;
        }
    }

    //ignore this function. it isnt working properly.
    /*
    public void AssignHost()
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        bool hostInGame = false;

        foreach (var view in photonViews)
        {
            
            if (view.gameObject.name == "NetworkPlayer(Clone)")
            {
                //Debug.Log(view.ViewID);
                PlayerTagTracker myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                if (myPlayerTagTrackerScript.isHost == true)
                {
                    hostInGame = true;
                }
            }
        }

        if (!hostInGame)
        {
            myPlayerTagTrackerScript.SetHost();
        }
    }*/

    private void StartGame()
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
                PlayerTagTracker myPlayerTagTrackerScript = view.gameObject.GetComponent<PlayerTagTracker>();
                myPlayerTagTrackerScript.MoveAtStart(isChaser);
            }
        }
    }

    [PunRPC]
    void StartGameRPC()
    {
        gameStarted = true;
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

    private void updateTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        timer.text = "Time Remaining: " + string.Format("{0:00}:{1:00}", mins, sec);
    }

    
}
