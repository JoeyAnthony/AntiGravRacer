using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour {

    public Transform Target;
    float Yaw;
    float Pitch;
    public float FixedCameraDistance = 5;
    public float camSpeed = 5;
    public float PitchClamp = 13;

	
	// Update is called once per frame
	void LateUpdate () {
        //get yaw and pitch from the controller
        //Yaw += Input.GetAxisRaw("RightAnalogX");
        Pitch += Input.GetAxisRaw("RightAnalogY");

        //clamp the camera
        Pitch = Mathf.Clamp(Pitch, -PitchClamp, PitchClamp);

        //tranform camera with the yaw and the pitch
        //transform.eulerAngles = new Vector3(Pitch, Yaw) *  camSpeed;

        //get negative value for left side
        //float angle = transform.eulerAngles.y;
        //float angleTarget = Target.eulerAngles.y;
        //angle = (angle > 180) ? angle - 360 : angle;
        //angleTarget = (angleTarget > 180) ? angleTarget - 360 : angleTarget;

        //float angleDifference = angleTarget - angle;

        float angleDifference = Target.eulerAngles.y - transform.eulerAngles.y;
        //print(angle + "_____" + angleTarget+"_____"+angleDifference);
        print(transform.eulerAngles.y+"_________"+Target.eulerAngles.y);

        if (Target.eulerAngles.y > 355)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y -= 360;
            rot.x = transform.eulerAngles.x;
            transform.eulerAngles = rot;
        }






        if (angleDifference > 59)
        {
            Vector3 rot = Target.eulerAngles;
            rot.y -= 59;
            rot.x = transform.eulerAngles.x;
            transform.eulerAngles = rot;
        }
        else if (angleDifference < -59)
        {
            Vector3 rot = Target.eulerAngles;
            rot.y += 59;
            rot.x = transform.eulerAngles.x;
            transform.eulerAngles = rot;
        }

        //calculate position of the camera, MUST happen AFTER the rotation
        Vector3 camPos = Target.position - transform.forward * FixedCameraDistance;
        transform.position = camPos;
    }
}
