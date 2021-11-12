using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TextまたはImageのFadeIn/FadeOutを制御するスクリプト
/// </summary>
public class FadeUI : MonoBehaviour
{
    public enum RepeatType { NoRepeat, OneRepeat, Repeat };
    enum FadeType { FadeIn, FadeOut, Delay };

    public MaskableGraphic targetUi;
    [Range(0, 1)] public float startAlpha = 1;
    [Range(0, 1)] public float endAlpha = 0f;
    public float startDelayTime;
    public float fadeTime = 1f;
    public RepeatType repeatType;
    public float repeatDelayTime; // FadeIn/FadeOutの切り替える時間
    public bool isAutoDestroy;
    public bool isAutoActive;

    float oneSecondVal; // 1秒間の変動値
    float alpha; // カラーのα値
    float alphaMin; // alpha値のmin値
    float alphaMax; // alpha値のmax値

    FadeType fadeType;
    int repeatCount;
    bool isStart = true;
    bool IsStart
    {
        get { return isStart; }
        set
        {
            isStart = value;
            if (isAutoActive)
            {
                targetUi.gameObject.SetActive(isStart);
            }
        }
    }

    bool isEnd;
    bool IsEnd
    {
        get { return isEnd; }
        set
        {
            isEnd = value;
            // 自動削除
            if (isEnd && isAutoDestroy)
            {
                Destroy(gameObject);
            }
            // 自動Active
            if (isAutoActive)
            {
                targetUi.gameObject.SetActive(!isEnd);
            }
        }
    }

    void Awake()
    {
        // fadeIn/fadeOutの設定
        if (startAlpha < endAlpha)
        {
            fadeType = FadeType.FadeIn;
            alphaMin = startAlpha;
            alphaMax = endAlpha;
        }
        else
        {
            fadeType = FadeType.FadeOut;
            alphaMin = endAlpha;
            alphaMax = startAlpha;
        }

        // 1秒間の変動値計算
        oneSecondVal = (endAlpha - startAlpha) / fadeTime;

        // 初期のalpha値設定
        alpha = startAlpha;
        SetAlpha(targetUi, alpha);

        // ディレイ処理
        if (0 < startDelayTime)
        {
            IsStart = false;
            _ = DelayMethod(startDelayTime, () =>
            {
                IsStart = true;
            });
        }
    }

    void Update()
    {
        if (!isStart) return;
        if (IsEnd) return;
        // 終了||ループ処理
        if (fadeType == FadeType.FadeIn)
        {
            alpha += oneSecondVal * Time.deltaTime;
            SetAlpha(targetUi, alpha); // alpha値の変更
            if (alphaMax < alpha)
            {
                if (repeatType != RepeatType.NoRepeat)
                {
                    // 1リピートしたら終了
                    if (repeatType == RepeatType.OneRepeat)
                    {

                        if (0 < repeatCount)
                        {
                            IsEnd = true;
                        }
                        else
                        {
                            repeatCount++;
                        }
                    }

                    // 一定時間後に反転する
                    fadeType = FadeType.Delay;
                    _ = DelayMethod(repeatDelayTime, () =>
                    {
                        fadeType = FadeType.FadeOut; // fadeType反転
                        oneSecondVal *= -1; // 変動値反転
                    });
                }
                else
                {
                    IsEnd = true; // リピートしないなら終了
                }
            }

        }
        else if (fadeType == FadeType.FadeOut)
        {
            alpha += oneSecondVal * Time.deltaTime;
            SetAlpha(targetUi, alpha); // alpha値の変更
            if (alpha < alphaMin)
            {
                if (repeatType != RepeatType.NoRepeat)
                {
                    // 1リピートしたら終了
                    if (repeatType == RepeatType.OneRepeat)
                    {
                        if (0 < repeatCount)
                        {
                            IsEnd = true;
                        }
                        else
                        {
                            repeatCount++;
                        }
                    }

                    // 一定時間後に反転する
                    fadeType = FadeType.Delay;
                    _ = DelayMethod(repeatDelayTime, () =>
                    {
                        fadeType = FadeType.FadeIn; // fadeType反転
                        oneSecondVal *= -1; // 変動値反転
                    });
                }
                else
                {
                    IsEnd = true; // リピートしないなら終了
                }
            }
        }
    }

    /// <summary>
    /// 第一引数のコンポーネントのcolorを変更する
    /// </summary>
    /// <param name="comp"></param>
    /// <param name="colorAlpha"></param>
    void SetAlpha<T>(T comp, float colorAlpha) where T : MaskableGraphic
    {
        comp.color = new Color(comp.color.r, comp.color.g, comp.color.b, colorAlpha);
    }

    /// <summary>
    /// マルチスレッド
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    async Task DelayMethod(float waitTime, Action action)
    {
        await Task.Delay((int)(waitTime * 1000)); // 別スレッドのディレイが終了するのを待つ
        action();
    }
}
