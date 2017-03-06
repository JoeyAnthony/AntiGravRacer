using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform FollowTarget;
    public float Speed;

	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 displacementTarget = FollowTarget.position - transform.position;
        Vector3 directionTarget = displacementTarget.normalized;
        Vector3 velocity = directionTarget * Speed;

        float distanceToTarget = displacementTarget.magnitude;

        if (distanceToTarget > 1.5f)
        {
            transform.Translate(velocity * Time.deltaTime,Space.World);
            transform.Rotate(Vector3.up * 180 * Time.deltaTime, Space.World); 
        }
    }
}
