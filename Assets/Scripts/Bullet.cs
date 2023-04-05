using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(direction);   
    }
}
