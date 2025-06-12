using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

// ポーズメニュー全般を制御するクラス。
// ESCキーでポーズ、各種設定（グラフィック・サウンド）や
// タイトルへの戻り、ゲーム終了などを行うメニューを表示/非表示にする。
public class PauseMenuController : MonoBehaviour
{
    [Header("サウンド設定")]
    //public SoundManager soundManager; // SoundManager の参照をInspectorで指定

    [Header("プレイヤー動作スクリプト")]
    public FirstPersonMovement playerMovement;  // プレイヤーの動作スクリプトを指定

    [Header("ポーズメニュー関連")]
    public GameObject pauseMenuCanvas;  // ポーズメニューCanvasをInspectorで指定
    public Button resumeButton;         // ゲーム再開ボタン
    public Button graphicsSettingsButton; // グラフィック設定ボタン
    public Button soundSettingsButton;    // サウンド設定ボタン
    public Button instructionsButton; // 操作方法画面を開くボタン
    public Button titleButton;            // タイトルへ戻るボタン
    public Button quitButton;             // ゲーム終了ボタン

    [Header("Graphics設定関連")]
    public GameObject graphicsSettingsCanvas;  // グラフィック設定CanvasをInspectorで指定
    public Button closeGraphicsSettingsButton; // グラフィック設定を閉じるボタンをInspectorで指定
    public TMP_Dropdown resolutionDropdown;     // 解像度設定用ドロップダウン
    public TMP_Dropdown fullScreenDropdown;     // フルスクリーン設定用ドロップダウン
    public TMP_Dropdown qualityDropdown;        // 品質設定用ドロップダウン
    public TMP_Dropdown antiAliasingDropdown;   // アンチエイリアス設定用ドロップダウン
    public TMP_Dropdown shadowQualityDropdown;  // 陰影品質設定用ドロップダウン
    public TMP_Dropdown frameRateDropdown;      // フレームレート設定用ドロップダウン
    public TMP_Dropdown vSyncDropdown;          // VSync用ドロップダウン
    public Button graphicsSettingsDefaultButton;// グラフィック設定デフォルト復帰ボタン

    [Header("サウンド設定関連")]
    public GameObject soundSettingsCanvas;      // サウンド設定Canvas
    public Button closeSoundSettingsButton;     // サウンド設定閉じるボタン
    public Slider masterVolumeSlider;           // マスター音量用スライダー
    public Slider bgmVolumeSlider;              // BGM音量用スライダー
    public Slider sfxVolumeSlider;              // SFX音量用スライダー
    public TMP_Text masterVolumeText;           // マスター音量テキスト表示用
    public TMP_Text bgmVolumeText;              // BGM音量テキスト表示用
    public TMP_Text sfxVolumeText;              // SFX音量テキスト表示用
    public Button soundSettingsDefaultButton;   // サウンド設定デフォルト復帰ボタン

    [Header("操作方法画面関連")]
    public GameObject instructionsCanvas;        // 操作方法用Canvas
    public Button closeInstructionsButton;       // 操作方法画面を閉じるボタン
    public Slider lookSpeedSlider;               // マウス感度用スライダー
    public TMP_Text lookSpeedText;               // マウス感度を表示するテキスト

    // キー設定用UI
    public Button forwardKeyButton;
    public TMP_Text forwardKeyText;
    public Button backwardKeyButton;
    public TMP_Text backwardKeyText;
    public Button leftKeyButton;
    public TMP_Text leftKeyText;
    public Button rightKeyButton;
    public TMP_Text rightKeyText;
    public Button runKeyButton;
    public TMP_Text runKeyText;
    public Button confirmKeyButton;
    public TMP_Text confirmKeyText;
    public Button serveKeyButton;
    public TMP_Text serveKeyText;
    public Button pauseKeyButton;
    public TMP_Text pauseKeyText;
    private string waitingForKey = null; // 現在どのキー再設定を待っているか
    public Sprite keyInputWaitingSprite; // キー入力待ちの表示画像
    public Sprite keyAssignedSprite;     // キー割り当て完了時の表示画像
    public Button instructionsSettingsDefaultButton; // 操作設定のリセットボタン

    [Header("確認用Canvas")]
    public GameObject titleConfirmCanvas; // タイトルへ戻る確認CanvasをInspectorで指定
    public Button titleConfirmYesButton;  // タイトルへ戻る確認の「はい」ボタン
    public Button titleConfirmNoButton;   // タイトルへ戻る確認の「いいえ」ボタン

    public GameObject quitConfirmCanvas;  // ゲーム終了確認CanvasをInspectorで指定
    public Button quitConfirmYesButton;   // ゲーム終了確認の「はい」ボタン
    public Button quitConfirmNoButton;    // ゲーム終了確認の「いいえ」ボタン

