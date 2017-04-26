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
    private float refVelY = 0f;
    private float refVelX = 0f;
    private float refVelZ = 0f;


    void LateUpdate () {

        //calculate the rotation of the camera. interpolates to target y rotation
        // Quaternion rot = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(transform.forward), 0.5f);
        //set y rotation and x rotation
        float wantedX = transform.rotation.eulerAngles.x;
        float wantedY = transform.rotation.eulerAngles.y;
        float wantedZ = transform.rotation.eulerAngles.z;

        wantedY = Mathf.SmoothDampAngle(camera.rotation.eulerAngles.y, wantedY, ref refVelY, 0.08f);
        wantedX = Mathf.SmoothDampAngle(camera.rotation.eulerAngles.x, wantedX, ref refVelX, 0.08f);
        wantedZ = Mathf.SmoothDampAngle(camera.rotation.eulerAngles.z, wantedZ, ref refVelZ, 0.08f);

        //calculate position of the camera, MUST happen AFTER the rotation or camera will stutter
        Vector3 camPos = transform.position - camera.transform.forward * MachineCamDistance + transform.TransformVector(Vector3.up) * StdMachineCamHight;

        camera.transform.position = Vector3.Lerp(camera.transform.position, camPos, 0.8f);
        camera.rotation = Quaternion.Euler(wantedX, wantedY, wantedZ);
        

    }
}

