using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class S_Card : MonoBehaviour
{
    // 변수:카드 애니메이션
    Animator Card_anim;

    // 변수:사운드
    AudioSource audioSoure;

    // 변수:카드 앞면 이미지
    int FImageNum = 1;

    // 변수:카드 뒷면 이미지
    int BImageNum = 1;

    // 변수:앞면이된 카드를 체크한다
    bool OpenCheck = false;

    // 변수:카드의 존재유무를 체크한다
    //bool CardCheck = false;

    // 변수:처음으로 카드를 보여주는 함수의 상태를 체크한다.
    bool CardViewCheck = false;


    // Start is called before the first frame update
    void Start()
    {
        Card_anim = GetComponent<Animator>();

        audioSoure = GetComponent<AudioSource>();

        CardView();
    }

    // Update is called once per frame
    void Update()
    {
        //왼쪽 마우스 클릭,터치   
        if (Input.GetButtonDown("Fire1") && GameManager.state == GameManager.STATE.IDLE && CardViewCheck == true)
        {
            Check_Card();
        }
    }

    //////////////////////////////////////////////MyFunction//////////////////////////////////////////////

    // 함수:스테이지 시작시 모든카드의 앞면을 보여준다
    void CardView()
    {
        CardViewCheck = false;
        int cardNum = int.Parse(transform.tag.Substring(4));

        FImageNum = (cardNum + 1) / 2;
        Card_anim.Play("CardFAnimation");

        GameManager.cardNum = cardNum;

        StartCoroutine(CardCloseStart());
    }

    // 함수:모든카드를 보여주고 난뒤 다시 카드를 뒤집는다
    IEnumerator CardCloseStart()
    {
        yield return new WaitForSeconds(2.75f);
        Card_anim.Play("CardBAnimation");
        CardViewCheck = true;
    }

    // 함수:카드의 상태를 체크한다
    void Check_Card()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //클릭한 카드를 레이트레이스로 식별한다
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            string tag = hit.transform.tag;
            if (tag.Substring(0, 4) == "Card")
            {
                //클릭한 카드의 Open_Card함수를 실행
                hit.transform.SendMessage("Open_Card", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    // 함수:카드Open
    void Open_Card()
    {
        if (OpenCheck) return;
        OpenCheck = true;

        int cardNum = int.Parse(transform.tag.Substring(4));

        FImageNum = (cardNum + 1) / 2;

        audioSoure.Play();

        //카드 여는 애니메이션 실행
        Card_anim.Play("CardFAnimation");

        GameManager.cardNum = cardNum;
        GameManager.state = GameManager.STATE.HIT;
    }

    // 함수:카드Close
    void Close_Card()
    {
        //카드 닫는 애니메이션 실행
        Card_anim.Play("CardBAnimation");
        OpenCheck = false;
    }

    // 함수:카드 앞면 설정
    void FrontImage()
    {
        transform.GetComponent<Renderer>().material.mainTexture = Resources.Load("card" + FImageNum) as Texture2D;
    }

    // 함수:카드 뒷면 설정
    void BackImage()
    {
        transform.GetComponent<Renderer>().material.mainTexture = Resources.Load("back" + BImageNum) as Texture2D;
    }
}
