using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector2(transform.position.x, 0.3f*Time.deltaTime));
        if (gameObject.transform.position.y > 7.5)
        {
            if(SceneManager.GetActiveScene().name == "OpeningScene")
                LoadingSceneManager.LoadScene("Stage01");
            else if(SceneManager.GetActiveScene().name == "EndingScene")
                LoadingSceneManager.LoadScene("TitleScene");
        }
            
    }
}
