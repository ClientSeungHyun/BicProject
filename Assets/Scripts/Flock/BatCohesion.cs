using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Bat))]
public class BatCohesion : MonoBehaviour
{
    private Bat bat;

    [InspectorName("another bat Search")]
    public float radius;

    void Start()
    {
        bat = GetComponent<Bat>();
    }

    void Update()
    {
        Bat[] bats = FindObjectsOfType<Bat>();
        Vector3 average = Vector3.zero;
        float found = 0;

        //linq where :: 
        foreach (Bat bat in bats.Where(b => b != bat)) {
            Vector3 diff = bat.transform.position - transform.position;

            if (diff.magnitude < radius)
            {
                average += diff;
                found += 1;
            }
        }
        if (found > 0) {
            average /= found;
            bat.velocity += Vector3.Lerp(Vector3.zero, 
                average, average.magnitude /radius);
        }
    }
}