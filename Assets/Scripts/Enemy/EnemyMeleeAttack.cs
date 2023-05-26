using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    [SerializeField] private Animator animator; // �ִϸ�����
    private bool isAttacking; // ���� ������ ����
    [SerializeField]private bool isDeath;
    public Transform target; // �÷��̾��� ��ġ
    private NavMeshAgent navMeshAgent;
    private float distance; // �÷��̾���� �Ÿ�
    private float defaultSpeed; // �⺻ �̵� �ӵ�
    private float timer;
    private float timerDuration;

    private GameManagers gameManagerScript;    //���� �Ŵ��� ��ũ��Ʈ
    private MonsterManager monsterManagerScript;

    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // �⺻ �̵� �ӵ� ����
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        timer = 0f;
        timerDuration = 2f;
        isDeath = false;
    }

    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
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
                //������ ����
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
            // Player�� �浹�ϸ� Death �ִϸ��̼� ��� �� �ı�
            isDeath = true;
            animator.SetTrigger("Death");
            monsterManagerScript.monsterCount++;
        }
    }
    
}