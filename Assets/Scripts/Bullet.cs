using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    public float damage = 20.0f; //�� �Ѿ� �� �� ����

    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(direction); //������ �������� ��
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy") //���� �ݸ��� �±װ� Enemy�̸� Enemy ������Ʈ�� hp�� ������ ��ŭ ����
        {
            col.gameObject.GetComponent<Enemy>().hp = col.gameObject.GetComponent<Enemy>().hp - damage;
        }
    }
}
