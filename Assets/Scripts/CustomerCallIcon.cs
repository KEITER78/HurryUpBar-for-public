using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
//using UnityEditor.Rendering.PostProcessing;

public class CustomerCallIcon : MonoBehaviour
{
    //public SoundManager soundManager; // SoundManager の参照を持つ
    public Animator animator;  // Animatorコンポーネントの参照を追加

    public GameObject callIcon;           // 呼出中アイコン（UIのImageオブジェクト）
    public Transform customer;            // 客の位置
    public Transform player;              // プレイヤーの位置
    public Transform PlayerCamera;        // PlayerCameraの位置
    public Transform neckBone; // Inspectorで首のボーンを指定, cf_s_headを割り当てる
    public bool isLookingAtPlayer = false; // プレイヤーの方を向くかどうかを管理
    public bool isRotationRestricted = false; // 回転を制限するフラグ
    private Vector3 currentLookAtPosition;  // 首のボーンのIK座標
    public float moveSpeed = 1f;  // 移動速度を設定（Inspectorから調整可能）
    public float rotationSpeed = 20f;  // 移動中の回転速度を設定（Inspectorで調整可能）
    public float neckRotationSpeed = 10f; // Inspectorで調整可能な首の回転速度
    public float seatArrivalDelay = 1f;  // 座席に着いてからの遅延時間（デフォルト1秒）
    public float paymentDepartureDelay = 1.5f;  // 会計後、移動開始までの遅延時間
    public GameObject callCanvas; // CallCanvasをInspectorでアサインする
    public float triggerDistance = 3f;    // プレイヤーが近づく距離の閾値
    public GameObject orderCanvas;        // 注文ウィンドウのCanvas
    public Image drinkImage;              // ドリンクの画像を表示するUI
    public Sprite[] drinkSprites;         // ドリンク画像（SakeA, SakeB, SakeCなどの画像）
    public float[] drinkPrices;           // 各ドリンクの価格を格納する配列

    public Sprite waitingIconSprite;      // 待機中アイコンの画像
    public Sprite callingIconSprite;      // 呼出中アイコンの画像
    public Sprite callingIconSprite_en;      // 呼出中アイコンの画像_en
    public Sprite completeMessageSprite;  // 提供完了メッセージの画像（Inspectorで指定）
    public Sprite completeMessageSprite_en;  // 提供完了メッセージの画像（Inspectorで指定）_en
    public Sprite failMessageSprite;      // 不一致メッセージの画像（Inspectorで指定）
    public Sprite failMessageSprite_en;      // 不一致メッセージの画像（Inspectorで指定）_en
    public Sprite orderMessageSprite;     // 「Eボタンで注文を受ける」メッセージの画像（Inspectorで指定）
    public Sprite recheckMessageSprite;   // 待機中に表示される「再確認」メッセージの画像（Inspectorで指定）
    public Sprite closeMessageSprite;     // 注文ウィンドウを閉じるメッセージの画像（Inspectorで指定）
    public Sprite reorderMessageSprite;   // 再注文時用のメッセージ画像（Inspectorで指定）
    public Sprite reorderMessageSprite_en;   // 再注文時用のメッセージ画像（Inspectorで指定）_en
    public Sprite paymentFailureSprite;   // 決済失敗メッセージ
    public Sprite paymentSuccessSprite;   // 決済成功メッセージ
    public Sprite paymentSuccessSprite_en;   // 決済成功メッセージ_en

    // メッセージ表示用のCanvasとImageを2つに分ける
    public GameObject messageCanvas1;     // Order, Recheck, Close用のCanvas
    public Image messageImage1;           // Order, Recheck, Close用のImage
    public TMP_Text messageText1;         // Order, Recheck, Close用のText
    public GameObject messageCanvas2;     // Complete, Fail用のCanvas
    public Image messageImage2;           // Complete, Fail用のImage

    // ServeMessageのSpriteと表示するCanvas、Imageを追加
    public Sprite serveMessageSprite;
    public GameObject messageCanvas3;
    public Image messageImage3;
    public TMP_Text messageText3;

    public float idleDurationMin = 10f; // Idle状態の最小持続時間（Inspectorで設定）
    public float idleDurationMax = 20f; // Idle状態の最大持続時間（Inspectorで設定）
    public Sprite payIconSprite;  // お会計状態を示すアイコン
    public Sprite payIconSprite_en;  // お会計状態を示すアイコン_en
    public Sprite payMessageSprite; // お会計時に表示されるメッセージの画像

    // 伝票用のCanvasとその中にある背景画像と金額テキスト
    public GameObject receiptCanvas;   // 伝票Canvas
    public Image receiptBackground;    // 伝票の背景画像
    public TextMeshProUGUI amountText;            // 金額を表示するテキスト
    public Button confirmButton;            // 請求額を決定するボタン

    public static float dailyRevenue = 0f; // 当日の売上金を保持する変数（プレイヤー全体で1つだけ必要）, 全ての客で共有するためにstaticにする
    public TextMeshProUGUI revenueText;    // 売上金を表示するTextMeshProの参照

    private Sprite selectedDrink;         // 客が選んだドリンク（個別に管理）
    private bool isOrderDisplayed = false;// 注文ウィンドウが表示されているかどうか
    private bool isDrinkSelected = false; // ドリンクが選択済みかどうか
    private bool isPlayerInRange = false; // プレイヤーがTrigger Distance内にいるかどうか
    private bool isWaiting = false;       // 待機中かどうかを管理
    private DrinkSelectionManager drinkSelectionManager;  // DrinkSelectionManagerの参照を保持する変数
    private bool isServeMessageDisplayed = false;  // ServeMessageの表示状態を個別に管理
    private bool isIdle = false; // Idle状態を管理するフラグ
    private List<string> drinkHistory = new List<string>(); // ドリンクの履歴を保存するリスト
    private int idleCount = 0; // Idle状態になった回数をカウント
    private bool isPaying = false; // お会計状態を管理するフラグ
    private bool isAwaitingPayment = false; // お会計状態を管理するフラグ
    private bool isSelectedCustomer = false; // 現在の客かどうかを示すフラグ
    private bool isReceiptDisplayed = false; // 伝票が表示されているかどうかを管理
    private bool isCalling = false; // 呼出中であるかを示すフラグ
    private bool isPayingInProgress = false; // お会計が進行中かどうかを示すフラグ（会計終了後にEを押して再び会計することを防ぐ）

    // 請求金額を表示するための変数とUI参照
    public TextMeshProUGUI billThousandsText;
    public TextMeshProUGUI billHundredsText;
    public TextMeshProUGUI billTensText;
    public TextMeshProUGUI billOnesText;

    public Button plusThousandsButton;
    public Button minusThousandsButton;
    public Button plusHundredsButton;
    public Button minusHundredsButton;
    public Button plusTensButton;
    public Button minusTensButton;
    public Button plusOnesButton;
    public Button minusOnesButton;

    private int billThousands = 0;
    private int billHundreds = 0;
    private int billTens = 0;
    private int billOnes = 0;
    private int billAmount = 0;  // 請求金額を保持する変数

    // 新たなメンバ変数としてメッセージ表示用の変数を追加
    public GameObject confirmationMessageCanvas; // 「本当に料金あってますか？」メッセージ用Canvas
    private bool isConfirmationMessageActive = false; // メッセージが表示中かどうかを管理

    // 支払いメニュー用の変数を追加
    public GameObject paymentMenuCanvas;    // 支払い選択メニューのCanvas
    public TextMeshProUGUI standardAmountText;  // 正規料金を表示するテキスト
    public TextMeshProUGUI billedAmountText;    // 請求金額を表示するテキスト
    public Button standardAmountButton;     // 正規料金を選択するボタン
    public Button billedAmountButton;       // 請求金額を選択するボタン

    // 不満度を表す変数とバーの参照
    public float satisfactionLevel = 0f;   // 不満度 (Inspectorで初期値設定可能)
    public Image satisfactionBar;            // 不満度バーのImageオブジェクト (Inspectorで設定)
    public Sprite normalSatisfactionBarSprite;    // 通常時の不満度バーの画像
    public Sprite blinkingSatisfactionBarSprite;  // 点滅時の不満度バーの画像
    public Image satisfactionBarBackground; // 不満度バー背景のImageオブジェクト
    public Sprite satisfactionBarBackgroundSprite; // 不満度バー背景の画像
    public Sprite satisfactionBarBackgroundSprite_en; // 不満度バー背景の画像_en
    private float satisfactionBlinkTimer = 0f;          // 点滅の残り時間を管理
    private Coroutine satisfactionBlinkCoroutine;       // ブリンク用のコルーチン参照
    private float satisfactionBlinkInterval1 = 0.25f;     // 点滅の間隔（点滅用画像の時間）
    private float satisfactionBlinkInterval2 = 0.4f;     // 点滅の間隔（通常用画像の時間）
    private float satisfactionBlinkDuration = 1.0f;     // 点滅を続ける時間
    private float previousSatisfactionLevel;            // 前フレームの不満度を保持

    // Inspectorで設定可能な呼出中に関する時間、減少速度、アイコンなどを追加
    public float callDecayStartTime = 10f; // 不満度が増加し始める時間
    public float fastCallDecayStartTime = 20f; // 不満度が速く増加し始める時間
    public float callDecaySpeed = 1f; // 通常の増加スピード
    public float fastCallDecaySpeed = 2f; // 速く増加するスピード
    public Sprite decayingCallIcon; // 増加が始まった時のアイコン
    public Sprite fastDecayingCallIcon; // 速く増加する時のアイコン
    public Sprite decayingCallIcon_en; // 増加が始まった時のアイコン_en
    public Sprite fastDecayingCallIcon_en; // 速く増加する時のアイコン_en

    // 呼出中に関する時間を追跡する変数
    private float callTimeElapsed = 0f;
    private bool isCallDecaying = false;
    private bool isFastCallDecaying = false;

    // Inspectorで設定可能な待機中に関する時間、減少速度、アイコンなどを追加
    public float waitDecayStartTime = 15f; // 不満度が増加し始める時間 (待機中)
    public float fastWaitDecayStartTime = 25f; // 不満度が速く増加し始める時間 (待機中)
    public float waitDecaySpeed = 0.8f; // 通常の増加スピード (待機中)
    public float fastWaitDecaySpeed = 1.6f; // 速く増加するスピード (待機中)
    public Sprite decayingWaitIcon; // 待機中で増加が始まった時のアイコン
    public Sprite fastDecayingWaitIcon; // 待機中で速く増加する時のアイコン

    // 待機中に関する時間を追跡する変数
    private float waitTimeElapsed = 0f;
    private bool isWaitDecaying = false;
    private bool isFastWaitDecaying = false;

    public float awaitingPaymentDecayStartTime = 10f;  // 支払い待ちが減少し始める時間
    public float fastAwaitingPaymentDecayStartTime = 20f;  // 支払い待ちが速く減少し始める時間
    public float awaitingPaymentDecaySpeed = 1f;  // 支払い待ちの通常増加速度
    public float fastAwaitingPaymentDecaySpeed = 2f;  // 支払い待ちの速い増加速度
    public Sprite decayingAwaitingPaymentIcon;  // 支払い待ち増加時のアイコン
    public Sprite fastDecayingAwaitingPaymentIcon;  // 支払い待ち速く増加時のアイコン
    public Sprite decayingAwaitingPaymentIcon_en;  // 支払い待ち増加時のアイコン_en
    public Sprite fastDecayingAwaitingPaymentIcon_en;  // 支払い待ち速く増加時のアイコン_en

    private float awaitingPaymentTimeElapsed = 0f;  // 支払い待ち時間経過
    private bool isAwaitingPaymentDecaying = false;  // 支払い待ち通常減少フラグ
    private bool isFastAwaitingPaymentDecaying = false;  // 支払い待ち速い減少フラグ

