using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTicketController : MonoBehaviour
{   

    

    private const int TICKET_NUM_PATTERN_1 = 1;
    private const int TICKET_NUM_PATTERN_2 = 2;

    private const int TICKET_NUM_PATTERN_3 = 3;
    // 使われる枚数の入力
    [Header("枚数 : 2 , 3 , 4")]
    public int ticeketNum;

    // プレイする際に貰える弾数


    private int playBulletNum;
    public Text ticketText;

    private void Start()
    {
        // 何枚かの反映
        ticketText.text = ticeketNum.ToString() + "枚";



    }

    /// <summary>
    /// OnClickで呼ばれる関数
    /// </summary>
    /// <param></param>
    public void SelectTicketClick()
    {
        
        // gameMangerのInstance;
        GameManager gameManger = GameManager.Instance;
        Debug.Log(gameManger);
        // チケットナンバーに応じて枚数を変更
        if (gameManger)
        {
            // 見つかったなら
            int cnt = TicketNumBullet(ticeketNum);
            // 返ってきた弾数をbulletCntに変更
            gameManger.bulletCnt = cnt;

            // 弾設定終わったのでboolをtrueへ
            gameManger.isBulletSet = true;
            // テキストの変更
            gameManger.startText.text = "Spaceキーでスタート!";

            // 設定が終わったのでGameManagerにあるParentSetActiveをfalse
            gameManger.startPointButtonParent.SetActive(false);
            
        }

        

    }

    private int TicketNumBullet(int ticket)
    {
        int retunCnt = 0;
        // 枚数に応じての弾数を返却
        switch (ticket)
        {
            case TICKET_NUM_PATTERN_1:
                retunCnt =  5;
                break;
            case TICKET_NUM_PATTERN_2:
                retunCnt = 12;
                break;
            case TICKET_NUM_PATTERN_3:
                retunCnt = 30;
                break;
            default:
                break;
        }

        return retunCnt;



    }

    
}
