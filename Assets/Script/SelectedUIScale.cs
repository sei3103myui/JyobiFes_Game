using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// このスクリプトをアタッチしたオブジェがSelectableを持ったオブジェだけ
/// ないオブジェには勝手につけてくれる(Selectable)
/// </summary>
[RequireComponent(typeof(Selectable))]
public class SelectedUIScale : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler
{
    public RectTransform targetRect; //スケールを変更するRect
    public float selectedScale = 1.1f; //選択された時のスケール
    public float pressedScale = 1.1f; //押された時のスケール

    private Vector2 defaultScale; //通常時のスケール

    private void Awake()
    {
        //初期スケールを取得
        defaultScale = targetRect.localScale;
    }


    /// <summary>
    /// このUIが非表示になった時に呼び出される
    /// </summary>
    private void OnDisable()
    {
        //スケールを元に戻す
        targetRect.localScale = defaultScale;

    }


    /// <summary>
    /// このUIが選択された時呼び出されます
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        //スケール変更
        targetRect.localScale = defaultScale * selectedScale;
    }

    /// <summary>
    /// このUIがクリックされた時呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //スケール変更
        targetRect.localScale = defaultScale * pressedScale;
    }
    /// <summary>
    /// このUIがクリックされて離された時呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //スケール変更
        targetRect.localScale = defaultScale;
    }
    /// <summary>
    /// このUIにカーソルが入った時に呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //スケール変更
        targetRect.localScale = defaultScale * selectedScale;
    }

    /// <summary>
    /// このUIからカーソルが出た時に呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //スケールを元に戻す
        targetRect.localScale = defaultScale;
    }

    /// <summary>
    /// このUIの選択が解除された時
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        //スケールを元に戻す
        targetRect.localScale = defaultScale;
    }
}
