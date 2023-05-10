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

    void Update()
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