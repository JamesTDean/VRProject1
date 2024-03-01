using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver, heart0, heart1, heart2, heart3;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI health_text;
    [SerializeField] private TextMeshProUGUI timer;

    public int health;
    public float timeRemaining = 120;
    public int playerID;

    private PhotonView playersView;

    // Start is called before the first frame update
    void Start()
    {
        health = 3;
        heart0.gameObject.SetActive(true);
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            updateTime(timeRemaining); 
        } else
        {
            updateTime(0);
            health = 0;
        }

        switch(health)
        {
            //case 4:
            //    heart0.gameObject.SetActive(true);
            //    heart1.gameObject.SetActive(true);
            //    heart2.gameObject.SetActive(true);
            //    heart3.gameObject.SetActive(true);
                //break;
            case 3:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                //heart3.gameObject.SetActive(true);
                health_text.text = "Lives: " + health;
                break;
            case 2:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                //heart3.gameObject.SetActive(false);
                health_text.text = "Lives: " + health;
                break;
            case 1:
                heart0.gameObject.SetActive(true);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                //heart3.gameObject.SetActive(false);
                health_text.text = "Lives: " + health;
                break;
            case 0:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                //heart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                health_text.text = "Lives: " + health;
                Time.timeScale = 0;
                restartButton.gameObject.SetActive(true);
                break;

            default:
                heart0.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                Time.timeScale = 0;
                restartButton.gameObject.SetActive(true);
                health_text.text = "Lives: " + health;
                break;

        }
    }

    public void updateHealth(int num)
    {
        health -= num;
        //health_text.SetText("Lives: " + health.ToString());
    }

    public void updateTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        timer.text = "Time Remaining: " + string.Format("{0:00}:{1:00}", mins, sec);
    }
}
