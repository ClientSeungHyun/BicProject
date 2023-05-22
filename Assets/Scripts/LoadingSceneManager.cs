using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    string currentSceneName;
    static string nextSceneName;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
  
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
       AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
       op.allowSceneActivation = false; //���� 90���� �ε��Ҷ� ������ٸ��� ���� ������ �Ѿ�� ����

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                //������ ���൵ 10���δ� 1�ʸ��� ����ǰ�
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public string NowSceneName()
    {
        return currentSceneName;
    }
    public void NowSceneName(string n)
    {
        currentSceneName = n;
    }

    public string NextSceneName()
    {
        return nextSceneName;
    }
    public void NextSceneName(string s)
    {
        nextSceneName = s;
    }
}
