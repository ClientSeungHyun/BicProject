using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private string currentSceneName;
    private string nextSceneName;
    public int stage = 0;
    private GameManagers gameManagerScript;    //���� �Ŵ��� ��ũ��Ʈ

    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        DontDestroyOnLoad(this);
    }


    private void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        CheckClearCondition();
    }

    // �ӽ� üũ��
    private bool CheckClearCondition()
    {
        if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (currentSceneName == "TitleScene")
            {
                nextSceneName = "Stage01";
                LoadingSceneManager.LoadScene(nextSceneName);
            }

            if (gameManagerScript.isStageClear)
            {
                if ((currentSceneName == "Stage01" || currentSceneName == "Stage02"))
                {
                    nextSceneName = "UpgradeScene";
                    stage++;
                }
                if (currentSceneName == "Stage03")
                {
                    nextSceneName = "EndingScene";
                }

                LoadingSceneManager.LoadScene(nextSceneName);
            }
            if (currentSceneName == "UpgradeScene")
            {
                if (stage == 1)
                    nextSceneName = "Stage02";
                else if (stage == 2)
                    nextSceneName = "Stage03";
                LoadingSceneManager.LoadScene(nextSceneName);
            }
        }

        return false;  // Ŭ���� ������ �������� �ʾ��� �� false�� ��ȯ
    }
}