using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public Vector3 platformPos;
    public Vector3 platformSize;
    private Transform sphereTransform;
    private float radius = 11f;

    GameObject[] spawnList;
    void Start()
    {
        platformPos = transform.position;
        platformSize = transform.localScale;
        sphereTransform = this.transform;
        spawnList = new GameObject[] {obj1, obj2, obj1, obj3, obj2 };
        StartCoroutine(Spawner());
        
    }

    IEnumerator Spawner()
    {
        
        yield return new WaitForSeconds(4f);

        float randomTheta = Random.Range(0f, Mathf.PI * 2);
        float randomPhi = Mathf.Acos(2f * Random.value - 1f);

        float randomX = sphereTransform.position.x + radius * Mathf.Sin(randomPhi) * Mathf.Cos(randomTheta);
        float randomY = sphereTransform.position.y + radius * Mathf.Sin(randomPhi) * Mathf.Sin(randomTheta);
        float randomZ = sphereTransform.position.z + radius * Mathf.Cos(randomPhi);

        Vector3 randomSpawnPos = new Vector3(randomX, randomY, randomZ);
        //Instantiate(obj, randomSpawnPos, Quaternion.identity);
        int i = Random.Range(0, 5);
        Instantiate(spawnList[i], randomSpawnPos, Quaternion.identity);

        StartCoroutine(Spawner());
    }
}
