using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player; // 플레이어 Transform
    public Animator animator; // 애니메이터
    public Transform target; // 플레이어의 위치
    public ObjectPool missilePool;

    public float trackingSpeed = 5f; // 추적 속도
    public float idleDistance = 7f; // 공격 거리
    public float attackDuration = 3f; // 공격 지속 시간
    private float defaultSpeed; // 기본 이동 속도
    private bool isAttacking; // 현재 공격 중인지 여부
    private float distance; // 플레이어와의 거리

    private float timer;
    private float timerDuration;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // 기본 이동 속도 저장
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timer = 0f;
        timerDuration = 2f;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        //목적지 도착
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
            // Bullet과 충돌하면 death 애니메이션 재생 및 파괴
            animator.SetTrigger("Death");
            // 애니메이션 재생 시간만큼 딜레이 후에 오브젝트 파괴
            float deathAnimationLength = GetDeathAnimationLength();
            StartCoroutine(DestroyAfterDelay(deathAnimationLength));
        }
    }

    private float GetDeathAnimationLength()
    {
        // Death 애니메이션 클립의 길이를 가져옴
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
        // 딜레이 후에 오브젝트 파괴
        Destroy(gameObject);
    }
}