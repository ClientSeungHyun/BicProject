using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 1.0f;
    public float speed = 1.0f;
    public float damage = 20.0f;
    public Animator anim;
    public bool isShooter = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (hp <= 0)
            Die();
    }

    void Die()
    {
        GameObject.Destroy(this.gameObject);
    }

    void Attack()
    {
        //Damage
    }
}