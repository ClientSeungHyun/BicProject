using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickExample : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 확인
        {
            Debug.Log("안녕");
        }
    }
}
