using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickExample : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� Ȯ��
        {
            Debug.Log("�ȳ�");
        }
    }
}
