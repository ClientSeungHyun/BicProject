using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniContr : MonoBehaviour
{
    public Animator anima;
    private EnemyNav enemyNav;

    void Start()
    {
        anima = GetComponent<Animator>();
        enemyNav = GetComponent<EnemyNav>();
    }

    void Update() //enemyNav함수에서 스피드 값을 받아 정해진 값에 따라 애니메이션이 변경됩니다. 
    {
        float speed = enemyNav.GetSpeed();
        if (speed == enemyNav.runSpeed)
        {
            anima.SetTrigger("run");
        }
        else if (speed == enemyNav.walkSpeed)
        {
            anima.SetTrigger("walk");
        }
    }
}