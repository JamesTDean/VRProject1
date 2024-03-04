using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporation : MonoBehaviour
{
    
    [SerializeField] private float _powerupDuration = 1;
    [SerializeField] private GameObject _artToDisable = null;


    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();


    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerObject = other.gameObject;
        Debug.Log("triggered");


        if (playerObject != null)
        {
            Debug.Log("player not null");
            Transform parentTransform = playerObject.transform.parent;
            
            
            GameObject parentObject = parentTransform.gameObject;
            Debug.Log(parentObject.name);
            MovementManager move = parentObject.GetComponent<MovementManager>();
                
            StartCoroutine(powerSequence(move));
            
            
        }




    }

    // powerup for runner
    public IEnumerator powerSequence(MovementManager move)
    {
        Debug.Log("changing");
        _collider.enabled = false;
        _artToDisable.SetActive(false);
        ActivatePowerUp(move);

        yield return new WaitForSeconds(_powerupDuration);
        DeactivatePowerUp(move);

        Destroy(gameObject);
    }

    private void ActivatePowerUp(MovementManager move)
    {
        move.teleportation_credit += 1;
    }

    private void DeactivatePowerUp(MovementManager move)
    {
        Debug.Log("dummy function");
    }

   
}
