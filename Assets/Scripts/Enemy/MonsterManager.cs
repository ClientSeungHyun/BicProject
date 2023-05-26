using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    private int stageClearCount;
    public int monsterCount;
    public bool isStageClear;
    Boss bossScript;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage01")
            stageClearCount = 180;
        else if (SceneManager.GetActiveScene().name == "Stage02")
            stageClearCount = 200;
        else if (SceneManager.GetActiveScene().name == "Stage03")
        {
            bossScript = GameObject.Find("Boss").GetComponent<Boss>();
            stageClearCount = 1;
        }

        monsterCount = 0;
        isStageClear = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Stage03")
        {
            if (bossScript.nowhp <= 0)
                isStageClear = true;
        }
        if (SceneManager.GetActiveScene().name != "Stage03" && monsterCount >= stageClearCount)
        {
            Debug.Log("asdf");
            isStageClear = true;
            //SceneManager.LoadScene("ClearSceneTest"); // ClearSceneTest 씬으로 넘어감
        }
    }
}