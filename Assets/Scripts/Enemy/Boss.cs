using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject enemyParent;
    public GameObject missileParent;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private Transform target;
    public GameObject[] summonEnemies;
    public GameObject missile;

    private float distance;
    private float timer = 0f;
    private float timerDuration = 5.0f;
    private float hp = 100.0f;

    void Start()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);
        navMeshAgent.SetDestination(target.position);

        if (distance <= 10f)
        {
            Skill();
            navMeshAgent.isStopped = true;
            this.gameObject.GetComponent<Animator>().SetTrigger("Stay");
        }
        else
        {
            navMeshAgent.isStopped = false;
            this.gameObject.GetComponent<Animator>().SetTrigger("Dash");
        }
    }

    private void Skill()
    {
        timer += Time.deltaTime;

        if (timer >= timerDuration)
        {
            int pattern = Random.Range(0, 2);
    
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
                Heal();
            }
            timer = 0;
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
    void Heal()
    {
        
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
}