    [Header("フェードアウト用設定")]
    public GameObject fadeCanvas;  // フェードアウト用のキャンバスをInspectorで指定
    public Image fadeImage;        // フェードアウトさせる画像をInspectorで指定

    public bool isPaused = false;        // ポーズ中かどうか
    private bool isInitializing = false;  // 設定読み込み中かどうか

    private Coroutine fadeCoroutine;  // コルーチンのインスタンスを保存する変数

    // キー設定用の変数
    private KeyCode pauseKey;

    void Start()
    {
        // 開始時はポーズ解除状態
        Time.timeScale = 1f;

        // PlayerPrefsからキー設定を取得（デフォルト値は指定された値）
        pauseKey = (KeyCode)PlayerPrefs.GetInt("PauseKey", (int)KeyCode.Escape);

        // 各種Canvasを非表示に設定（最初はポーズメニューは閉じた状態）
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);
        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(false);     // タイトル確認Canvas非表示
        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(false);       // 終了確認Canvas非表示

        // 各種ボタンにクリック時のイベントリスナーを登録
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (graphicsSettingsButton != null) graphicsSettingsButton.onClick.AddListener(OpenGraphicsSettings);
        if (soundSettingsButton != null) soundSettingsButton.onClick.AddListener(OpenSoundSettings);
        if (titleButton != null) titleButton.onClick.AddListener(ShowTitleConfirmCanvas);
        if (quitButton != null) quitButton.onClick.AddListener(ShowQuitConfirmCanvas);
        if (closeGraphicsSettingsButton != null) closeGraphicsSettingsButton.onClick.AddListener(CloseGraphicsSettings);
        if (closeSoundSettingsButton != null) closeSoundSettingsButton.onClick.AddListener(CloseSoundSettings);
        if (graphicsSettingsDefaultButton != null) graphicsSettingsDefaultButton.onClick.AddListener(ResetGraphicsSettingsToDefaults);
        if (soundSettingsDefaultButton != null) soundSettingsDefaultButton.onClick.AddListener(ResetSoundSettingsToDefaults);

        // タイトル確認Canvas用ボタン
        if (titleConfirmYesButton != null) titleConfirmYesButton.onClick.AddListener(ReturnToTitle);
        if (titleConfirmNoButton != null) titleConfirmNoButton.onClick.AddListener(CloseTitleConfirmCanvas);

        // 終了確認Canvas用ボタン
        if (quitConfirmYesButton != null) quitConfirmYesButton.onClick.AddListener(QuitGame);
        if (quitConfirmNoButton != null) quitConfirmNoButton.onClick.AddListener(CloseQuitConfirmCanvas);

        // 操作方法ボタンにリスナーを追加
        if (instructionsButton != null) instructionsButton.onClick.AddListener(OpenInstructions);
        
        // 操作方法画面の閉じるボタンにリスナーを追加
        if (closeInstructionsButton != null) closeInstructionsButton.onClick.AddListener(CloseInstructions);
        
