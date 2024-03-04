using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using TMPro;

public class MovementManager : MonoBehaviour
{
    public int teleportation_credit;
    private bool triggerPressedLastFrame = false;
    [SerializeField] private TMP_Text teleportationCredit_text;
    public bool isRepulsive;
    public bool isSpeedup;

    private PhotonView myView;
    private InputData inputData;

    private Transform myXRRig;
    private Transform mainCamera;
    private GameObject body;
    private Rigidbody bodyRB;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject uiMenu;
    [SerializeField] private GameObject leftController;
    private GameObject rightController;

    private float xInput;
    private float yInput;
    private float movementSpeed = 5f;

    [SerializeField] private TMP_Text debugText;

    private GameObject world;
    private float forceMagnitude = 25f;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();

        body = transform.Find("Body").gameObject;
        leftHand = transform.Find("LeftHand").gameObject;
        rightHand = transform.Find("RightHand").gameObject;
        bodyRB = body.GetComponent<Rigidbody>();

        leftController = GameObject.Find("Left Controller");
        rightController = GameObject.Find("Right Controller");
        uiMenu = GameObject.Find("UI");

        GameObject myXROrigin = GameObject.Find("XR Origin (XR Rig)");
        myXRRig = myXROrigin.transform;
        inputData = myXROrigin.GetComponent<InputData>();
        mainCamera = myXRRig.Find("Camera Offset").Find("Main Camera");

        world = GameObject.Find("World");

        teleportationCredit_text = GameObject.Find("GameManager").GetComponent<GameManager>().teleportCreditText.GetComponent<TMP_Text>();
        teleportation_credit = 0;
        isSpeedup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            //Vector3 distanceToCamera = new Vector3(mainCamera.localPosition.x, 0, mainCamera.localPosition.z);
            Vector3 distanceToCamera = Vector3.zero;
            myXRRig.transform.parent.position = body.transform.position;
            myXRRig.transform.localPosition = -mainCamera.localPosition + new Vector3(0,1.2f,0);

            myXRRig.transform.parent.rotation = body.transform.rotation;
            leftHand.transform.position = leftController.transform.position;
            rightHand.transform.position = rightController.transform.position;
            uiMenu.transform.position = leftController.transform.position;

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

            //teleportationCredit_text.text = "my tele credit: " + teleportation_credit;
            teleportationCredit_text.SetText("Teleport Credit: " + teleportation_credit);
            //debugText.SetText("tag: " + body.transform.parent.tag);
            //Debug.Log("tag: " + body.transform.parent.tag);
        }
    }

    private void FixedUpdate()
    {
        //find direction to center of world
        Vector3 toCenterDir = (world.transform.position - body.transform.position).normalized;
        Vector3 toBodyDir = (body.transform.position - world.transform.position).normalized;

        Vector3 forward = mainCamera.forward;
        Vector3 sideways = mainCamera.right;
        //bodyRB.AddForce(yInput * forward.normalized * movementSpeed);
        //bodyRB.AddForce(xInput * sideways.normalized * movementSpeed);

        Vector2 inputDir = new Vector2(xInput, yInput);
        if (inputDir != new Vector2(0, 0))
        {
            Vector3 lookDirection = (forward - toBodyDir * Vector3.Dot(forward, toBodyDir)).normalized;
            Quaternion rotation = Quaternion.AngleAxis(Vector2.Angle(new Vector2(0, 1f), inputDir.normalized), toBodyDir);
            if (xInput < 0)
            {
                rotation = Quaternion.AngleAxis(-Vector2.Angle(new Vector2(0, 1f), inputDir.normalized), toBodyDir);
            }
            Vector3 velocityDirection =  rotation * lookDirection;
            float pushMagnitude = Vector2.SqrMagnitude(inputDir);
            Vector3 verticalVelocity = Vector3.Dot(bodyRB.velocity, toCenterDir) * toCenterDir.normalized;
            bodyRB.velocity = movementSpeed * pushMagnitude * velocityDirection + verticalVelocity;
        }

        if (teleportation_credit > 0)
        {
            Vector3 lookDirection = (forward - toBodyDir * Vector3.Dot(forward, toBodyDir)).normalized;


            if (inputData.rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                // Check if the trigger is pulled (you can adjust the threshold as needed)
                if (triggerValue > 0.5f && !triggerPressedLastFrame)
                {
                    //Vector3 forward = mainCamera.forward;

                    bodyRB.transform.position = bodyRB.transform.position + lookDirection * 5;

                    teleportation_credit -= 1;


                }
                triggerPressedLastFrame = triggerValue > 0.5f;
            }
        }

        //debugText.SetText(string.Concat(myXRRig.parent.position.ToString(), "\n", myXRRig.localPosition.ToString(), "\n", mainCamera.localPosition.ToString()));
        //debugText.gameObject.transform.LookAt(mainCamera);

        //add false gravity so that character stays on surface
        bodyRB.AddForce(forceMagnitude * toCenterDir);

        // update the objects rotation in relation to the planet
        Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, toBodyDir) * body.transform.rotation;
        // smooth rotation
        body.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.fixedDeltaTime);
        uiMenu.transform.LookAt(mainCamera, mainCamera.transform.up);
    }

    public void setSpeed(float num)
    {
        movementSpeed += num;
    }
}