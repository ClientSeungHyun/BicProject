using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 50; // 오브젝트 풀의 몬스터 개수
    public float spawnRadius = 5f;
    public float spawnInterval = 3f;
    public int initialSpawnCount = 2; // 게임 시작 시 생성할 몬스터 개수
    public int monstersPerInterval = 2; // 스폰 간격마다 생성할 몬스터 개수

    private List<GameObject> monsterPool;
    private int currentMonsterIndex = 0;
    private float spawnTimer = 0f;
    private int monstersSpawned = 0;

    private GameManagers gameManagerScript;    //게임 매니저 스크립트
    
    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterPool = new List<GameObject>();

        // 오브젝트 풀에 몬스터 생성
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }

        // 게임 시작 시 초기 몬스터 생성
        SpawnMonsters(initialSpawnCount);
    }

    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnMonsters(monstersPerInterval);
                spawnTimer = 0f;
            }
        }
    }

    private void SpawnMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (monstersSpawned >= monsterCount)
                return;

            Vector3 randomPosition = GetRandomPosition();
            GameObject monster = monsterPool[currentMonsterIndex];

            monster.transform.position = randomPosition;
            monster.SetActive(true);

            currentMonsterIndex++;
            if (currentMonsterIndex >= monsterPool.Count)
            {
                currentMonsterIndex = 0;
            }

            monstersSpawned++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;
        return randomPosition;
    }
}