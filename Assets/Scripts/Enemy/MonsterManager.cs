using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    private float timer;
    private float distance;
    private int spawnIndex;
    private int stageClearCount;
    public int monsterSpawnCount;
    public int monsterDeathCount;

    public bool isStageClear;

    private GameObject[] monsterSpawnPoints;
    private ObjectPool monsterPool;             //오브젝트 풀 스크립트
    public GameObject[] monsterPrefabs;

    private GameManagers gameManagerScript;    //게임 매니저 스크립트
    private PlayerControl playerScript;
    Boss bossScript;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Stage03")
        {
            if (bossScript.nowhp <= 0)
                isStageClear = true;
        }
        if (SceneManager.GetActiveScene().name != "Stage03" )
        {
            MonsterSpawn();
            if (monsterDeathCount >= stageClearCount)
                isStageClear = true;
        }

        
    }

    //몬스터 스폰 
    private void MonsterSpawn()
    {
        if (gameManagerScript.IsPlaying() && !gameManagerScript.IsStageClear())
        {
            timer += Time.deltaTime;

            if (timer >= 0.3f)
            {
                while (true)
                {
                    spawnIndex = Random.Range(0, monsterSpawnPoints.Length);
                    distance = Vector3.Distance(monsterSpawnPoints[spawnIndex].transform.position, playerScript.transform.position);
                    
                    //시야 내에 없고 거리가 일정 이상이면
                    if (distance >= 15f)
                    {
                        Debug.Log(spawnIndex);
                        Debug.Log(monsterSpawnPoints[spawnIndex]);
                        monsterPool.prefab = monsterPrefabs[Random.Range(0, 3)];
                        monsterPool.GetObject(monsterSpawnPoints[spawnIndex].transform.position, new Quaternion(0, 0, 0, 0));
                        monsterSpawnCount++;
                        timer = 0;
                        break;
                    }
                }
            }
        }
        
    }

    private void Init()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        monsterPool = GetComponent<ObjectPool>();
        monsterSpawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");

        if (SceneManager.GetActiveScene().name == "Stage01")
            stageClearCount = 180;
        else if (SceneManager.GetActiveScene().name == "Stage02")
            stageClearCount = 200;
        else if (SceneManager.GetActiveScene().name == "Stage03")
        {
            bossScript = GameObject.Find("Boss").GetComponent<Boss>();
            stageClearCount = 1;
        }

        monsterSpawnCount = 0;
        monsterDeathCount = 0;
        isStageClear = false;
    }
}