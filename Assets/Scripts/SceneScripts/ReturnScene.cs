using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScene : MonoBehaviour
{
    void OnMouseDown()
    {
        // Ŭ���� ������Ʈ�� �̸��� ���������ڵ��Դϴ�.
        string objectName = gameObject.name;
        
        // �̵��� ���� �̸��� �����ϴ��ڵ��Դϴ�.
        string sceneName = "";

        // Ŭ���� ������Ʈ�� �̸��� ���� �̵��� ���� �����ϴ��ڵ��Դϴ�.
        if (objectName == "ReturnOption")
        {
            sceneName = "TitleScene";
        }
        else if (objectName == "ReturnExit")
        {
            sceneName = "TitleScene";
        }
        else if (objectName == "ReturnStart")
        {
            sceneName = "TitleScene";
        }

        // ������ ������ �̵��ϴ��ڵ��Դϴ�.
        SceneManager.LoadScene(sceneName);
    }
}