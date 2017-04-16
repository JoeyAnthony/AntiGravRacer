using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour {

    public Transform camera;
    float Yaw = 0;
    float Pitch;
    public float PitchClamp = 13;

    public float MachineCamDistance = 10;
    public float StdMachineCamHight = 15;
    public float camSpeed = 0.1f;

    Quaternion EnLdocation;

    // Update is called once per frame
    void Start()
    {

    }


    void LateUpdate () {

        ////get yaw and pitch from the controller
        //Yaw += Input.GetAxisRaw("RightAnalogX");
        //Pitch += Input.GetAxisRaw("RightAnalogY");

        ////clamp the camera
        //Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);
        
        //calculate the rotation of the camera. interpolates to target y rotation
        Quaternion rot = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(transform.forward), 1);
        //set y rotation and x rotation
        

        //calculate position of the camera, MUST happen AFTER the rotation or camera will stutter
        Vector3 camPos = transform.position - camera.transform.forward * MachineCamDistance + transform.TransformVector(Vector3.up) * StdMachineCamHight;
        camera.transform.position = Vector3.Lerp(camera.transform.position, camPos, 1);

        camera.transform.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, transform.rotation.eulerAngles.z);


    }
}

