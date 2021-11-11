using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleImageMoveONClick : MonoBehaviour
{   
   
    public RectTransform rect;
    public Canvas canvas;
    // テンプレート
    void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(rect.transform.position);
        //Ray ray = new Ray(Camera.main.transform.position, new Vector3(0.5f, 0.2f, 1f));

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        Debug.Log(ray.origin + "origin");
        Debug.Log(ray.direction + "dir");
    }

    Vector2 GetUIScreenPos(RectTransform rt)
    {

        Debug.Log(rt);
        //UIのCanvasに使用されるカメラは Hierarchy 上には表示されないので、
        //変換したいUIが所属しているcanvasを映しているカメラを取得し、 WorldToScreenPoint() で座標変換する
        Debug.Log(canvas.worldCamera.name + "worldCameraある");
        
        return RectTransformUtility.WorldToScreenPoint(Camera.main, rt.position);

    }
}
