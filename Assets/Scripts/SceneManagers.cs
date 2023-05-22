using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private string currentSceneName;
    private string nextSceneName;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }


    private void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        CheckClearCondition();
    }

    // 임시 체크용
    private bool CheckClearCondition()
    {
        if (currentSceneName == "Stage01")
            nextSceneName = "Stage02";
        if (currentSceneName == "TitleScene")
            nextSceneName = "Stage01";

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("SceneChange"))
                    LoadingSceneManager.LoadScene(nextSceneName);
                if (hit.collider.CompareTag("Option"))
                    SceneManager.LoadScene("OptionScene");
                
            }
        }

        return false;  // 클리어 조건이 충족되지 않았을 때 false를 반환
    }
}