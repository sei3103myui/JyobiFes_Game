using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


/// <summary>
/// フェイズ管理
/// </summary>
public enum Phase 
{
    StandBy, // ゲーム開始前
    Choice, // 景品選択
    ChoiceGaze, // 選択後の注視
    Ready, // Shoot移行前の待機
    Shoot, // 射撃待機
    ShootEnd, // 射撃終了
    Result // リザルト
}

public class GameManager : Singleton<GameManager>
{
    public Text textHit;
    //[SerializeField] private PlayerInput playerInput;
    [Header("ゲームフェイズ管理")]
    public Phase gamePhase;

    [Header("スタート画面の設定値")]
    [SerializeField] private GameObject startPanel; // スタート画面で表示するパネル
    [SerializeField] private Text startText;　// スタート画面のテキスト
    [SerializeField] private Text countDownText; // カウントダウン用のテキスト


    [Header("ゲーム中のUI")]
    [SerializeField] private Text scoreText;

    public int score = 0;
    private const int MAX_SOCRE = 9999;

    // 現在選択している景品のナンバー
    public int choiceNum;

    // 弾数
    public int bulletCnt;
    [System.NonSerialized] public int maxBulletCnt;


    [Header("射撃フェイズ前の待機時に使用する設定値")]
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private Text readyText;
    
    [Header("リザルト画面の設定値")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text resultScoreText;
    
    // ルーチン
    private IEnumerator countDownRoutine; // カウントダウンを行うルーチン格納用
    private IEnumerator readyRoutine; // Shoot移行前の待機ルーチン格納用
    private IEnumerator shootEndRoutine; // 射撃終了後のルーチン格納用

    // 撃ったならTrueにする
    public static bool isShoot;
    // 的が倒れるかのBool
    public static bool isChoiceDown;

    private bool isReset;

    [Header("ImagePosManagerを格納")]
    public UIImagePosManager uiImagePosManager;

    // レティクルを格納
    [Header("ReticleImageを格納")]
    public ReticleController reticleController;


    // inputSystem
    //[SerializeField] private string spaceInputName;
    //private InputAction spaceInputAction;

    // 初期値設定
    private void Awake()
    {
        // フェイズ
        gamePhase = Phase.StandBy;
        maxBulletCnt = bulletCnt;
        // UIの初期表示状態
        startPanel.SetActive(true);
        startText.gameObject.SetActive(true);

        countDownText.gameObject.SetActive(false);
        resultPanel.SetActive(false);
        
        scoreText.text = score.ToString();
        resultScoreText.text = score.ToString();
    }

    // テンプレート
    void Start()
    {
        // InputSystemを使用する場合
        //spaceInputAction = playerInput.actions.FindAction(spaceInputName);

        //spaceInputAction.performed += (InputAction.CallbackContext context) =>
        //{
        //    //カウントダウン用のコルーチン
        //    countDownRoutine = CountdownRoutine();
        //StartCoroutine(CountdownRoutine());
        //};
        //AudioManager.Instance.PlayClip(0);
    }

    // ゴリ押しなので処理は汚い
    // 綺麗にするのは時間があったら
    void Update()
    {
        // スペースキーが押されたかつ現在のフェイズがStandByなら
        if (Keyboard.current.spaceKey.wasReleasedThisFrame &&
            gamePhase == Phase.StandBy)
        {   
            if(countDownRoutine == null)
            {
                countDownRoutine = CountdownRoutine();
                StartCoroutine(CountdownRoutine());
            }
        }

        
        // これだと、毎回Update判定入るが急遽なのでこのままで
        // 現在のフェイズによって処理を変更する
        switch (gamePhase)
        {

            case Phase.StandBy:
                break;
            case Phase.Choice:

                startPanel.SetActive(false);
                
                break;

            case Phase.ChoiceGaze:
                // 注視してる場合
                break;
            case Phase.Shoot:
                // Inputで射撃をONにする
                ShootAwait();
                break;

            case Phase.ShootEnd:
                // 再挑戦するか、やり直すか表示
                // 倒した場合、強制的にChoice、弾が0になったらResultへ移行する管理関数
                
                
                break;
            case Phase.Result:

                resultPanel.SetActive(true);
                break;
        }


        // テスト用

        // スコアを加算する
        // 条件: 景品を獲得したとき
        // 実際の挙動はできた為削除して良き
        if (Keyboard.current.aKey.wasReleasedThisFrame)
        {
            Debug.LogError("スコアが入りました");
            AddScore(1000);
        }

        // リザルトの表示
        // 条件: 全弾撃ち終えたら
        // 実際の挙動はできた為削除して良き
        if (Keyboard.current.cKey.wasReleasedThisFrame)
        {
            Debug.LogError("リザルトを表示します");
            gamePhase = Phase.Result;

        }

        // シーンの再読み込み
        // 条件: リザルト画面でもどるボタンが押されたとき
        // 実際の挙動はできた為削除して良き
        if (Keyboard.current.rKey.wasReleasedThisFrame)
        {
            Debug.LogError("最初に戻る");
            OnPushBackButton();
        }

        // Shoot移行前の待機ルーチン格納用
        // 条件: 景品を選択して射撃フェイズに移行するタイミング
        // 関数に移動した為削除して良き
        if (Keyboard.current.oKey.wasReleasedThisFrame)
        {
            Debug.LogError("Readyフェイズ読み込み");
            
        }

    }


   /// <summary>
   /// 
   /// </summary>
   /// <param name="num">枚数</param>
    public void SelectSheetClick(GameObject obj)
    {

    }

    /// <summary>
    /// ボタン(遊ぶ)に追加する関数
    /// </summary>
    public void ShootStart()
    {
        if (readyRoutine == null)
        {
            ChoiceClick();
        }
    }

    public void ShootReset()
    {   
        // 撃ったUIはなくす
        uiImagePosManager.parentShotUIObj.SetActive(false);
        // このIF文ひどすぎる
        if (bulletCnt > 0)
        {
            if (isChoiceDown)
            {
                isChoiceDown = false;
                gamePhase = Phase.Choice;
                
               
                uiImagePosManager.FreebieUISetActiveChange(choiceNum);
                uiImagePosManager.CMShotCam.gameObject.SetActive(false);
                // Choiceになったらやりたい関数
                StartCoroutine(MyCoroutine.Delay(uiImagePosManager.uiAnimationSpeed, () =>
                {

                    // FreebieのUIをONにする

                    uiImagePosManager.parentFreebieUIObject.SetActive(true);

                }));
                
            }
            else
            {
                gamePhase = Phase.ChoiceGaze;
                uiImagePosManager.parentChoiceUIObj.SetActive(true);
                // 注視になったらやりたい関数
            }
        }
        else
        {
            gamePhase = Phase.Result;
            
        }
        
        
        
    }

    public void ShootAwait()
    {   
        // 射撃ボタン
        // コントローラーの場合修正
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isShoot)
        {
            bulletCnt--;
            Debug.Log("撃ったよ");
            // もし撃つボタンが押されたなら
            // ReticleのImageを止める
            isShoot = true;
            // Imageから弾を発射させる
            uiImagePosManager.ShootPrefabInstantiate();
            // ShootEndへ移行
            
            // 
            if(shootEndRoutine == null)
            {
                shootEndRoutine = EndRoutine();
                StartCoroutine(MyCoroutine.Delay(5, () =>
                {
                    gamePhase = Phase.ShootEnd;
                    textHit.gameObject.SetActive(false);
                    StartCoroutine(shootEndRoutine);
                }));

            }
        }
    }

