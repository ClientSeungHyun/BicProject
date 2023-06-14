using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform target; // �÷��̾��� ��ġ
    private float speed = 10f; // �̻��� �ӵ�


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
        // �÷��̾ ���� �̻��� �̵�
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetTarget(Transform targetTransform)
    {
        // �÷��̾� ����
        target = targetTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shield")) 
        {
            // �÷��̾�� �浹 �� �̻��� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }

    }


  

    

}