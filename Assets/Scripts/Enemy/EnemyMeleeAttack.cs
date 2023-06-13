using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    [SerializeField] private Animator animator; // 애니메이터
    private bool isAttacking; // 공격 중인지 여부
    [SerializeField]private bool isDeath;
    public Transform target; // 플레이어의 위치
    private NavMeshAgent navMeshAgent;
    private float distance; // 플레이어와의 거리
    private float defaultSpeed; // 기본 이동 속도
    private float timer;
    private float timerDuration;

    private GameManagers gameManagerScript;    //게임 매니저 스크립트
    private MonsterManager monsterManagerScript;

    private void OnEnable()
    {
        if(GetComponent<CapsuleCollider>()!= null)
            GetComponent<CapsuleCollider>().enabled = true;
        if (GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().enabled = true;
        navMeshAgent.speed = 2;
    }

    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // 기본 이동 속도 저장
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        timer = 0f;
        timerDuration = 2f;
        isDeath = false;
    }

    private void Update()
    {
        //클리어되면 모두 비활성화
        if (gameManagerScript.IsStageClear())
        {
            DeathCode();
        }

        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        if (isDeath)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                gameObject.SetActive(false);
            }
        }
        else if (!isDeath)
        {
            navMeshAgent.speed = 2f;
            //목적지 도착
            if (distance <= 2f)
            {
                StartAttack();
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetTrigger("Run");
                navMeshAgent.isStopped = false;
            }
        }
    }

    private void StartAttack()
    {
        timer += Time.deltaTime;

        if (timer >= timerDuration)
        {
            isAttacking = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            timer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            DeathCode();
        }
    }

    public void DeathCode()
    {
        isDeath = true;
        navMeshAgent.speed = 0;
        animator.SetTrigger("Death");

        if (GetComponent<CapsuleCollider>() != null)
            GetComponent<CapsuleCollider>().enabled = false;
        if (GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().enabled = false;

        if (SceneManager.GetActiveScene().name != "Stage03" && !isDeath)
            monsterManagerScript.monsterDeathCount++;
    }
}