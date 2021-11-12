using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.EventSystems;

public class UIImagePosManager : MonoBehaviour
{   
    // Canvas
    public Canvas canvas;

    // 遷移する秒数(ここはBrainの時間と同じ秒数とする)
    public float uiAnimationSpeed;

    // これでいいのか?
    //public RectTransform _imageRect;

    public ReticleController reticleController;

    //public RectTransform _imageSample;

    // Cinemachine用変数、景品選択の際のカメラ
    public CinemachineVirtualCamera CMStageCam;
    // 選択後、弾が発射されるカメラ
    public CinemachineVirtualCamera CMShotCam;

    public List<GameObject> freebieObjects;
    public List<GameObject> freebieUIObjects = new List<GameObject>();

    public GameObject parentFreebieUIObject;

    // 景品選択のUIの親にしたいオブジェ
 

    // 選択後のUIの親にしたいオブジェ
    public GameObject parentChoiceUIObj;

    // ショット状態で動かしたい親
    public GameObject parentShotUIObj;

    // 湧かせたいPrefab
    public GameObject instanceUIObject;

    // コルク(弾)のPrefab
    public GameObject shotPrefab;


    private Vector2 pos;

    private RectTransform canvasRect;
    public RectTransform sampleRect;
    private Camera mainCamera;

    public GameManager gameManger;
    private void Start()
    {   
        // カメラ移動時の時間の格納
        uiAnimationSpeed = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;
        Debug.Log(uiAnimationSpeed);
        // objによるUI作成
        canvasRect = canvas.GetComponent<RectTransform>();
        //canvas.worldCamera = Camera.main;
        mainCamera = Camera.main;
        CMShotCam.gameObject.SetActive(false);
        Init();
    }

    void Init()
    {

        parentShotUIObj.SetActive(false);
        parentChoiceUIObj.SetActive(false);

        
        // Listで景品の分回す
        for (int i = 0; i < freebieObjects.Count; i++)
        {
            // それぞれのobjによるUIのInstanceを開始
            var uiObj = WourldToScreenImageInstance(freebieObjects[i].transform.position, freebieObjects[i]);
            // UIを格納
            
            freebieUIObjects.Add(uiObj);
            var freebieController = freebieObjects[i].GetComponent<FreebieController>();
            freebieController.unitNum = i;
        }

        // UIの非表示
        parentFreebieUIObject.SetActive(false);


    }
    void Update()
    {
        //Debug.Log(Camera.main);
        // Ray ray = Camera.main.ScreenPointToRay(GetUIScreenPos(_imageRect));
        // Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        //RaycastHit hit = new RaycastHit();
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //    WourldToScreenImageInstance(hit.transform.position);
        //}

        // GameMangerのShootがOnになったらReticleから座標を取得して打つ

        // UIからRayを飛ばす
        //Ray ray = Camera.main.ScreenPointToRay(GetUIScreenPos(reticleController.rect));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        // FreebieChoiceLayerChange(1);

    }

    public void ExitClick()
    {

        // Cinemachineの切り替え

        CMShotCam.gameObject.SetActive(false);
        parentChoiceUIObj.SetActive(false);
        parentShotUIObj.SetActive(false);
        // ディレイをかけてそれっぽく
        StartCoroutine(MyCoroutine.Delay(uiAnimationSpeed, () =>
        {

            // FreebieのUIをONにする
            parentFreebieUIObject.SetActive(true);


        }));

    }

    public void FreebieIconClick(GameObject obj)
    {
        var worldPos = Vector3.zero;
        CMShotCam.gameObject.SetActive(true);
        parentFreebieUIObject.SetActive(false);

        // 選択した番号を格納
        gameManger.choiceNum = obj.GetComponent<FreebieController>().unitNum;

        StartCoroutine(MyCoroutine.Delay(uiAnimationSpeed, () =>
        {
            // parentFreebieUIObject.gameObject.SetActive(true);
            parentChoiceUIObj.SetActive(true);
           
            // 触ったらその座標にCMVcamを移動させてCMCamを切り替える作業
            // UIの座標をワールドへ変換
            // いらんな、対応するイベント
            // RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, obj.transform.position, null, out worldPos);
            Debug.Log("呼び出し");
           

        }));

        worldPos = obj.transform.position;
        worldPos = new Vector3(worldPos.x, worldPos.y,worldPos.z - 3);
        CMShotCam.transform.position = worldPos;
    }

