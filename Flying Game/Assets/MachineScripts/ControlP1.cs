﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlP1 : MonoBehaviour
{
    private Rigidbody RbPlayer;

    private float yaw;
    private float pitch = 0;
    private float velocity;
    
    public float pitchClamp = 20;
    public float rotationSpeed = 100;

    public float acceleration = 1;
    public float accelerationDevider = 1;
    public float desceleration;
    public float maxVelocity = 3000;
    public float maxSpeed_Vector = 200;
    private float addedVelocity;


    private float fallingSpeed;
    public float forw;
    public float fall;

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
            velocity += (Input.GetAxisRaw("KeyUpDown") * acceleration) / accelerationDevider;
            velocity += (Input.GetAxisRaw("LeftAnalogY") * acceleration) / accelerationDevider;

            velocity = Mathf.Clamp(velocity, 0, maxVelocity);

            addedVelocity += velocity;
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
        RbPlayer.rotation = Quaternion.Slerp(RbPlayer.rotation, roadNormal * Quaternion.Euler(rotationInput * Time.fixedDeltaTime), 0.5f);

        //raycast to detect the road
        Vector3 rayDir = RbPlayer.transform.TransformDirection(Vector3.down);
        bool hit = Physics.Raycast(RbPlayer.transform.position, rayDir, out hitInfo, 2.5f);
        if (hit)
        {
            //set object above the road
            if (hitInfo.transform.tag.Equals("Road"))
            {
                RbPlayer.position = hitInfo.point + RbPlayer.transform.up * 2;
            }
        }
        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + rayDir * 5, Color.green);
        Debug.DrawLine(hitInfo.point, hitInfo.point + RbPlayer.transform.up * 100, Color.blue);


        RbPlayer.AddRelativeForce(Vector3.forward * addedVelocity, ForceMode.Acceleration);

        Vector3 vel = RbPlayer.velocity;
        float y = vel.y;


        Vector3 endDir = RbPlayer.transform.forward * vel.magnitude - vel;
        if (hit)
        {
            RbPlayer.useGravity = false;
        }
        else
        {
            RbPlayer.useGravity = true;
        }

        RbPlayer.velocity += endDir;
        



        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + RbPlayer.velocity * 5, Color.red);

        print("velocity: " + RbPlayer.velocity);
        print("endDir= " + endDir);
        print("dir: " + RbPlayer.transform.forward);
    }

    public float getForce(float force)
    {
        return force / (50 * RbPlayer.mass);
    }
}