using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotPrefabController : MonoBehaviour
{
    public Text textUI;
    // Prefabに付けるコントローラー
    // Rigidbody
    private Rigidbody rb;

    // 弾速
    public float speed;

    // 攻撃力
    public int attack;

    // ぶつけたいレイヤー
    public LayerMask hitLayer;

    // 透明にさせたいレイヤー(当たってほしくないレイヤー)
    public LayerMask ignoreLayer;
    // テンプレート

    public bool isDebug;

    private void Awake()
    {
        // Rigidbody取得
        rb = GetComponent<Rigidbody>();
        textUI = GameManager.Instance.textHit;
    }
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (!isDebug)
        {
            // 自身のRigidbodyに加算
            if (rb.velocity.magnitude < 30f)
            {
                rb.AddForce(transform.forward * speed);
            }
        }
     
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // まず当たったobjのレイヤーを見る
        // ビット演算
       
        Debug.Log(other.gameObject.name + "気ミ");
        int layer = 1 << other.gameObject.layer;
        Debug.Log("当たったLayer" + layer);
        if(layer != ignoreLayer)
        {
            // 対応するレイヤーなら
            // Unitbaseを取得
            // さぁ楽しくなってまいりました
            
            // LayerMaskだとビットなので変換必須な為面倒なのでString検索にします
            // Layermaskめんどくさ

            Debug.Log(hitLayer.value);
            if(layer == hitLayer)
            {
                // しょうがない、これで
                textUI.gameObject.SetActive(true);
           
                var unit = other.transform.parent.gameObject.GetComponent<UnitBase>();
                // もしUnitが取得できたなら
                if (unit)
                {
                    Debug.Log("ヒット");
                    unit.HitDamage(attack);
                    var itemCon = unit.gameObject.GetComponent<FreebieController>().itemMoveCtn;
                    itemCon.isMove = true;
                }
                
            }
            
         
        }

        // 自身の弾を削除
        Destroy(gameObject);

    }
}
