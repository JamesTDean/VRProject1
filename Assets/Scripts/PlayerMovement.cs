using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject world;
    private Transform eyes;
    private Rigidbody rb;
    private float speed = 3f;
    private float forceMagnitude = 25f;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World");
        eyes = transform.parent.Find("CenterEyeAnchor");
        rb = transform.parent.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //move character when key press is detected
        if (Input.GetKey("up"))
        {
            rb.velocity = eyes.forward * speed;
        }

        Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        rb.velocity = speed * axis.y * eyes.forward;

        //VR move character on joystick touch

        //find direction to center of world
        Vector3 toCenterDir = (world.transform.position - gameObject.transform.position).normalized;
        Vector3 toBodyDir = (gameObject.transform.position - world.transform.position).normalized;
        
        //add false gravity so that character stays on surface
        rb.AddForce(forceMagnitude * toCenterDir);

        // update the objects rotation in relation to the planet
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, toBodyDir) * transform.rotation;
        // smooth rotation
        transform.parent.parent.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.fixedDeltaTime);

    }
}