    //UIの座標をスクリーン座標に変換する関数
    Vector2 GetUIScreenPos(RectTransform rt)
    {

        Debug.Log(rt);
        //UIのCanvasに使用されるカメラは Hierarchy 上には表示されないので、
        //変換したいUIが所属しているcanvasを映しているカメラを取得し、 WorldToScreenPoint() で座標変換する
        
        return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rt.position);

    }

    GameObject WourldToScreenImageInstance(Vector3 targetPos, GameObject targetObj)
    {

        // ターゲットオブジェからScriptによる番号Noを設定してI番と同じにする事で
        // 整合性を取る


        // 変数の初期化
        pos = Vector2.zero;




        // スクリーンから見た座標、詳しくは分からん
        // ワールド座標をスクリーン座標へ
        var screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, targetPos);
        //var screenPos = Camera.main.WorldToScreenPoint(targetPos + Vector3.zero);

        // 下でスクリーン座標をRectTransform座標へ変換、結果をposへ格納
        // おかしい
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out pos);
        // posはワールド座標

        var freebieImage = Instantiate(instanceUIObject, Vector3.zero, Quaternion.identity);

        // 湧かせたImageを対応する座標へ移動
        freebieImage.transform.parent = parentFreebieUIObject.transform;
        pos = new Vector2(pos.x, pos.y + 30f);
        freebieImage.transform.localPosition = pos;

        //// UIに対してイベントを設定するしかないか
        //var eventTrigger = freebieImage.AddComponent<EventTrigger>();
        //EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };

        var eventTrigger = freebieImage.GetComponent<Button>();
        eventTrigger.onClick.AddListener(() =>
        {
            FreebieIconClick(targetObj);
        });

        //entry.callback.AddListener((eventData) => FreebieIconClick(targetObj));

        //eventTrigger.triggers.Add(entry);

        // イベントシステムがnullだったら = 最初の生成の場合
        if(EventSystem.current.currentSelectedGameObject == null)
        {   
            
            EventSystem.current.SetSelectedGameObject(freebieImage);
            var setUI = parentFreebieUIObject.GetComponent<SetSelectedUI>();
            setUI.firstSelected = freebieImage;
        }

        // UIもListで作って管理する、そうすればNum云々は1つで終わる
        return freebieImage;

    }


    public void ShootPrefabInstantiate()
    {
        // 弾を生成、
        Ray ray = Camera.main.ScreenPointToRay(reticleController.rect.transform.position);
        var rot = Quaternion.LookRotation(ray.direction);

        var shotObj = Instantiate(shotPrefab, CMShotCam.transform.position, rot);
        // 弾発射おわーり

        // これだけか?
    }

    public void FreebieChoiceLayerChange(int num)
    {   
        // 名前指定するしかない、あとで修正

        var hitlayer = LayerMask.NameToLayer("hitLayer");

        var ignorelayer = LayerMask.NameToLayer("ignoreLayer");
        
        for(int i = 0; i < freebieObjects.Count; i++)
        {
            Debug.Log(i + "番目");
            if (i == num)
            {
                freebieObjects[i].gameObject.layer = hitlayer;
                freebieObjects[i].gameObject.transform.GetChild(0).gameObject.layer = hitlayer;
                //foreach(Transform child in freebieObjects[i].transform.GetChild(0).transform)
                //{
                //    child.gameObject.layer = hitlayer;
                //}

                continue;
            }
            
            freebieObjects[i].layer = ignorelayer;
            freebieObjects[i].gameObject.transform.GetChild(0).gameObject.layer = ignorelayer;
            //Debug.Log(freebieObjects[i].transform.childCount);
            //foreach(Transform child in freebieObjects[i].transform.GetChild(0).transform)
            //{   

            //    child.gameObject.layer = ignorelayer;
            //}
            
            
        }
    }

    /// <summary>
    /// 選択番号のobjのリストをfalse
    /// </summary>
    /// <param name="num"></param>
    public void FreebieUISetActiveChange(int num)
    {
        // freebieObjects[num].SetActive(false);
        freebieUIObjects[num].SetActive(false);
        freebieObjects[num].SetActive(false);
    }

    //public void FixedUpdate()
    //{
    //    Debug.Log(EventSystem.current.currentSelectedGameObject.transform.localPosition + "を見てるおやの名前");
    //}
}