    /// <summary>
    /// 景品選択後のスタート押された際の関数
    /// </summary>
    public void ChoiceClick()
    {
        AudioManager.Instance.StopBGM();
        gamePhase = Phase.Ready;
        uiImagePosManager.parentChoiceUIObj.SetActive(false);
        uiImagePosManager.parentShotUIObj.SetActive(true);
        if(choiceNum == 4)
        {
            // bgmをアンパンマンへ
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayBGM(1);
        }
        else if (choiceNum == 0)
        {
            AudioManager.Instance.PlayBGM(2);
        }
        else
        {
            AudioManager.Instance.StopBGM();
            // 通常bgmを再生
            AudioManager.Instance.PlayBGM(3);
        }

        // choiceNumに対応するobjのLayerを変更
        uiImagePosManager.FreebieChoiceLayerChange(choiceNum);

        readyRoutine = ReadyRoutine();
        StartCoroutine(readyRoutine);
    }

    /// <summary>
    /// スコア加算,テキスト更新用のメソッド
    /// </summary>
    /// <param name="val">加算するスコア</param>
    public void AddScore(int val)
    {
        // スコアの閾値制限
        score = Mathf.Clamp(score + val, 0, MAX_SOCRE);
        // テキストの更新
        scoreText.text = score.ToString();
        resultScoreText.text = score.ToString();

        uiImagePosManager.FreebieUISetActiveChange(choiceNum);
    }

    /// <summary>
    /// リザルトでもどるボタンが押されたとき実行されるメソッド
    /// </summary>
    public void OnPushBackButton()
    {
        // ボタンSEを再生させるためディレイさせてから実行
        StartCoroutine(MyCoroutine.Delay(1.5f ,() => 
        {
            // 現在のシーンを再読み込みをする
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }));
    }



    /// <summary>
    /// カウントダウンの処理
    /// </summary>
    /// <returns>1秒ごとにテキストを更新</returns>
    IEnumerator CountdownRoutine()
    {
        // UIの表示非表示
        countDownText.gameObject.SetActive(true);
        startText.gameObject.SetActive(false);

        AudioManager.Instance.PlaySE(0);
        // カウントダウンをテキストに変更する
        countDownText.text = "3";
        yield return new WaitForSeconds(1.0f);

        countDownText.text = "2";
        yield return new WaitForSeconds(1.0f);

        countDownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        countDownText.text = "スタート！";
        yield return new WaitForSeconds(1.0f);

        countDownText.text = "";

        
        // カウントダウンが終了したら
        gamePhase = Phase.Choice;
        uiImagePosManager.parentFreebieUIObject.SetActive(true);

        // routineの解放
        countDownRoutine = null;
    }

    /// <summary>
    ///  景品を選択した後読み込まれる待機ルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ReadyRoutine()
    {
        readyPanel.SetActive(true);

        readyText.text = "レディー・・・";
        yield return new WaitForSeconds(1.0f);

        readyText.text = "ゴー！";
        yield return new WaitForSeconds(1.0f);

        readyText.text = "";

        // パネルを非表示
        readyPanel.SetActive(false);

        // 射撃フェイズに移行
        gamePhase = Phase.Shoot;
        
   
        // routineの解放
        readyRoutine = null;

    }

    IEnumerator EndRoutine()
    {
        isShoot = false;
        // 流石に5秒も待ったら終わってるはず
        ShootReset();
        shootEndRoutine = null;
        // 
        yield return null;
    }


    
}
