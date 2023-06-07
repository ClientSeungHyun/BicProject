using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject missileParent;
    public GameObject bossShieldObject;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;

    private Transform target;
    public GameObject missile;

    private GameManagers gameManager;
    private MonsterManager monsterManagerScript;

    private float distance;
    private float dashDistance;
    private float timer;
    private float timerDuration = 7.0f;
    private float chaseTimer;
    private float sprintTimer;
    private float maxhp = 100.0f;
    public float nowhp = 100.0f;
    public float currentShieldSize = 0.0f;
    public float maxShieldSize;
    private int pattern;
    

    private Vector3 dashTarget;

    private bool isDashStart;
    private bool isBossShield;
    private bool isIdle;
    private bool isSprint;
    private bool isCast;
    private bool isDead;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagers>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();
      
        bossShieldObject.SetActive(false);

        timer = 5f;
        chaseTimer = 0f;
        sprintTimer = 0f;
        maxShieldSize = 6.0f;
        pattern = -1;
        isDashStart = false;
        isBossShield = false;
        isIdle = isSprint = isCast = isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPlaying())
        {
            navMeshAgent.SetDestination(target.position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                isSprint = false;
                isIdle = true;
                Skill();
                if (navMeshAgent.remainingDistance > 4f && !isDashStart)
                    transform.LookAt(target);
            }
            else
            {
                sprintTimer += Time.deltaTime;
                if (sprintTimer >= 8f)
                {
                    monsterManagerScript.MonsterSpawn();
                    isCast = true;
                    sprintTimer = 0f;
                }
                isSprint = true;
                isIdle = false;
            }
        }
        if (nowhp <= 0)
        {
            monsterManagerScript.monsterDeathCount++;
            Dead();
        }

        AnimatorManage();
        runToPlayer();
    }

    private void Skill()
        {
            timer += Time.deltaTime;

            if (timer >= timerDuration)
            {
                isCast = true;
                pattern = Random.Range(0, 101);


                if (0 <= pattern && pattern <= 45) //45%
                {
                    monsterManagerScript.MonsterSpawn();
                }
                if (45 < pattern && pattern <= 80) //35%
                {
                    Missile();
                }
                if (80 < pattern && pattern <= 100) //20%
                {
                    isDashStart = true;
                }
                timer = 0;

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
        isDead = true;
        GameObject.Destroy(this);
    }

    void runToPlayer()
    {
        if (isDashStart == true )
        {
            timer = 0;
            isSprint = true;
            BossShieldOn(); //쉴드를 킴

            //플레이어 방향으로 돌진함
            if (currentShieldSize == maxShieldSize) 
            {
                transform.Translate(Vector3.forward * 28f * Time.deltaTime);
                chaseTimer += Time.deltaTime;
            }

            if(chaseTimer > 0.3f)
            {
                chaseTimer = 0f;
                currentShieldSize = 0f;
                bossShieldObject.SetActive(false);
                //navMeshAgent.enabled = true;
                isBossShield = false;
                isDashStart = false;
                isSprint = false;
            }
        }
    }

    public void BossShieldOn()
    {
        // 쉴드의 크기를 증가시키고, 최대 크기 지정
        isBossShield = true;
        bossShieldObject.SetActive(true);
        currentShieldSize = Mathf.Min(currentShieldSize + 5 * Time.deltaTime, maxShieldSize);
        bossShieldObject.transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);

    }

    private void AnimatorManage()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            isCast = false;
        }
        animator.SetBool("Sprint", isSprint);  //sprint
        animator.SetBool("IDLE", isIdle);  //idle
        animator.SetBool("Cast", isCast);  //cast
        animator.SetBool("Dead", isDead); //dead
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isBossShield)
        {
            nowhp--;
        }
    }
}
