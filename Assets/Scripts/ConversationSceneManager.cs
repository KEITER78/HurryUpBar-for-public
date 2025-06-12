using System.Collections;
using UnityEngine;
using TMPro; // TextMeshProを使用
using UnityEngine.SceneManagement;

public class ConversationSceneManager : MonoBehaviour
{
    public TextMeshProUGUI conversationText; // 会話テキストを表示するTextMeshPro
    public GameObject conversationCanvas; // 会話用のCanvas（UI全体）
    public TextMeshProUGUI pressEText; // 「Eを押して次へ」を表示するText
    public float idleTimeBeforeHint = 5.0f; // ヒントを表示するまでの待機時間
    private Coroutine hintCoroutine; // ヒント表示のコルーチン用
    public CanvasGroup fadeCanvasGroup; // フェード用CanvasGroupをInspectorで指定
    public float fadeDuration = 1.5f; // フェードイン・アウトの時間

    [TextArea(2, 5)] // Inspectorでの表示用（複数行のテキストを表示可能）
    public string[] conversationMessages; // 会話内容を保存する配列 (Inspectorから設定可能)
    [TextArea(2, 5)] // Inspectorでの表示用（複数行のテキストを表示可能）
    public string[] conversationMessages_en; // 会話内容を保存する配列 (Inspectorから設定可能)

    private int conversationIndex = 0; // 現在の会話メッセージのインデックス
    private bool isConversing = false; // 会話中かどうかを管理するフラグ
    public float typingSpeed = 0.05f; // 1文字ずつ表示する速度（秒）

    public float targetRevenue; // 目標金額をInspectorで指定できるようにする変数

    [Header("次に読み込むシーンを設定")]
    public string nextSceneName;   // 読み込むシーン名をInspectorで指定

    [Header("Start内で、BGMを再生する処理を行うか")]
    public bool playBGM = true; // BGM再生の有無をInspectorで指定

    // コールバックを設定可能
    private System.Action onConversationEndCallback;

    private bool isTyping = false; // タイピング中かどうか
    private Coroutine typingCoroutine; // コルーチンの参照を保存する変数

    [Header("フェードイン設定")]
    public bool enableFadeIn = false; // フェードインを有効にするかどうかをInspectorで設定
    private CanvasGroup fadeInCanvasGroup; // フェードイン用のCanvasGroup
    private float fadeInDuration = 1.6f;   // フェードインの時間（秒）

    [Header("開始時間の設定")]
    public bool useLongStartDelay = false;  // 長めの開始時間を使用するか（デフォルトはfalse）
    private float defaultStartDelay = 0.1f; // デフォルトの開始遅延時間（秒）
    private float longStartDelay = 1.8f;    // 長めの開始遅延時間（秒）

    private bool isCursorLocked = true; // カーソルがロックされているかどうかのフラグ

    private KeyCode confirmKey; // キー設定用の変数

    // 言語設定のインデックス
    private int languageIndex;

    [Header("Index 1:月曜日, 2:火曜日, 3:水曜日, 4:木曜日, 5:金曜日, 6:土曜日, 7:日曜日")]
    public TMP_Text dayofWeekText;
    public int dayofWeekIndex = 1;

    [Header("Stage906_2でのみTrue")]
    public bool saveStage116 = false;

    void Start()
    {
        // PlayerPrefsから言語設定のインデックスを取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルト値は0

        // 静的オブジェクトに言語設定を適用
        ApplyLanguage(languageIndex);

        // カーソルを非表示
        LockCursor();

        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);

        // conversationText を空白に設定
        conversationText.text = "";

        if (playBGM)
        {
            // 会話シーン用のBGMを再生
            SoundManager.instance.PlayConversationBGM();
        }

        // 「Eを押して次へ」のTextを非表示に
        pressEText.gameObject.SetActive(false);

        
        switch (languageIndex)
        {
            case 0: // 英語
                pressEText.text = $"Press <size=90>{confirmKey}</size> to continue";
                break;
            case 1: // 日本語
                pressEText.text = $"<size=90>{confirmKey}</size> を押して次へ";
                break;
        }


