using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager_Title : MonoBehaviour
{
    // 오브젝트:타이틀카드 애니메이션 실행용
    public Transform TCard1, TCard2, TCard3, TCard4, TCard5;

    // 변수:타이틀 애니메이션
    Animator Title_anim;

    // 변수:사운드
    AudioSource audioSoure;

    public Button Button_Title;

    // 함수:오브젝트 생성(카드 애니메이션 실행용 오브젝트)
    public void newObject()
    {
        Instantiate(TCard1, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(TCard2, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(TCard3, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(TCard4, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(TCard5, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void Start()
    {
        Title_anim = GetComponent<Animator>();

        audioSoure = GetComponent<AudioSource>();

        Button_Title.onClick.AddListener(ButtonClick);
    }

    // 함수: 버튼클릭시 실행
    public void ButtonClick()
    {
        audioSoure.Play();
        Title_anim.Play("TitleSoundAnimation");
        Invoke("AfterButtonClick", 1.45f);
    }

    // 함수: 실행
    public void AfterButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }

}
