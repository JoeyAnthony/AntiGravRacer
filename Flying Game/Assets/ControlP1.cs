using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlP1 : MonoBehaviour
{
    private Rigidbody RbPlayer;
    private float yaw;
    private float pitch;
    private float velocity;
    private float addedVelocity;
    
    public float pitchClamp = 20;
    public float rotationSpeed = 100;

    public float acceleration = 10;
    public float desceleration = 20;
    public float maxVelocity = 3000;

    void Start()
    {
        RbPlayer = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        //get yaw and pitch from the controller
        yaw += Input.GetAxisRaw("LeftAnalogX");
        yaw += Input.GetAxisRaw("KeyLeftRight");

        //clamp x rotation
        //Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);

        if (Input.GetAxisRaw("KeyUpDown") > 0 || Input.GetAxisRaw("LeftAnalogZ") > 0)
        {
            velocity += Input.GetAxisRaw("KeyUpDown") * acceleration;
            velocity += Input.GetAxisRaw("LeftAnalogZ") * acceleration;

            velocity = Mathf.Clamp(velocity, 0, maxVelocity);
            addedVelocity = velocity;
        }
        else
        {
            velocity -= desceleration;
            velocity = Mathf.Clamp(velocity, 0, maxVelocity);

            addedVelocity = 0;
        }

        print(velocity + "___"+ Input.GetAxisRaw("KeyUpDown"));

    }

    private void FixedUpdate()
    {
        //rotation vector
        Vector3 a = new Vector3(pitch, yaw) * rotationSpeed;
        RbPlayer.rotation = (Quaternion.Euler(a * Time.fixedDeltaTime));
        RbPlayer.AddRelativeForce(Vector3.forward * addedVelocity);

        //store the rigidbody velocity for the gravity
        Vector3 rigidbodyVelocity = RbPlayer.velocity;
        RbPlayer.velocity = Vector3.zero;

        

        //make new vector for the velocity in the right direction
        Vector3 rigidbodyVelocityNoY = new Vector3(rigidbodyVelocity.x, 0, rigidbodyVelocity.z);
        Vector3 newVelocityVector = transform.forward * rigidbodyVelocityNoY.magnitude;
        newVelocityVector.y = 0;

        //add gravity to the direction
        Vector3 actualVelocityVector = new Vector3(0, rigidbodyVelocity.y, 0) + newVelocityVector;
        RbPlayer.velocity = actualVelocityVector;
    }
}
