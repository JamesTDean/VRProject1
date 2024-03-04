using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class powerupSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] spawnList;

    void Start()
    {
        spawnList = new string[] {"SpeedPowerUp","TeleportationPowerUp","ForcePowerUp"};
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(4f);

        Vector3 spawnDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Vector3 randomSpawnPos = transform.position + (transform.localScale.x / 2 + 0.5f) * spawnDirection.normalized;
        int i = Random.Range(0, 3);
        var powerUpName = spawnList[i];
        var powerUp = PhotonNetwork.Instantiate(powerUpName, transform.position, transform.rotation);
        powerUp.transform.position = randomSpawnPos;

        StartCoroutine(Spawner());
    }
}
