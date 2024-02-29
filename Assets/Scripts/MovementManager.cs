using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class MovementManager : MonoBehaviour
{
    private PhotonView myView;
    private InputData inputData;

    private Transform myXRRig;
    private GameObject body;
    private Rigidbody bodyRB;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject leftController;
    private GameObject rightController;

    private float xInput;
    private float yInput;
    private float movementSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();

        body = transform.Find("Body").gameObject;
        leftHand = transform.Find("LeftHand").gameObject;
        rightHand = transform.Find("RightHand").gameObject;
        bodyRB = body.GetComponent<Rigidbody>();

        GameObject myXROrigin = GameObject.Find("XR Origin (XR Rig)");
        myXRRig = myXROrigin.transform;
        inputData = myXROrigin.GetComponent<InputData>();


    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            myXRRig.position = body.transform.position;

            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                xInput = movement.x;
                yInput = movement.y;
            }
        }
        if (myView.IsMine)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        bodyRB.AddForce(xInput * movementSpeed, 0, yInput * movementSpeed);
    }
}