    public float drinkMatchSatisfactionIncrease = 10f;  // ドリンク一致時の不満度増加量
    public float drinkMismatchSatisfactionDecrease = 15f;  // ドリンク不一致時の不満度増加量
    public float recheckSatisfactionDecrease = 5f; // 再注文確認時の不満度増加量

    public Transform spawnPoint; // 入り口の位置をInspectorから指定
    public float spawnDelay = 5f; // スポーンまでの遅延時間（Inspectorで設定）
    public int idleMaxCount = 3; // Idle状態の最大回数を設定
    private bool isSpawned = false; // 客がスポーンしたかどうかのフラグ
    private SeatManager seatManager; // SeatManagerへの参照
    private int selectedSeatIndex;    // 選択された座席のインデックス

    public Canvas conversationCanvas; // 会話用のCanvas（Inspectorで指定）
    public Image conversationBackground; // 会話ウィンドウの背景（Inspectorで指定）
    public TextMeshProUGUI conversationText; // 会話メッセージを表示するTextMeshPro（Inspectorで指定）
    public string[] conversationMessages; // 会話内容を保存する配列
    private int conversationIndex = 0; // 会話の現在のインデックス
    private bool isConversing = false; // 会話中かどうかを管理するフラグ
    private System.Action onConversationEndCallback; // 会話終了時のコールバック
    private bool isTyping = false; // タイピング中かどうかを示すフラグ
    public float typingSpeed = 0.05f; // 文字を1文字ずつ表示する速度を指定（Inspectorから設定可能）
    private bool isConversationJustStarted = false;
    private Coroutine typingCoroutine; // コルーチンの参照を保存する変数

    private static Queue<CustomerCallIcon> waitingCustomers = new Queue<CustomerCallIcon>(); // 待機中の客を管理するキュー

    private bool wasRotationRestricted = false; // 前フレームの状態を記録

    // 静的カウンターを追加
    public static int totalCustomers = 0;
    public static int completedCustomers = 0;

    // ゲームオーバー用のCanvasと関連するUI要素
    public GameObject gameOverCanvas; // ゲームオーバーCanvas
    public Image gameOverBlackImage; // 画面全体を覆う黒い画像
    public TextMeshProUGUI gameOverText; // "ゲームオーバー" と表示するテキスト
    public Button returnToMenuButton; // メニューに戻るボタン
    private static bool isGameOver = false; // ゲームオーバーが発生したかどうかのフラグ

    private float playerFieldOfViewAngle = 100f; // プレイヤーの視界の角度（Inspectorで調整可能）

    // ゲームオーバー後、メニュー画面に戻る際のフェード処理
    private CanvasGroup fadeCanvasGroup; // フェード用のCanvasGroup
    private float fadeDuration = 0.6f; // フェードの時間

    //　客の回転を検知し、顔のIK制御のバグを抑制
    private float previousYRotation = 0f;   // 前回のY軸回転
    public float rotationThreshold = 0.1f;   // 急回転とみなす角度のしきい値（度単位）

    // キー設定用の変数
    private KeyCode confirmKey;
    private KeyCode serveKey;

    // 言語設定のインデックス
    private int languageIndex;

    void Start()
    {
        // static 変数のリセット
        dailyRevenue = 0f;                         // 売上金をリセット
        waitingCustomers.Clear();                  // 待機中の客キューをクリア
        //totalCustomers = 0;                        // 総客数をリセット（それぞれの客がこれを実行すると最終的にtotalCustomersが1になるから削除）
        completedCustomers = 0;                    // 完了した客数をリセット
        isGameOver = false;                        // ゲームオーバー状態をリセット

        isLookingAtPlayer = true;

        triggerDistance = 1.3f; // 反応距離を設定

        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        serveKey = (KeyCode)PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);

        // PlayerPrefsから言語設定のインデックスを取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルト値は0

        // 静的オブジェクトに言語設定を適用
        ApplyLanguage(languageIndex);

        // Animatorコンポーネントの取得
        animator = GetComponent<Animator>();

        // 歩行アニメーションを開始
        animator.SetBool("isWalking", true);  // Walkingアニメーションを再生

        // 客の見た目を非表示にする（オブジェクト自体はアクティブのまま）
        HideCustomer();

        // CallCanvasも非表示にする
        callCanvas.SetActive(false);

        // 席に着くまでは客をIdle状態に設定
        isIdle = true;

        // 指定時間後に客をスポーンさせる
        StartCoroutine(SpawnCustomerAfterDelay());

        seatManager = FindObjectOfType<SeatManager>();

        if (seatManager == null)
        {
            Debug.LogError("SeatManagerが見つかりませんでした。シーン内に配置されているか確認してください。");
        }

        // 初期状態では注文ウィンドウとドリンク画像は非表示
        orderCanvas.SetActive(false);
        drinkImage.gameObject.SetActive(false);

        // メッセージ用のImagesも非表示
        messageCanvas1.SetActive(false);
        messageCanvas2.SetActive(false);

        drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();

        if (drinkSelectionManager == null)
        {
            Debug.LogError("DrinkSelectionManagerが見つかりませんでした。シーン内に配置されているか確認してください。");
        }

        // drinkSpritesとdrinkPricesの長さを確認
        if (drinkSprites.Length != drinkPrices.Length)
        {
            Debug.LogError("drinkSprites と drinkPrices の要素数が一致していません。");
        }

        // confirmButtonにクリック時の処理を追加
        confirmButton.onClick.AddListener(OnConfirmButtonClick);

        // 伝票Canvasを最初は非表示に設定
        receiptCanvas.SetActive(false);

        // 売上金表示を0円に初期化
        UpdateRevenueDisplay(); // 初期表示を0円に設定

