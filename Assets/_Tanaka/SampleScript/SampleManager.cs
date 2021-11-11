using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{   
    // ManagerはInstanceをし、生成したobjに渡すではなく、objのStartでInstanceし渡すように変更(objBase)

    // 制限時間
    [Header("制限時間")]
    public int limitTime;
    private float time;

    // スコア
    [Header("スコア")]
    public int scoreCount;

    [Header("開始のbool")]

    public bool isStart;
    public bool isEnd;


    [Header("開始UIのObject")]
    public GameObject startUIParent;

    [Header("終了UIのObject")]
    public GameObject endUIParent;
    // 
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        isStart = false;
        scoreCount = 0;

        // 終了UIの非表示
        endUIParent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
            if(time > limitTime)
            {
                time = limitTime;
                isStart = false;

                // 終了UIの表示
                endUIParent.SetActive(true);
            }
        }
    }


    public void ClickStart()
    {   
        // StartUIのObjをOFF
        startUIParent.SetActive(false);
    }

    public void ClickEnd()
    {
        
    }

    public void ClickRestart()
    {   
        // シーンの再読み込み
        SceneManager.LoadScene("");
    }
}