        // 操作方法キャンバスを初期非表示に設定
        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);

        // マスター音量スライダーの初期化
        if (masterVolumeSlider != null && masterVolumeText != null)
        {
            masterVolumeSlider.wholeNumbers = true;     // 整数のみ
            masterVolumeSlider.minValue = 0;
            masterVolumeSlider.maxValue = 100;

            // PlayerPrefs から整数として読み込む
            int storedMasterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // デフォルト100
                                                                              // スライダーに反映
            masterVolumeSlider.value = storedMasterVolume;
            // テキストに反映
            masterVolumeText.text = storedMasterVolume.ToString();
            // リスナー（コールバック）を設定
            masterVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetMasterVolume((int)value);
                masterVolumeText.text = ((int)value).ToString();
            });
        }

        // BGM音量スライダーの初期化
        if (bgmVolumeSlider != null && bgmVolumeText != null)
        {
            bgmVolumeSlider.wholeNumbers = true;       // 整数のみ
            bgmVolumeSlider.minValue = 0;
            bgmVolumeSlider.maxValue = 100;

            int storedBgmVolume = PlayerPrefs.GetInt("BGMVolume", 30); // デフォルト30
            bgmVolumeSlider.value = storedBgmVolume;
            bgmVolumeText.text = storedBgmVolume.ToString();
            bgmVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetBGMVolume((int)value);
                bgmVolumeText.text = ((int)value).ToString();
            });
        }

        // 効果音音量スライダーの初期化
        if (sfxVolumeSlider != null && sfxVolumeText != null)
        {
            sfxVolumeSlider.wholeNumbers = true;       // 整数のみ
            sfxVolumeSlider.minValue = 0;
            sfxVolumeSlider.maxValue = 100;

            int storedSfxVolume = PlayerPrefs.GetInt("SFXVolume", 100); // デフォルト100
            sfxVolumeSlider.value = storedSfxVolume;
            sfxVolumeText.text = storedSfxVolume.ToString();
            sfxVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetSFXVolume((int)value);
                sfxVolumeText.text = ((int)value).ToString();
            });
        }

        // マウス感度スライダーおよびテキストの初期化
        if (lookSpeedSlider != null && lookSpeedText != null)
        {
            lookSpeedSlider.wholeNumbers = true;  // 整数スライダーを有効にする
            lookSpeedSlider.minValue = 1;
            lookSpeedSlider.maxValue = 100;

            // intで取得し、デフォルトは50
            int storedLookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);
            SetLookSpeed(storedLookSpeed);

            // スライダーとテキストに反映
            lookSpeedSlider.value = storedLookSpeed;
            lookSpeedText.text = storedLookSpeed.ToString();

            // 値変更時のコールバック
            lookSpeedSlider.onValueChanged.AddListener((value) =>
            {
                int newValue = (int)value;   // float→intにキャスト
                SetLookSpeed(newValue);
                lookSpeedText.text = newValue.ToString();
            });
        }

        // 各キー設定ボタンにリスナーを追加
        if (forwardKeyButton != null) forwardKeyButton.onClick.AddListener(() => StartKeyRebind("ForwardKey"));
        if (backwardKeyButton != null) backwardKeyButton.onClick.AddListener(() => StartKeyRebind("BackwardKey"));
        if (leftKeyButton != null) leftKeyButton.onClick.AddListener(() => StartKeyRebind("LeftKey"));
        if (rightKeyButton != null) rightKeyButton.onClick.AddListener(() => StartKeyRebind("RightKey"));
        if (runKeyButton != null) runKeyButton.onClick.AddListener(() => StartKeyRebind("RunKey"));
        if (confirmKeyButton != null) confirmKeyButton.onClick.AddListener(() => StartKeyRebind("ConfirmKey"));
        if (serveKeyButton != null) serveKeyButton.onClick.AddListener(() => StartKeyRebind("serveKey"));
        if (pauseKeyButton != null) pauseKeyButton.onClick.AddListener(() => StartKeyRebind("PauseKey"));

        // 操作設定リセットボタンにリスナーを設定
        if (instructionsSettingsDefaultButton != null) instructionsSettingsDefaultButton.onClick.AddListener(ResetInstructionsSettingsToDefaults);
        
        // 設定のロード処理開始
        isInitializing = true;
        LoadAllSettings(); // 保存済みの設定値を反映
        isInitializing = false;
    }

    void Update()
    {
        // キー入力を監視
        if (!string.IsNullOrEmpty(waitingForKey))
        {
            // どのキーが押されたかチェック
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    // PlayerPrefsにint型でキーコードを保存
                    PlayerPrefs.SetInt(waitingForKey, (int)code);
                    PlayerPrefs.Save();

                    // 表示を更新
                    UpdateKeyText(waitingForKey, code.ToString());

                    // 他スクリプトにキー設定をロードさせる
                    UpdateKeySettings();

                    SoundManager.instance.PlayClickSound(); // クリック音を再生

                    // キー割り当て完了時の画像を表示
                    //if (keyAssignedSprite != null) ShowAndFadeOutImage(keyAssignedSprite);

                    Debug.Log(waitingForKey + " が " + code.ToString() + " に割り当てられました。");
                    waitingForKey = null;
                    break;
                }
            }
        }

        // ESCキーでポーズメニューの表示/非表示を切り替える
        else if (Input.GetKeyDown(pauseKey))
        {
            // グラフィック設定が表示中の場合は閉じてポーズメニューに戻る
            if (graphicsSettingsCanvas != null && graphicsSettingsCanvas.activeSelf)
            {
                CloseGraphicsSettings();
                return;
            }

            // サウンド設定が表示中の場合は閉じてポーズメニューに戻る
            if (soundSettingsCanvas != null && soundSettingsCanvas.activeSelf)
            {
                CloseSoundSettings();
                return;
            }

            // タイトル確認画面が表示中の場合は閉じてポーズメニューに戻る
            if (titleConfirmCanvas != null && titleConfirmCanvas.activeSelf)
            {
                CloseTitleConfirmCanvas();
                return;
            }

            // 終了確認画面が表示中の場合は閉じてポーズメニューに戻る
            if (quitConfirmCanvas != null && quitConfirmCanvas.activeSelf)
            {
                CloseQuitConfirmCanvas();
                return;
            }

            // 操作方法画面が表示中の場合は閉じてポーズメニューに戻る
            if (instructionsCanvas != null && instructionsCanvas.activeSelf)
            {
                CloseInstructions();
                return;
            }

            // 通常のポーズ/再開切り替え
            if (isPaused)
            {
                // ポーズ中であれば再開
                ResumeGame();
            }
            else
            {
                // ポーズメニューを開く
                PauseGame();
            }
        }
    }

    // 画像を表示してフェードアウトさせる関数
    public void ShowAndFadeOutImage(Sprite imageSprite)
    {
        // 既存のフェードアウトコルーチンが動作している場合、停止させる
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // 画像とキャンバスを表示
        fadeImage.sprite = imageSprite;
        fadeImage.color = new Color(1, 1, 1, 1); // アルファ値をリセット
        fadeCanvas.SetActive(true);

        // フェードアウト処理を開始
        fadeCoroutine = StartCoroutine(FadeOutImage(2.0f, 0.5f)); // 2秒表示して0.5秒かけてフェードアウト
    }

    // 画像をフェードアウトさせるコルーチン
    private IEnumerator FadeOutImage(float displayTime, float fadeDuration)
    {
        // 指定された表示時間を待つ
        yield return new WaitForSeconds(displayTime);

        // フェードアウト処理
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        // フェードアウト後にキャンバスを非表示にする
        fadeCanvas.SetActive(false);
    }

    // ゲームをポーズ状態にする処理
    void PauseGame()
    {
        // 決定音再生
        SoundManager.instance.PlayDecisionSound();

        isPaused = true;     // ポーズ中フラグ

        // ポーズメニューCanvasを表示
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);

        // プレイヤーの移動・視点操作を無効化
        FirstPersonMovement playerMovement = FindObjectOfType<FirstPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
            playerMovement.DisableLook();
        }

        // 足音の再生を停止（←isPausedフラグにより自動的にFistPersonMovementで停止されるため不要となった）
        //if (SoundManager.instance != null)
        //{
        //    SoundManager.instance.StopFootsteps();
        //}

        // マウスカーソルを表示してUI操作可能に
        playerMovement.UnlockCursor();

        Time.timeScale = 0f; // 時間停止
    }

    // ゲームを再開する処理
    public void ResumeGame()
    {
        // 決定音再生
        SoundManager.instance.PlayDecisionSound();

        isPaused = false;    // ポーズフラグ解除

        // ポーズメニューを非表示
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);

        // プレイヤーの操作を有効化
        FirstPersonMovement playerMovement = FindObjectOfType<FirstPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
            playerMovement.EnableLook();
        }

        // マウスカーソル非表示でゲーム操作に復帰
        playerMovement.LockCursor();

        Time.timeScale = 1f; // 時間進行再開
    }

    // タイトルへ戻る確認Canvasを表示する処理
    void ShowTitleConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        // ポーズメニューを閉じてタイトル確認Canvasを表示
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(true);
    }

    // タイトル確認Canvasを閉じ、ポーズメニューに戻る
    void CloseTitleConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // ゲーム終了確認Canvasを表示する処理
    void ShowQuitConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        // ポーズメニューを閉じて終了確認Canvasを表示
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(true);
    }

    // 終了確認Canvasを閉じ、ポーズメニューへ戻る
    void CloseQuitConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // グラフィック設定Canvasを表示する処理
    void OpenGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(true);
    }

    // グラフィック設定Canvasを閉じ、ポーズメニューへ戻る
    void CloseGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // サウンド設定Canvasを表示する処理
    void OpenSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(true);
    }

    // サウンド設定Canvasを閉じ、ポーズメニューへ戻る
    void CloseSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // タイトルへ戻る（タイトルシーンへ移行）
    void ReturnToTitle()
    {
        SoundManager.instance.PlayDecisionSound();
        Time.timeScale = 1f; // 時間進行を元に戻す
        SceneManager.LoadScene("MenuScene"); // "MenuScene"をロード
    }

    // ゲームを終了する処理（ビルド後のみ有効）
    void QuitGame()
    {
        SoundManager.instance.PlayDecisionSound();
        Debug.Log("ゲームを終了します。");
        Application.Quit();
    }

    // 全設定をロードして反映する処理
    void LoadAllSettings()
    {
        // 各種グラフィックス設定値をPlayerPrefsから取得
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode", 0);
        int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 5);
        int antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0);
        int shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2);
        int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit", 1);
        int vSyncSetting = PlayerPrefs.GetInt("VSync", 1); // VSync設定も読み込む

        // 値を実際の設定へ反映
        SetResolution(resolutionIndex);
        SetFullScreen(fullScreenMode);
        SetQuality(graphicsQuality);
        SetAntiAliasing(antiAliasing);
        SetShadowQuality(shadowQuality);
        SetFrameRate(frameRateLimit);
        SetVSync(vSyncSetting);

        // ドロップダウンメニューの表示を更新
        UpdateGraphicsDropdownValues(resolutionIndex, fullScreenMode, graphicsQuality, antiAliasing, shadowQuality, frameRateLimit, vSyncSetting);

        // サウンド設定読み込み
        int masterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // デフォルト100
        int bgmVolume = PlayerPrefs.GetInt("BGMVolume", 30);     // デフォルト30
        int sfxVolume = PlayerPrefs.GetInt("SFXVolume", 100);    // デフォルト100

        // サウンド設定反映
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        // サウンド用スライダー表示更新
        UpdateSoundSliderValues(masterVolume, bgmVolume, sfxVolume);

        // ドロップダウンやスライダーに変更時リスナーを設定
        SetupDropdownListeners();
        SetupSoundSliderListeners();

        // キー設定のロード
        LoadKeySettings();
    }

    // グラフィック用ドロップダウンの初期値セット
    void UpdateGraphicsDropdownValues(int resolutionIndex, int fullScreenMode, int graphicsQuality, int antiAliasing, int shadowQuality, int frameRateLimit, int vSyncSetting)
    {
        // 解像度ドロップダウン
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            // 現状固定解像度
            List<string> resolutions = new List<string> { "1920 x 1080" };
            resolutionDropdown.AddOptions(resolutions);
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        // フルスクリーンドロップダウン
        if (fullScreenDropdown != null)
        {
            fullScreenDropdown.ClearOptions();
            fullScreenDropdown.AddOptions(new List<string> { "Full Screen", "Windowed" });
            fullScreenDropdown.value = fullScreenMode;
            fullScreenDropdown.RefreshShownValue();
        }

        // 品質設定ドロップダウン
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
            qualityDropdown.value = graphicsQuality;
            qualityDropdown.RefreshShownValue();
        }

        // アンチエイリアスドロップダウン
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.ClearOptions();
            antiAliasingDropdown.AddOptions(new List<string> { "Off", "2x", "4x", "8x" });
            antiAliasingDropdown.value = antiAliasing;
            antiAliasingDropdown.RefreshShownValue();
        }

        // 陰影品質ドロップダウン
        if (shadowQualityDropdown != null)
        {
            shadowQualityDropdown.ClearOptions();
            shadowQualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High" });
            shadowQualityDropdown.value = shadowQuality;
            shadowQualityDropdown.RefreshShownValue();
        }

        // フレームレートドロップダウン
        if (frameRateDropdown != null)
        {
            frameRateDropdown.ClearOptions();
            frameRateDropdown.AddOptions(new List<string> { "30 FPS", "60 FPS", "120 FPS", "Unlimited" });
            frameRateDropdown.value = frameRateLimit;
            frameRateDropdown.RefreshShownValue();
        }

        // VSyncドロップダウン初期化
        if (vSyncDropdown != null)
        {
            vSyncDropdown.ClearOptions();
            vSyncDropdown.AddOptions(new List<string> { "Off", "On" });
            vSyncDropdown.value = vSyncSetting;
            vSyncDropdown.RefreshShownValue();
        }
    }

    // サウンド用スライダーとテキストの初期表示設定
    void UpdateSoundSliderValues(int masterVol, int bgmVol, int sfxVol)
    {
        if (masterVolumeSlider != null && masterVolumeText != null)
        {
            masterVolumeSlider.value = masterVol;
            masterVolumeText.text = masterVol.ToString();
        }

        if (bgmVolumeSlider != null && bgmVolumeText != null)
        {
            bgmVolumeSlider.value = bgmVol;
            bgmVolumeText.text = bgmVol.ToString();
        }

        if (sfxVolumeSlider != null && sfxVolumeText != null)
        {
            sfxVolumeSlider.value = sfxVol;
            sfxVolumeText.text = sfxVol.ToString();
        }
    }

    // グラフィック用ドロップダウンに値変更時のリスナーを追加する
    void SetupDropdownListeners()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.AddListener((int index) => {
                SetResolution(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (fullScreenDropdown != null)
        {
            fullScreenDropdown.onValueChanged.AddListener((int index) => {
                SetFullScreen(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.AddListener((int index) => {
                SetQuality(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.onValueChanged.AddListener((int index) => {
                SetAntiAliasing(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (shadowQualityDropdown != null)
        {
            shadowQualityDropdown.onValueChanged.AddListener((int index) => {
                SetShadowQuality(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (frameRateDropdown != null)
        {
            frameRateDropdown.onValueChanged.AddListener((int index) => {
                SetFrameRate(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();
            });
        }

        if (vSyncDropdown != null)
        {
            vSyncDropdown.onValueChanged.AddListener((int index) => {
                SetVSync(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound();               
            });
        }
    }

    // サウンド用スライダーに値変更時のリスナーを追加する
    void SetupSoundSliderListeners()
    {
        if (masterVolumeSlider != null && masterVolumeText != null)
        {
            masterVolumeSlider.wholeNumbers = true;
            masterVolumeSlider.minValue = 0;
            masterVolumeSlider.maxValue = 100;

            masterVolumeSlider.onValueChanged.AddListener((float value) => {
                int newValue = (int)value;
                SetMasterVolume(newValue);
                masterVolumeText.text = newValue.ToString();
            });
        }

        if (bgmVolumeSlider != null && bgmVolumeText != null)
        {
            bgmVolumeSlider.wholeNumbers = true;
            bgmVolumeSlider.minValue = 0;
            bgmVolumeSlider.maxValue = 100;

            bgmVolumeSlider.onValueChanged.AddListener((float value) => {
                int newValue = (int)value;
                SetBGMVolume(newValue);
                bgmVolumeText.text = newValue.ToString();
            });
        }

        if (sfxVolumeSlider != null && sfxVolumeText != null)
        {
            sfxVolumeSlider.wholeNumbers = true;
            sfxVolumeSlider.minValue = 0;
            sfxVolumeSlider.maxValue = 100;

            sfxVolumeSlider.onValueChanged.AddListener((float value) => {
                int newValue = (int)value;
                SetSFXVolume(newValue);
                sfxVolumeText.text = newValue.ToString();
            });
        }
    }

    // 解像度設定処理（本コードでは固定の1920x1080）
    void SetResolution(int resolutionIndex)
    {
        // 固定で1920x1080に設定している
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    // フルスクリーン設定処理
    void SetFullScreen(int fullScreenModeIndex)
    {
        bool isFullScreen = (fullScreenModeIndex == 0);
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreenMode", fullScreenModeIndex);
    }

    // 品質設定処理
    void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    // アンチエイリアス設定処理
    void SetAntiAliasing(int index)
    {
        int[] antiAliasingValues = { 0, 2, 4, 8 };
        QualitySettings.antiAliasing = antiAliasingValues[index];
        PlayerPrefs.SetInt("AntiAliasing", index);
    }

    // シャドウ品質設定処理
    void SetShadowQuality(int index)
    {
        //0:Low(Disable),1:Medium(HardOnly),2:High(All)
        ShadowQuality[] shadowQualities = { ShadowQuality.Disable, ShadowQuality.HardOnly, ShadowQuality.All };
        QualitySettings.shadows = shadowQualities[index];
        PlayerPrefs.SetInt("ShadowQuality", index);
    }

    // フレームレート設定処理
    void SetFrameRate(int index)
    {
        // index=0:30fps,1:60fps,2:120fps,3:Unlimited(-1で制限なし)
        int[] frameRates = { 30, 60, 120, -1 };
        Application.targetFrameRate = frameRates[index];

        // Unlimited時はvSyncCountを0に、他は1(仮仕様)
        //QualitySettings.vSyncCount = (index == 3) ? 0 : 1;

        PlayerPrefs.SetInt("FrameRateLimit", index);
    }

    void SetVSync(int index)
    {
        // index == 0 -> オフ, 1 -> オン
        if (index == 0)
        {
            QualitySettings.vSyncCount = 0; // VSync Off
        }
        else
        {
            QualitySettings.vSyncCount = 1; // VSync On
        }

        PlayerPrefs.SetInt("VSync", index);
        Debug.Log($"VSync設定: {(index == 0 ? "Off" : "On")}");
    }

    // マスター音量設定処理
    void SetMasterVolume(int value)
    {
        AudioListener.volume = value / 100f;       // 実際の音量は 0.0～1.0 で扱う
        PlayerPrefs.SetInt("MasterVolume", value); // int で保存
    }

    // BGM音量設定処理
    void SetBGMVolume(int value)
    {
        if (SoundManager.instance != null && SoundManager.instance.bgmSource != null)
        {
            SoundManager.instance.bgmSource.volume = value / 100f;
        }
        PlayerPrefs.SetInt("BGMVolume", value); // int で保存
    }

    // SFX音量設定処理
    void SetSFXVolume(int value)
    {
        if (SoundManager.instance != null)
        {
            if (SoundManager.instance.sfxSource != null)
                SoundManager.instance.sfxSource.volume = value / 100f;
            if (SoundManager.instance.footstepsSource != null)
                SoundManager.instance.footstepsSource.volume = value / 100f;
            if (SoundManager.instance.typingSource != null)
                SoundManager.instance.typingSource.volume = value / 100f;
        }
        PlayerPrefs.SetInt("SFXVolume", value); // int で保存
    }

    // グラフィック設定をデフォルト値へリセット
    void ResetGraphicsSettingsToDefaults()
    {
        isInitializing = true;
        SoundManager.instance.PlayClickSound();

        // 各種設定を初期値へ
        SetResolution(0);
        SetFullScreen(0);
        SetQuality(5);
        SetAntiAliasing(0);
        SetShadowQuality(2);
        SetFrameRate(1);
        SetVSync(1);

        // PlayerPrefsにも保存
        PlayerPrefs.SetInt("ResolutionIndex", 0);
        PlayerPrefs.SetInt("FullScreenMode", 0);
        PlayerPrefs.SetInt("GraphicsQuality", 5);
        PlayerPrefs.SetInt("AntiAliasing", 0);
        PlayerPrefs.SetInt("ShadowQuality", 2);
        PlayerPrefs.SetInt("FrameRateLimit", 1);
        PlayerPrefs.SetInt("VSync", 1);

        // ドロップダウン再表示
        UpdateGraphicsDropdownValues(0, 0, 5, 0, 2, 1, 1);
        PlayerPrefs.Save();
        isInitializing = false;
    }

    // サウンド設定をデフォルト値へリセット
    void ResetSoundSettingsToDefaults()
    {
        SoundManager.instance.PlayClickSound();

        // 各種音量をデフォルトへ
        SetMasterVolume(100);
        SetBGMVolume(30);
        SetSFXVolume(100);

        // PlayerPrefsにも保存
        PlayerPrefs.SetFloat("MasterVolume", 100);
        PlayerPrefs.SetFloat("BGMVolume", 30);
        PlayerPrefs.SetFloat("SFXVolume", 100);

        // スライダー表示更新
        UpdateSoundSliderValues(100, 30, 100);
        PlayerPrefs.Save();
    }

    // 操作方法画面を表示する処理
    void OpenInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);  // PauseMenuCanvasを非表示
        if (instructionsCanvas != null) instructionsCanvas.SetActive(true);  // InstructionsCanvasCanvasを表示
    }

    // 操作方法画面を閉じる処理
    void CloseInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);  // InstructionsCanvasCanvasを非表示
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);  // PauseMenuCanvasを表示
    }

    // マウス感度を適用する関数
    void SetLookSpeed(int value)
    {
        PlayerPrefs.SetInt("LookSpeed", value); // int で保存
        Debug.Log($"マウス感度を設定: {value}");

        // FirstPersonMovement.cs のスクリプトを探す
        FirstPersonMovement firstPersonMovement = FindObjectOfType<FirstPersonMovement>();
        if (firstPersonMovement != null)
        {
            firstPersonMovement.LoadKeySettings();
        }
    }

    // キー再設定を開始するメソッド
    void StartKeyRebind(string keyName)
    {
        SoundManager.instance.PlayClickSound(); // クリック音を再生
        waitingForKey = keyName;
        Debug.Log(keyName + " の新しいキー入力待ち...");

        // キー入力待ち画像を表示
        //if (keyInputWaitingSprite != null) ShowAndFadeOutImage(keyInputWaitingSprite);

        // 該当するキー名のテキストを「……」に変更
        if (keyName == "ForwardKey" && forwardKeyText != null) forwardKeyText.text = "……";
        else if (keyName == "BackwardKey" && backwardKeyText != null) backwardKeyText.text = "……";
        else if (keyName == "LeftKey" && leftKeyText != null) leftKeyText.text = "……";
        else if (keyName == "RightKey" && rightKeyText != null) rightKeyText.text = "……";
        else if (keyName == "RunKey" && runKeyText != null) runKeyText.text = "……";
        else if (keyName == "ConfirmKey" && confirmKeyText != null) confirmKeyText.text = "……";
        else if (keyName == "serveKey" && serveKeyText != null) serveKeyText.text = "……";
        else if (keyName == "PauseKey" && pauseKeyText != null) pauseKeyText.text = "……";
    }

    // キー再設定時のテキスト更新メソッド
    void UpdateKeyText(string keyName, string newKey)
    {
        if (keyName == "ForwardKey" && forwardKeyText != null) forwardKeyText.text = newKey;
        else if (keyName == "BackwardKey" && backwardKeyText != null) backwardKeyText.text = newKey;
        else if (keyName == "LeftKey" && leftKeyText != null) leftKeyText.text = newKey;
        else if (keyName == "RightKey" && rightKeyText != null) rightKeyText.text = newKey;
        else if (keyName == "RunKey" && runKeyText != null) runKeyText.text = newKey;
        else if (keyName == "ConfirmKey" && confirmKeyText != null) confirmKeyText.text = newKey;
        else if (keyName == "serveKey" && serveKeyText != null) serveKeyText.text = newKey;
        else if (keyName == "PauseKey" && pauseKeyText != null) pauseKeyText.text = newKey;
    }

    void ResetInstructionsSettingsToDefaults()
    {
        isInitializing = true;  // クリック音の多重再生を防ぐため

        SoundManager.instance.PlayClickSound(); // クリック音を再生

        SetLookSpeed(50); // マウス感度デフォルト: 50

        // PlayerPrefsに保存
        PlayerPrefs.SetInt("ForwardKey", (int)KeyCode.W);
        PlayerPrefs.SetInt("BackwardKey", (int)KeyCode.S);
        PlayerPrefs.SetInt("LeftKey", (int)KeyCode.A);
        PlayerPrefs.SetInt("RightKey", (int)KeyCode.D);
        PlayerPrefs.SetInt("RunKey", (int)KeyCode.LeftShift);
        PlayerPrefs.SetInt("ConfirmKey", (int)KeyCode.E);
        PlayerPrefs.SetInt("serveKey", (int)KeyCode.Q);
        PlayerPrefs.SetInt("PauseKey", (int)KeyCode.Escape);

        // マウス感度もPlayerPrefsに保存
        PlayerPrefs.SetFloat("LookSpeed", 50);

        // テキストをデフォルト値に更新
        if (forwardKeyText != null) forwardKeyText.text = KeyCode.W.ToString();
        if (backwardKeyText != null) backwardKeyText.text = KeyCode.S.ToString();
        if (leftKeyText != null) leftKeyText.text = KeyCode.A.ToString();
        if (rightKeyText != null) rightKeyText.text = KeyCode.D.ToString();
        if (runKeyText != null) runKeyText.text = KeyCode.LeftShift.ToString();
        if (confirmKeyText != null) confirmKeyText.text = KeyCode.E.ToString();
        if (serveKeyText != null) serveKeyText.text = KeyCode.Q.ToString();
        if (pauseKeyText != null) pauseKeyText.text = KeyCode.Escape.ToString();

        // マウス感度のスライダーとテキストをデフォルト値に更新
        if (lookSpeedSlider != null) lookSpeedSlider.value = 50; // デフォルト値に設定
        if (lookSpeedText != null) lookSpeedText.text = "50"; // デフォルト値を表示

        PlayerPrefs.Save(); // 設定を保存

        // 他スクリプトにキー設定をロードさせる
        UpdateKeySettings();

        isInitializing = false;  // 最後にこのbool変数を元に戻す
    }

    void UpdateKeySettings()
    {
        // Hierarchy上の全ての客オブジェクトを取得
        CustomerCallIcon[] allIcons = FindObjectsOfType<CustomerCallIcon>();

        // 全てのCustomerCallIconに対してLoadKeySettingsを呼び出し
        foreach (CustomerCallIcon icon in allIcons)
        {
            icon.LoadKeySettings();
        }

        // Hierarchy上の全ての客オブジェクトを取得
        CustomerCallIcon_Endless[] allIcons_Endless = FindObjectsOfType<CustomerCallIcon_Endless>();

        // 全てのCustomerCallIcon_Endlessに対してLoadKeySettingsを呼び出し
        foreach (CustomerCallIcon_Endless icon_Endless in allIcons_Endless)
        {
            icon_Endless.LoadKeySettings();
        }

        // DrinkSelectionManager.cs のスクリプトを探す
        DrinkSelectionManager drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();
        if (drinkSelectionManager != null)
        {
            drinkSelectionManager.LoadKeySettings();
        }

        // FirstPersonMovement.cs のスクリプトを探す
        FirstPersonMovement firstPersonMovement = FindObjectOfType<FirstPersonMovement>();
        if (firstPersonMovement != null)
        {
            firstPersonMovement.LoadKeySettings();
        }
    }

    void LoadKeySettings()
    {
        KeyCode forwardKeyCode = (KeyCode)PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        KeyCode backwardKeyCode = (KeyCode)PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        KeyCode leftKeyCode = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        KeyCode rightKeyCode = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        KeyCode runKeyCode = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
        KeyCode confirmKeyCode = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        KeyCode serveKeyCode = (KeyCode)PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);
        KeyCode pauseKeyCode = (KeyCode)PlayerPrefs.GetInt("PauseKey", (int)KeyCode.Escape);

        if (forwardKeyText != null) forwardKeyText.text = forwardKeyCode.ToString();
        if (backwardKeyText != null) backwardKeyText.text = backwardKeyCode.ToString();
        if (leftKeyText != null) leftKeyText.text = leftKeyCode.ToString();
        if (rightKeyText != null) rightKeyText.text = rightKeyCode.ToString();
        if (runKeyText != null) runKeyText.text = runKeyCode.ToString();
        if (confirmKeyText != null) confirmKeyText.text = confirmKeyCode.ToString();
        if (serveKeyText != null) serveKeyText.text = serveKeyCode.ToString();
        if (pauseKeyText != null) pauseKeyText.text = pauseKeyCode.ToString();
    }
}
