using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowUI : MonoBehaviour
{
    public Graphic targetGraphic;
    [Range(0.1f, 3f)] public float speed = 1f;

    private Color colorRGB;
    private int colorId;
    private float _speed;

    private void Start()
    {
        colorRGB = targetGraphic.color;
        _speed = speed;
    }

    private void Update()
    {
        // 次の色を計算
        colorRGB[colorId] = Mathf.Clamp01(colorRGB[colorId] + _speed * Time.deltaTime);
        if (colorRGB[colorId] <= 0 || 1 <= colorRGB[colorId])
        {
            colorId = (2 < ++colorId) ? 0 : colorId; // idの切り替え
            _speed *= -1; // 加算値の反転
        }

        // 色の更新
        targetGraphic.color = colorRGB;
    }
}
