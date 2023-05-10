using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 5;
    public float spawnRadius = 10f;

    private List<GameObject> monsterPool;
    private int currentMonsterIndex = 0;

    private void Start()
    {
        monsterPool = new List<GameObject>();

        // ���� ���� �� Ǯ�� �߰�
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }

        // ���� ��ġ �����ϰ� ����
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject monster = monsterPool[currentMonsterIndex];

            monster.transform.position = randomPosition;
            monster.SetActive(true);

            currentMonsterIndex++;
            if (currentMonsterIndex >= monsterPool.Count)
            {
                currentMonsterIndex = 0;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;
        return randomPosition;
    }
}