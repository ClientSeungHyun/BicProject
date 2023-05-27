using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyRangedAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player; // �÷��̾� Transform
    [SerializeField] private Animator animator; // �ִϸ�����
    public Transform target; // �÷��̾��� ��ġ
    public ObjectPool missilePool;
    private bool isAttacking; // ���� ���� ������ ����
    [SerializeField] private bool isDeath;
    private float distance; // �÷��̾���� �Ÿ�

    private float timer;
    private float timerDuration;

    private GameManagers gameManagerScript;    //���� �Ŵ��� ��ũ��Ʈ
    private MonsterManager monsterManagerScript;

    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        timer = 0f;
        timerDuration = 5f;
        isDeath = false;
    }

    private void Update()
    {
        if (gameManagerScript.IsPlaying() && !gameManagerScript.IsStageClear())
        {
            distance = Vector3.Distance(transform.position, player.position);
            navMeshAgent.SetDestination(player.position);

            if (isDeath)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    gameObject.SetActive(false);
            }
            else if (!isDeath)
            {
                //������ ����
                if (distance <= 12f)
                {
                    StartAttack();
                    navMeshAgent.isStopped = true;
                    animator.SetTrigger("Idle");
                }
                else
                {
                    animator.SetTrigger("Run");
                    navMeshAgent.isStopped = false;
                }
            }
        }

        //Ŭ����Ǹ� ��� ��Ȱ��ȭ
        if (gameManagerScript.IsStageClear())
        {
            isDeath = true;
        }
    }

    private void StartAttack()
    {
        timer += Time.deltaTime;
        transform.LookAt(player.transform);
        if (timer >= timerDuration)
        {
            isAttacking = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            rotation *= Quaternion.Euler(0f, -90f, 0f);
            missilePool.GetObject(transform.position + transform.forward, rotation);
            timer = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Bullet�� �浹�ϸ� death �ִϸ��̼� ��� �� �ı�
            isDeath = true;
            animator.SetTrigger("Death");

            if(SceneManager.GetActiveScene().name != "Stage03")
                monsterManagerScript.monsterDeathCount++;
        }
    }

    
}