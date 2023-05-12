using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus;

public class SceneChangerVR : MonoBehaviour
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

            // ������ ������ �̵��ϴ��ڵ��Դϴ�.
            SceneManager.LoadScene(sceneName);
        }
    }
}