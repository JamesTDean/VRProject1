using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class powerupSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] spawnList;
    private GameManager myGameManagerScript;
    private bool isStarted;

    void Start()
    {
        isStarted = false;
        myGameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnList = new string[] {"SpeedPowerUp","TeleportationPowerUp","ForcePowerUp"};
        if (myGameManagerScript.gameStarted)
        {
            StartCoroutine(Spawner());
            isStarted = true;
        }
    }

    void Update()
    {
        if (!isStarted)
        {
            if (myGameManagerScript.gameStarted)
            {
                StartCoroutine(Spawner());
                isStarted = true;
            }
        }   
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(10f);

        Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Vector3 randomSpawnPos = transform.position + (transform.localScale.x / 2 + 0.5f) * spawnDirection.normalized;
        int i = Random.Range(0, 3);
        var powerUpName = spawnList[i];
        var powerUp = PhotonNetwork.Instantiate(powerUpName, transform.position, transform.rotation);
        powerUp.transform.position = randomSpawnPos;

        StartCoroutine(Spawner());
    }
}
