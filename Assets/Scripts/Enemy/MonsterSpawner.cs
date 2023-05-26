using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 50; // ������Ʈ Ǯ�� ���� ����
    public int monsterMaxCount;
    public int clearCount;
    public float spawnRadius = 5f;
    public float spawnInterval = 3f;
    public int initialSpawnCount = 2; // ���� ���� �� ������ ���� ����
    public int monstersPerInterval = 2; // ���� ���ݸ��� ������ ���� ����

    private List<GameObject> monsterPool;
    private int currentMonsterIndex = 0;
    private float spawnTimer = 0f;
    private int monstersSpawned = 0;

    private GameManagers gameManagerScript;    //���� �Ŵ��� ��ũ��Ʈ
    
    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        monsterPool = new List<GameObject>();

        // ������Ʈ Ǯ�� ���� ����
        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }

        // ���� ���� �� �ʱ� ���� ����
        SpawnMonsters(initialSpawnCount);

        if (SceneManager.GetActiveScene().name == "Stage01")
            monsterMaxCount = 130;
        if (SceneManager.GetActiveScene().name == "Stage02")
            monsterMaxCount = 150;
        clearCount = 50;
    }

    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
            if (monsterMaxCount >= clearCount)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval)
                {
                    SpawnMonsters(monstersPerInterval);
                    spawnTimer = 0f;
                }
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
            clearCount++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;
        return randomPosition;
    }
}