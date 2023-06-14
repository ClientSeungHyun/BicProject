using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    private float creditSpeed;

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.A) || Input.GetKey(KeyCode.A))
        {
            creditSpeed = 1.0f;
        }
        else
        {
            creditSpeed = 0.5f;
        }

        gameObject.transform.Translate(new Vector2(transform.position.x, creditSpeed * Time.deltaTime));

        if (SceneManager.GetActiveScene().name == "OpeningScene" && gameObject.transform.position.y > 7.5)
            LoadingSceneManager.LoadScene("Stage01");
        else if (SceneManager.GetActiveScene().name == "EndingScene" && gameObject.transform.position.y > 18f)
            LoadingSceneManager.LoadScene("TitleScene");


    }
}
