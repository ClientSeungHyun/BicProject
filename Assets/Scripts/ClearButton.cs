using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");

        if (gameManagerObject != null)
        {
            GameManagers gameManager = gameManagerObject.GetComponent<GameManagers>();

            if (gameManager != null)
            {
                if (gameManager.LoadScene("LoadingScene")) // ȣ��� LoadScene�� ���������� ó���Ǿ��� ��
                {
                    if (gameManager.sceneName == "Stage01")
                    {
                        gameManager.LoadScene("Stage02");
                    }
                    else if (gameManager.sceneName == "Stage02")
                    {
                        gameManager.LoadScene("Stage03");
                    }
                }
            }
        }
    }
}
