using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreebieController : UnitBase
{

    public TextMesh childObjText;
    public ItemMoveControler itemMoveCtn;

    private Rigidbody rb;

    

    private void Awake()
    {
        childObjText.text = point.ToString();
        rb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
        itemMoveCtn = GetComponent<ItemMoveControler>();
    }

    protected override void Death()
    {
        // 死んだらManagerにスコアを加算する処理を実行 + 自分のScore分
        // 吹っ飛ぶ
        // ここが可笑しい
        // transformを移動させる、
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        StartCoroutine(MyCoroutine.Delay(1, () =>
        {
            GameManager.Instance.AddScore(point);
            // 対応するUIのSetActiveをFalseにする
            GameManager.isChoiceDown = true;
        }));
       
        // 自身のSetActiveをFalseにする(消すとList管理が面倒)
    }

    // 当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        // こっちに付けると毎回ステージと判定するので無し
    }
}
