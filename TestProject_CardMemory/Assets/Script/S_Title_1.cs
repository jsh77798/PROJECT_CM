using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Title_1 : MonoBehaviour
{
    // 변수:애니메이션
    Animator Title_anim;

    // Start is called before the first frame update
    void Start()
    {
        Title_anim = GetComponent<Animator>();
        Title_anim.Play("UITitle1Animation");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
