using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementManagerOld : MonoBehaviour
{
    private PhotonView myView;

    private GameObject myBody;
    private Rigidbody myRB;

    private float xInput;
    private float yInput;
    private float movementSpeed = 10f;

    private MeshRenderer myMesh;
    private int index = 0;
    [SerializeField] List<Material> myMaterials;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();

        myBody = transform.GetChild(0).gameObject;
        myRB = myBody.GetComponent<Rigidbody>();

        myMesh = myBody.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myView.RPC("changeMaterial", RpcTarget.All);
        }
    }

    private void FixedUpdate()
    {
        myRB.AddForce(xInput * movementSpeed, 0, yInput * movementSpeed);
    }

    [PunRPC]
    void changeMaterial()
    {
        if (myView.IsMine)
        {
            if (index == myMaterials.Count)
            {
                index = 0;
            }

            myMesh.material = myMaterials[index];
            index++;
        }
    }
}