        // フェード用のCanvasGroupを初期化（透明化）
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f; // 完全に透明
        }

        // フェードイン処理を追加
        if (enableFadeIn)
        {
            // FadeInCanvas内のWhiteImageを取得
            GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
            if (fadeInCanvas != null)
            {
                GameObject whiteImage = fadeInCanvas.transform.Find("WhiteImage")?.gameObject;
                if (whiteImage != null)
                {
                    fadeInCanvasGroup = whiteImage.GetComponent<CanvasGroup>();
                    if (fadeInCanvasGroup != null)
                    {
                        // 初期状態で完全に不透明に設定
                        fadeInCanvasGroup.alpha = 1f;
                        // フェードインコルーチンを開始
                        StartCoroutine(FadeIn());
                    }
                    else
                    {
                        Debug.LogError("WhiteImageにCanvasGroupコンポーネントが見つかりませんでした。");
                    }
                }
                else
                {
                    Debug.LogError("FadeInCanvas内にWhiteImageが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvasが見つかりませんでした。");
            }
        }
        else
        {
            // フェードインを行わない場合は、WhiteImageのalphaを0に設定
            GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
            if (fadeInCanvas != null)
            {
                GameObject whiteImage = fadeInCanvas.transform.Find("WhiteImage")?.gameObject;
                if (whiteImage != null)
                {
                    CanvasGroup whiteCanvasGroup = whiteImage.GetComponent<CanvasGroup>();
                    if (whiteCanvasGroup != null)
                    {
                        whiteCanvasGroup.alpha = 0f;
                    }
                    else
                    {
                        Debug.LogError("WhiteImageにCanvasGroupコンポーネントが見つかりませんでした。");
                    }
                }
                else
                {
                    Debug.LogError("FadeInCanvas内にWhiteImageが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvasが見つかりませんでした。");
            }
        }

        // 遅延時間を選択
        float selectedDelay = useLongStartDelay ? longStartDelay : defaultStartDelay;

        // 会話を遅延して開始し、会話終了後の処理を設定
        StartCoroutine(DelayWithAction(selectedDelay, () =>
        {
            StartConversation(() =>
            {
                LoadNextScene(); // 会話終了後に次のシーンをロード
            });
        }));

        // 目標金額を保存
        SaveTargetRevenue(); // 関数を呼び出してtargetRevenueを保存
    }

    void Update()
    {
        // プレイヤーがEキーを押したときに次のメッセージを表示
        if (isConversing && Input.GetKeyDown(confirmKey))
        {
            if (!isTyping) // 文字がすべて表示されていれば次へ
            {
                DisplayNextMessage();

                // ヒント表示のコルーチンを停止し、非表示にする
                if (hintCoroutine != null)
                {
                    StopCoroutine(hintCoroutine);
                    pressEText.gameObject.SetActive(false);
                }
            }
            else
            {
                // まだタイピング中の場合、メッセージを全表示
                if (typingCoroutine != null) // コルーチンが実行中の場合
                {
                    StopCoroutine(typingCoroutine); // そのコルーチンを停止
                }
                
                switch (languageIndex)
                {
                    case 0: // 英語
                        conversationText.text = conversationMessages_en[conversationIndex - 1]; // 現在の全文を表示
                        break;
                    case 1: // 日本語
                        conversationText.text = conversationMessages[conversationIndex - 1]; // 現在の全文を表示
                        break;
                }

                isTyping = false;

                // タイピング終了時に会話音を停止
                SoundManager.instance.StopTypingSound();
            }

            // 次のアイドル時間まで再度コルーチンを開始
            if (hintCoroutine != null) // 実行中のコルーチンを停止してリセット
            {
                StopCoroutine(hintCoroutine);
                hintCoroutine = null;
            }
            hintCoroutine = StartCoroutine(ShowHintAfterDelay());
        }
    }

    // 会話を進める関数
    private void DisplayNextMessage()
    {
        
        switch (languageIndex)
        {
            case 0: // 英語
                // まだメッセージが残っている場合
                if (conversationIndex < conversationMessages_en.Length)
                {
                    // 効果音（メッセージ音）を再生
                    SoundManager.instance.PlayMessageSound();

                    // メッセージを1文字ずつ表示するコルーチンを開始
                    typingCoroutine = StartCoroutine(TypeMessage(conversationMessages_en[conversationIndex])); // コルーチンの参照を保存
                    conversationIndex++;
                }
                else
                {
                    EndConversation(); // 会話終了
                }
                break;
            case 1: // 日本語
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
                break;
        }
    }

    // 1文字ずつテキストを表示するコルーチン
    IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        conversationText.text = ""; // テキストをクリア

        // タイピング音をループ再生開始
        SoundManager.instance.PlayTypingSound(); // 新たに追加したメソッドでタイピング音を再生

        foreach (char letter in message.ToCharArray())
        {
            conversationText.text += letter; // 1文字ずつ追加
            yield return new WaitForSeconds(typingSpeed); // 指定の速度で待機
        }

        // タイピング終了時に音を停止
        SoundManager.instance.StopTypingSound();
        isTyping = false; // タイピング終了
    }

    // 会話が終了した際の処理
    private void EndConversation()
    {
        isConversing = false;
        conversationCanvas.SetActive(false); // 会話用のCanvasを非表示
        onConversationEndCallback?.Invoke(); // 会話終了時のコールバックを呼び出し
    }

    // 会話を開始するメソッド（外部からも呼べるようにする）
    public void StartConversation(System.Action callback = null)
    {
        conversationCanvas.SetActive(true); // Canvasを表示
        isConversing = true; // 会話中に設定
        onConversationEndCallback = callback; // 会話終了後の処理を設定
        conversationIndex = 0; // 会話の最初のメッセージから開始
        DisplayNextMessage(); // 最初のメッセージを表示

        // ヒント表示のコルーチンを開始
        hintCoroutine = StartCoroutine(ShowHintAfterDelay());
    }

    // 目標金額を保存する関数
    private void SaveTargetRevenue()
    {
        PlayerPrefs.SetFloat("TargetRevenue", targetRevenue); // PlayerPrefsに保存
        PlayerPrefs.Save(); // 保存
        Debug.Log("目標金額が保存されました: " + targetRevenue);
    }

    // 次のシーンをロードする関数
    public void LoadNextScene()
    {
        // 会話を省略した会話シーンであるStage916をセーブ
        if (saveStage116)
        {
            SaveManager.SaveGame(116, 0f);
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // フェードとシーン遷移をコルーチンで実行
            StartCoroutine(FadeAndLoadScene(nextSceneName));
        }
        else
        {
            Debug.LogError("シーン名が設定されていません。");
        }
    }


    //「Eを押して次へ」のヒント表示コルーチン
    private IEnumerator ShowHintAfterDelay()
    {
        // 指定時間待機
        yield return new WaitForSeconds(idleTimeBeforeHint);

        // Textを有効化し、点滅アニメーションを開始
        pressEText.gameObject.SetActive(true);
        float alpha = 0f;
        bool increasing = true;

        while (true)
        {
            // Alpha値を徐々に増減させる
            alpha += (increasing ? 0.02f : -0.02f);
            if (alpha >= 1f) increasing = false;
            else if (alpha <= 0f) increasing = true;

            // Alpha値をTextに適用
            pressEText.color = new Color(pressEText.color.r, pressEText.color.g, pressEText.color.b, alpha);

            // 次のフレームまで待機
            yield return new WaitForSeconds(0.05f);
        }
    }

    //フェード処理用のコルーチン
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
        float startAlpha = fadeCanvasGroup.alpha; // 現在のアルファ値を取得
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration); // Alphaを線形補間
            yield return null; // 次のフレームまで待機
        }

        // 最終的なAlpha値を目標値に設定（補間誤差の解消）
        fadeCanvasGroup.alpha = targetAlpha;
    }

    // 汎用的な遅延関数
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // 指定時間の遅延
        yield return new WaitForSeconds(delayTime);

        // 遅延後にコールバックとして渡されたアクションを実行
        action?.Invoke();
    }

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

    // カーソルをロックするメソッド
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    // カーソルをアンロックするメソッド
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    // カーソルのロック/アンロックを切り替えるメソッド
    public void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void LoadKeySettings()
    {
        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
    }

    private void ApplyLanguage(int index)
    {
        // dayofWeekText が null なら処理をスキップ
        if (dayofWeekText == null)
        {
            Debug.Log("dayofWeekText が未割当です。曜日表示をスキップします。");
            return;
        }

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
