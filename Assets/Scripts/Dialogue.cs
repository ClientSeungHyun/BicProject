using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    List<string> dialogue = new List<string>() { "���1", "���2", "���3" };  //���
    private int SentanceNum = 0;    //����� ��� �ε���
    private TextMeshProUGUI Text;
    private bool show = false;      //���â �������� ����
    private Color color;
    public float fadeTime = 0.5f; // Panel�� ������� �� �ɸ��� �ð�

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
            // Panel�� ������ ������� �ϱ�
            for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(1, 0, t / fadeTime);
                gameObject.GetComponent<Image>().color = color;
                yield return null;
            }

        }

        IEnumerator FadeIn()
        {

            // Panel�� ������ ��Ÿ����
            for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(0, 1, t / fadeTime);
                gameObject.GetComponent<Image>().color = color;
                yield return null;
            }

        }

    }
}
