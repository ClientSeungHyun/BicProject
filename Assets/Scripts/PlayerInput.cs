using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Button retryButton;
    public Button returnToTitleButton;

    private int currentButtonIndex;

    void Start()
    {
        currentButtonIndex = 0;

        retryButton.gameObject.GetComponent<Image>().color = new Color32(255, 66, 91, 255);
        returnToTitleButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        retryButton.onClick.AddListener(Retry);
        returnToTitleButton.onClick.AddListener(ReturnToTitle);
    }

    void Update()
    {

        float joystickVertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;


        if (joystickVertical > 0.5f || Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            currentButtonIndex = 0;
            retryButton.gameObject.GetComponent<Image>().color = new Color32(255, 66, 91, 255);
            returnToTitleButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (joystickVertical < -0.5f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentButtonIndex = 1;
            retryButton.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            returnToTitleButton.GetComponent<Image>().color = new Color32(255, 66, 91, 255);
        }

        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentButtonIndex == 0)
            {
                Retry();
            }
            else if (currentButtonIndex == 1)
            {
                ReturnToTitle();
            }
        }
    }

    public void Retry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string retrySceneName = "";

        if (currentSceneName == "Stage01")
        {
            retrySceneName = "Stage01";
        }
        else if (currentSceneName == "Stage02")
        {
            retrySceneName = "Stage02";
        }
        else if (currentSceneName == "Stage03")
        {
            retrySceneName = "Stage03";
        }
        SceneManager.LoadScene(retrySceneName);
    }

    public void ReturnToTitle()
    {
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("SceneManager"));
        SceneManager.LoadScene("TitleScene");
    }
}