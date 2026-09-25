using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_GameManager : MonoBehaviour
{
    // 버튼:EndUI의 버튼, 
    public Button TitleUI_Button;

    // Start is called before the first frame update
    void Start()
    {
        TitleUI_Button.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
