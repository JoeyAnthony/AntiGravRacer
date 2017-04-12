using System.Collections;
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
    RaycastHit hitInfo;

    void Start()
    {
        RbPlayer = GetComponent<Rigidbody>();
        RbPlayer.useGravity = false;
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
        //Quaternion.RotateTowards();
        //Vector3.RotateTowards();

        

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        RbPlayer.AddRelativeForce(Vector3.forward * addedVelocity, ForceMode.Acceleration);

        Vector3 vel = RbPlayer.velocity;
        RbPlayer.velocity = Vector3.zero;

        //Vector3 dir = transform.forward * velocity;
        //Vector3 fall = Vector3.down * getForce(fallingSpeed);

        

        Vector3 vec = Vector3.Lerp(RbPlayer.velocity, RbPlayer.transform.forward * vel.magnitude, 1);

        Vector3 fell = new Vector3(0, (RbPlayer.transform.TransformDirection(Vector3.down) * fallingSpeed).y, 0);
        Vector3 vecy = new Vector3(0, vec.y, 0);
        Vector3 add = fell + vecy;

        vec = new Vector3(vec.x, fell.y, vec.z);
        RbPlayer.velocity = vec;


        if (hit)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += 1;
            //RbPlayer.velocity = Vector3.Lerp(RbPlayer.velocity, RbPlayer.transform.TransformDirection(Vector3.down) * fallingSpeed, Time.fixedDeltaTime);
        }

        
        
        
        
        //RbPlayer.transform.TransformDirection(Vector3.down)


        Debug.DrawLine(RbPlayer.transform.position, RbPlayer.transform.position + RbPlayer.velocity * 5, Color.red);
        print("velocity: " + RbPlayer.velocity);
        print("fall"+RbPlayer.transform.TransformDirection(Vector3.down) * fallingSpeed);
        print("vec: " + vec);
    }

    public float getForce(float force)
    {
        return force / (50 * RbPlayer.mass);
    }
}