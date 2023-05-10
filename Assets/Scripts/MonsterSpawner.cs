using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 20;
    public float spawnRadius = 10f;

    private List<GameObject> monsterPool;
    private int currentMonsterIndex = 0;

    private void Start()
    {
        monsterPool = new List<GameObject>();

        // 몬스터 풀 초기화
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
    }

    private void Update()
    {
        // 몬스터 스폰 처리
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        GameObject monster = monsterPool[currentMonsterIndex];
        monster.transform.position = GetRandomPosition();
        monster.SetActive(true);

        currentMonsterIndex++;
        if (currentMonsterIndex >= monsterPool.Count)
        {
            currentMonsterIndex = 0;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;
        return randomPosition;
    }
}