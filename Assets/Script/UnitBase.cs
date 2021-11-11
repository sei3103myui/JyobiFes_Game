using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public abstract class UnitBase : MonoBehaviour
{

    public float unitHp; // ユニットの現在の体力
    public float unitMaxHp = 1000; // ユニットの最大体力

    // この景品が落とされた時に取得できるポイント

    public int unitNum;

    public int point = 0;

    // テンプレート
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        // 体力を最大値に合わせる
        unitHp = unitMaxHp;
    }


    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    /// <param name="buff"></param>
    /// <returns></returns>
    public virtual void HitDamage(float damage)
    {
        // HPが0よりも低くならないように制限する処理
        unitHp = Mathf.Clamp(unitHp - damage, 0, unitMaxHp);

        // HPが0になったら
        if (unitHp <= 0)
        {
            // 死亡処理を呼ぶ
            Death();
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected abstract void Death();



}
