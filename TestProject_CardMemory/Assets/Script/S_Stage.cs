using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Stage : MonoBehaviour
{
    static public string[][] stage = new string[][]
    {
        // 1스테이지
        new string[]
        {
            "     .*.-.*     ",
            "     /......     ",
            "     .*.-.*     "
        },

        // 2스테이지
        new string[]
        {
            "     .*.-.*.-.*.-.*     ",
            "     /..............     ",
            "     .*.-.*.-.*.-.*     "
        },

        // 3스테이지
        new string[]
        {
            "     .*.-.*.-.*.-.*.-.*.-.*     ",
            "     /......................     ",
            "     .*.-.*.-.*.-.*.-.*.-.*     ",
            "     /......................     ",
            "     .*.-.*.-.*.-.*.-.*.-.*     "
        },
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
