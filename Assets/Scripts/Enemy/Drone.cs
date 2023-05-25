using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private Transform player;
    private float distance;
    public GameObject explosion;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance <= 2)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
