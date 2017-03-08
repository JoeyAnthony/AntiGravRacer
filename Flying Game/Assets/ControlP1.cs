using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlP1 : MonoBehaviour
{
    
    Rigidbody RbPlayer;
    public float PitchClamp = 20;
    public float RotationSpeed = 5;
    public float Acceleration = 10;
    public float speed;

    private float Yaw;
    private float Pitch;
    private float Velocity;
    
    void Start()
    {
        RbPlayer = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        //get yaw and pitch from the controller
        Yaw += Input.GetAxisRaw("LeftAnalogX");
        Yaw += Input.GetAxisRaw("KeyLeftRight");

        //clamp the camera
        Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);

        if (Input.GetAxisRaw("KeyUpDown") > 0 || Input.GetAxisRaw("LeftAnalogZ") > 0)
        {
            Velocity += Input.GetAxisRaw("KeyUpDown") * speed;
            Velocity += Input.GetAxisRaw("LeftAnalogZ") * speed;
            
        }
        //else
        //{
        //    Velocity -= speed * 2;
        //}

            
        //Velocity = Mathf.Clamp(Velocity, 0, 1000);

        print(Velocity +"___"+ Input.GetAxisRaw("KeyUpDown"));

    }

    private void FixedUpdate()
    {
        Vector3 a = new Vector3(Pitch, Yaw) * RotationSpeed * Time.smoothDeltaTime;
        RbPlayer.rotation = (Quaternion.Euler(a));
        
        RbPlayer.position = RbPlayer.transform.forward * Velocity * Time.fixedDeltaTime;
    }

}
