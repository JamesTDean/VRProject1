using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class repulsiveForce : MonoBehaviour
{
    [SerializeField] private GameObject _artToDisable = null;
    [SerializeField] private float _powerupDuration = 5;
    private Collider _collider;
    private float pullForce = 0.5f;
    private PhotonView myView;
    GameObject playerObject;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        myView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        //Vector3 posRunner = playerShip.myBody.position;

        

        if (playerObject != null )
        {

            Transform parentTransform = playerObject.transform.parent;
            GameObject parentObject = parentTransform.gameObject; // networkplayer
            Rigidbody thisRB = playerObject.GetComponent<Rigidbody>();
            Vector3 pos = thisRB.transform.position;
            if (parentObject.tag == "runner")
            {
                myView.RPC("ActivatePowerUpRunner", RpcTarget.All, playerObject, pos);
                //ActivatePowerUpRunner(playerObject, pos);
            }

            if (parentObject.tag == "chaser")
            {
                myView.RPC("ActivatePowerUpChaser", RpcTarget.All, playerObject, pos);
                //Rigidbody thisRB = playerObject.GetComponent<Rigidbody>();
                //Vector3 pos = thisRB.transform.position;
                //ActivatePowerUpChaser(playerObject, pos);
            }





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
            myView.RPC("ActivatePowerUpRunner", RpcTarget.All, playerObject, pos);
            //ActivatePowerUpRunner(playerObject, pos);
            yield return new WaitForSeconds(_powerupDuration);
            //DeactivatePowerUpRunner(parentObject);
        }
        if (parentObject.tag == "chaser")
        {
            myView.RPC("ActivatePowerUpChaser", RpcTarget.All, playerObject, pos);
            //ActivatePowerUpChaser(playerObject, pos);
            yield return new WaitForSeconds(_powerupDuration);
            //DeactivatePowerUp(player);
        }

        Destroy(gameObject);
    }

    [PunRPC]
    void ActivatePowerUpRunner(GameObject runner, Vector3 pos)
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

    [PunRPC]
    void ActivatePowerUpChaser(GameObject chaser, Vector3 pos)
    {
        GameObject[] runnerObjects = GameObject.FindGameObjectsWithTag("runner");

        Rigidbody chaserRB = chaser.GetComponent<Rigidbody>();

        foreach (GameObject runner in runnerObjects)
        {

            GameObject runnerBody = runner.transform.Find("Body").gameObject;
            MovementManager runnerMove = runner.GetComponent<MovementManager>();
            //chaserMove.isRepulsive = true;
            Rigidbody runnerRB = runnerBody.GetComponent<Rigidbody>();
            Vector3 direction = pos - runnerBody.transform.position;
            runnerRB.AddForce(direction.normalized * pullForce, ForceMode.Acceleration);

        }

    }
}
