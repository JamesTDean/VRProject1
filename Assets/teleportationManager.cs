using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class teleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private bool _isActive;
    // Start is called before the first frame update
    InputAction  _trigger;
    void Start()
    {
        //rayInteractor.enabled = false;


        var activate = actionAsset.FindActionMap("XRI RightHand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI RightHand Locomotion").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        //_trigger = actionAsset.FindActionMap("XRI RightHand Locomotion").FindAction("Move");
        //_trigger.Enable();

        var select = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select");
        select.Enable();
        select.performed += selectActivate;


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("updateing");
        if (!_isActive)
        {
            Debug.Log("disabled!");
            return;
        }

        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }

        //if (_trigger.triggered)
        //{
        //    return;
        //}


        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,

        };
        provider.QueueTeleportRequest(request);


    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        _isActive = true;
        rayInteractor.enabled = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        _isActive = false;
        rayInteractor.enabled = false;
    }


    private void selectActivate(InputAction.CallbackContext context)
    {
        Debug.Log("function running!");
        _isActive = false;
        rayInteractor.enabled = false;
    }
}
