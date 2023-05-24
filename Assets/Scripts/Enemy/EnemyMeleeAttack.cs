using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public Animator animator; // �ִϸ�����
    private bool isAttacking; // ���� ������ ����
    public Transform target; // �÷��̾��� ��ġ
    private NavMeshAgent navMeshAgent;
    private float distance; // �÷��̾���� �Ÿ�
    private float defaultSpeed; // �⺻ �̵� �ӵ�

    private float timer;
    private float timerDuration;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // �⺻ �̵� �ӵ� ����
        navMeshAgent.enabled = true;

        timer = 0f;
        timerDuration = 2f;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player�� �浹�ϸ� Death �ִϸ��̼� ��� �� �ı�
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