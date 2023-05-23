using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public GameObject missilePrefab; // �̻��� ������
    public float missileSpeed = 10f; // �̻��� �ӵ�
    public float trackingDistance = 100f; // ���� �Ÿ�
    public float trackingSpeed = 5f; // ���� �ӵ�
    public float idleDistance = 7f; // ���� �Ÿ�
    public float attackDuration = 3f; // ���� ���� �ð�
    public Animator animator; // �ִϸ�����
    public Transform target; // �÷��̾��� ��ġ
    private NavMeshAgent navMeshAgent;
    private float defaultSpeed; // �⺻ �̵� �ӵ�
    private float attackTimer; // ���� Ÿ�̸�
    private bool isAttacking; // ���� ���� ������ ����
    private float distance; // �÷��̾���� �Ÿ�
    public ObjectPool missilePool;
    public Transform missileStartTransform;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // �⺻ �̵� �ӵ� ����
        navMeshAgent.enabled = true;
        
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        if (distance > trackingDistance) // ���� ���� ��
        {
            StopTracking();
        }
        else if (distance <= idleDistance) // ���� ���� ��
        {
            if (!isAttacking)
            {
                StartAttack();
            }
            else
            {
                UpdateAttack();
            }
        }
        else // ���� ���� ��
        {
            StopAttack(); // �߰�: ���� ����
            TrackPlayer();
        }

        // �ִϸ��̼� ������Ʈ
        if (isAttacking)
        {
            animator.SetTrigger("Attack");
        }
        else if (distance <= trackingDistance) // ���� ���� �������� Run �ִϸ��̼� ���
        {
            animator.SetTrigger("run");
        }
    }

    private void StopAttack()
    {
        // ���� ����
        isAttacking = false;
        attackTimer = 0f;
    }

    private void TrackPlayer()
    {
        // �÷��̾ �����մϴ�.
        navMeshAgent.speed = trackingSpeed;
    }

    private void StopTracking()
    {
        // �̵��� ���߰� �׺���̼� ��θ� �ʱ�ȭ�մϴ�.
        navMeshAgent.speed = defaultSpeed;
    }

    private void StartAttack()
    {
        // �̵��� ���߰� ���� �ִϸ��̼��� ����մϴ�.
        navMeshAgent.speed = 0f;
        animator.SetTrigger("Attack");

        isAttacking = true;
        attackTimer = 0f;

        FireMissile(); // �̻��� �߻�
    }

    private void UpdateAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        { 
            EndAttack();
        }
    }

    private void FireMissile()
    {
        
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation *= Quaternion.Euler(0f, -90f, 0f);
        GameObject missileObject = Instantiate(missilePrefab, transform.position + transform.forward * 0.5f, rotation);
        Missile missile = missileObject.GetComponent<Missile>();
        
        if (missile != null)
        {
            missile.SetTarget(player);
            missile.Launch(missileSpeed); // �̻��� �߻� ����
        }
        

        missilePool.GetObject(transform.position + transform.forward * 0.5f);
    }

    private void EndAttack()
    {
        // ������ �����մϴ�.
        isAttacking = false;
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