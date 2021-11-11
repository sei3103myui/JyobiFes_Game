using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreebieController : UnitBase
{
    protected override void Death()
    {
        // 死んだらManagerにスコアを加算する処理を実行 + 自分のScore分
        GameManager.Instance.AddScore(point);
        // 対応するUIのSetActiveをFalseにする
        GameManager.isChoiceDown = true;
        // 自身のSetActiveをFalseにする(消すとList管理が面倒)
    }

    // 当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        // こっちに付けると毎回ステージと判定するので無し
    }
}
