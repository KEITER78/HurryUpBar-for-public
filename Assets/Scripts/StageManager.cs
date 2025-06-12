using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // TextMeshProを使用するために追加

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public static float totalRevenue = 0f;  // 合計売上を保持する変数

    public GameObject resultScreenCanvas;  // リザルト画面のCanvas
    public Image resultBackgroundImage;     // リザルト画面の背景画像（フェードイン用）
    public TextMeshProUGUI revenueText;    // リザルト画面内の売上金表示用テキスト
    public TextMeshProUGUI totalRevenueText;    // 合計売上表示用テキスト
    public TextMeshProUGUI targetRevenueText;    // 目標金額表示用テキスト
    public TextMeshProUGUI stageClearText;  // ステージクリア用のテキスト
    public TextMeshProUGUI clearTimeText; // クリア時間表示用テキスト

    public FirstPersonMovement playerMovement;  // プレイヤーの動作スクリプトを指定

    public int nextStageNumber = 1;  // 次のステージ番号を管理する変数
    public Button nextStageButton;  // 次のステージに進むためのボタン
    
    public TextMeshProUGUI elapsedTimeText;   // 残り時間を表示するTextMeshProUGUIをInspectorで指定
    public float elapsedTime = 0f;  // 経過時間（秒） 
    private bool hasShownResult = false;    // ShowResultScreenが既に呼び出されたかを管理するフラグ

    private Coroutine fadeInCoroutine;  // フェードイン処理を管理するコルーチンの参照

    // 「セーブして次へ」クリック後のフェード処理
    private CanvasGroup fadeCanvasGroup; // フェード用のCanvasGroup
    private float fadeDuration = 0.6f; // フェードの時間

    // ゲーム開始時のフェード処理
    private CanvasGroup fadeInCanvasGroup; // フェードイン用のCanvasGroup
    private float fadeInDuration = 2.0f;   // フェードインの時間（秒）

    // 言語設定のインデックス
    private int languageIndex;

    [Header("Index 1:月曜日, 2:火曜日, 3:水曜日, 4:木曜日, 5:金曜日, 6:土曜日, 7:日曜日")]
    public TMP_Text dayofWeekText;
    public int dayofWeekIndex = 1;


    void Awake()
    {
        // シングルトンパターンの実装
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は破棄
        }
    }

    void Start()
    {
        // PlayerPrefsから言語設定のインデックスを取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルト値は0

        // 静的オブジェクトに言語設定を適用
        ApplyLanguage(languageIndex);

        // 初期化時にリザルトキャンバスを非表示に設定
        resultScreenCanvas.SetActive(false);

        // 次のステージへ進むボタンにリスナーを設定
        nextStageButton.onClick.AddListener(LoadNextStage);

        // 顧客カウントをリセット
        //CustomerCallIcon.totalCustomers = 0;
        //CustomerCallIcon.completedCustomers = 0;

        // WhiteFadeCanvas内のWhiteImageを自動検索し、CanvasGroupを取得
        GameObject fadeCanvas = GameObject.Find("WhiteFadeCanvas");
        if (fadeCanvas != null)
        {
            GameObject blackImage = fadeCanvas.transform.Find("WhiteImage")?.gameObject;
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
                    Debug.LogError("WhiteImageにCanvasGroupコンポーネントが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("WhiteFadeCanvas内にBlackImageが見つかりませんでした。");
            }
        }
        else
        {
            Debug.LogError("WhiteFadeCanvasが見つかりませんでした。");
        }

        // フェードイン用のCanvasGroupを取得
        GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
        if (fadeInCanvas != null)
        {
            GameObject blackImage = fadeInCanvas.transform.Find("BlackImage")?.gameObject;
            if (blackImage != null)
            {
                fadeInCanvasGroup = blackImage.GetComponent<CanvasGroup>();
                if (fadeInCanvasGroup != null)
                {
                    // 初期状態で完全に不透明に設定
                    fadeInCanvasGroup.alpha = 1f;
                    // フェードインコルーチンを開始
                    StartCoroutine(FadeIn());
                }
                else
                {
                    Debug.LogError("BlackImageにCanvasGroupコンポーネントが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvas内にBlackImageが見つかりませんでした。");
            }
        }
        else
        {
            Debug.LogError("FadeInCanvasが見つかりませんでした。");
        }
    }

    void Update()
    {
        // 経過時間を増加
        elapsedTime += Time.deltaTime;

        // 経過時間が0未満にならないように制御
        if (elapsedTime < 0)
            elapsedTime = 0;

        // 経過時間をテキストに表示（分:秒形式）
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        if (elapsedTimeText != null)
        {           
            switch (languageIndex)
            {
                case 0: // 英語
                    elapsedTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
                    break;
                case 1: // 日本語
                    elapsedTimeText.text = string.Format("経過時間: {0:00}:{1:00}", minutes, seconds);
                    break;
            }
        }   
    }

    // リザルト画面を表示するメソッド
    public void ShowResultScreen()
    {
        // 既にリザルト画面が表示されている場合は何もしない
        if (hasShownResult)
            return;

        hasShownResult = true; // フラグを立てる

        // マウスカーソルを表示
        playerMovement.UnlockCursor();

        // elapsedTimeの加算を停止させるために、Updateメソッドでの加算処理を止める
        elapsedTimeText.enabled = false;

        // クリア時間を表示
        if (clearTimeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            
            switch (languageIndex)
            {
                case 0: // 英語
                    clearTimeText.text = string.Format("Clear Time: {0:00}:{1:00}", minutes, seconds);
                    break;
                case 1: // 日本語
                    clearTimeText.text = string.Format("クリア時間: {0:00}:{1:00}", minutes, seconds);
                    break;
            }
        }

        if (resultScreenCanvas != null && revenueText != null)
        {
            // 売上金をテキストに設定           
            switch (languageIndex)
            {
                case 0: // 英語
                    revenueText.text = "Today's Sales: " + Mathf.FloorToInt(CustomerCallIcon.dailyRevenue).ToString() + " yen";
                    break;
                case 1: // 日本語
                    revenueText.text = "本日の売上: " + Mathf.FloorToInt(CustomerCallIcon.dailyRevenue).ToString() + " 円";
                    break;
            }

            // 合計売上に本日の売上を加算
            totalRevenue += CustomerCallIcon.dailyRevenue;

            // 合計売上をテキストに設定
            if (totalRevenueText != null)
            {     
                switch (languageIndex)
                {
                    case 0: // 英語
                        totalRevenueText.text = "Total Sales: " + Mathf.FloorToInt(totalRevenue).ToString() + " yen";
                        break;
                    case 1: // 日本語
                        totalRevenueText.text = "合計売上: " + Mathf.FloorToInt(totalRevenue).ToString() + " 円";
                        break;
                }
            }

            // PlayerPrefsからTargetRevenueを取得して表示
            if (targetRevenueText != null)
            {
                float targetRevenue = PlayerPrefs.GetFloat("TargetRevenue", 0f); // 目標金額を取得
                
                switch (languageIndex)
                {
                    case 0: // 英語
                        targetRevenueText.text = "Target Sales: " + Mathf.FloorToInt(targetRevenue).ToString() + " yen";
                        break;
                    case 1: // 日本語
                        targetRevenueText.text = "目標金額: " + Mathf.FloorToInt(targetRevenue).ToString() + " 円";
                        break;
                }
            }

            // リザルトキャンバス内のUI要素のアルファを0に設定
            if (resultBackgroundImage != null)
            {
                Color bgColor = resultBackgroundImage.color;
                bgColor.a = 0f;
                resultBackgroundImage.color = bgColor;
            }

            if (revenueText != null)
            {
                Color revColor = revenueText.color;
                revColor.a = 0f;
                revenueText.color = revColor;
            }

            if (totalRevenueText != null)
            {
                Color totalRevColor = totalRevenueText.color;
                totalRevColor.a = 0f;
                totalRevenueText.color = totalRevColor;
            }

            if (targetRevenueText != null)
            {
                Color targetRevColor = targetRevenueText.color;
                targetRevColor.a = 0f;
                targetRevenueText.color = targetRevColor;
            }

            if (stageClearText != null)
            {
                Color stageClearColor = stageClearText.color;
                stageClearColor.a = 0f;
                stageClearText.color = stageClearColor;
            }

            if (clearTimeText != null)
            {
                Color clearTimeColor = clearTimeText.color;
                clearTimeColor.a = 0f;
                clearTimeText.color = clearTimeColor;
            }

            // リザルトキャンバスを表示
            resultScreenCanvas.SetActive(true);

            // プレイヤーの動きと視点移動を禁止（Inspectorで指定された playerMovement を使用）
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
                playerMovement.DisableLook();
            }
            // リザルトBGMを再生
            SoundManager.instance.PlayResultBGM();

            // フェードイン処理を開始
            if (fadeInCoroutine == null)
            {
                fadeInCoroutine = StartCoroutine(FadeInResultScreen());
            }
        }
    }


    // 次のシーンを読み込む
    void LoadNextStage()
    {
        // リザルト画面を非表示にする
        //resultScreenCanvas.SetActive(false);

        // 顧客カウントをリセット
        CustomerCallIcon.totalCustomers = 0;
        CustomerCallIcon.completedCustomers = 0;

        // 売上金をリセット（次の日になるため）
        CustomerCallIcon.dailyRevenue = 0f;

        // プレイヤーの動きを再度有効化
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
            playerMovement.EnableLook();
        }

        // ゲームの進捗をセーブ
        SaveManager.SaveGame(nextStageNumber, totalRevenue);

        // 次のステージをロード
        string nextSceneName = "Stage" + nextStageNumber;  // ステージ名は "Stage1", "Stage2" などと仮定

        // フェードアウトしてシーンをロードするコルーチンを開始
        StartCoroutine(DelayWithAction(0.1f, () =>
        {
            StartCoroutine(FadeAndLoadScene(nextSceneName));
        }));
    }

    // リザルト画面をフェードインさせるコルーチン
    private IEnumerator FadeInResultScreen()
    {
        float fadeDuration = 1f; // フェードインにかける時間（秒）
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            // 背景画像のフェードイン
            if (resultBackgroundImage != null)
            {
                Color bgColor = resultBackgroundImage.color;
                bgColor.a = alpha;
                resultBackgroundImage.color = bgColor;
            }

            // 売上金テキストのフェードイン
            if (revenueText != null)
            {
                Color revColor = revenueText.color;
                revColor.a = alpha;
                revenueText.color = revColor;
            }

            // 合計売上テキストのフェードイン
            if (totalRevenueText != null)
            {
                Color totalRevColor = totalRevenueText.color;
                totalRevColor.a = alpha;
                totalRevenueText.color = totalRevColor;
            }

            // 目標金額テキストのフェードイン
            if (targetRevenueText != null)
            {
                Color targetRevColor = targetRevenueText.color;
                targetRevColor.a = alpha;
                targetRevenueText.color = targetRevColor;
            }

            // ステージクリア用テキストのフェードイン
            if (stageClearText != null)
            {
                Color targetRevColor = stageClearText.color;
                targetRevColor.a = alpha;
                stageClearText.color = targetRevColor;
            }

            // クリア時間テキストのフェードイン
            if (clearTimeText != null)
            {
                Color clearTimeColor = clearTimeText.color;
                clearTimeColor.a = alpha;
                clearTimeText.color = clearTimeColor;
            }

            yield return null;
        }

        // 完全にフェードインした状態にする
        if (resultBackgroundImage != null)
        {
            Color bgColor = resultBackgroundImage.color;
            bgColor.a = 1f;
            resultBackgroundImage.color = bgColor;
        }

        if (revenueText != null)
        {
            Color revColor = revenueText.color;
            revColor.a = 1f;
            revenueText.color = revColor;
        }

        if (totalRevenueText != null)
        {
            Color totalRevColor = totalRevenueText.color;
            totalRevColor.a = 1f;
            totalRevenueText.color = totalRevColor;
        }

        if (targetRevenueText != null)
        {
            Color targetRevColor = targetRevenueText.color;
            targetRevColor.a = 1f;
            targetRevenueText.color = targetRevColor;
        }

        if (stageClearText != null)
        {
            Color targetRevColor = stageClearText.color;
            targetRevColor.a = 1f;
            stageClearText.color = targetRevColor;
        }

        if (clearTimeText != null)
        {
            Color clearTimeColor = clearTimeText.color;
            clearTimeColor.a = 1f;
            clearTimeText.color = clearTimeColor;
        }

        fadeInCoroutine = null; // コルーチンの参照をクリア
    }

    // 汎用的な遅延関数
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // 指定時間の遅延
        yield return new WaitForSeconds(delayTime);

        // 遅延後にコールバックとして渡されたアクションを実行
        action?.Invoke();
    }

    // フェードアウトしてシーンをロードするコルーチン
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // フェードアウト
        yield return StartCoroutine(Fade(1f));

        // 遅延を挟む
        yield return StartCoroutine(DelayWithAction(0.3f, null));

        // シーンをロード
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // フェードイン
        // yield return StartCoroutine(Fade(0f));
    }

    // フェード処理のコルーチン
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

    // フェードイン用のコルーチン
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            fadeInCanvasGroup.alpha = alpha;
            yield return null;
        }

        // 完全にフェードインし終わったらalphaを0に設定
        fadeInCanvasGroup.alpha = 0f;
    }

    private void ApplyLanguage(int index)
    {
        switch (dayofWeekIndex)
        {
            case 1: // 月曜日                
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Monday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "月曜日";
                        break;
                }
                break;

            case 2: // 火曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Tuesday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "火曜日";
                        break;
                }
                break;

            case 3: // 水曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Wednesday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "水曜日";
                        break;
                }
                break;

            case 4: // 木曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Thursday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "木曜日";
                        break;
                }
                break;

            case 5: // 金曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Friday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "金曜日";
                        break;
                }
                break;

            case 6: // 土曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Saturday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "土曜日";
                        break;
                }
                break;

            case 7: // 日曜日
                switch (index)
                {
                    case 0: // 英語
                        dayofWeekText.text = "Sunday";
                        break;
                    case 1: // 日本語
                        dayofWeekText.text = "日曜日";
                        break;
                }
                break;
        }
    }
}
