using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public float radius = 2;
    public float speed = 100;
    private Vector3 startPos;
    private float angle;

    public RectTransform rect;

    // 追加項目
    public float timeSpan; // ランダム処理を実行させる間隔
    public int maxReticleSpeed = 800; // 最大値 (レティクルのスピード)
    public int minReticleSpeed = 300; // 最小値(レティクルのスピード)
    public int maxSpanTime = 2; // 最大値（スピードが変わる間隔）
    public int minSpanTime = 0; // 最小値（スピードが変わる間隔）
    public bool isFunc; // 一度だけ呼びたい処理

    private float timeCount; // 時間計測用
    private int reticleMoveSpeed; // ランダムで出力したスピード値を格納する変数

    void Start()
    {   
        // 円移動の中心地点を決める
        startPos = rect.transform.localPosition;
    }

    private void OnEnable()
    {   
        if(startPos != null)
        {
            rect.transform.localPosition = startPos;
        }
 
    }

    private void FixedUpdate()
    {

        if (!GameManager.isShoot)
        {
            // 速度が一定ずつ加算
            angle += Time.deltaTime * RandomGenerator();
            // 何度からラジアンに変換
            var rad = angle * Mathf.Deg2Rad;
            // 何度をInt型でキャスト
            // 角度 / 360 の余りが0の場合は1、余りが出た場合-1を返す
            // ? の左辺を評価、Trueなら : の左辺、Falseなら右辺
            // 余りに関して、castしてるので360過ぎるまで0が返る
            // 右回転、左回転を交互に繰り返す
            var f = (int)angle / 360 % 2 == 0 ? 1 : -1;
            // Mathf.Signで正か負かを判定
            // 1、つまりTrueなのか
            // ↑の?と同じことを実行
            // radius = 範囲なのかな?、それを正or負で返す
            var diffPos = Mathf.Sign(f) == 1 ? -radius : radius;
            // 自身の座標に対して代入
            // 円周上の座標を求めるには、半径 * cosラジアン, 半径 * sinラジアン
            rect.transform.localPosition =
                startPos +
                new Vector3(
                    // 楕円を作ってる?
                    // 
                    Mathf.Cos(rad) * radius * f + diffPos,
                    // 半径の分(100なら-100 ~ 100)
                    Mathf.Sin(rad) * radius
                    );


            // Sin関数は「半径１の棒に、Timeの糸をぐるぐると巻き付けて、巻き終わった位置を教えてくれる」
            // 時間は進み続け線で表すと直線になる、それを円にするには
            // Sin関数は-1 ~ 1の値(常に)
        }

    }

    // @追加
    /// <summary>
    /// ランダム生成用の関数
    /// </summary>
    private int RandomGenerator()
    {
        if (!isFunc)
        {
            // ランダムな数値を格納
            timeSpan = Random.Range(minSpanTime, maxSpanTime);
            Debug.LogWarning("■ランダムの間隔" + timeSpan);
            isFunc = true;
        }

        timeCount += Time.deltaTime;

        // ランダムで出力した間隔になったら実行
        if (timeCount >= timeSpan)
        {
            // スピードのランダム設定 
            // 指定した閾値でランダムのスピード値を格納する
            reticleMoveSpeed = Random.Range(minReticleSpeed, maxReticleSpeed);
            Debug.LogWarning("△ランダムなスピード" + reticleMoveSpeed);
            isFunc = false;
            // カウントの初期化
            timeCount = 0;
        }

        // ランダムで出力したスピード値を返す
        return reticleMoveSpeed;
    }


}
