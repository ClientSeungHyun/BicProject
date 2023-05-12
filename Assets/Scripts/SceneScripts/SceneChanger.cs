using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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

        // 지정한 씬으로 이동하는코드입니다.
        SceneManager.LoadScene(sceneName);
    }
}