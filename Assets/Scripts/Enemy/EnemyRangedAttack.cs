using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player; // �÷��̾� Transform
    public Animator animator; // �ִϸ�����
    public Transform target; // �÷��̾��� ��ġ
    public ObjectPool missilePool;

    public float trackingSpeed = 5f; // ���� �ӵ�
    public float idleDistance = 7f; // ���� �Ÿ�
    public float attackDuration = 3f; // ���� ���� �ð�
    private float defaultSpeed; // �⺻ �̵� �ӵ�
    private bool isAttacking; // ���� ���� ������ ����
    private float distance; // �÷��̾���� �Ÿ�

    private float timer;
    private float timerDuration;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // �⺻ �̵� �ӵ� ����
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timer = 0f;
        timerDuration = 2f;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        //������ ����
        if (distance <= 8f)
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
            missilePool.GetObject(transform.position + transform.forward * 0.5f, rotation);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Bullet�� �浹�ϸ� death �ִϸ��̼� ��� �� �ı�
            animator.SetTrigger("Death");
            // �ִϸ��̼� ��� �ð���ŭ ������ �Ŀ� ������Ʈ �ı�
            float deathAnimationLength = GetDeathAnimationLength();
            StartCoroutine(DestroyAfterDelay(deathAnimationLength));
        }
    }

    private float GetDeathAnimationLength()
    {
        // Death �ִϸ��̼� Ŭ���� ���̸� ������
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Death")
            {
                return clip.length;
            }
        }
        return 0f;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ������ �Ŀ� ������Ʈ �ı�
        Destroy(gameObject);
    }
}