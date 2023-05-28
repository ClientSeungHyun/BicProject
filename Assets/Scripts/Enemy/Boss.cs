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

    private GameManagers gameManager;
    private MonsterManager monsterManagerScript;

    private float distance;
    private float dashDistance;
    private float timer = 0f;
    private float timerDuration = 10.0f;
    private float maxhp = 100.0f;
    public float nowhp = 100.0f;
    private int maxMonster = 50;
    private int nowMonster = 0;
    private int pattern;

    private Vector3 dashTarget;

    private bool dashStart;

    void Start()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.enabled = true;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();

        pattern = -1;
        dashStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.IsPlaying())
        {
            distance = Vector3.Distance(transform.position, target.position);
            navMeshAgent.SetDestination(target.position);
            if (distance < 20f)
            {
                Skill();
                navMeshAgent.isStopped = true;
                this.gameObject.GetComponent<Animator>().SetTrigger("Stay");

                if (distance > 1f) 
                    transform.LookAt(target);
            }
            else
            {
                //navMeshAgent.isStopped = false;
                this.gameObject.GetComponent<Animator>().SetTrigger("Dash");
            }
        }
        if (nowhp <= 0)
        {
            monsterManagerScript.monsterDeathCount++;
            Dead();
        }
    }

    private void Skill()
    {
        timer += Time.deltaTime;

        if (timer >= timerDuration)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Cast");
            pattern = Random.Range(0, 2);
    
            if (pattern == 0)
            {
                Summon();
            }
            if (pattern == 1)
            {
                Missile();
            }
            //if (pattern == 2)
            //{
            //    dashStart = true;
            //    runToPlayer();
            //}

            timer = 0;
        }
    }
    
    void Summon()
    {
        if(nowMonster <= maxMonster)
        {
            foreach (Transform child in enemyParent.transform)
            {
                Instantiate(summonEnemies[Random.Range(0, summonEnemies.Length)], child.position, Quaternion.identity);
            }
            nowMonster += 8;
        }
    }
    void Missile()
    {
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation *= Quaternion.Euler(0f, -90f, 0f);
        foreach (Transform child in missileParent.transform) {
            Instantiate(missile, child.position, rotation);
        }
    }

    void Dead()
    {
        //Animation
        //Destroy Obj (effect?)
    }

    //void runToPlayer()
    //{
    //    Debug.Log("°¡ÀÚ");
    //    if (dashStart == true)
    //    {
    //        dashStart = false;
    //        dashTarget = transform.forward * 5;
    //        navMeshAgent.SetDestination(dashTarget);
    //    }
    //    timer = 3.1f;
    //    dashDistance  = Vector3.Distance(transform.position, dashTarget);
    //    this.gameObject.GetComponent<Animator>().SetTrigger("Dash");
    //    navMeshAgent.isStopped = false;

    //    if (dashDistance <= 0.3f)
    //    {
    //        navMeshAgent.SetDestination(target.position);
    //        navMeshAgent.isStopped = true;
    //        pattern = -1;
    //        timer = 0;
    //    }

    //}

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            nowhp--;
        }
    }
}
