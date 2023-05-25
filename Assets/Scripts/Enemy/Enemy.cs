using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private EnemyRangedAttack rangedAttack; // EnemyRangedAttack 스크립트 참조

    private void Start()
    {
        rangedAttack = GetComponent<EnemyRangedAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int monsterCount = rangedAttack.MonsterCount;
            if (monsterCount >= 180)
            {
                SceneManager.LoadScene("ClearSceneTest"); // ClearSceneTest 씬으로 넘어감
            }
        }
    }
}