using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject world;
    private Transform cameraRig;
    private Rigidbody rb;
    private float speed = 3f;
    private float forceMagnitude = 5f;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World");
        cameraRig = transform.Find("OVRCameraRig");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //move character when key press is detected
        if (Input.GetKey("up"))
        {
            rb.velocity = cameraRig.forward * speed;
        }

        //VR move character on joystick touch

        //find direction to center of world
        Vector3 toCenterDir = (world.transform.position - gameObject.transform.position).normalized;
        Vector3 toBodyDir = (gameObject.transform.position - world.transform.position).normalized;
        
        //add false gravity so that character stays on surface
        rb.AddForce(forceMagnitude * toCenterDir);

        // update the objects rotation in relation to the planet
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, toBodyDir) * transform.rotation;
        // smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.fixedDeltaTime);

    }
}
