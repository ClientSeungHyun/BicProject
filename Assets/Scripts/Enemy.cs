using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 1.0f; //ü��
    public float speed = 1.0f; //���ǵ�
    public float damage = 20.0f; //�÷��̾�� �ִ� ������
    public Animator anim;
    public bool isShooter = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) //hp 0 ���ϸ� �ı�
            GameObject.Destroy(this.gameObject);
    }
}