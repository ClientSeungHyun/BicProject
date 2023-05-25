using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject enemyParent;
    public GameObject missileParent;

    public GameObject[] summonEnemies;
    public GameObject missile;
    private float dashTimer = 3f;
    private float dashSpeed = 0.5f;
    public bool isDash = false;

    private Transform target;

    void Start()
    {
        InvokeRepeating("Skill", 0.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (dashTimer > 0f && isDash == true) 
        {
            Vector3 direction = target.position - transform.position;
            transform.LookAt(direction);
            transform.Translate(direction * dashSpeed * Time.deltaTime, Space.World);
            dashTimer -= Time.deltaTime;
        }
        else
        {
            isDash = false;
            this.gameObject.GetComponent<Animator>().SetTrigger("Stay");
        }
    }

    public void Skill()
    {
        int pattern = Random.Range(0, 3);
        
        if (pattern == 0)
        {
            Summon();
        }
        if (pattern == 1)
        {
            Missile();
        }
        if (pattern == 2)
        {
            Dash();
        }
    }
    
    void Summon()
    {
        foreach (Transform child in enemyParent.transform) {
            Instantiate(summonEnemies[Random.Range(0, 2)], child.position, Quaternion.identity);
        }
    }
    void Missile()
    {
        foreach (Transform child in missileParent.transform) {
            Instantiate(missile, child.position, Quaternion.identity);
        }
    }
    void Dash()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        Vector3 direction = target.position - transform.position;
        this.gameObject.GetComponent<Animator>().SetTrigger("Dash");
        isDash = true;
        dashTimer = 3f;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
}
