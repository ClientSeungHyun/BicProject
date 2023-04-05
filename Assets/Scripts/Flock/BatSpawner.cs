using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject prefab;

    public float radius;
    public int number;

    void Start()
    {
        for (int i = 0; i < number; i++) {
            Instantiate(prefab, transform.position +
                Random.insideUnitSphere * radius, Random.rotation);
        }
    }
}