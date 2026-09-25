using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    // 함수:버튼 클릭시 실행
    public void ButtonClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
