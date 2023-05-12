using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //������ OnMouseDown()�Լ� ��� Update()�Լ��� �����Ͽ� �� �����Ӹ��� ��ư Ŭ���� �����Ҽ� �ֵ��� �Ͽ����ϴ�. 
    void Update()
    {
        // ��Ʈ�ѷ� Ʈ���� ��ư�� �������� �����ϰ� ���� �ڵ��ε� ���콺 ��� Ʈ���ſ� �����ϰ� �����߽��ϴ�. ������ ���� ��ŧ������ ���� �۵��Ǵ��� Ȯ���� ���� �����Դϴ�. 
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
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
            else if (objectName == "ReturnOption")
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
        SceneManager.LoadScene(sceneName);
    }
}