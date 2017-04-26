using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlP1 : MonoBehaviour
{
    private Rigidbody RbPlayer;

    private float yaw;
    private float pitch = 0;
    private float velocity;

    public float hoverDistance = 2;

    public float pitchClamp = 20;
    public float rotationSpeed = 100;

    public float acceleration = 1;
    public float desceleration = 20;
    public float fallAcceleration = 1;

    public float maxVelocity = 3000;
    public float maxSpeed_Vector = 200;

    private float addedVelocity;
    private float fallingSpeedRayDir;
    private float fallingSpeed;

    private Vector3 startPos = new Vector3();
    private Vector3 startRot = new Vector3();

    float z = 0;

    RaycastHit hitInfo;

    void Start()
    {
        RbPlayer = GetComponent<Rigidbody>();
        RbPlayer.useGravity = false;
        RbPlayer.freezeRotation = true;
        startPos = transform.position;
        startRot = transform.rotation.eulerAngles;

        //setStart();
    }
    
    void Update()
    {
        //get yaw and pitch from the controller
        yaw += Input.GetAxisRaw("LeftAnalogX");
        yaw += Input.GetAxisRaw("KeyLeftRight");
        pitch += Input.GetAxisRaw("LeftAnalogY");
        pitch += Input.GetAxisRaw("KeyUpDown");

        //clamp x rotation
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        if (Input.GetAxisRaw("G") > 0 || Input.GetButton("AButton"))
        {
            velocity += acceleration;

            velocity = Mathf.Clamp(velocity, 0, maxVelocity);

            addedVelocity += velocity;
        }
        else
        {
            velocity -= desceleration;
            velocity = Mathf.Clamp(velocity, 0, maxVelocity);

            addedVelocity = 0;
        }
        checkDeath();
    }


    void FixedUpdate()
    {
        //raycast to detect the road
        Vector3 rayDir = RbPlayer.transform.TransformDirection(Vector3.down);
        Vector3 rotationInput = new Vector3();
        bool hit = Physics.Raycast(RbPlayer.transform.position, rayDir, out hitInfo, 15f);
        if (hit)
        {
            //set object above the road
            if (hitInfo.transform.tag.Equals("Road"))
            {
                if(hitInfo.distance <= hoverDistance+0.5)
                {
                    RbPlayer.position = hitInfo.point + RbPlayer.transform.up * hoverDistance;
                    fallingSpeedRayDir = 0;
                }
                else
                {
                    fallingSpeedRayDir += 50 / hitInfo.distance + fallingSpeed;
                }
            }
            //can't use pitch if on a road
            rotationInput = new Vector3(0, yaw) * rotationSpeed;
            pitch = 0;
        }
        else
        {
            //can use pitch in air
            rotationInput = new Vector3(pitch, yaw) * rotationSpeed;
            fallingSpeedRayDir = 0;
        }

        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + rayDir * 15, Color.green);
        //Debug.DrawLine(hitInfo.point, hitInfo.point + RbPlayer.transform.up * 100, Color.blue);

        //take normal from surface of the road
        Quaternion roadNormal = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

        
        //rotation vector multiply with normal of the road and slerp
        RbPlayer.rotation = Quaternion.Lerp(RbPlayer.rotation, (roadNormal * Quaternion.Euler(rotationInput * Time.fixedDeltaTime)), 0.4f);


        Vector3 vel = RbPlayer.velocity;

        RbPlayer.AddRelativeForce(addedVelocity * Vector3.forward);
        float forwardMag = vel.magnitude - fallingSpeed;
       
        Vector3 endDir = RbPlayer.transform.forward * vel.magnitude - vel;

        RbPlayer.AddForce(getAngledForce(endDir + Vector3.down * fallingSpeed));
        RbPlayer.AddForce(getAngledForce(rayDir * fallingSpeedRayDir));

        if (RbPlayer.velocity.magnitude > maxSpeed_Vector)
        {
            RbPlayer.velocity = Vector3.ClampMagnitude(RbPlayer.velocity, maxSpeed_Vector);
        }

        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + RbPlayer.velocity * 5, Color.red);

        //print("velocitymag: " + RbPlayer.velocity.magnitude);


        if (hit)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += fallAcceleration;
        }
    }

    public Vector3 getAngledForce(Vector3 force)
    {
        return force * (50 * RbPlayer.mass);
    }

    public void setStart()
    {
        RbPlayer.position = startPos;
        RbPlayer.rotation = Quaternion.Euler(startRot);
    }

    public void checkDeath()
    {
        if (RbPlayer.position.y < -980)
        {
            setStart();
            RbPlayer.velocity = Vector3.zero;
        }
        
    }
}