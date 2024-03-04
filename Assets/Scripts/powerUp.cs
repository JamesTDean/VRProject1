using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class powerUp : MonoBehaviour
{
    [SerializeField] private float _speedIncreaseAmount = 5;
    [SerializeField] private float _powerupDuration = 5;
    [SerializeField] private GameObject _artToDisable = null;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        
        GameObject playerObject = other.gameObject;
        Debug.Log(playerObject.name);
        

        if (playerObject != null)
        {
            Transform parentTransform = playerObject.transform.parent;
            GameObject parentObject = parentTransform.gameObject;
            Debug.Log(parentObject.name);
            Debug.Log(parentObject == null);
            MovementManager move = parentObject.GetComponent<MovementManager>();
            //powerup sequence
            if (!move.isSpeedup)
            {
                StartCoroutine(powerSequence(move));
            } 
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
        move.isSpeedup = true;
        move.setSpeed(_speedIncreaseAmount); 
    }

    private void DeactivatePowerUp(MovementManager move)
    {
        move.isSpeedup = false;
        move.setSpeed(-_speedIncreaseAmount); 
    }  
}
