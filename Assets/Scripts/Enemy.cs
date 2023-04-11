using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 1.0f; //체력
    public float speed = 1.0f; //스피드
    public float damage = 20.0f; //플레이어에게 주는 데미지
    public Animator anim;
    public bool isShooter = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) //hp 0 이하면 파괴
            GameObject.Destroy(this.gameObject);
    }
}