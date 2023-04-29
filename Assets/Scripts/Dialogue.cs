using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    List<string> dialogue = new List<string>() { "대사1", "대사2", "대사3" };  //대사
    private int SentanceNum = 0;    //출력할 대사 인덱스
    private TextMeshProUGUI Text;
    private bool show = false;      //대사창 보여줄지 말지
    private Color color;
    public float fadeTime = 0.5f; // Panel이 사라지는 데 걸리는 시간

    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        color = gameObject.GetComponent<Image>().color;
        color.a = 0;
        Text = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (show == false) StartCoroutine(FadeIn());
            show = true;

            if (SentanceNum >= dialogue.Count)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                StartCoroutine(FadeOut());
                SentanceNum = 0;
                show = false;
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Text.text = dialogue[SentanceNum];
                SentanceNum++;
            }


        }


        IEnumerator FadeOut()
        {
            // Panel을 서서히 사라지게 하기
            for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(1, 0, t / fadeTime);
                gameObject.GetComponent<Image>().color = color;
                yield return null;
            }

        }

        IEnumerator FadeIn()
        {

            // Panel을 서서히 나타내기
            for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(0, 1, t / fadeTime);
                gameObject.GetComponent<Image>().color = color;
                yield return null;
            }

        }

    }
}
