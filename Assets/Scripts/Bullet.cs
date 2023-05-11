using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �Ѿ��� �ӵ�

    private void OnEnable()
    {
        // �Ѿ��� �߻��ϸ� ���� �ð� �Ŀ� �ڵ����� ��Ȱ��ȭ�մϴ�.
        Invoke("Deactivate", 3f);
    }

    private void OnDisable()
    {
        // �Ѿ��� ��Ȱ��ȭ�Ǹ� ����մϴ�.
        CancelInvoke();
    }

    private void Update()
    {
        // �Ѿ��� ������ �̵���ŵ�ϴ�.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �Ѿ��� �ٸ� �浹ü�� �ε����� ��Ȱ��ȭ�մϴ�.
        Deactivate();
    }

    private void Deactivate()
    {
        // �Ѿ��� ��Ȱ��ȭ�ϰ� ������Ʈ Ǯ�� ��ȯ�մϴ�.
        gameObject.SetActive(false);
    }
}
