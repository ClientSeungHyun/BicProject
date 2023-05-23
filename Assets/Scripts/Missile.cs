using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform target; // �÷��̾��� ��ġ
    private float speed; // �̻��� �ӵ�


    private void Update()
    {
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

    public void Launch(float missileSpeed)
    {
        // �̻��� �߻� ����
        speed = missileSpeed;
        gameObject.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� �浹 �� �̻��� ��Ȱ��ȭ
            gameObject.SetActive(false);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        // �Ѿ��� ��Ȱ��ȭ�ϰ� ������Ʈ Ǯ�� ��ȯ�մϴ�.
        gameObject.SetActive(false);
    }

  

    

}