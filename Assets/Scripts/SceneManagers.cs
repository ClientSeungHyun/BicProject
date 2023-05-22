using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private string sceneName;
    void OnMouseDown()
    {
        // Ŭ���� ������Ʈ�� �̸��� ���������ڵ��Դϴ�.
        string objectName = gameObject.name;

        // �̵��� ���� �̸��� �����ϴ��ڵ��Դϴ�.
        string sceneName = "";

        // Ŭ���� ������Ʈ�� �̸��� ���� �̵��� ���� �����ϴ��ڵ��Դϴ�.
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
        // ������ ������ �̵��ϴ��ڵ��Դϴ�.
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

        return false;  // Ŭ���� ������ �������� �ʾ��� �� false�� ��ȯ
    }

    public bool LoadScene(string sceneName)
    {
        if (sceneName == "Stage01")


        return false;
    }

    private bool CheckClearConditionStage01()
    {
        // Stage01�� Ŭ���� ������ Ȯ���ϴ� ���� ����
        return Input.GetMouseButtonDown(0); // �ӽ÷� ���콺 ���� ��ư Ŭ���� Ŭ���� �������� ����
    }

    private bool CheckClearConditionStage02()
    {
        // Stage02�� Ŭ���� ������ Ȯ���ϴ� ���� ����
        return Input.GetMouseButtonDown(0); // �ӽ÷� ���콺 ���� ��ư Ŭ���� Ŭ���� �������� ����
    }

    private bool CheckClearConditionStage03()
    {
        // Stage03�� Ŭ���� ������ Ȯ���ϴ� ���� ����
        return Input.GetMouseButtonDown(0); // �ӽ÷� ���콺 ���� ��ư Ŭ���� Ŭ���� �������� ����
    }
}