        // 請求金額の各桁を調整するためのボタンにリスナーを追加
        plusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, 1, billThousandsText));
        minusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, -1, billThousandsText));
        plusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, 1, billHundredsText));
        minusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, -1, billHundredsText));
        plusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, 1, billTensText));
        minusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, -1, billTensText));
        plusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, 1, billOnesText));
        minusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, -1, billOnesText));

        // 請求金額の初期表示を設定
        UpdateBillAmount();

        // メッセージキャンバスを最初は非表示
        confirmationMessageCanvas.SetActive(false);

        // 支払いメニューキャンバスを最初は非表示
        paymentMenuCanvas.SetActive(false);

        // 不満度バーを初期化
        UpdateSatisfactionBar();

        //SoundManagerの設定がされているか確認
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManagerが設定されていません。");
        //}

        //ゲーム中のBGMを再生
        SoundManager.instance.PlayGameBGM();

        // 会話用のCanvasは最初は非表示にしておく
        conversationCanvas.gameObject.SetActive(false);

        // 客の登録
        RegisterCustomer();

        // 前回の不満度を初期化
        previousSatisfactionLevel = satisfactionLevel;

        // ゲームオーバーCanvasとメニューに戻るボタンを非アクティブに設定
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        if (returnToMenuButton != null)
        {
            returnToMenuButton.gameObject.SetActive(false);
            returnToMenuButton.onClick.AddListener(OnReturnToMenuButtonClicked);
        }

        // FadeCanvas内のBlackImageを自動検索し、CanvasGroupを取得
        GameObject fadeCanvas = GameObject.Find("FadeCanvas");
        if (fadeCanvas != null)
        {
            GameObject blackImage = fadeCanvas.transform.Find("BlackImage")?.gameObject;
            if (blackImage != null)
            {
                fadeCanvasGroup = blackImage.GetComponent<CanvasGroup>();
                if (fadeCanvasGroup != null)
                {
                    // 初期状態で透明に設定
                    fadeCanvasGroup.alpha = 0f;
                }
                else
                {
                    Debug.LogError("BlackImageにCanvasGroupコンポーネントが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("FadeCanvas内にBlackImageが見つかりませんでした。");
            }
        }
        else
        {
            Debug.LogError("FadeCanvasが見つかりませんでした。");
        }

        // デバッグにtotalCustomersとcompleteCustomersを表示
        Debug.Log("Total Customers: " + totalCustomers);
        Debug.Log("Completed Customers: " + completedCustomers);
    }

    void Update()
    {
        // CallCanvasをプレイヤーの方へ向ける
        Vector3 directionToCallCanvas = player.position - callCanvas.transform.position;
        directionToCallCanvas.y = 0; // Y軸の回転を防ぐ
        callCanvas.transform.rotation = Quaternion.LookRotation(directionToCallCanvas);
        callCanvas.transform.Rotate(0, 180, 0); // プレイヤーを正しく向くように回転

        // プレイヤーが近づいたときのメッセージ表示処理
        float distance = Vector3.Distance(player.position, customer.position);

        // 支払い状態でない場合、通常の注文や待機処理を行う
        if (!isPaying && distance <= triggerDistance && !isPlayerInRange && !isRotationRestricted && IsClosestCustomer() && IsCustomerInPlayerView())
        {
            // プレイヤーが範囲に入ったときにメッセージを表示
            isPlayerInRange = true;

            // 待機中なら再確認メッセージを表示、それ以外は注文メッセージを表示
            if (isWaiting)
            {
                switch (languageIndex)
                {
                    case 0: // 英語
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                        break;
                    case 1: // 日本語
                        ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を再確認する");
                        break;
                }
            }
            else
            {

                switch (languageIndex)
                {
                    case 0: // 英語
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                        break;
                    case 1: // 日本語
                        ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を受ける");
                        break;
                }
            }
        }
        else if (!isPaying && (!IsCustomerInPlayerView() || distance > triggerDistance) && isPlayerInRange)
        {
            // プレイヤーが範囲を出たときにメッセージを非表示
            isPlayerInRange = false;
            HideMessageImage1();
        }

        // お会計状態の場合の処理
        if (isPaying && distance <= triggerDistance && !isPlayerInRange && !isRotationRestricted && IsClosestCustomer() && IsCustomerInPlayerView())
        {
            // プレイヤーが範囲に入った場合にお会計メッセージを表示
            isPlayerInRange = true;

            switch (languageIndex)
            {
                case 0: // 英語
                    ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                    break;
                case 1: // 日本語
                    ShowMessageImage1($"<size=90>{confirmKey}</size> を押してお会計する");
                    break;
            }
        }
        else if (isPaying && (!IsCustomerInPlayerView() || distance > triggerDistance) && isPlayerInRange)
        {
            // プレイヤーが範囲を出たときにメッセージを非表示
            isPlayerInRange = false;
            HideMessageImage1();
        }

        // isRotationRestricted が true に変わった瞬間にメッセージを非表示にする
        if (!wasRotationRestricted && isRotationRestricted)
        {
            HideMessageImage1();
        }

        // isRotationRestricted が false に変わった瞬間にメッセージを再表示する
        if (wasRotationRestricted && !isRotationRestricted)
        {
            if (distance <= triggerDistance && IsClosestCustomer() && IsCustomerInPlayerView())  // 最も近い客かどうかを確認
            {
                // プレイヤーが範囲内であればメッセージを表示
                isPlayerInRange = true;

                if (!isPaying)
                {
                    if (isWaiting)
                    {
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                                break;
                            case 1: // 日本語
                                ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を再確認する");
                                break;
                        }
                    }
                    else
                    {
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                                break;
                            case 1: // 日本語
                                ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を受ける");
                                break;
                        }
                    }
                }
                else
                {
                    switch (languageIndex)
                    {
                        case 0: // 英語
                            ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                            break;
                        case 1: // 日本語
                            ShowMessageImage1($"<size=90>{confirmKey}</size> を押してお会計する");
                            break;
                    }
                }
            }
        }

        // 現在の状態を記録
        wasRotationRestricted = isRotationRestricted;

        // 会話中でなく、金額確認メッセージ表示中にEボタンが押された場合、メッセージを非表示にし、メニューを表示
        if (isConfirmationMessageActive && Input.GetKeyDown(confirmKey) && !isConversing)
        {
            // メッセージを非表示にする
            confirmationMessageCanvas.SetActive(false);

            // 支払いメニューを表示
            ShowPaymentMenu();
        }

        // 注文ウィンドウ表示中の処理
        if (Input.GetKeyDown(confirmKey) && !isGameOver && isOrderDisplayed)
        {
            SoundManager.instance.PlayDecisionSound();  // 決定音を再生

            // 注文ウィンドウが表示されている場合は閉じる
            CloseOrderCanvas();
        }

        // 金額確認メッセージが表示されていなく、かつ会話中でない場合のみ、他のEボタンの処理を行う
        else if (!isConfirmationMessageActive && !isConversing && !isRotationRestricted && IsClosestCustomer() && !isGameOver)
        {
            // プレイヤーがEボタンを押した場合の処理（Idle状態でない場合のみ反応）
            if (!isIdle && isPlayerInRange && Input.GetKeyDown(confirmKey) && IsCustomerInPlayerView())
            {
                // すぐにプレイヤーの移動を無効化（視点移動はOK）
                DisablePlayerMovement();

                if (!isPaying)
                {
                    if (isOrderDisplayed)
                    {
                        //機能を別の場所へ移動させた
                    }
                    else
                    {
                        // ドリンクが未選択の場合はランダムに選択
                        if (!isDrinkSelected)
                        {
                            SelectDrink();

                            SoundManager.instance.PlayDecisionSound();  // 決定音を再生

                            // 注文ウィンドウを表示し、プレイヤーの移動を無効化
                            ShowOrderCanvas();
                        }
                        else
                        {
                            // 効果音（メッセージ音）を再生
                            //SoundManager.instance.PlayMessageSound();

                            // ここで会話内容を設定                           
                            switch (languageIndex)
                            {
                                case 0: // 英語
                                    conversationMessages = new string[]
                                    {
                                        "You: \"May I ask for your order again?\"",   // 最初に表示される会話
                                        "Customer: \"Uh, oh, yes...\""  // Eボタンを押すと次に表示される
                                    };
                                    break;
                                case 1: // 日本語
                                    conversationMessages = new string[]
                                    {
                                        "あなた「再度ご注文をお伺いしてもよろしいですか？」",   // 最初に表示される会話
                                        "お客さん「え、あぁはい……」"  // Eボタンを押すと次に表示される
                                    };
                                    break;
                            }

                            // プレイヤーの移動を無効化（視点移動はOK）
                            DisablePlayerMovement();

                            // 会話を開始し、会話終了後に処理を実行
                            StartConversation(() =>
                            {
                                // 0.25秒遅延してから処理を行う
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // 不満度を減少させる処理を追加
                                    satisfactionLevel += recheckSatisfactionDecrease;
                                    satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100); // 不満度を0から100の範囲に制限
                                    UpdateSatisfactionBar();  // 不満度バーを更新

                                    Debug.Log("再度注文を聞いたため、不満度が増加しました。");

                                    //再注文用のメッセージ画像を表示                                   
                                    switch (languageIndex)
                                    {
                                        case 0: // 英語
                                            ShowMessageImage2(reorderMessageSprite_en);
                                            break;
                                        case 1: // 日本語
                                            ShowMessageImage2(reorderMessageSprite);
                                            break;
                                    }

                                    //SoundManager.instance.PlayDecisionSound();  // 決定音を再生

                                    // 注文ウィンドウを表示し、プレイヤーの移動を無効化
                                    ShowOrderCanvas();
                                }));
                            });
                        }
                    }
                }

                // プレイヤーがEボタンを押した場合の処理（お会計状態のとき）
                if (isPaying && !isPayingInProgress)
                {
                    SoundManager.instance.PlayDecisionSound();  // 決定音を再生

                    isPayingInProgress = true;
                    StopAwaitingPayment();
                    SetAsSelectedCustomer(); // この客を選択状態に設定
                    // ShowReceipt();  // 伝票を表示する関数を呼び出し


                    // プレイヤーの移動と視点移動を一時的に停止
                    DisablePlayerControl();

                    // フラグを立てる（伝票が表示されていることを示す）
                    isReceiptDisplayed = true;

                    // MessageCanvas1を非表示にする
                    HideMessageImage1();

                    // 金額計算を関数で行う
                    float totalAmount = CalculateTotalAmount();

                    // 会話の開始（支払い金額の確認）
                    StartCoroutine(DelayWithAction(0.1f, () =>
                    {
                        // お客さんの会話                       
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                conversationMessages = new string[]
                                {
                                    $"You: \"The total is {totalAmount} yen.\"",
                                    $"Customer: \"Alright, {totalAmount} yen.\""
                                    // $"お客さん「じゃあ{(UnityEngine.Random.value > 0.5f ? "現金" : "PoyPoy")}で。」"  // ランダムでカードかキャッシュを選択
                                };
                                break;
                            case 1: // 日本語
                                conversationMessages = new string[]
                                {
                                    $"あなた「お会計{totalAmount}円です。」",
                                    $"お客さん「はい。{totalAmount}円ね。」"  // 請求金額を表示
                                    // $"お客さん「じゃあ{(UnityEngine.Random.value > 0.5f ? "現金" : "PoyPoy")}で。」"  // ランダムでカードかキャッシュを選択
                                };
                                break;
                        }

                        // ReceiptCanvasを非表示にする
                        // receiptCanvas.SetActive(false);

                        // メッセージ音を再生
                        //SoundManager.instance.PlayMessageSound();

                        // 会話を開始し、次の処理へ移行
                        StartConversation(() =>
                        {
                            // 0.5秒の遅延を追加
                            StartCoroutine(DelayWithAction(0.5f, () =>
                            {
                                // 支払い音を再生
                                SoundManager.instance.PlayPaymentSound();

                                //決済成功のメッセージ画像を表示                               
                                switch (languageIndex)
                                {
                                    case 0: // 英語
                                        ShowMessageImage2(paymentSuccessSprite_en);
                                        break;
                                    case 1: // 日本語
                                        ShowMessageImage2(paymentSuccessSprite);
                                        break;
                                }

                                // 売上金を加算
                                dailyRevenue += totalAmount;
                                UpdateRevenueDisplay(); // 売上金の表示を更新

                                // 遅延して、プレイヤーの会話
                                StartCoroutine(DelayWithAction(1.5f, () =>
                                {
                                    // あなたの会話                                   
                                    switch (languageIndex)
                                    {
                                        case 0: // 英語
                                            conversationMessages = new string[]
                                            {
                                                "You: \"Thank you for your payment.\"",
                                                "You: \"We look forward to serving you again.\""
                                            };
                                            break;
                                        case 1: // 日本語
                                            conversationMessages = new string[]
                                            {
                                                "あなた「お支払いありがとうございます。」",
                                                "あなた「またのお越しをお待ちしております。」"
                                            };
                                            break;
                                    }

                                    // メッセージ音を再生
                                    //SoundManager.instance.PlayMessageSound();

                                    // 会話を開始し、終了後に次の処理を実行
                                    StartConversation(() =>
                                    {
                                        StartCoroutine(DelayWithAction(0.25f, () =>
                                        {
                                            // フラグをリセット（伝票が閉じられたことを示す）
                                            isReceiptDisplayed = false;

                                            // プレイヤーの移動と視点移動を有効化
                                            EnablePlayerControl();

                                            // 支払い後に顧客を席から出入口まで移動させる処理を開始
                                            StartCoroutine(MoveToSpawnAfterPayment());
                                        }));
                                    });
                                }));
                            }));
                        });
                    }));
                }
            }
        }

        // プレイヤーがQボタンを押した場合の処理（Idle状態、会話中でない場合のみ反応）
        if (!isIdle && distance <= triggerDistance && Input.GetKeyDown(serveKey) && !isConversing && !isRotationRestricted && !isOrderDisplayed &&
            IsClosestCustomer() && !isGameOver && IsCustomerInPlayerView())
        {
            if (!isPaying)
            {
                // プレイヤーが持っているドリンクを確認して一致をチェック
                DrinkSelectionManager drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();
                if (drinkSelectionManager != null && (drinkSelectionManager.currentDrinks[0] != null || drinkSelectionManager.currentDrinks[1] != null))
                {
                    if (CheckDrinkMatch(drinkSelectionManager.currentDrinks))
                    {
                        // 待機中の解除
                        StopWaiting();

                        // 効果音（成功音）を再生
                        SoundManager.instance.PlaySuccessSound();

                        // 提供完了メッセージを表示                       
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                ShowMessageImage2(completeMessageSprite_en);
                                break;
                            case 1: // 日本語
                                ShowMessageImage2(completeMessageSprite);
                                break;
                        }

                        //RecheckMessageを非表示にする
                        HideMessageImage1();

                        // 客をIdle状態に設定
                        isIdle = true;

                        // Idle状態の回数を増加
                        idleCount++;

                        // Idle状態解除のコルーチンを開始
                        StartCoroutine(ResetIdleAfterDelay());

                        Debug.Log("ドリンクが一致しました。");
                    }
                    else
                    {
                        // 効果音（メッセージ音）を再生
                        //SoundManager.instance.PlayMessageSound();

                        // ここで会話内容を設定                        
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                conversationMessages = new string[]
                                {
                                    "Customer: \"Huh? This seems different \nfrom what I ordered...\"",   // 最初に表示される会話
                                    "It looks like the order doesn't match." ,
                                    "Let's bring the correct drink."// Eボタンを押すと次に表示される
                                };
                                break;
                            case 1: // 日本語
                                conversationMessages = new string[]
                                {
                                    "お客さん「あれ？注文したものと違うような……」",   // 最初に表示される会話
                                    "どうやら注文と一致しないようです。" ,
                                    "正しいドリンクを持ってきましょう。"// Eボタンを押すと次に表示される
                                };
                                break;
                        }

                        // プレイヤーの移動を無効化（視点移動はOK）
                        DisablePlayerMovement();

                        // 会話を開始し、会話終了後に処理を実行
                        StartConversation(() =>
                        {
                            // 0.25秒遅延してから処理を行う
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                satisfactionLevel += drinkMismatchSatisfactionDecrease;  // Inspectorで指定した不満度増加量を使用
                                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100); // 不満度を0から100の範囲に制限
                                UpdateSatisfactionBar();  // 不満度バーを更新

                                // 不一致の場合、不一致メッセージを表示                               
                                switch (languageIndex)
                                {
                                    case 0: // 英語
                                        ShowMessageImage2(failMessageSprite_en);
                                        break;
                                    case 1: // 日本語
                                        ShowMessageImage2(failMessageSprite);
                                        break;
                                }

                                // 効果音（失敗音）を再生
                                //SoundManager.instance.PlayFailureSound();

                                EnablePlayerMovement();
                            }));
                        });
                    }
                }
                else
                {
                    Debug.Log("ドリンクを持っていません。");
                }
            }
        }
        // プレイヤーがドリンクを持っていて、TriggerDistance内にいる場合かつ、isWaitingがtrueの場合
        if (distance <= triggerDistance && drinkSelectionManager != null &&
           (drinkSelectionManager.currentDrinks[0] != null || drinkSelectionManager.currentDrinks[1] != null) && isWaiting && !isRotationRestricted && !isOrderDisplayed &&
           IsClosestCustomer() && !isGameOver && IsCustomerInPlayerView())
        {
            ShowServeMessage();
        }
        else
        {
            HideServeMessage();
        }

        // プレイヤーがXボタンを押した場合の処理
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("この客のドリンク履歴:");
            float totalAmount = 0f;  // 合計金額を保持する変数
            if (drinkHistory.Count > 0)
            {
                for (int i = 0; i < drinkHistory.Count; i++)
                {
                    string drinkName = drinkHistory[i];
                    Debug.Log((i + 1) + "番目: " + drinkName);

                    // ドリンク名に基づいて金額を取得
                    for (int j = 0; j < drinkSprites.Length; j++)
                    {
                        if (drinkSprites[j].name == drinkName)
                        {
                            totalAmount += drinkPrices[j];
                            break;
                        }
                    }
                }
                Debug.Log("合計金額: " + totalAmount + "円");
            }
            else
            {
                Debug.Log("履歴が存在しません。");
            }

            // Idle状態の回数を表示
            Debug.Log("この客は " + idleCount + " 回Idle状態になりました。");
        }

        // 呼出中の場合、経過時間に基づいて不満度を増加させる処理
        if (isCalling)
        {
            // 経過時間を増加
            callTimeElapsed += Time.deltaTime;

            // 通常の増加が始まるタイミング
            if (!isCallDecaying && callTimeElapsed >= callDecayStartTime)
            {
                // 不満度の増加を開始し、アイコンを変更
                isCallDecaying = true;
                switch (languageIndex)
                {
                    case 0: // 英語
                        callIcon.GetComponent<Image>().sprite = decayingCallIcon_en;
                        break;
                    case 1: // 日本語
                        callIcon.GetComponent<Image>().sprite = decayingCallIcon;
                        break;
                }

                // 通常の点滅を開始
                StopCoroutine("BlinkFastCallIcon"); // 速い増加のコルーチンを停止
                StopCoroutine("BlinkNormalCallIcon"); // 通常増加のコルーチンを再度停止しておく
                StartCoroutine(BlinkNormalCallIcon(0.5f)); // 通常の増加の点滅を開始
            }

            // 速く増加が始まるタイミング
            if (!isFastCallDecaying && callTimeElapsed >= fastCallDecayStartTime)
            {
                // 速く不満度が増加する状態に移行し、アイコンを変更
                isFastCallDecaying = true;
                switch (languageIndex)
                {
                    case 0: // 英語
                        callIcon.GetComponent<Image>().sprite = fastDecayingCallIcon_en;
                        break;
                    case 1: // 日本語
                        callIcon.GetComponent<Image>().sprite = fastDecayingCallIcon;
                        break;
                }

                // 速い点滅を開始
                StopCoroutine("BlinkNormalCallIcon"); // 通常増加のコルーチンを停止
                StartCoroutine(BlinkFastCallIcon(0.25f)); // 速い増加の点滅を開始
            }

            // 不満度の増加処理
            if (isCallDecaying)
            {
                if (isFastCallDecaying)
                {
                    // 速く増加
                    satisfactionLevel += fastCallDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // 通常の速度で増加
                    satisfactionLevel += callDecaySpeed * Time.deltaTime;
                }

                // 不満度を範囲内に制限
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // バーの更新
            }
        }

        if (isWaiting)
        {
            // 経過時間を増加
            waitTimeElapsed += Time.deltaTime;

            // 通常の増加が始まるタイミング
            if (!isWaitDecaying && waitTimeElapsed >= waitDecayStartTime)
            {
                isWaitDecaying = true;
                callIcon.GetComponent<Image>().sprite = decayingWaitIcon; // 増加アイコンに変更

                // 通常の点滅を開始
                StopCoroutine("BlinkFastWaitIcon"); // 速い増加のコルーチンが動作していれば停止
                StopCoroutine("BlinkNormalWaitIcon"); // 通常増加のコルーチンも再度停止しておく
                StartCoroutine(BlinkNormalWaitIcon(0.5f)); // 通常の増加の点滅を開始
            }

            // 速い増加が始まるタイミング
            if (!isFastWaitDecaying && waitTimeElapsed >= fastWaitDecayStartTime)
            {
                isFastWaitDecaying = true;
                callIcon.GetComponent<Image>().sprite = fastDecayingWaitIcon; // 速い増加アイコンに変更

                // 速い点滅を開始
                StopCoroutine("BlinkNormalWaitIcon"); // 通常増加のコルーチンが動作していれば停止
                StartCoroutine(BlinkFastWaitIcon(0.25f)); // 速い増加の点滅を開始
            }

            // 不満度の増加処理
            if (isWaitDecaying)
            {
                if (isFastWaitDecaying)
                {
                    // 速く増加
                    satisfactionLevel += fastWaitDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // 通常の速度で増加
                    satisfactionLevel += waitDecaySpeed * Time.deltaTime;
                }

                // 不満度を範囲内に制限
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // バーの更新
            }
        }

        if (isAwaitingPayment)
        {
            // 経過時間を増加
            awaitingPaymentTimeElapsed += Time.deltaTime;

            // 通常の増加が始まるタイミング
            if (!isAwaitingPaymentDecaying && awaitingPaymentTimeElapsed >= awaitingPaymentDecayStartTime)
            {
                isAwaitingPaymentDecaying = true;
                switch (languageIndex)
                {
                    case 0: // 英語
                        callIcon.GetComponent<Image>().sprite = decayingAwaitingPaymentIcon_en;
                        break;
                    case 1: // 日本語
                        callIcon.GetComponent<Image>().sprite = decayingAwaitingPaymentIcon;
                        break;
                }

                // 通常の点滅を開始
                StopCoroutine("BlinkFastAwaitingPaymentIcon"); // 速い増加のコルーチンが動作していれば停止
                StopCoroutine("BlinkNormalAwaitingPaymentIcon"); // 通常増加のコルーチンも再度停止しておく
                StartCoroutine(BlinkNormalAwaitingPaymentIcon(0.5f)); // 通常の増加の点滅を開始
            }

            // 速く増加が始まるタイミング
            if (!isFastAwaitingPaymentDecaying && awaitingPaymentTimeElapsed >= fastAwaitingPaymentDecayStartTime)
            {
                isFastAwaitingPaymentDecaying = true;
                switch (languageIndex)
                {
                    case 0: // 英語
                        callIcon.GetComponent<Image>().sprite = fastDecayingAwaitingPaymentIcon_en;
                        break;
                    case 1: // 日本語
                        callIcon.GetComponent<Image>().sprite = fastDecayingAwaitingPaymentIcon;
                        break;
                }

                // 速い点滅を開始
                StopCoroutine("BlinkNormalAwaitingPaymentIcon"); // 通常増加のコルーチンが動作していれば停止
                StartCoroutine(BlinkFastAwaitingPaymentIcon(0.25f)); // 速い増加の点滅を開始
            }

            // 不満度の増加処理
            if (isAwaitingPaymentDecaying)
            {
                if (isFastAwaitingPaymentDecaying)
                {
                    // 速く増加
                    satisfactionLevel += fastAwaitingPaymentDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // 通常の速度で増加
                    satisfactionLevel += awaitingPaymentDecaySpeed * Time.deltaTime;
                }

                // 不満度を範囲内に制限
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // バーの更新
            }
        }

        // プレイヤーがEキーを押したときに次のメッセージを表示
        if (isConversing && Input.GetKeyDown(confirmKey) && !isConversationJustStarted)
        {
            if (!isTyping)
            {
                DisplayNextMessage();
            }
            else
            {
                // まだタイピング中の場合、メッセージを全表示
                if (typingCoroutine != null) // コルーチンが実行中の場合
                {
                    StopCoroutine(typingCoroutine); // そのコルーチンを停止
                }
                conversationText.text = conversationMessages[conversationIndex - 1];
                isTyping = false;
                SoundManager.instance.StopTypingSound();
            }
        }

        // 不満度の変化を検出
        if (satisfactionLevel != previousSatisfactionLevel)
        {
            // タイマーをリセット
            satisfactionBlinkTimer = satisfactionBlinkDuration;

            // ブリンクコルーチンが動いていない場合は開始
            if (satisfactionBlinkCoroutine == null)
            {
                satisfactionBlinkCoroutine = StartCoroutine(BlinkSatisfactionBar());
            }
        }

        // タイマーの更新
        if (satisfactionBlinkTimer > 0)
        {
            satisfactionBlinkTimer -= Time.deltaTime;
        }
        else if (satisfactionBlinkCoroutine != null)
        {
            // タイマーが0になったらブリンクを停止
            StopCoroutine(satisfactionBlinkCoroutine);
            satisfactionBlinkCoroutine = null;

            // 通常の画像に戻す
            satisfactionBar.sprite = normalSatisfactionBarSprite;
        }

        // 前回の不満度を更新
        previousSatisfactionLevel = satisfactionLevel;

        // SatisfactionLevelが100に達したらゲームオーバー
        if (!isGameOver && satisfactionLevel >= 100f)
        {
            GameOver();
        }

        // フレームの最後でフラグをリセット
        if (isConversationJustStarted)
        {
            isConversationJustStarted = false;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (neckBone != null && PlayerCamera != null)
        {
            // 回転検出とリセット処理
            DetectAndResetForRapidRotation();

            float distanceToPlayer = Vector3.Distance(PlayerCamera.position, neckBone.position);

            // プレイヤーがtriggerDistance内にいない場合は、常にisRotationRestrictedをtrueに設定
            if (distanceToPlayer > triggerDistance)
            {
                isRotationRestricted = true;
            }
            else
            {
                // プレイヤーのカメラ方向に向かうグローバルな回転を計算
                Vector3 directionToPlayerCamera = (PlayerCamera.position - neckBone.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayerCamera);

                // グローバルな回転をローカル座標系に変換
                Quaternion predictedLocalRotation = Quaternion.Inverse(neckBone.parent.rotation) * lookRotation;
                float predictedYRotation = predictedLocalRotation.eulerAngles.y;

                // ローカルY軸の回転が180度を超えた場合の処理
                if (predictedYRotation > 180f)
                {
                    predictedYRotation -= 360f;
                }

                // Y軸の回転が -65度から +65度 の範囲に収まっている場合
                if (predictedYRotation >= -65f && predictedYRotation <= 65f)
                {
                    isRotationRestricted = false; // 制限を解除
                }
                else
                {
                    isRotationRestricted = true; // 制限を有効化
                }
            }

            // 制限されていない場合、プレイヤーカメラの方へ首を向ける
            if (!isRotationRestricted && isLookingAtPlayer)
            {
                Vector3 targetPosition = PlayerCamera.position;
                if (currentLookAtPosition == Vector3.zero)
                {
                    currentLookAtPosition = targetPosition;
                }

                // 方向ベクトルをSlerpし、最短回転で補間
                Vector3 fromDir = (currentLookAtPosition - neckBone.position).normalized;
                Vector3 toDir = (targetPosition - neckBone.position).normalized;
                Vector3 newDir = Vector3.Slerp(fromDir, toDir, Time.deltaTime * neckRotationSpeed);
                float dist = (currentLookAtPosition - neckBone.position).magnitude;
                if (dist < 0.001f) dist = (targetPosition - neckBone.position).magnitude;
                currentLookAtPosition = neckBone.position + newDir * dist;

                animator.SetLookAtWeight(1.0f);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
            // 制限されている場合、首を正面に向ける
            else if (isRotationRestricted)
            {
                Vector3 targetForwardPosition = neckBone.position + neckBone.forward * 10f;
                if (currentLookAtPosition == Vector3.zero)
                {
                    currentLookAtPosition = targetForwardPosition;
                }

                // 正面方向へも最短経路で補間
                Vector3 fromDir = (currentLookAtPosition - neckBone.position).normalized;
                Vector3 toDir = (targetForwardPosition - neckBone.position).normalized;
                Vector3 newDir = Vector3.Slerp(fromDir, toDir, Time.deltaTime * neckRotationSpeed);
                float dist = (currentLookAtPosition - neckBone.position).magnitude;
                if (dist < 0.001f) dist = (targetForwardPosition - neckBone.position).magnitude;
                currentLookAtPosition = neckBone.position + newDir * dist;

                animator.SetLookAtWeight(1.0f);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
            else
            {
                // isRotationRestrictedがfalse、かつisLookingAtPlayerがfalseのときも首を正面へ戻す
                Vector3 targetForwardPosition = neckBone.position + neckBone.forward * 10f;
                if (currentLookAtPosition == Vector3.zero)
                {
                    currentLookAtPosition = targetForwardPosition;
                }

                Vector3 fromDir = (currentLookAtPosition - neckBone.position).normalized;
                Vector3 toDir = (targetForwardPosition - neckBone.position).normalized;
                Vector3 newDir = Vector3.Slerp(fromDir, toDir, Time.deltaTime * neckRotationSpeed);
                float dist = (currentLookAtPosition - neckBone.position).magnitude;
                if (dist < 0.001f) dist = (targetForwardPosition - neckBone.position).magnitude;
                currentLookAtPosition = neckBone.position + newDir * dist;

                animator.SetLookAtWeight(1.0f);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
        }
    }

    private void DetectAndResetForRapidRotation()
    {
        float currentYRotation = customer.rotation.eulerAngles.y;

        // Y軸回転の変化量を計算
        float rotationDelta = Mathf.Abs(Mathf.DeltaAngle(previousYRotation, currentYRotation));

        // 急回転を検出
        if (rotationDelta > rotationThreshold)
        {
            // 急回転発生時、currentLookAtPositionをリセット
            Vector3 resetForwardPosition = neckBone.position + customer.forward * 10f;
            currentLookAtPosition = new Vector3(resetForwardPosition.x, currentLookAtPosition.y, resetForwardPosition.z);

            Debug.Log("急回転を検出し、currentLookAtPositionをリセットしました。");
        }

        // 現在のY軸回転を保存
        previousYRotation = currentYRotation;
    }


    private void SelectDrink()
    {
        // 個別の客ごとにランダムでドリンクを選択
        selectedDrink = drinkSprites[UnityEngine.Random.Range(0, drinkSprites.Length)];
        isDrinkSelected = true;
        Debug.Log("客が注文したドリンク: " + selectedDrink.name);

        // ドリンクを履歴に追加
        drinkHistory.Add(selectedDrink.name);
    }

    private void ShowOrderCanvas()
    {
        // 注文ウィンドウとドリンク画像を表示
        orderCanvas.SetActive(true);
        drinkImage.gameObject.SetActive(true);
        drinkImage.sprite = selectedDrink; // 選択されたドリンクを表示
        isOrderDisplayed = true;

        // 待機中でないなら、呼出中の停止
        if (!isWaiting)
        {
            StopCalling();
        }

        // プレイヤーの移動を無効化（視点移動はOK）
        DisablePlayerMovement();

        // CloseMessageを表示      
        switch (languageIndex)
        {
            case 0: // 英語
                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to close");
                break;
            case 1: // 日本語
                ShowMessageImage1($"<size=90>{confirmKey}</size> を押して閉じる");
                break;
        }
    }

    private void CloseOrderCanvas()
    {
        // 注文ウィンドウを非表示にする
        orderCanvas.SetActive(false);
        isOrderDisplayed = false;

        if (!isWaiting)
        {
            // 待機中の開始
            StartWaiting();
        }

        // プレイヤーの移動を再度有効化
        EnablePlayerMovement();

        // 再確認メッセージを表示
        switch (languageIndex)
        {
            case 0: // 英語
                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                break;
            case 1: // 日本語
                ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を再確認する");
                break;
        }
    }

    // ドリンク一致を確認するメソッド
    public bool CheckDrinkMatch(GameObject[] playerDrinks)
    {
        // テストプレイ中の場合、無条件で一致とする
        if (TestPlayManager.instance != null && TestPlayManager.instance.isTestPlay)
        {
            if (playerDrinks[1] != null) // 2つ目のドリンクが存在する場合
            {
                Destroy(playerDrinks[1]); // 2つ目のドリンクを削除
                playerDrinks[1] = null;
            }
            else if (playerDrinks[0] != null) // 1つ目のドリンクが存在する場合
            {
                Destroy(playerDrinks[0]); // 1つ目のドリンクを削除
                playerDrinks[0] = null;
            }
            else
            {
                return false; // ドリンクを持っていない場合はfalse
            }

            satisfactionLevel -= drinkMatchSatisfactionIncrease; // 不満度を減少
            satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
            UpdateSatisfactionBar();
            return true;
        }

        bool firstDrinkMatches = playerDrinks[0] != null && playerDrinks[0].name.Contains(selectedDrink.name);
        bool secondDrinkMatches = playerDrinks[1] != null && playerDrinks[1].name.Contains(selectedDrink.name);

        // どちらかのドリンクが一致した場合
        if (firstDrinkMatches || secondDrinkMatches)
        {
            // 2つ目のドリンクが一致した場合
            if (secondDrinkMatches)
            {
                Destroy(playerDrinks[1]); // 2つ目のドリンクを削除
                playerDrinks[1] = null;
            }

            // 1つ目のドリンクが一致した場合
            else if (firstDrinkMatches)
            {
                Destroy(playerDrinks[0]); // 1つ目のドリンクを削除
                playerDrinks[0] = null;
            }

            // 不満度を減少させる処理
            satisfactionLevel -= drinkMatchSatisfactionIncrease;
            satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
            UpdateSatisfactionBar();
            return true;
        }
        else
        {
            return false;
        }
    }

    // メッセージ画像1（Order, Recheck, Close）を表示（Idle状態でない場合のみ）
    private void ShowMessageImage1(string message)
    {
        if (!isIdle && !isReceiptDisplayed && IsClosestCustomer()) // アイドル状態ではなく、かつ伝票が表示されていない場合のみ実行
        {
            // MessageImage1のSpriteを動的に変更
            //messageImage1.sprite = messageSprite;
            //messageImage1.color = new Color(1, 1, 1, 1); // アルファ値をリセット

            // MessageText1のSpriteを動的に変更
            messageText1.text = message;
            messageText1.color = new Color(1, 1, 1, 1); // アルファ値をリセット

            // MessageImage1を表示
            messageCanvas1.SetActive(true);
        }
    }

    // メッセージ画像1を非表示にする
    private void HideMessageImage1()
    {
        if (IsClosestCustomer())
        {
            messageCanvas1.SetActive(false);
        }
    }

    // メッセージ画像2（Complete, Fail）を表示（Idle状態でない場合のみ）
    private void ShowMessageImage2(Sprite messageSprite)
    {
        if (!isIdle)
        {
            // 既存のフェードアウトコルーチンが動作している場合、停止させる
            StopCoroutine(FadeOutMessageImage2());

            // MessageImage2のSpriteを動的に変更
            messageImage2.sprite = messageSprite;
            messageImage2.color = new Color(1, 1, 1, 1); // アルファ値をリセット

            // MessageImage2を表示
            messageCanvas2.SetActive(true);

            // フェードアウトコルーチンを再スタート
            StartCoroutine(FadeOutMessageImage2());
        }
    }

    // メッセージ画像2をフェードアウトさせるコルーチン
    private IEnumerator FadeOutMessageImage2()
    {
        // 一定時間後にフェードアウト
        yield return new WaitForSeconds(2.0f); // 1.5秒表示

        // フェードアウト処理
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            messageImage2.color = new Color(messageImage2.color.r, messageImage2.color.g, messageImage2.color.b, alpha);
            yield return null;
        }

        // 画像を非表示
        messageCanvas2.SetActive(false);
    }

    // プレイヤーの移動を無効化する関数
    private void DisablePlayerMovement()
    {
        // 移動を無効化するが、視点移動はそのまま
        player.GetComponent<FirstPersonMovement>().DisableMovement();
        player.GetComponent<FirstPersonMovement>().DisableLook(); // 試験的に追加
    }

    // プレイヤーの移動を有効化する関数
    private void EnablePlayerMovement()
    {
        // 移動を再度有効化
        player.GetComponent<FirstPersonMovement>().EnableMovement();
        player.GetComponent<FirstPersonMovement>().EnableLook(); // 試験的に追加
    }

    // プレイヤーの移動と視点移動を両方無効化する関数
    private void DisablePlayerControl()
    {
        // 移動を無効化するが、視点移動はそのまま
        player.GetComponent<FirstPersonMovement>().DisableMovement();
        player.GetComponent<FirstPersonMovement>().DisableLook();
    }

    // プレイヤーの移動と視点移動を両方有効化する関数
    private void EnablePlayerControl()
    {
        // 移動を再度有効化
        player.GetComponent<FirstPersonMovement>().EnableMovement();
        player.GetComponent<FirstPersonMovement>().EnableLook();
    }

    // ServeMessageを表示（Idle状態でない場合のみ）
    private void ShowServeMessage()
    {
        if (!isIdle && !isServeMessageDisplayed)
        {
            switch (languageIndex)
            {
                case 0: // 英語
                    messageText3.text = $"Press <size=90>{serveKey}</size> to serve";
                    break;
                case 1: // 日本語
                    messageText3.text = $"<size=90>{serveKey}</size> を押して提供";
                    break;
            }

            messageText3.color = new Color(1, 1, 1, 1); // アルファ値をリセット
            messageCanvas3.SetActive(true);
            isServeMessageDisplayed = true;
        }
    }

    private void HideServeMessage()
    {
        if (isServeMessageDisplayed)
        {
            messageCanvas3.SetActive(false);
            isServeMessageDisplayed = false;
        }
    }

    // Idle状態にしてから一定時間経過後にisIdleをFalseに戻すコルーチン
    private IEnumerator ResetIdleAfterDelay()
    {
        // 10秒から20秒の間でランダムな時間を待機
        float idleTime = UnityEngine.Random.Range(idleDurationMin, idleDurationMax);
        yield return new WaitForSeconds(idleTime);

        // Idle状態が最大回数に達した場合はお会計状態に移行
        if (idleCount >= idleMaxCount)
        {
            isIdle = false;
            isPaying = true; // お会計状態に設定

            // お会計状態になった時にプレイヤーがtriggerDistance内にいる場合、メッセージを表示
            float currentDistance = Vector3.Distance(player.position, customer.position);
            if (currentDistance <= triggerDistance && !isRotationRestricted && IsClosestCustomer())
            {
                switch (languageIndex)
                {
                    case 0: // 英語
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                        break;
                    case 1: // 日本語
                        ShowMessageImage1($"<size=90>{confirmKey}</size> を押してお会計する");
                        break;
                }
            }

            StartAwaitingPayment(); // 支払い待ち状態を開始（isAwaitingPaymentがTrueになる）
            Debug.Log("お会計状態に進みました。");
        }
        else
        {
            // Idle状態を解除し、通常の呼び出し状態に戻す
            isIdle = false;

            // 呼出中の開始
            StartCalling();

            // ドリンク未選択状態に戻す
            isDrinkSelected = false;

            Debug.Log("注文受付状態に戻りました。");
        }
    }

    // 伝票を表示する関数
    private void ShowReceipt()
    {
        // 伝票Canvasを表示
        receiptCanvas.SetActive(true);

        // 金額をテキストに反映
        float totalAmount = CalculateTotalAmount(); // 金額計算を関数で行う
        amountText.text = "正規料金: " + totalAmount.ToString() + " 円";

        // 請求金額の各桁をリセット
        billThousands = 0;
        billHundreds = 0;
        billTens = 0;
        billOnes = 0;

        //請求金額をリセット
        billAmount = 0;

        // UIの表示もリセット
        billThousandsText.text = billThousands.ToString();
        billHundredsText.text = billHundreds.ToString();
        billTensText.text = billTens.ToString();
        billOnesText.text = billOnes.ToString();

        // 再度、請求金額の合計を計算して billAmount を更新
        UpdateBillAmount();

        // ボタンのリスナーをクリアして再登録
        ClearButtonListeners();

        // プレイヤーの移動と視点移動を一時的に停止
        DisablePlayerControl();

        // フラグを立てる（伝票が表示されていることを示す）
        isReceiptDisplayed = true;

        // MessageCanvas1を非表示にする
        HideMessageImage1();
    }

    // この客を選択状態に設定する関数
    private void SetAsSelectedCustomer()
    {
        // 他のすべての客の選択状態を解除
        CustomerCallIcon[] allCustomers = FindObjectsOfType<CustomerCallIcon>();
        foreach (CustomerCallIcon customer in allCustomers)
        {
            customer.isSelectedCustomer = false;
        }

        // この客だけを選択状態にする
        isSelectedCustomer = true;
    }

    // 請求金額が決定されたときの処理
    private void OnConfirmButtonClick()
    {
        // 選択されていない客は処理しない
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        // 合計金額を計算
        float totalAmount = CalculateTotalAmount();

        // 支払い確率を計算
        bool willPay = CalculatePaymentProbability(totalAmount, billAmount);

        // 客が支払いをした場合、売上金を加算し、伝票Canvasを非表示にする
        if (willPay)
        {
            // 会話の開始（支払い金額の確認）
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // お客さんの会話
                conversationMessages = new string[]
                {
                    $"お客さん「はい。{billAmount}円ね。」",  // 請求金額を表示
                    $"お客さん「じゃあ{(UnityEngine.Random.value > 0.5f ? "カード" : "キャッシュ")}で。」"  // ランダムでカードかキャッシュを選択
                };

                // ReceiptCanvasを非表示にする
                receiptCanvas.SetActive(false);

                // メッセージ音を再生
                //SoundManager.instance.PlayMessageSound();

                // 会話を開始し、次の処理へ移行
                StartConversation(() =>
                {
                    // 0.5秒の遅延を追加
                    StartCoroutine(DelayWithAction(0.5f, () =>
                    {
                        // 支払い音を再生
                        SoundManager.instance.PlayPaymentSound();

                        //決済成功のメッセージ画像を表示
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                ShowMessageImage2(paymentSuccessSprite_en);
                                break;
                            case 1: // 日本語
                                ShowMessageImage2(paymentSuccessSprite);
                                break;
                        }

                        // 売上金を加算
                        dailyRevenue += billAmount;
                        UpdateRevenueDisplay(); // 売上金の表示を更新

                        // 遅延して、プレイヤーの会話
                        StartCoroutine(DelayWithAction(1.5f, () =>
                        {
                            // あなたの会話
                            conversationMessages = new string[]
                            {
                                "あなた「お支払いありがとうございます。」",
                                "あなた「またのお越しをお待ちしております。」"
                            };

                            // メッセージ音を再生
                            //SoundManager.instance.PlayMessageSound();

                            // 会話を開始し、終了後に次の処理を実行
                            StartConversation(() =>
                            {
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // フラグをリセット（伝票が閉じられたことを示す）
                                    isReceiptDisplayed = false;

                                    // プレイヤーの移動と視点移動を有効化
                                    EnablePlayerControl();

                                    // 支払い後に顧客を席から出入口まで移動させる処理を開始
                                    StartCoroutine(MoveToSpawnAfterPayment());
                                }));
                            });
                        }));
                    }));
                });
            }));
        }
        else
        {
            // 支払いが拒否された場合、確認メッセージを表示する
            OnPaymentRefused();
        }

        // デバッグに結果を表示
        Debug.Log("請求金額: " + billAmount + " 円");
        Debug.Log(willPay ? "客は支払いました。" : "客は支払いを拒否しました。");
    }

    // 金額を計算する関数
    private float CalculateTotalAmount()
    {
        float totalAmount = 0f;
        for (int i = 0; i < drinkHistory.Count; i++)
        {
            string drinkName = drinkHistory[i];
            for (int j = 0; j < drinkSprites.Length; j++)
            {
                if (drinkSprites[j].name == drinkName)
                {
                    totalAmount += drinkPrices[j];
                    break;
                }
            }
        }
        return totalAmount;
    }

    // 支払い確率を計算し、支払いの可否を返す関数
    private bool CalculatePaymentProbability(float totalAmount, float billAmount)
    {
        // 請求金額が正規料金以下の場合、必ず支払う
        if (billAmount <= totalAmount)
        {
            return true; // 支払い
        }
        // 請求金額が正規料金の2倍以下の場合、80%の確率で支払う
        else if (billAmount <= 2 * totalAmount)
        {
            return UnityEngine.Random.value <= 0.9f * ((100f - satisfactionLevel) / 100f); // 90％×(100 - 不満度)（％）
        }
        // それ以外の場合、(totalAmount / billAmount) × 1.8 × 100%の確率で支払う
        else
        {
            float probability = (totalAmount / billAmount) * 1.8f * ((100f - satisfactionLevel) / 100f); // 不満度を考慮
            return UnityEngine.Random.value <= probability; // 計算した確率で支払う
        }
    }

    // 売上金の表示を更新する関数
    private void UpdateRevenueDisplay()
    {
        // 売上金をTextMeshProに表示      
        switch (languageIndex)
        {
            case 0: // 英語
                revenueText.text = "Sales: " + Mathf.FloorToInt(dailyRevenue).ToString() + " yen";
                break;
            case 1: // 日本語
                revenueText.text = "本日の売上: " + Mathf.FloorToInt(dailyRevenue).ToString() + " 円";
                break;
        }
    }

    // 請求金額の各桁を調整するための関数
    private void AdjustBillDigit(ref int digit, int increment, TextMeshProUGUI text)
    {
        // 桁の値が0のときに-ボタンを押すと9に、9のときに+ボタンを押すと0になる
        digit = (digit + increment + 10) % 10;

        // 調整した値をUIに反映
        text.text = digit.ToString();

        // 全体の請求金額を更新
        UpdateBillAmount();
    }

    // 請求金額を更新して表示する関数
    private void UpdateBillAmount()
    {
        // 各桁の値を元に請求金額を計算
        billAmount = (billThousands * 1000) + (billHundreds * 100) + (billTens * 10) + billOnes;

        // 各桁の表示は直接ボタンで操作されるので、ここで表示は不要
        Debug.Log("請求金額: " + billAmount + " 円");
    }

    // 支払いが拒否されたときの処理
    private void OnPaymentRefused()
    {
        // レシートキャンバスを閉じる
        receiptCanvas.SetActive(false);

        // 効果音（メッセージ音）を再生
        //SoundManager.instance.PlayMessageSound();

        // ここで会話内容を設定
        conversationMessages = new string[]
        {
                                "お客さん「え、本当にその値段で合ってますか？」",   // 最初に表示される会話
                                "あなた「えーっと……」"  // Eボタンを押すと次に表示される
        };

        // 会話を開始し、会話終了後に処理を実行
        StartConversation(() =>
        {
            // 0.25秒遅延してから処理を行う
            StartCoroutine(DelayWithAction(0.25f, () =>
            {
                // 支払いメニューを表示
                ShowPaymentMenu();
            }));
        });
    }

    // 支払いメニューを表示する関数
    private void ShowPaymentMenu()
    {
        // メニューキャンバスを表示
        paymentMenuCanvas.SetActive(true);

        // 正規料金と請求金額を表示
        float totalAmount = CalculateTotalAmount();
        standardAmountText.text = "正規料金: " + totalAmount.ToString() + " 円";
        billedAmountText.text = "請求金額: " + billAmount.ToString() + " 円";

        // ボタンにリスナーを追加
        standardAmountButton.onClick.AddListener(OnStandardAmountSelected);
        billedAmountButton.onClick.AddListener(OnBilledAmountSelected);
    }

    // 正規料金が選択されたときの処理
    private void OnStandardAmountSelected()
    {
        // 選択されていない客は処理しない
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        //正規料金を算出
        float totalAmount = CalculateTotalAmount();

        // 会話の開始（支払い金額の確認）
        StartCoroutine(DelayWithAction(0.1f, () =>
        {
            // お客さんの会話
            conversationMessages = new string[]
            {
                $"あなた「失礼しました。金額が間違っていました。",
                $"あなた「正しくは{totalAmount}円です。",
                $"お客さん「なんだ。{totalAmount}円ね。」",  // 請求金額を表示
                $"お客さん「じゃあ{(UnityEngine.Random.value > 0.5f ? "カード" : "キャッシュ")}で。」"  // ランダムでカードかキャッシュを選択
            };

            // メニューを閉じる
            paymentMenuCanvas.SetActive(false);

            // メッセージ音を再生
            //SoundManager.instance.PlayMessageSound();

            // 会話を開始し、次の処理へ移行
            StartConversation(() =>
            {
                // 0.5秒の遅延を追加
                StartCoroutine(DelayWithAction(0.5f, () =>
                {
                    // 支払い音を再生
                    SoundManager.instance.PlayPaymentSound();

                    //決済成功のメッセージ画像を表示
                    switch (languageIndex)
                    {
                        case 0: // 英語
                            ShowMessageImage2(paymentSuccessSprite_en);
                            break;
                        case 1: // 日本語
                            ShowMessageImage2(paymentSuccessSprite);
                            break;
                    }

                    // 売上金を加算
                    dailyRevenue += totalAmount;
                    UpdateRevenueDisplay(); // 売上金の表示を更新

                    // 遅延して、プレイヤーの会話
                    StartCoroutine(DelayWithAction(1.5f, () =>
                    {
                        // あなたの会話
                        conversationMessages = new string[]
                        {
                                "あなた「お支払いありがとうございます！」",
                                "あなた「またのお越しをお待ちしております。」"
                        };

                        // メッセージ音を再生
                        //SoundManager.instance.PlayMessageSound();

                        // 会話を開始し、終了後に次の処理を実行
                        StartConversation(() =>
                        {
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                // フラグをリセット（伝票が閉じられたことを示す）
                                isReceiptDisplayed = false;

                                // プレイヤーの移動と視点移動を有効化
                                EnablePlayerControl();

                                // 支払い後に顧客を席から出入口まで移動させる処理を開始
                                StartCoroutine(MoveToSpawnAfterPayment());
                            }));
                        });
                    }));
                }));
            });
        }));
    }

    // 請求金額が選択されたときの処理
    private void OnBilledAmountSelected()
    {
        // 選択されていない客は処理しない
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        // 支払い確率を計算 (totalAmount / billAmount)
        float totalAmount = CalculateTotalAmount();
        float paymentProbability = totalAmount / billAmount;

        // 確率で支払いが行われるか判定
        if (UnityEngine.Random.value <= paymentProbability)
        {
            // 会話の開始（支払い金額の確認）
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // お客さんの会話
                conversationMessages = new string[]
                {
                $"あなた「いえ。料金は{billAmount}円で合っております。",
                $"お客さん「えぇ？{billAmount}円もするの？」",  // 請求金額を表示
                $"お客さん「はぁ。じゃあ{(UnityEngine.Random.value > 0.5f ? "カード" : "キャッシュ")}で。」"  // ランダムでカードかキャッシュを選択
                };

                // メニューを閉じる
                paymentMenuCanvas.SetActive(false);

                // メッセージ音を再生
                //SoundManager.instance.PlayMessageSound();

                // 会話を開始し、次の処理へ移行
                StartConversation(() =>
                {
                    // 0.5秒の遅延を追加
                    StartCoroutine(DelayWithAction(0.5f, () =>
                    {
                        // 支払い音を再生
                        SoundManager.instance.PlayPaymentSound();

                        //決済成功のメッセージ画像を表示
                        switch (languageIndex)
                        {
                            case 0: // 英語
                                ShowMessageImage2(paymentSuccessSprite_en);
                                break;
                            case 1: // 日本語
                                ShowMessageImage2(paymentSuccessSprite);
                                break;
                        }

                        // 売上金を加算
                        dailyRevenue += billAmount;
                        UpdateRevenueDisplay(); // 売上金の表示を更新

                        // 遅延して、プレイヤーの会話
                        StartCoroutine(DelayWithAction(1.5f, () =>
                        {
                            // あなたの会話
                            conversationMessages = new string[]
                            {
                                "あなた「お支払いありがとうございます！」",
                                "あなた「またのお越しをお待ちしております。」"
                            };

                            // メッセージ音を再生
                            //SoundManager.instance.PlayMessageSound();

                            // 会話を開始し、終了後に次の処理を実行
                            StartConversation(() =>
                            {
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // フラグをリセット（伝票が閉じられたことを示す）
                                    isReceiptDisplayed = false;

                                    // プレイヤーの移動と視点移動を有効化
                                    EnablePlayerControl();

                                    // 支払い後に顧客を席から出入口まで移動させる処理を開始
                                    StartCoroutine(MoveToSpawnAfterPayment());
                                }));
                            });
                        }));
                    }));
                });
            }));
        }
        else
        {
            // 会話の開始（支払い金額の確認）
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // お客さんの会話
                conversationMessages = new string[]
                {
                $"あなた「いえ。料金は{billAmount}円で合っております。",
                $"お客さん「えぇ？{billAmount}円もするの？」",  // 請求金額を表示
                $"お客さん「絶対おかしいわ。こんなのぼったくりよ！」",
                $"お客さん「私、払わないから！！」"
                };

                // メニューを閉じる
                paymentMenuCanvas.SetActive(false);

                // メッセージ音を再生
                //SoundManager.instance.PlayMessageSound();

                // 会話を開始し、次の処理へ移行
                StartConversation(() =>
                {
                    // 0.3秒の遅延を追加
                    StartCoroutine(DelayWithAction(0.3f, () =>
                    {
                        // 効果音（失敗音）を再生
                        SoundManager.instance.PlayFailureSound();

                        // 0.5秒の遅延を追加
                        StartCoroutine(DelayWithAction(0.5f, () =>
                        {
                            //決済失敗のメッセージ画像を表示
                            ShowMessageImage2(paymentFailureSprite);

                            paymentDepartureDelay = 0f; //すぐに席を立つように変更
                            moveSpeed = 2.25f; // 移動速度を変更
                            animator.speed = 1.5f; // アニメーション速度を変更

                            // 支払い後に顧客を席から出入口まで移動させる処理を開始
                            StartCoroutine(MoveToSpawnAfterPayment());

                            // プレイヤーの視点移動のみ有効化
                            EnablePlayerControl();
                            DisablePlayerMovement();

                            // 遅延して、プレイヤーの会話
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                // あなたの会話
                                conversationMessages = new string[]
                                {
                                    "あなた「ちょ、ちょっと待って！！」"
                                };

                                // メッセージ音を再生
                                //SoundManager.instance.PlayMessageSound();

                                // 会話を開始し、終了後に次の処理を実行
                                StartConversation(() =>
                                {
                                    StartCoroutine(DelayWithAction(0.25f, () =>
                                    {
                                        // フラグをリセット（伝票が閉じられたことを示す）
                                        isReceiptDisplayed = false;

                                        // プレイヤーの移動と視点移動を有効化
                                        EnablePlayerControl();
                                    }));
                                });
                            }));
                        }));
                    }));
                });
            }));
        }
    }

    // ボタンリスナーをクリアして再登録する関数
    private void ClearButtonListeners()
    {
        // ボタンのリスナーをクリア
        plusThousandsButton.onClick.RemoveAllListeners();
        minusThousandsButton.onClick.RemoveAllListeners();
        plusHundredsButton.onClick.RemoveAllListeners();
        minusHundredsButton.onClick.RemoveAllListeners();
        plusTensButton.onClick.RemoveAllListeners();
        minusTensButton.onClick.RemoveAllListeners();
        plusOnesButton.onClick.RemoveAllListeners();
        minusOnesButton.onClick.RemoveAllListeners();

        // 新しいリスナーを追加
        plusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, 1, billThousandsText));
        minusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, -1, billThousandsText));
        plusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, 1, billHundredsText));
        minusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, -1, billHundredsText));
        plusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, 1, billTensText));
        minusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, -1, billTensText));
        plusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, 1, billOnesText));
        minusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, -1, billOnesText));
    }

    // 不満度バーの表示を更新する関数
    private void UpdateSatisfactionBar()
    {
        // 不満度を0から1の範囲に正規化し、バーのfillAmountに反映
        satisfactionBar.fillAmount = satisfactionLevel / 100f;
    }

    private void StartCalling()
    {
        isCalling = true;
        callTimeElapsed = 0f; // 経過時間をリセット
        isCallDecaying = false; // 通常の減少フラグをリセット
        isFastCallDecaying = false; // 速い減少フラグをリセット

        // 呼出中アイコンを設定       
        switch (languageIndex)
        {
            case 0: // 英語
                callIcon.GetComponent<Image>().sprite = callingIconSprite_en;
                break;
            case 1: // 日本語
                callIcon.GetComponent<Image>().sprite = callingIconSprite;
                break;
        }
        callIcon.SetActive(true);
    }

    private void StopCalling()
    {
        isCalling = false;

        isCallDecaying = false; // フラグをリセット
        isFastCallDecaying = false; // フラグをリセット

        // アイコンの点滅を停止
        StopCoroutine("BlinkNormalCallIcon");
        StopCoroutine("BlinkFastCallIcon");

        callIcon.SetActive(false); // 呼出中アイコンを非表示
    }

    private void StartWaiting()
    {
        // 待機中の経過時間をリセット
        waitTimeElapsed = 0f;
        isWaitDecaying = false;
        isFastWaitDecaying = false;

        // 待機中のフラグを設定
        isWaiting = true;

        // 待機中アイコンを表示（最初は通常の待機アイコン）
        callIcon.GetComponent<Image>().sprite = waitingIconSprite;
        callIcon.SetActive(true);

        Debug.Log("待機中が開始されました。");
    }

    private void StopWaiting()
    {
        // 待機中のフラグを解除
        isWaiting = false;

        isWaitDecaying = false; // フラグをリセット
        isFastWaitDecaying = false; // フラグをリセット

        // アイコンの点滅を停止
        StopCoroutine("BlinkNormalWaitIcon");
        StopCoroutine("BlinkFastWaitIcon");

        // 待機中アイコンを非表示
        callIcon.SetActive(false);

        Debug.Log("待機中が停止されました。");
    }

    private void StartAwaitingPayment()
    {
        // 支払い待ち状態を開始
        isAwaitingPayment = true;
        awaitingPaymentTimeElapsed = 0f; // 経過時間をリセット
        isAwaitingPaymentDecaying = false; // 通常の減少フラグをリセット
        isFastAwaitingPaymentDecaying = false; // 速い減少フラグをリセット

        // 支払い待ち中アイコンを設定       
        switch (languageIndex)
        {
            case 0: // 英語
                callIcon.GetComponent<Image>().sprite = payIconSprite_en;
                break;
            case 1: // 日本語
                callIcon.GetComponent<Image>().sprite = payIconSprite;
                break;
        }
        callIcon.SetActive(true);

        Debug.Log("支払い待ちが開始されました。");
    }

    private void StopAwaitingPayment()
    {
        isAwaitingPayment = false; // 支払い待ち状態を停止

        isAwaitingPaymentDecaying = false; // フラグをリセット
        isFastAwaitingPaymentDecaying = false; // フラグをリセット

        // アイコンの点滅を停止
        StopCoroutine("BlinkNormalAwaitingPaymentIcon");
        StopCoroutine("BlinkFastAwaitingPaymentIcon");

        callIcon.SetActive(false); // 支払い待ち中アイコンを非表示

        Debug.Log("支払い待ちが終了しました。");
    }

    private IEnumerator SpawnCustomerAfterDelay()
    {
        Debug.Log("スポーン待機開始");

        // スポーン時間まで待機
        yield return new WaitForSeconds(spawnDelay);

        // 席が空いているかチェック
        if (seatManager.GetAvailableSeats().Count > 0)
        {
            // 席が空いている場合、客をスポーンさせる
            SpawnCustomer();
        }
        else
        {
            // 席が空いていない場合、キューに追加して待機
            waitingCustomers.Enqueue(this);
            Debug.Log("空席がないため、客が待機リストに追加されました。");
        }
    }

    // 席が空いたときに呼ばれる関数
    public static void OnSeatAvailable()
    {
        if (waitingCustomers.Count > 0)
        {
            // 待機している最初の客をスポーンさせる
            CustomerCallIcon nextCustomer = waitingCustomers.Dequeue();
            nextCustomer.SpawnCustomer();
        }
    }

    // 実際に客をスポーンさせる処理を行う関数
    private void SpawnCustomer()
    {
        // 実際に客をスポーンさせる処理
        ShowCustomer();
        transform.position = spawnPoint.position; // 入り口の位置に移動
        isSpawned = true;

        Debug.Log("客がスポーンしました。");

        // 空いている座席に移動する処理
        MoveToRandomSeat();
    }

    private void HideCustomer()
    {
        // モデル（見た目）を非表示にする（例: Rendererをオフにする）
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // 必要ならコライダーも無効にする
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void ShowCustomer()
    {
        // モデルを再表示する
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        // コライダーも有効にする
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    // 空いている座席に移動する処理
    private void MoveToRandomSeat()
    {
        // 空いている座席のリストを取得
        List<int> availableSeats = seatManager.GetAvailableSeats();

        // 空いている座席がない場合
        if (availableSeats.Count == 0)
        {
            Debug.LogError("空いている席がありません！");
            return;
        }

        // ランダムに空き席を選択
        selectedSeatIndex = availableSeats[UnityEngine.Random.Range(0, availableSeats.Count)];
        seatManager.ReserveSeat(selectedSeatIndex); // 座席を予約

        // 選択された座席に移動
        Transform targetSeat = seatManager.GetSeatTransform(selectedSeatIndex);
        StartCoroutine(MoveToSeat(targetSeat));  // 座席への移動処理
    }

    // 座席へ移動するコルーチン
    private IEnumerator MoveToSeat(Transform targetSeat)
    {
        // 座席に対応するウェイポイントのリストを取得
        Transform[] waypoints = seatManager.GetWaypointsForSeat(selectedSeatIndex);

        // 各ウェイポイントを順番に移動
        foreach (var waypoint in waypoints)
        {
            while (Vector3.Distance(transform.position, waypoint.position) > 0.1f)
            {
                // 進行方向を向くように回転
                Vector3 direction = (waypoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // 回転速度を調整

                transform.position = Vector3.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // 最後に座席に移動
        while (Vector3.Distance(transform.position, targetSeat.position) > 0.1f)
        {
            // 進行方向を向くように回転
            Vector3 direction = (targetSeat.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // 回転速度を調整

            transform.position = Vector3.MoveTowards(transform.position, targetSeat.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("客が席に着きました。");

        // SeatManagerからY軸の回転角を取得
        float targetYRotation = seatManager.seatRotations[selectedSeatIndex];

        // 指定されたY軸の回転角に向かって回転させる
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        Debug.Log("客が座席の指定された方向に向きました。");

        // 歩行アニメーションを停止し、座っているアニメーションを開始
        animator.SetBool("isWalking", false);  // Walkingアニメーションを停止
        animator.SetBool("isSitting", true);   // Sittingアニメーションを再生

        // 指定した遅延時間を待機
        yield return new WaitForSeconds(seatArrivalDelay);

        // CallCanvasを表示する
        callCanvas.SetActive(true);

        // 呼出中の開始
        StartCalling();

        // 席に着いたらIdle状態を解除
        isIdle = false;

        // 客が席についた際にプレイヤーがtriggerDistance内にいる場合、メッセージを表示
        float currentDistance = Vector3.Distance(player.position, customer.position);
        if (currentDistance <= triggerDistance && !isRotationRestricted && IsClosestCustomer())
        {
            if (isWaiting)
            {
                switch (languageIndex)
                {
                    case 0: // 英語
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                        break;
                    case 1: // 日本語
                        ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を再確認する");
                        break;
                }
            }
            else
            {
                switch (languageIndex)
                {
                    case 0: // 英語
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                        break;
                    case 1: // 日本語
                        ShowMessageImage1($"<size=90>{confirmKey}</size> を押して注文を受ける");
                        break;
                }
            }
        }
    }

    // 支払い後に顧客を座席から入り口まで移動させるコルーチン
    private IEnumerator MoveToSpawnAfterPayment()
    {
        // CallCanvasを非表示にする
        callCanvas.SetActive(false);

        // Idle状態にする
        isIdle = true;

        // 指定した遅延時間を待機
        yield return new WaitForSeconds(paymentDepartureDelay);

        // 座っているアニメーションを停止し、歩行アニメーションを再生
        animator.SetBool("isSitting", false);   // Sittingアニメーションを停止
        animator.SetBool("isWalking", true);    // Walkingアニメーションを再生

        // 座席に対応するウェイポイントのリストを逆順に取得
        Transform[] waypoints = seatManager.GetWaypointsForSeat(selectedSeatIndex);
        for (int i = waypoints.Length - 1; i >= 0; i--)
        {
            Transform waypoint = waypoints[i];
            while (Vector3.Distance(transform.position, waypoint.position) > 0.1f)
            {
                // 進行方向を向くように回転
                Vector3 direction = (waypoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // 回転速度を調整

                transform.position = Vector3.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // 最後にspawnPointに戻る
        while (Vector3.Distance(transform.position, spawnPoint.position) > 0.1f)
        {
            // 進行方向を向くように回転
            Vector3 direction = (spawnPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // 回転速度を調整

            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 顧客を非表示にする
        HideCustomer();

        // 座席の状態をリセット
        seatManager.ReleaseSeat(selectedSeatIndex);

        // 客の完了を登録
        CompleteCustomer();
    }

    // 1文字ずつテキストを表示するコルーチン
    private IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        conversationText.text = ""; // テキストをクリア

        // タイピング音を再生
        SoundManager.instance.PlayTypingSound();

        foreach (char letter in message.ToCharArray())
        {
            conversationText.text += letter; // 1文字ずつ追加
            yield return new WaitForSeconds(typingSpeed); // 指定の速度で待機
        }

        // タイピング終了時に音を停止
        SoundManager.instance.StopTypingSound();
        isTyping = false; // タイピング終了
    }


    // 会話の開始処理（遅延付き）
    private IEnumerator StartConversationWithDelay()
    {
        // 会話用のCanvasを表示
        conversationCanvas.gameObject.SetActive(true);

        // 最初のメッセージをセット
        conversationIndex = 0;

        // コルーチンで文字を徐々に表示
        yield return StartCoroutine(TypeMessage(conversationMessages[conversationIndex]));

        Debug.Log("会話が開始されました。");

        isConversing = true;
    }

    public void StartConversation(System.Action callback = null)
    {
        //messageImage1と3のalphaを0に変更
        if (messageImage1 != null)
        {
            Color color = messageImage1.color;
            color.a = 0f; // アルファ値を0に設定
            messageImage1.color = color;
        }
        if (messageImage3 != null)
        {
            Color color = messageImage3.color;
            color.a = 0f; // アルファ値を0に設定
            messageImage3.color = color;
        }

        conversationCanvas.gameObject.SetActive(true); // Canvasを表示
        isConversing = true; // 会話中に設定
        isConversationJustStarted = true; // ここでフラグを設定
        onConversationEndCallback = callback; // 会話終了後の処理を設定
        conversationIndex = 0; // 会話の最初のメッセージから開始
        DisplayNextMessage(); // 最初のメッセージを表示
    }

    private void DisplayNextMessage()
    {
        // まだメッセージが残っている場合
        if (conversationIndex < conversationMessages.Length)
        {
            // 効果音（メッセージ音）を再生
            SoundManager.instance.PlayMessageSound();

            // メッセージを1文字ずつ表示するコルーチンを開始
            typingCoroutine = StartCoroutine(TypeMessage(conversationMessages[conversationIndex])); // コルーチンの参照を保存
            conversationIndex++;
        }
        else
        {
            EndConversation(); // 会話終了
        }
    }


    private void EndConversation()
    {
        //messageImage1と3のalphaを1に変更
        if (messageImage1 != null)
        {
            Color color = messageImage1.color;
            color.a = 1f; // アルファ値を1に設定
            messageImage1.color = color;
        }
        if (messageImage3 != null)
        {
            Color color = messageImage3.color;
            color.a = 1f; // アルファ値を1に設定
            messageImage3.color = color;
        }

        isConversing = false;
        conversationCanvas.gameObject.SetActive(false); // 会話用のCanvasを非表示
        onConversationEndCallback?.Invoke(); // 会話終了時のコールバックを呼び出し
    }


    // 汎用的な遅延関数
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // 指定時間の遅延
        yield return new WaitForSeconds(delayTime);

        // 遅延後にコールバックとして渡されたアクションを実行
        action?.Invoke();
    }

    // 通常の呼出中アイコンの点滅処理
    private IEnumerator BlinkNormalCallIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isCallDecaying && !isFastCallDecaying) // 通常の増加が行われている間
        {
            switch (languageIndex)
            {
                case 0: // 英語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite_en : decayingCallIcon_en;
                    break;
                case 1: // 日本語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite : decayingCallIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // 速い呼出中アイコンの点滅処理
    private IEnumerator BlinkFastCallIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastCallDecaying) // 速い増加が行われている間
        {
            switch (languageIndex)
            {
                case 0: // 英語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite_en : fastDecayingCallIcon_en;
                    break;
                case 1: // 日本語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite : fastDecayingCallIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator BlinkNormalWaitIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isWaitDecaying && !isFastWaitDecaying) // 通常の増加が行われている間
        {
            callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? waitingIconSprite : decayingWaitIcon;
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator BlinkFastWaitIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastWaitDecaying) // 速い増加が行われている間
        {
            callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? waitingIconSprite : fastDecayingWaitIcon;
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // 通常の支払い待ちアイコンの点滅処理
    private IEnumerator BlinkNormalAwaitingPaymentIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isAwaitingPaymentDecaying && !isFastAwaitingPaymentDecaying) // 通常の増加が行われている間
        {
            switch (languageIndex)
            {
                case 0: // 英語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite_en : decayingAwaitingPaymentIcon_en;
                    break;
                case 1: // 日本語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite : decayingAwaitingPaymentIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // 速い支払い待ちアイコンの点滅処理
    private IEnumerator BlinkFastAwaitingPaymentIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastAwaitingPaymentDecaying) // 速い増加が行われている間
        {
            switch (languageIndex)
            {
                case 0: // 英語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite_en : fastDecayingAwaitingPaymentIcon_en;
                    break;
                case 1: // 日本語
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite : fastDecayingAwaitingPaymentIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // 客数をカウント
    private void RegisterCustomer()
    {
        totalCustomers++;
    }

    //　客が帰った後の処理
    private void CompleteCustomer()
    {
        completedCustomers++;

        // デバッグにtotalCustomersとcompleteCustomersを表示
        Debug.Log("Total Customers: " + totalCustomers);
        Debug.Log("Completed Customers: " + completedCustomers);

        // すべての客が完了したかチェック
        if (completedCustomers >= totalCustomers)
        {
            // 2秒の遅延を入れてからリザルト画面を表示
            StartCoroutine(DelayWithAction(2.0f, () =>
            {
                StageManager.instance.ShowResultScreen();
            }));
        }
    }

    // 不満度バーを点滅させるコルーチン
    private IEnumerator BlinkSatisfactionBar()
    {
        while (true)
        {
            // 点滅用の画像に切り替え
            satisfactionBar.sprite = blinkingSatisfactionBarSprite;
            yield return new WaitForSeconds(satisfactionBlinkInterval1);

            // 通常の画像に戻す
            satisfactionBar.sprite = normalSatisfactionBarSprite;
            yield return new WaitForSeconds(satisfactionBlinkInterval2);
        }
    }

    // ゲームオーバーを実行するメソッド
    private void GameOver()
    {
        // 既にゲームオーバーになっている場合は何もしない
        if (isGameOver)
            return;

        isGameOver = true;

        // プレイヤーの移動と視点移動を停止
        DisablePlayerControl();

        // マウスカーソルを表示
        //player.GetComponent<FirstPersonMovement>().UnlockCursor();

        // 黒い画像とゲームオーバー文字のアルファを0に設定
        if (gameOverBlackImage != null)
        {
            Color blackColor = gameOverBlackImage.color;
            blackColor.a = 0f;
            gameOverBlackImage.color = blackColor;
        }

        if (gameOverText != null)
        {
            Color textColor = gameOverText.color;
            textColor.a = 0f;
            gameOverText.color = textColor;
        }

        // ゲームオーバーCanvasをアクティブにする
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        // フェードイン処理を開始
        StartCoroutine(FadeInGameOver());
    }

    // ゲームオーバーCanvas内の黒い画像とテキストをフェードインさせるコルーチン
    private IEnumerator FadeInGameOver()
    {
        // 黒い画像を徐々に暗くする
        float blackFadeDuration = 2f; // 黒い画像のフェードイン時間
        float elapsed = 0f;

        while (elapsed < blackFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / blackFadeDuration);

            if (gameOverBlackImage != null)
            {
                Color blackColor = gameOverBlackImage.color;
                blackColor.a = alpha;
                gameOverBlackImage.color = blackColor;
            }

            yield return null;
        }

        // 黒い画像のアルファを完全に1に設定
        if (gameOverBlackImage != null)
        {
            Color blackColor = gameOverBlackImage.color;
            blackColor.a = 1f;
            gameOverBlackImage.color = blackColor;
        }

        // ゲームオーバーテキストを素早くフェードインする
        float textFadeDuration = 1f; // テキストのフェードイン時間
        elapsed = 0f;

        while (elapsed < textFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / textFadeDuration);

            if (gameOverText != null)
            {
                Color textColor = gameOverText.color;
                textColor.a = alpha;
                gameOverText.color = textColor;
            }

            yield return null;
        }

        // テキストのアルファを完全に1に設定
        if (gameOverText != null)
        {
            Color textColor = gameOverText.color;
            textColor.a = 1f;
            gameOverText.color = textColor;
        }

        // メニューに戻るボタンをアクティブにする
        if (returnToMenuButton != null)
        {
            returnToMenuButton.gameObject.SetActive(true);
        }

        // マウスカーソルを表示
        player.GetComponent<FirstPersonMovement>().UnlockCursor();
    }

    // メニューに戻るボタンがクリックされたときに呼ばれるメソッド
    private void OnReturnToMenuButtonClicked()
    {
        // 遅延を入れてからフェードアウトとシーン遷移を行うコルーチンを開始
        StartCoroutine(DelayWithAction(0.1f, () =>
        {
            StartCoroutine(FadeAndLoadScene("MenuScene"));
        }));
    }

    private bool IsClosestCustomer()
    {
        CustomerCallIcon[] allCustomers = FindObjectsOfType<CustomerCallIcon>();
        float closestDistance = float.MaxValue;
        CustomerCallIcon closestCustomer = null;

        foreach (CustomerCallIcon customer in allCustomers)
        {
            float distance = Vector3.Distance(player.position, customer.customer.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCustomer = customer;
            }
        }

        // この客が最も近い客であればtrueを返す
        return closestCustomer == this;
    }

    // プレイヤーの視界に客が入っているかを判定するメソッド
    private bool IsCustomerInPlayerView()
    {
        Vector3 directionToCustomer = (customer.position - PlayerCamera.position).normalized;
        float angle = Vector3.Angle(PlayerCamera.forward, directionToCustomer);
        return angle < playerFieldOfViewAngle * 0.5f;
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // フェードアウト
        yield return StartCoroutine(Fade(1f));

        // 遅延を挟む
        yield return StartCoroutine(DelayWithAction(0.3f, null));

        // シーンをロード
        SceneManager.LoadScene(sceneName);

        // フェードイン
        //yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        // 最終的なAlpha値を目標値に設定
        fadeCanvasGroup.alpha = targetAlpha;
    }

    public void LoadKeySettings()
    {
        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        serveKey = (KeyCode)PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);
    }

    private void ApplyLanguage(int index)
    {
        switch (index)
        {
            case 0: // 英語
                satisfactionBarBackground.sprite = satisfactionBarBackgroundSprite_en;
                break;
            case 1: // 日本語
                satisfactionBarBackground.sprite = satisfactionBarBackgroundSprite;
                break;
        }
    }
}