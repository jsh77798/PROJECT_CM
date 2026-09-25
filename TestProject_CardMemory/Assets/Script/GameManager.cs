using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
//using Unity.Notifications.iOS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 텍스트:시간, 스테이지번호, 점수, 게임종료UI 점수, 스테이지 시작번호 
    public TextMeshProUGUI  TimeText, stageText, hitPointText, EndhitPointText, stageStartText;

    // 이미지:StartTextBox, UI_END(3종 이미지), 
    public Image BoxImage, EndImage_BackGround, EndImage1, EndImage2;

    // 사운드
    AudioSource audioSoure;

    // 버튼:EndUI의 버튼, 
    public Button EndUI_Button;

    // 클릭시 해당하는 카드번호
    static public int cardNum;

    // 이전에 카드번호를 저장하고 있는다
    int lastNum = 0;

    // 스테이지 모든카드 개수
    int cardCnt;

    // 카드클릭 점수
    int hitCnt = 0;

    // 점수
    int PointCount = 0;

    // 스테이지 번호
    static public int stageNum = 1;

    // 총스테이지 수
    int stageCnt = 3;

    // 게임 시간
    float startTime;
    
    // 스테이지의 지나간 시간 
    float stageTime;

    // 카드 배열 
    int[] arCards = new int[50];

    // 이넘:상태구분
    public enum STATE
    {
        IDLE, WAIT, START, HIT, CLEAR, END
    };

    static public STATE state = STATE.START;

    // Start is called before the first frame update
    void Start()
    {
        audioSoure = GetComponent<AudioSource>();

        Screen.orientation = ScreenOrientation.LandscapeRight;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        startTime = stageTime = Time.time;

        //게임종료UI를 감춘다
        EndImage_BackGround.gameObject.SetActive(false);
        EndImage1.gameObject.SetActive(false);
        EndImage2.gameObject.SetActive(false);
        EndUI_Button.gameObject.SetActive(false);
        EndhitPointText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int time1 = (int)(Time.time - startTime);
        int time2 = (int)(Time.time - stageTime);

        int TimeCount = (60 - time2);

        //게임시간을 출력한다
        if (TimeCount >= 0)
        {
            TimeText.text = "" + TimeCount;
        }

        //게임시간종료시 게임이종료된다(게임종료UI실행)
        if (TimeCount <= 0)
        {
            state = STATE.END;
        }

        //점수를 출력한다.
        hitPointText.text = "" + PointCount;
        
        //현재 스테이지를 출력한다
        stageText.text = "" + stageNum;

        //현재상태를 지정한다
        switch (state)
        {
            //START-게임을 시작한다. 카드를 배치한다
            case STATE.START:
                StartCoroutine(MakeStage());
                break;

            //HIT-뒤집은 카드를 체크한다.
            case STATE.HIT:
                CheckCard();
                break;

            //CLEAR-게임을 클리어시 다음 스테이지를 실행한다
            case STATE.CLEAR:
                StartCoroutine(StageClear());
                break;

            //END-게임종료
            case STATE.END:
                StartCoroutine(EndStage());
                break;
        }

        //게임을 강제 종료시킨다
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //////////////////////////////////////////////MyFunction//////////////////////////////////////////////

    // 함수:카드의 상태를 조사하고 상태에 맞는 효과를 준다
    void CheckCard()
    {
        state = STATE.WAIT;

        //첫 번째 카드
        if (lastNum == 0)
        {
            //현재 카드 보존
            lastNum = cardNum;
            state = STATE.IDLE;
            return;
        }

        //이미지 찾고 저장한다
        int img1 = (cardNum + 1) / 2;
        int img2 = (lastNum + 1) / 2;

        //IF:다른 카드라면 카드를 닫는 함수를 실행한다
        if (img1 != img2)
        {
            StartCoroutine(CloseCards());
        
            lastNum = 0;
            state = STATE.IDLE;
            return;
        }

        //같은카드라면 점수를 올린다, 오디오재생
        hitCnt += 2;
        PointCount += 3;
        audioSoure.Play();
        
        //IF:카드가 모두 열려있다면 종료시킨다
        if (hitCnt == cardCnt)
        {
            if(stageNum==3)
            {
                state = STATE.END; //END->게임종료UI실행
                return;
            }
            state = STATE.CLEAR; //CLEAR->다음 스테이지
            return;
        }

        lastNum = 0;
        state = STATE.IDLE;
    }


    // 함수:카드를 닫는다
    IEnumerator CloseCards()
    {
        //태그찾기
        GameObject card1 = GameObject.FindWithTag("Card" + lastNum);
        GameObject card2 = GameObject.FindWithTag("Card" + cardNum);

        //카드닫기
        yield return new WaitForSeconds(0.65f);
        card1.SendMessage("Close_Card", SendMessageOptions.DontRequireReceiver);
        card2.SendMessage("Close_Card", SendMessageOptions.DontRequireReceiver);

    }


    // 함수:스테이지 클리어
    IEnumerator StageClear()
    {
        state = STATE.WAIT;

        yield return new WaitForSeconds(2);

        //스테이지의 카드를 제거한다
        for (int i = 1; i <= cardCnt; i++)
        {
            GameObject card = GameObject.FindWithTag("Card" + i);
            Destroy(card);
        }

        //다음 스테이지를 실행한다
        ++stageNum;
        if (stageNum > stageCnt)
        {
            SceneManager.LoadScene("GameStart");
        }
        
        //스테이지를 초기화한다
        stageTime = Time.time;
        lastNum = 0;
        hitCnt = 0;

        state = STATE.START;
    }


    // 함수:게임종료
    IEnumerator EndStage()
    {
        state = STATE.WAIT;

        //게임종료UI를 드러낸다
        EndImage_BackGround.gameObject.SetActive(true);
        EndImage1.gameObject.SetActive(true);
        EndImage2.gameObject.SetActive(true);
        EndUI_Button.gameObject.SetActive(true);
        EndhitPointText.gameObject.SetActive(true);

        //총점수를 출력한다
        EndhitPointText.text = "" + PointCount;

        yield return new WaitForSeconds(0.1f);
    }


    // 함수:스테이지를 만든다(카드배치)
    IEnumerator MakeStage()
    {
        state = STATE.WAIT;

        StartCoroutine(ShowStageNum());

        //시작카드의 x좌표설정
        float sx = 0.0f;

        //시작카드의 z좌표설정
        float sz = 0.0f;

        SetCardPos(out sx, out sz);

        //카드섞는 함수 실행
        ShuffleCard();

        //시작카드의 번호
        int n = 1;

        string[] str = S_Stage.stage[stageNum - 1];

        //배열의 수만큼 반복한다
        foreach (string t in str)
        {
            char[] ch = t.Trim().ToCharArray();

            //카드의 x축 좌표 설정
            float x = sx;

            foreach (char c in ch)
            {
                switch (c)
                {
                    // *->카드배치
                    case '*':
                        //카드 만들기
                        GameObject card = Instantiate(Resources.Load("Prefab/Card")) as GameObject;
                        
                        //카드 좌표설정
                        card.transform.position = new Vector3(x, 0, sz);

                        //섞인카드 태그 달기
                        card.tag = "Card" + arCards[n++];
                        x++;
                        break;

                    // .->빈칸
                    case '.':
                        x++;
                        break;

                    // - ->자간 띄어쓰기
                    case '-':
                        x += 3.1f;
                        break;

                    // /->줄 행간
                    case '/':
                        sz += 11.5f;
                        break;
                }

                if (c == '*')
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            // 
            sz--;
        }
        // 
        state = STATE.IDLE;
    }


    // 함수: 카드의 시작위치를 계산한다
    void SetCardPos(out float sx, out float sz)
    {
        float x = 0;

        float z = 0;

        //X축 카드의 최대수
        float maxX = 0;

        //스테이지 전체 카드수 설정
        cardCnt = 0;

        string[] str = S_Stage.stage[stageNum - 1];

        //행의 수만큼 반복한다
        for (int i = 0; i < str.Length; i++)
        {
            string t = str[i].Trim();

            //각행의 카드수 초기화
            x = 0;

            //각행의 글자 수만큼 반복
            for (int j = 0; j < t.Length; j++)
            {
                switch (t[j])
                {
                    case '.':
                    case '*':

                        //카드배치 공간
                        x++;
                        if (t[j] == '*')
                        {
                            //전체 카드수 계산
                            cardCnt++;
                        }
                        break;
                    case '-':
                        x += 3.1f;
                        break;
                    case '/':
                        z -= 11.5f;
                        break;

                }
            }

            //최대 카드수 계산
            if (x > maxX)
            {
                maxX = x;
            }
            //행의 수 계산
            z++;
        }

        //X축 시작 위치
        sx = -maxX / 2;
        sz = (z - 1) / 2;
    }


    // 함수: 카드를 섞는다
    void ShuffleCard()
    {
        for (int i = 1; i <= cardCnt; i++)
        {
            arCards[i] = i;
        }

        for (int i = 1; i <= 15; i++)
        {
            int n1 = Random.Range(1, cardCnt + 1);
            int n2 = Random.Range(1, cardCnt + 1);

            int t = arCards[n1];
            arCards[n1] = arCards[n2];
            arCards[n2] = t;
        }
    }


    // 함수: 스테이지 시작시 스테이지 번호를 보여준다
    IEnumerator ShowStageNum()
    {
        stageStartText.text = "STAGE " + stageNum;
        BoxImage.color = new Color32(0, 0, 0, 196);

        yield return new WaitForSeconds(0.75f);

        stageStartText.text = "";
        BoxImage.color = new Color32(0, 0, 0, 0);
    }
}

