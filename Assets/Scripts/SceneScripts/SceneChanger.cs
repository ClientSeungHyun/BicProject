using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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

        // ������ ������ �̵��ϴ��ڵ��Դϴ�.
        SceneManager.LoadScene(sceneName);
    }
}