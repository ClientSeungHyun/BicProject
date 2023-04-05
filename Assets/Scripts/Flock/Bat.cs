using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Vector3 velocity;
    public float maxVelocity;

    void Start()
    {
        velocity = transform.forward * maxVelocity;
    }

    void Update()
    {
        if (velocity.magnitude > maxVelocity) {
            velocity = velocity.normalized * maxVelocity;
        }
        this.transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);

    }
}