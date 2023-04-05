using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bat))]
public class BatContainer : MonoBehaviour
{
    private Bat bat;

    public float radius;
    public float boundaryForce;

    void Start()
    {
        bat = GetComponent<Bat>();
    }

    void Update()
    {
        if (bat.transform.position.magnitude > radius)
        {
            bat.velocity += transform.position.normalized * 
                (radius - bat.transform.position.magnitude) * 
                boundaryForce * Time.deltaTime;
        }
    }
}