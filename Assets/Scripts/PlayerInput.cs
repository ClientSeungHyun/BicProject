using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Button retryButton;
    public Button returnToTitleButton;

    private int currentButtonIndex;
    private bool isButtonSelected;

    void Start()
    {
        currentButtonIndex = 0;
        isButtonSelected = false;

        retryButton.onClick.AddListener(Retry);
        returnToTitleButton.onClick.AddListener(ReturnToTitle);
    }

    void Update()
    {

        float joystickVertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        if (joystickVertical > 0.5f || joystickVertical < -0.5f)
        {
            if (!isButtonSelected)
            {
                if (joystickVertical > 0.5f)
                {
                    currentButtonIndex = 0;
                }
                else if (joystickVertical < -0.5f)
                {
                    currentButtonIndex = 1;
                }

                isButtonSelected = true;
            }
        }
        else
        {
            isButtonSelected = false;
        }

        bool aButtonPressed = OVRInput.GetDown(OVRInput.Button.One);

        if (aButtonPressed)
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
        SceneManager.LoadScene("TitleScene");
    }
}