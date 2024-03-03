using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repulsiveForce : MonoBehaviour
{
    [SerializeField] private GameObject _artToDisable = null;
    [SerializeField] private float _powerupDuration = 5;
    private Collider _collider;
    

    private float pullForce = 0.5f;

    

    GameObject playerObject;




    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }


    private void Update()
    {
        //Vector3 posRunner = playerShip.myBody.position;
        Rigidbody thisRB = playerObject.GetComponent<Rigidbody>();
        Transform parentTransform = playerObject.transform.parent;
        GameObject parentObject = parentTransform.gameObject; // networkplayer

        if (playerObject != null && parentObject.tag == "runner")
        {
            
            Vector3 pos = thisRB.transform.position;
            ActivatePowerUpRunner(playerObject, pos);

        }
        if (playerObject != null && parentObject.tag == "chaser")
        {
            Vector3 pos = thisRB.transform.position;
            ActivatePowerUpChaser(playerObject, pos);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        playerObject = other.gameObject;
        Debug.Log(playerObject.name);


        if (playerObject != null)
        {
            Debug.Log("not null!");
            Transform parentTransform = playerObject.transform.parent;
            GameObject parentObject = parentTransform.gameObject;
            Debug.Log(parentObject.name);
            Debug.Log(parentObject == null);
            //MovementManager move = parentObject.GetComponent<MovementManager>();
            //powerup sequence
            Rigidbody thisRB = playerObject.GetComponent<Rigidbody>();
            Vector3 pos = thisRB.transform.position;
            StartCoroutine(powerSequence(playerObject, pos));
        }




    }

    public IEnumerator powerSequence(GameObject playerObject, Vector3 pos) // body
    {
        
        _collider.enabled = false;
        _artToDisable.SetActive(false);
        Transform parentTransform = playerObject.transform.parent;
        GameObject parentObject = parentTransform.gameObject; // networkplayer
        if (parentObject.tag == "runner")
        {
            ActivatePowerUpRunner(playerObject, pos);
            yield return new WaitForSeconds(_powerupDuration);
            //DeactivatePowerUpRunner(parentObject);
        }
        if (parentObject.tag == "chaser")
        {
            ActivatePowerUpChaser(playerObject, pos);
            yield return new WaitForSeconds(_powerupDuration);
            //DeactivatePowerUp(player);
        }



        Destroy(gameObject);
    }

    private void ActivatePowerUpRunner(GameObject runner, Vector3 pos)
    {
        GameObject[] chaserObjects = GameObject.FindGameObjectsWithTag("chaser");
        
        Rigidbody runnerRB = runner.GetComponent<Rigidbody>();

        foreach (GameObject chaser in chaserObjects)
        {
            
            GameObject chaserBody = chaser.transform.Find("Body").gameObject;
            MovementManager chaserMove = chaser.GetComponent<MovementManager>();
            //chaserMove.isRepulsive = true;
            Rigidbody chaserRB = chaserBody.GetComponent<Rigidbody>();
            Vector3 direction = chaserBody.transform.position - pos;
            chaserRB.AddForce(direction.normalized * pullForce, ForceMode.Acceleration);

        }

    }


    private void ActivatePowerUpChaser(GameObject chaser, Vector3 pos)
    {
        GameObject[] runnerObjects = GameObject.FindGameObjectsWithTag("runner");

        Rigidbody chaserRB = chaser.GetComponent<Rigidbody>();

        foreach (GameObject runner in runnerObjects)
        {

            GameObject runnerBody = chaser.transform.Find("Body").gameObject;
            MovementManager runnerMove = runner.GetComponent<MovementManager>();
            //chaserMove.isRepulsive = true;
            Rigidbody runnerRB = runnerBody.GetComponent<Rigidbody>();
            Vector3 direction = pos - runnerBody.transform.position;
            runnerRB.AddForce(direction.normalized * pullForce, ForceMode.Acceleration);

        }

    }

    //private void DeactivatePowerUpRunner(GameObject playerObject)
    //{
    //    GameObject[] chaserObjects = GameObject.FindGameObjectsWithTag(test);
    //    foreach(GameObject chaser in chaserObjects) {
    //        MovementManager chaserMove = chaser.GetComponent<MovementManager>();
    //        //chaserMove.isRepulsive = false;
    //    }

    //}
}
