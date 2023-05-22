using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private string sceneName;
    void OnMouseDown()
    {
        // 클릭한 오브젝트의 이름을 가져오는코드입니다.
        string objectName = gameObject.name;

        // 이동할 씬의 이름을 지정하는코드입니다.
        string sceneName = "";

        // 클릭한 오브젝트의 이름에 따라 이동할 씬을 지정하는코드입니다.
        if (objectName == "ClickStart")
        {
            sceneName = "StartScene";
        }
        else if (objectName == "ClickOption")
        {
            sceneName = "OptionScene";
        }
        else if (objectName == "ClickExit")
        {
            sceneName = "ExitScene";
        }
        else if (objectName == "ReturnExit")
        {
            sceneName = "TitleScene";
        }
        else if (objectName == "ReturnStart")
        {
            sceneName = "TitleScene";
        }
        else if (objectName == "ReturnOption")
        {
            sceneName = "TitleScene";
        }
        // 지정한 씬으로 이동하는코드입니다.
        LoadingSceneManager.LoadScene(sceneName);
    }
    private bool CheckClearCondition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ClearButton"))
                {
                    sceneName = SceneManager.GetActiveScene().name;
                }
            }
        }

        return false;  // 클리어 조건이 충족되지 않았을 때 false를 반환
    }

    public bool LoadScene(string sceneName)
    {
        if (sceneName == "Stage01")


        return false;
    }

    private bool CheckClearConditionStage01()
    {
        // Stage01의 클리어 조건을 확인하는 로직 구현
        return Input.GetMouseButtonDown(0); // 임시로 마우스 왼쪽 버튼 클릭을 클리어 조건으로 설정
    }

    private bool CheckClearConditionStage02()
    {
        // Stage02의 클리어 조건을 확인하는 로직 구현
        return Input.GetMouseButtonDown(0); // 임시로 마우스 왼쪽 버튼 클릭을 클리어 조건으로 설정
    }

    private bool CheckClearConditionStage03()
    {
        // Stage03의 클리어 조건을 확인하는 로직 구현
        return Input.GetMouseButtonDown(0); // 임시로 마우스 왼쪽 버튼 클릭을 클리어 조건으로 설정
    }
}