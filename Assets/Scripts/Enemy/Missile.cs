using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform target; // 플레이어의 위치
    private float speed = 10f; // 미사일 속도


    private void Update()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // 플레이어를 향해 미사일 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetTarget(Transform targetTransform)
    {
        // 플레이어 설정
        target = targetTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shield")) 
        {
            // 플레이어와 충돌 시 미사일 비활성화
            gameObject.SetActive(false);
        }

    }


  

    

}