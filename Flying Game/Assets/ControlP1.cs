using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlP1 : MonoBehaviour
{
    
    Rigidbody RbPlayer;
    float Yaw;
    float Pitch;
    public float PitchClamp = 20;
    public float RotationSpeed = 5;

    void Start()
    {
        RbPlayer = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        //get yaw and pitch from the controller
        Yaw += Input.GetAxisRaw("LeftAnalogX");
        //Pitch += Input.GetAxisRaw("LeftAnalogZ");
        
        //clamp the camera
        Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);

    }

    private void FixedUpdate()
    {
        Vector3 a = new Vector3(Pitch, Yaw) * RotationSpeed *Time.deltaTime;
        RbPlayer.rotation = (Quaternion.Euler(a));
    }

}
