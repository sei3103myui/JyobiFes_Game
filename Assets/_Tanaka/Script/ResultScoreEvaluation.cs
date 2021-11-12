using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultScoreEvaluation : MonoBehaviour
{
    // テンプレート

    public Text text;

    public GameManager gameManger;

    private float score;


    void Start()
    {
        
    }

    void Update()
    {
        score = gameManger.score;

        if(score >= 1500)
        {
            text.text = "S";
        }
        else if(score >= 1000)
        {
            text.text = "A";
        }
        else  if(score >= 500)
        {
            text.text = "B";
        }
        else
        {
            text.text = "C";
        }
    }
}
