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
    public float maxSpeed_Vector = 200;

    RaycastHit hitInfo;

    void Start()
    {
        RbPlayer = GetComponent<Rigidbody>();

    }
    
    void Update()
    {
        //get yaw and pitch from the controller
        yaw += Input.GetAxisRaw("LeftAnalogX");
        yaw += Input.GetAxisRaw("KeyLeftRight");

        //clamp x rotation
        //Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);

        if (Input.GetAxisRaw("KeyUpDown") > 0 || Input.GetAxisRaw("LeftAnalogY") > 0)
        {
            velocity += Input.GetAxisRaw("KeyUpDown") * acceleration;
            velocity += Input.GetAxisRaw("LeftAnalogY") * acceleration;

            velocity = Mathf.Clamp(velocity, 0, maxVelocity);
            addedVelocity = velocity;
        }
        else
        {
            velocity -= desceleration;
            velocity = Mathf.Clamp(velocity, 0, maxVelocity);
            
            addedVelocity = 0;
        }

        //print(velocity + "___"+ Input.GetAxisRaw("KeyUpDown"));

    }


    private void FixedUpdate()
    {
        Quaternion roadNormal = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        
        //take normal from surface of the road
       
        //rotation vector multiply with normal of the road and slerp
        Vector3 rotationInput = new Vector3(pitch, yaw) * rotationSpeed;
        RbPlayer.rotation = Quaternion.Slerp(RbPlayer.rotation, Quaternion.Euler(hitInfo.normal) * Quaternion.Euler(rotationInput * Time.fixedDeltaTime), 0.5f);

        Vector3 rayDir = RbPlayer.transform.TransformDirection(Vector3.down);
        bool hit = Physics.Raycast(RbPlayer.transform.position, rayDir, out hitInfo, 2.5f);
        if (hit)
        {
            if (hitInfo.transform.tag.Equals("Road"))
            {
                RbPlayer.position = hitInfo.point + RbPlayer.transform.up * 2;
            }
        }
        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + rayDir * 5, Color.green);
        Debug.DrawLine(hitInfo.point, hitInfo.point + RbPlayer.transform.up  * 100, Color.blue);


        //add forward force
        //RbPlayer.AddRelativeForce( Vector3.forward * addedVelocity + roadNormal.eulerAngles);
        //forwardPush
        RbPlayer.AddForce(RbPlayer.transform.forward * addedVelocity);
        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + RbPlayer.velocity * 5, Color.red);

        //store the rigidbody velocity for the gravity
        Vector3 rigidbodyVelocity = RbPlayer.velocity;
        RbPlayer.velocity = Vector3.zero;
        
        //filter the x and y axes so the falling speed is not added to the forward vector
        Vector3 rigidbodyVelocityNoY = new Vector3(rigidbodyVelocity.x, 0, rigidbodyVelocity.z);

        //make new vector for the velocity in the right direction
        //clamp is for maximum velocity on de velocity vector
        Vector3 newVelocityVector = RbPlayer.transform.forward * Mathf.Clamp(rigidbodyVelocityNoY.magnitude, 0, maxSpeed_Vector); 
        newVelocityVector.y = 0;

        if (hit)
        {
            RbPlayer.useGravity = false;
        }
        else
        {
            RbPlayer.useGravity = true;
        }

        //add gravity to the right directions
        Vector3 actualVelocityVector = new Vector3(0, rigidbodyVelocity.y, 0) + newVelocityVector;
        RbPlayer.velocity = actualVelocityVector;

        print(RbPlayer.velocity);
    }
}