using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    public float damage = 20.0f; //각 총알 별 딜 지정

    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(direction); //나가는 방향으로 쭉
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy") //만난 콜리더 태그가 Enemy이면 Enemy 컴포넌트의 hp를 데미지 만큼 감소
        {
            col.gameObject.GetComponent<Enemy>().hp = col.gameObject.GetComponent<Enemy>().hp - damage;
        }
    }
}
