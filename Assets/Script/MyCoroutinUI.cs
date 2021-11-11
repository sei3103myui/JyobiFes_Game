using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCoroutinUI : MyCoroutine
{
    /// <summary>
    /// SetSelectedgameObject
    /// 1フレームずらすことでOnSelect関数が呼ばれる
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IEnumerator SetSelectedGameObject(GameObject obj)
    {
        //ずらさないと、OnSelect関数が呼ばれないため
        yield return null;
        //開始位置を指定
        EventSystem.current.SetSelectedGameObject(obj);

    }
}
