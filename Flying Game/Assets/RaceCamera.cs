using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour {

    public Transform Target;
    float Yaw = 0;
    float Pitch;
    public float PitchClamp = 13;

    public float MachineCamDistance = 10;
    public float StdMachineCamHight = 15;
    public float camSpeed = 0.1f;

    Quaternion EnLdocation;
	
	// Update is called once per frame
	void LateUpdate () {

        ////get yaw and pitch from the controller
        //Yaw += Input.GetAxisRaw("RightAnalogX");
        //Pitch += Input.GetAxisRaw("RightAnalogY");

        ////clamp the camera
        //Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);


        //calculate the rotation of the camera. interpolates to target y rotation
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.forward), Time.fixedDeltaTime / camSpeed);


        //set y rotation and x rotation
        transform.eulerAngles = new Vector3(StdMachineCamHight, rot.eulerAngles.y, 0);

        //calculate position of the camera, MUST happen AFTER the rotation or camera will stutter
        Vector3 camPos = Target.position - transform.forward * MachineCamDistance; //good vector
        transform.position = camPos;


    }
}

