using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus;

public class ReturnSceneVR : MonoBehaviour
{
    //기존에 OnMouseDown()함수 대신 Update()함수로 변경하여 매 프레임마다 버튼 클릭을 감지할수 있도록 하였습니다. 
    void Update()
    {
        // 컨트롤러 트리거 버튼이 눌렸을때 반응하게 만든 코드인데 마우스 대신 트리거에 반응하게 변경했습니다. 하지만 아직 오큘러스가 없어 작동되는지 확인은 못한 상태입니다. 
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // 클릭한 오브젝트의 이름을 가져오는코드입니다.
            string objectName = gameObject.name;

            // 이동할 씬의 이름을 지정하는코드입니다.
            string sceneName = "";

            // 클릭한 오브젝트의 이름에 따라 이동할 씬을 지정하는코드입니다.
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

            // 지정한 씬으로 이동하는코드입니다.
            SceneManager.LoadScene(sceneName);
        }
    }
}