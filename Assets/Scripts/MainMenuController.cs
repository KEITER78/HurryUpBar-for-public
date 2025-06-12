using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using TMPro;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [Header("サウンド設定")]
    //public SoundManager soundManager; // SoundManager の参照を持つ

    [Header("メニュー画面設定")]
    public GameObject menuCanvas;  // メインメニューCanvasをInspectorで指定
    public Button startGameButton; // ゲームスタートボタンをInspectorで指定
    public Button quitGameButton;  // ゲーム終了ボタンをInspectorで指定
    public Button loadGameButton;  // ゲームロードボタンをInspectorで指定
    public Button settingsButton;  // 新しい設定ボタンをInspectorで指定
    public Button languageSettingsButton; // 言語設定ボタンをInspectorで指定
    public Button creditsButton;          // クレジットボタンをInspectorで指定

    [Header("Settings画面関連")]
    public GameObject settingsCanvas;      // 設定用CanvasをInspectorで指定
    public Button graphicsSettingsButton;  // グラフィック設定ボタンをInspectorで指定
    public Button soundSettingsButton;     // サウンド設定ボタンをInspectorで指定
    public Button instructionsButton;            // 操作方法ボタンをInspectorで指定
    public Button resetGameButton;   // 設定画面内のリセットボタンをInspectorで指定
    public Button closeSettingsButton;    // 設定画面を閉じるボタンをInspectorで指定

    [Header("グラフィック設定関連")]
    public GameObject graphicsSettingsCanvas;  // グラフィック設定CanvasをInspectorで指定
    public Button closeGraphicsSettingsButton; // グラフィック設定を閉じるボタンをInspectorで指定
    public TMP_Dropdown resolutionDropdown;  // 解像度設定用TMP_Dropdown
    public TMP_Dropdown fullScreenDropdown;  // フルスクリーン設定用TMP_Dropdown
    public TMP_Dropdown qualityDropdown;     // グラフィック品質設定用TMP_Dropdown
    public TMP_Dropdown antiAliasingDropdown;  // アンチエイリアス設定用TMP_Dropdown
    public TMP_Dropdown shadowQualityDropdown; // 陰影の品質設定用TMP_Dropdown
    public TMP_Dropdown frameRateDropdown;     // フレームレート制限用TMP_Dropdown
    public TMP_Dropdown vSyncDropdown;         // VSync設定用TMP_Dropdown
    public Button graphicsSettingsDefaultButton; // グラフィック設定のリセットボタン

    [Header("サウンド設定関連")]
    public GameObject soundSettingsCanvas;     // サウンド設定CanvasをInspectorで指定
    public Button closeSoundSettingsButton;    // サウンド設定を閉じるボタンをInspectorで指定
    public Slider masterVolumeSlider;  // マスター音量スライダー
    public Slider bgmVolumeSlider;     // BGM音量スライダー
    public Slider sfxVolumeSlider;     // 効果音音量スライダー
    public TMP_Text masterVolumeText;  // マスター音量を表示するテキスト
    public TMP_Text bgmVolumeText;     // BGM音量を表示するテキスト
    public TMP_Text sfxVolumeText;     // 効果音音量を表示するテキスト
    public Button soundSettingsDefaultButton; // グラフィック設定のリセットボタン

    [Header("操作方法画面関連")]
    public GameObject instructionsCanvas;        // 操作方法用CanvasをInspectorで指定
    public Button closeInstructionsButton;       // 操作方法画面を閉じるボタンをInspectorで指定
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
    public Sprite keyInputWaitingSprite; // キー入力待ちの表示画像をInspectorで指定
    public Sprite keyAssignedSprite; // キー割り当て完了時の表示画像をInspectorで指定
    public Button instructionsSettingsDefaultButton; // 操作設定のリセットボタンをInspectorで指定

    [Header("シーン設定")]
    public string gameSceneName;   // 読み込むシーン名をInspectorで指定

    [Header("カメラ設定")]
    public Camera mainCamera;               // メインカメラをInspectorで指定
    public float cameraMoveSpeed = 1f;      // カメラの移動速度
    public float cameraMoveDistance = 5f;   // カメラの移動範囲（距離）
    private Vector3 initialCameraPosition;  // 初期カメラ位置
    private bool movingRight = true;        // カメラが右に移動しているかどうかを管理

    [Header("リセット処理設定")]
    public GameObject confirmResetCanvas;   // リセット確認CanvasをInspectorで指定
    public Button confirmYesButton;         // 確認キャンバス内の「はい」ボタンをInspectorで指定
    public Button confirmNoButton;          // 確認キャンバス内の「いいえ」ボタンをInspectorで指定
    public Sprite resetCompleteSprite;  　　// 「リセット完了」を示す画像をInspectorで指定
    public Sprite resetCompleteSprite_en;  　　// 「リセット完了」を示す画像をInspectorで指定_en

    [Header("フェードアウト用設定")]
    public GameObject fadeCanvas;  // フェードアウト用のキャンバスをInspectorで指定
    public Image fadeImage;        // フェードアウトさせる画像をInspectorで指定

    [Header("ロード設定")]
    public Sprite noSaveDataSprite;  　　// 「セーブデータがない」ことを示す画像をInspectorで指定
    public Sprite noSaveDataSprite_en;  　　// 「セーブデータがない」ことを示す画像をInspectorで指定_en

    private Coroutine fadeCoroutine;  // コルーチンのインスタンスを保存する変数
    private bool isInitializing = false; // 初期化中かどうかを示すフラグ

    private bool isCursorLocked = true; // カーソルがロックされているかどうかのフラグ

    [Header("言語設定関連")]
    public GameObject languageSettingsCanvas;   // 言語設定用CanvasをInspectorで指定
    public TMP_Dropdown languageDropdown;       // 言語設定用のドロップダウンをInspectorで指定
    public Button closeLanguageSettingsButton;  // 言語設定画面を閉じるボタンをInspectorで指定
    private int languageIndex = 0; // 現在の言語インデックス（0: English, 1: 日本語）
    public ApplyLanguage_MenuScene languageScript;

    [Header("クレジット関連")]
    public GameObject creditsCanvas;      // クレジット用CanvasをInspectorで指定
    public Button closeCreditsButton;     // クレジット画面を閉じるボタンをInspectorで指定

    void Start()
    {
        isInitializing = true; // 初期化開始

        // カーソルを表示、およびウィンドウ内に留める
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // PlayerPrefsから言語設定のインデックスを取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルト値は0

        // グラフィック設定の読み込み
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0); // デフォルトは最初の解像度
        int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode", 0); // デフォルトはフルスクリーン
        int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 5); // デフォルトはUltra
        int antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0); // デフォルトはOff
        int shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2); // デフォルトはHigh
        int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit", 1); // デフォルトは60 FPS
        int vSyncSetting = PlayerPrefs.GetInt("VSync", 1); // デフォルトはOn

        SetResolution(resolutionIndex);
        SetFullScreen(fullScreenMode);
        SetQuality(graphicsQuality);
        SetAntiAliasing(antiAliasing);
        SetShadowQuality(shadowQuality);
        SetFrameRate(frameRateLimit);
        SetVSync(vSyncSetting);

        // ドロップダウンに反映
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullScreenDropdown.value = fullScreenMode;
        fullScreenDropdown.RefreshShownValue();
        qualityDropdown.value = graphicsQuality;
        qualityDropdown.RefreshShownValue();
        antiAliasingDropdown.value = antiAliasing;
        antiAliasingDropdown.RefreshShownValue();
        shadowQualityDropdown.value = shadowQuality;
        shadowQualityDropdown.RefreshShownValue();
        frameRateDropdown.value = frameRateLimit;
        frameRateDropdown.RefreshShownValue();
        vSyncDropdown.value = vSyncSetting;
        vSyncDropdown.RefreshShownValue();

        // サウンド設定の読み込み
        int masterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // デフォルトは100%
        int bgmVolume = PlayerPrefs.GetInt("BGMVolume", 30); // デフォルトは50%
        int sfxVolume = PlayerPrefs.GetInt("SFXVolume", 100); // デフォルトは50%

        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        // スライダーとテキストに反映
        masterVolumeSlider.value = masterVolume;
        masterVolumeText.text = Mathf.RoundToInt(masterVolume).ToString();
        bgmVolumeSlider.value = bgmVolume;
        bgmVolumeText.text = Mathf.RoundToInt(bgmVolume).ToString();
        sfxVolumeSlider.value = sfxVolume;
        sfxVolumeText.text = Mathf.RoundToInt(sfxVolume).ToString();


        //SoundManagerの設定がされているか確認
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManagerが設定されていません。");
        //}

        //メインメニューのBGMを再生
        SoundManager.instance.PlayMenuBGM();

        // ボタンが指定されていれば、対応する機能を割り当てる
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }

        if (quitGameButton != null)
        {
            quitGameButton.onClick.AddListener(QuitGame);
        }

        // 他のボタンと同様に、ロードボタンにリスナーを追加
        if (loadGameButton != null)
        {
            loadGameButton.onClick.AddListener(LoadGame);
        }

        // カメラの初期位置を記録
        if (mainCamera != null)
        {
            initialCameraPosition = mainCamera.transform.position;
        }

        // 初期状態でセーブ削除確認Canvasを非表示に設定
        confirmResetCanvas.SetActive(false);

        // リセットボタンにリスナーを設定
        if (resetGameButton != null)
        {
            resetGameButton.onClick.AddListener(ShowResetConfirm);
        }

        // 確認キャンバスの「はい」ボタンにリスナーを設定
        if (confirmYesButton != null)
        {
            confirmYesButton.onClick.AddListener(ConfirmReset);
        }

        // 確認キャンバスの「いいえ」ボタンにリスナーを設定
        if (confirmNoButton != null)
        {
            confirmNoButton.onClick.AddListener(CancelReset);
        }

        // 確認キャンバスを初期非表示に設定
        if (confirmResetCanvas != null)
        {
            confirmResetCanvas.SetActive(false);
        }

        // フェードアウト表示用のCanvasを非表示に設定
        fadeCanvas.SetActive(false);

        // 設定ボタンにリスナーを追加
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(ShowSettings);
        }

        // 設定画面を閉じるボタンにリスナーを追加
        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.AddListener(CloseSettings);
        }


        // 設定Canvasを初期非表示に設定
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }

        // メニューCanvasを初期表示に設定
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }

        // グラフィック設定ボタンにリスナーを追加
        if (graphicsSettingsButton != null)
        {
            graphicsSettingsButton.onClick.AddListener(OpenGraphicsSettings);
        }

        // サウンド設定ボタンにリスナーを追加
        if (soundSettingsButton != null)
        {
            soundSettingsButton.onClick.AddListener(OpenSoundSettings);
        }

        // グラフィック設定閉じるボタンにリスナーを追加
        if (closeGraphicsSettingsButton != null)
        {
            closeGraphicsSettingsButton.onClick.AddListener(CloseGraphicsSettings);
        }

        // サウンド設定閉じるボタンにリスナーを追加
        if (closeSoundSettingsButton != null)
        {
            closeSoundSettingsButton.onClick.AddListener(CloseSoundSettings);
        }

        // グラフィック設定Canvasを初期非表示に設定
        if (graphicsSettingsCanvas != null)
        {
            graphicsSettingsCanvas.SetActive(false);
        }

        // サウンド設定Canvasを初期非表示に設定
        if (soundSettingsCanvas != null)
        {
            soundSettingsCanvas.SetActive(false);
        }

        // 解像度リストを初期化
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> resolutions = new List<string> { "1920 x 1080" }; // 1920x1080を一択に設定
            resolutionDropdown.AddOptions(resolutions);

            // デフォルトを1920x1080に設定
            resolutionDropdown.value = 0; // 一択なのでインデックスは0
            resolutionDropdown.RefreshShownValue();

            // 解像度変更のリスナーを設定
            resolutionDropdown.onValueChanged.AddListener((int index) => {
                SetResolution(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // クリック音を再生
                }
            });
        }

        // フルスクリーンの選択肢を初期化
        if (fullScreenDropdown != null)
        {
            fullScreenDropdown.ClearOptions();
            List<string> fullScreenModes = new List<string> { "Full Screen", "Windowed" };
            fullScreenDropdown.AddOptions(fullScreenModes);

            // 現在のフルスクリーンモードをDropdownに反映
            fullScreenDropdown.value = Screen.fullScreen ? 0 : 1;
            fullScreenDropdown.RefreshShownValue();

            // フルスクリーン変更のリスナーを設定
            fullScreenDropdown.onValueChanged.AddListener((int index) => {
                SetFullScreen(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // クリック音を再生
                }
            });
        }

        // グラフィック品質設定のリストを初期化
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            List<string> qualityLevels = new List<string>(QualitySettings.names);
            qualityDropdown.AddOptions(qualityLevels);

            // 現在の品質レベルをDropdownに反映
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.RefreshShownValue();

            // グラフィック品質変更のリスナーを設定
            qualityDropdown.onValueChanged.AddListener((int index) => {
                SetQuality(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // クリック音を再生
                }
            });
        }

        // アンチエイリアス設定の初期化
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.ClearOptions();
            List<string> antiAliasingOptions = new List<string> { "Off", "2x", "4x", "8x" };
            antiAliasingDropdown.AddOptions(antiAliasingOptions);

            // 現在のアンチエイリアス設定を反映
            int currentAntiAliasing = QualitySettings.antiAliasing > 0
                ? (int)Mathf.Log(QualitySettings.antiAliasing, 2)
                : 0; // 0は「Off」を表す

            antiAliasingDropdown.value = currentAntiAliasing;
            antiAliasingDropdown.RefreshShownValue();

            // アンチエイリアス変更のリスナーを設定
            antiAliasingDropdown.onValueChanged.AddListener((int index) => {
                SetAntiAliasing(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // クリック音を再生
                }
            });
        }

        // 陰影の品質設定の初期化
        if (shadowQualityDropdown != null)
        {
            shadowQualityDropdown.ClearOptions();
            List<string> shadowQualityOptions = new List<string> { "Low", "Medium", "High"};
            shadowQualityDropdown.AddOptions(shadowQualityOptions);

            // 現在の陰影設定を反映
            shadowQualityDropdown.value = (int)QualitySettings.shadows; // 0: Low, 1: Medium, 2: High, 3: Very High
            shadowQualityDropdown.RefreshShownValue();

            // 陰影品質変更のリスナーを設定
            shadowQualityDropdown.onValueChanged.AddListener((int index) => {
                SetShadowQuality(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // クリック音を再生
                }
            });
        }

        // フレームレート設定の初期化
        if (frameRateDropdown != null)
        {
            frameRateDropdown.ClearOptions();

            // 例: 0→30FPS, 1→60FPS, 2→120FPS, 3→無制限
            List<string> frameRateOptions = new List<string> { "30 FPS", "60 FPS", "120 FPS", "無制限" };
            frameRateDropdown.AddOptions(frameRateOptions);

            // フレームレート設定をドロップダウンに反映
            frameRateDropdown.value = frameRateLimit;
            frameRateDropdown.RefreshShownValue();

            // ドロップダウン変更時のリスナー
            frameRateDropdown.onValueChanged.AddListener((int index) => {
                SetFrameRate(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound(); // クリック音を再生
            });
        }

        // VSync設定の初期化
        if (vSyncDropdown != null)
        {
            vSyncDropdown.ClearOptions();

            // 例: 0→Off, 1→On
            List<string> vSyncOptions = new List<string> { "Off", "On" };
            vSyncDropdown.AddOptions(vSyncOptions);

            // VSync設定をドロップダウンに反映
            vSyncDropdown.value = vSyncSetting;
            vSyncDropdown.RefreshShownValue();

            // ドロップダウン変更時のリスナー
            vSyncDropdown.onValueChanged.AddListener((int index) => {
                SetVSync(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound(); // クリック音を再生
            });
        }

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

        // グラフィック設定リセットボタンにリスナーを追加
        if (graphicsSettingsDefaultButton != null)
        {
            graphicsSettingsDefaultButton.onClick.AddListener(ResetGraphicsSettingsToDefaults);
        }

        // サウンド設定リセットボタンにリスナーを追加
        if (soundSettingsDefaultButton != null)
        {
            soundSettingsDefaultButton.onClick.AddListener(ResetSoundSettingsToDefaults);
        }

        // 操作方法ボタンにリスナーを追加
        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(OpenInstructions);
        }

        // 操作方法画面の閉じるボタンにリスナーを追加
        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.AddListener(CloseInstructions);
        }

        // 操作方法キャンバスを初期非表示に設定
        if (instructionsCanvas != null)
        {
            instructionsCanvas.SetActive(false);
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
        if (instructionsSettingsDefaultButton != null)
        {
            instructionsSettingsDefaultButton.onClick.AddListener(ResetInstructionsSettingsToDefaults);
        }

        // 言語設定ボタンにリスナーを追加（言語設定画面を開く）
        if (languageSettingsButton != null)
        {
            languageSettingsButton.onClick.AddListener(OpenLanguageSettings);
        }

        // 言語設定Canvasを初期非表示に設定
        if (languageSettingsCanvas != null)
        {
            languageSettingsCanvas.SetActive(false);
        }

        // 言語設定Canvasの閉じるボタンにリスナーを追加
        if (closeLanguageSettingsButton != null)
        {
            closeLanguageSettingsButton.onClick.AddListener(CloseLanguageSettings);
        }

        // 言語ドロップダウンの初期化
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            List<string> languageOptions = new List<string> { "English", "日本語" };
            languageDropdown.AddOptions(languageOptions);

            // 前回選択した言語をPlayerPrefsから読み込む（デフォルトは0: English）
            int savedLanguage = PlayerPrefs.GetInt("Language", 0);

            // ドロップダウンの値をセット
            languageDropdown.value = savedLanguage;
            languageDropdown.RefreshShownValue();

            // ドロップダウン変更時のリスナー
            languageDropdown.onValueChanged.AddListener((int index) =>
            {
                SetLanguage(index);
                SoundManager.instance.PlayClickSound(); // クリック音を再生
            });
        }

        // クレジットボタンにリスナーを追加
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OpenCredits);
        }

        // クレジット画面の閉じるボタンにリスナーを追加
        if (closeCreditsButton != null)
        {
            closeCreditsButton.onClick.AddListener(CloseCredits);
        }

        // クレジットCanvasを初期非表示に設定
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false);
        }

        // 初回起動時に言語設定画面を表示
        if (!PlayerPrefs.HasKey("Language"))
        {
            // MenuCanvasを非表示
            if (menuCanvas != null) menuCanvas.SetActive(false);

            // 言語設定Canvasを表示
            if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(true);

            Debug.Log("言語が選択されていないため、言語設定画面を表示します。");
        }

        // マウスカーソルを表示
        UnlockCursor();

        // 各種設定の読み込み。Startの最後に実行。
        LoadSettings();

        isInitializing = false; // 初期化終了
    }

    void Update()
    {
        // カメラを滑らかに左右に動かす処理
        if (mainCamera != null)
        {
            // 時間に基づいたSin関数で滑らかに左右に移動させる
            // Mathf.PI / 2 を追加して、サイン関数が1の位置から始まるように調整
            float offsetX = Mathf.Sin(Time.time * cameraMoveSpeed + Mathf.PI / 2) * cameraMoveDistance;

            // カメラのX座標を滑らかに移動させる
            mainCamera.transform.localPosition = new Vector3(initialCameraPosition.x + offsetX, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);
        }

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

                    SoundManager.instance.PlayClickSound(); // クリック音を再生

                    // キー割り当て完了時の画像を表示
                    //if (keyAssignedSprite != null) ShowAndFadeOutImage(keyAssignedSprite);

                    Debug.Log(waitingForKey + " が " + code.ToString() + " に割り当てられました。");
                    waitingForKey = null;
                    break;
                }
            }
        }
    }


    // ゲームスタート機能
    public void StartGame()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            // ゲームシーンを読み込む
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("シーン名が設定されていません。");
        }
    }

    // ゲーム終了機能
    public void QuitGame()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        Debug.Log("ゲームを終了します。");
        Application.Quit();
    }

    // ゲームロード機能
    public void LoadGame()
    {
        // セーブデータが存在するか確認
        if (File.Exists(SaveManager.SaveFilePath)) // SaveFilePathがpublicなので直接アクセス可能
        {
            // 決定音を再生
            SoundManager.instance.PlayDecisionSound();

            // SaveManagerからセーブデータをロード
            (int stageNumber, float loadedTotalRevenue) = SaveManager.LoadGame();

            // StageManagerのtotalRevenueにロードしたデータを代入
            StageManager.totalRevenue = loadedTotalRevenue;

            // ロードされたステージを読み込む（例: Stage3）
            string sceneName = "Stage" + stageNumber;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // セーブデータがない場合、失敗音を再生
            SoundManager.instance.PlayFailureSound();
            Debug.Log("セーブデータが存在しません。");

            // 「セーブデータがない」テキスト画像を表示           
            switch (languageIndex)
            {
                case 0: // 英語
                    ShowAndFadeOutImage(noSaveDataSprite_en);
                    break;
                case 1: // 日本語
                    ShowAndFadeOutImage(noSaveDataSprite);
                    break;
            }
        }
    }


    // リセット確認Canvasを表示するメソッド
    void ShowResetConfirm()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        //設定キャンバスを非表示にする
        settingsCanvas.SetActive(false);

        // 確認キャンバスを表示にする
        confirmResetCanvas.SetActive(true);
    }

    // 「はい」を押したときの処理
    void ConfirmReset()
    {
        SoundManager.instance.PlaySuccessSound();  // 成功音を再生

        // セーブデータを削除
        SaveManager.ClearSave();
        // 確認キャンバスを非表示にする
        confirmResetCanvas.SetActive(false);

        //メニューキャンバスを表示する
        menuCanvas.SetActive(true);

        Debug.Log("セーブデータが削除されました。");

        // リセット完了のテキスト画像を表示       
        switch (languageIndex)
        {
            case 0: // 英語
                ShowAndFadeOutImage(resetCompleteSprite_en);
                break;
            case 1: // 日本語
                ShowAndFadeOutImage(resetCompleteSprite);
                break;
        }
    }

    // 「いいえ」を押したときの処理
    void CancelReset()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        // 確認キャンバスを再び非表示にする
        confirmResetCanvas.SetActive(false);

        //設定キャンバスを表示する
        settingsCanvas.SetActive(true);
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

    // 設定画面を表示する処理
    void ShowSettings()
    {
        SoundManager.instance.PlayDecisionSound(); // 決定音を再生

        if (menuCanvas != null) menuCanvas.SetActive(false);  // MenuCanvasを非表示
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvasを表示
    }

    // 設定画面を閉じる処理
    void CloseSettings()
    {
        SoundManager.instance.PlayDecisionSound(); // 決定音を再生

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvasを非表示
        if (menuCanvas != null) menuCanvas.SetActive(true);  // MenuCanvasを表示
    }

    // グラフィック設定を開く処理
    void OpenGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvasを非表示
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(true);  // GraphicsSettingsCanvasを表示
    }

    // グラフィック設定を閉じる処理
    void CloseGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);  // GraphicsSettingsCanvasを非表示
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvasを表示
    }

    // サウンド設定を開く処理
    void OpenSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvasを非表示
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(true);  // SoundSettingsCanvasを表示
    }

    // サウンド設定を閉じる処理
    void CloseSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);  // SoundSettingsCanvasを非表示
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvasを表示
    }

    // 解像度を設定する関数
    void SetResolution(int resolutionIndex)
    {
        // 解像度を固定で1920x1080に設定
        int width = 1920;
        int height = 1080;

        // フルスクリーンの状態を維持しながら解像度を設定
        Screen.SetResolution(width, height, Screen.fullScreen);

        // 設定を保存
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex); // 保存
        Debug.Log($"解像度を変更: {width} x {height}");
    }

    // 現在の解像度のインデックスを取得する関数
    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.currentResolution.width == Screen.resolutions[i].width &&
                Screen.currentResolution.height == Screen.resolutions[i].height)
            {
                return i;
            }
        }
        return 0; // デフォルトとして最初の解像度を返す
    }

    // フルスクリーン設定を切り替える関数
    void SetFullScreen(int fullScreenModeIndex)
    {
        bool isFullScreen = (fullScreenModeIndex == 0); // 0: フルスクリーン, 1: ウィンドウモード
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreenMode", fullScreenModeIndex); // 保存
        Debug.Log($"フルスクリーンモード: {isFullScreen}");
    }

    // グラフィック品質を設定する関数
    void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex); // 保存
        Debug.Log($"グラフィック品質を変更: {QualitySettings.names[qualityIndex]}");
    }

    // アンチエイリアスを設定
    void SetAntiAliasing(int index)
    {
        int[] antiAliasingValues = { 0, 2, 4, 8 }; // Off, 2x, 4x, 8x
        QualitySettings.antiAliasing = antiAliasingValues[index];
        PlayerPrefs.SetInt("AntiAliasing", index); // 保存
        Debug.Log($"アンチエイリアス設定: {antiAliasingValues[index]}x");
    }

    // 陰影の品質を設定
    void SetShadowQuality(int index)
    {
        ShadowQuality[] shadowQualities = { ShadowQuality.Disable, ShadowQuality.HardOnly, ShadowQuality.All };
        QualitySettings.shadows = shadowQualities[index];
        PlayerPrefs.SetInt("ShadowQuality", index); // 保存
        Debug.Log($"陰影の品質: {shadowQualities[index]}");
    }

    // フレームレートを設定
    void SetFrameRate(int index)
    {
        int[] frameRates = { 30, 60, 120, -1 }; // -1 は無制限
        Application.targetFrameRate = frameRates[index];
        //QualitySettings.vSyncCount = (index == 3) ? 0 : 1; // 無制限のときはVSyncを無効化
        PlayerPrefs.SetInt("FrameRateLimit", index); // 保存
        Debug.Log($"フレームレート制限: {frameRates[index]} FPS");
    }

    // 現在のフレームレートに対応するインデックスを取得
    int GetFrameRateIndex(int frameRate)
    {
        switch (frameRate)
        {
            case 30: return 0;
            case 60: return 1;
            case 120: return 2;
            default: return 3; // 無制限
        }
    }

    // VSyncを設定する関数
    void SetVSync(int index)
    {
        if (index == 0)
        {
            QualitySettings.vSyncCount = 0; // Off
        }
        else
        {
            QualitySettings.vSyncCount = 1; // On
        }
        PlayerPrefs.SetInt("VSync", index); // 設定を保存
        Debug.Log($"VSync設定: {(index == 0 ? "Off" : "On")}");
    }

    // マスター音量を設定する関数
    void SetMasterVolume(int value)
    {
        // 実際の音量は 0.0～1.0 で扱うため、100で割る
        AudioListener.volume = value / 100f;

        // PlayerPrefs も int で保存
        PlayerPrefs.SetInt("MasterVolume", value);

        Debug.Log($"マスター音量を設定: {value}");
    }

    // BGM音量を設定する関数
    void SetBGMVolume(int value)
    {
        if (SoundManager.instance.bgmSource != null)
        {
            SoundManager.instance.bgmSource.volume = value / 100f;
            PlayerPrefs.SetInt("BGMVolume", value);
            Debug.Log($"BGM音量を設定: {value}");
        }
    }

    // 効果音音量を設定する関数
    void SetSFXVolume(int value)
    {
        if (SoundManager.instance.sfxSource != null)
            SoundManager.instance.sfxSource.volume = value / 100f;
        if (SoundManager.instance.footstepsSource != null)
            SoundManager.instance.footstepsSource.volume = value / 100f;
        if (SoundManager.instance.typingSource != null)
            SoundManager.instance.typingSource.volume = value / 100f;

        PlayerPrefs.SetInt("SFXVolume", value);
        Debug.Log($"効果音音量を設定: {value}");
    }

    // 保存済みの設定をロードする処理
    void LoadSettings()
    {
        // 解像度
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            SetResolution(resolutionIndex);
            resolutionDropdown.value = resolutionIndex;
        }

        // フルスクリーン
        if (PlayerPrefs.HasKey("FullScreenMode"))
        {
            int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode");
            SetFullScreen(fullScreenMode);
            fullScreenDropdown.value = fullScreenMode;
        }

        // グラフィック品質
        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality");
            SetQuality(graphicsQuality);
            qualityDropdown.value = graphicsQuality;
        }

        // アンチエイリアス
        if (PlayerPrefs.HasKey("AntiAliasing"))
        {
            int antiAliasing = PlayerPrefs.GetInt("AntiAliasing");
            SetAntiAliasing(antiAliasing);
            antiAliasingDropdown.value = antiAliasing;
        }

        // 陰影の品質
        if (PlayerPrefs.HasKey("ShadowQuality"))
        {
            int shadowQuality = PlayerPrefs.GetInt("ShadowQuality");
            SetShadowQuality(shadowQuality);
            shadowQualityDropdown.value = shadowQuality;
        }

        // フレームレート
        if (PlayerPrefs.HasKey("FrameRateLimit"))
        {
            int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit");
            SetFrameRate(frameRateLimit);
            frameRateDropdown.value = frameRateLimit;
        }

        // VSync設定の読み込み
        if (PlayerPrefs.HasKey("VSync"))
        {
            int vSyncSetting = PlayerPrefs.GetInt("VSync");
            SetVSync(vSyncSetting);
            vSyncDropdown.value = vSyncSetting;
        }

        // マスター音量
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            int masterVolume = PlayerPrefs.GetInt("MasterVolume");
            SetMasterVolume(masterVolume);
            masterVolumeSlider.value = masterVolume;
            masterVolumeText.text = masterVolume.ToString();
        }

        // BGM音量
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            int bgmVolume = PlayerPrefs.GetInt("BGMVolume");
            SetBGMVolume(bgmVolume);
            bgmVolumeSlider.value = bgmVolume;
            bgmVolumeText.text = bgmVolume.ToString();
        }

        // 効果音音量
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            int sfxVolume = PlayerPrefs.GetInt("SFXVolume");
            SetSFXVolume(sfxVolume);
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeText.text = sfxVolume.ToString();
        }

        // マウス感度の読み込み
        if (PlayerPrefs.HasKey("LookSpeed"))
        {
            int lookSpeedValue = PlayerPrefs.GetInt("LookSpeed");
            SetLookSpeed(lookSpeedValue);

            if (lookSpeedSlider != null) lookSpeedSlider.value = lookSpeedValue;
            if (lookSpeedText != null) lookSpeedText.text = lookSpeedValue.ToString();
        }

        // キー設定の読み込み（なければデフォルトキー）
        int forwardKeyInt = PlayerPrefs.GetInt("ForwardKey", (int)KeyCode.W);
        int backwardKeyInt = PlayerPrefs.GetInt("BackwardKey", (int)KeyCode.S);
        int leftKeyInt = PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        int rightKeyInt = PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        int runKeyInt = PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
        int confirmKeyInt = PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        int serveKeyInt = PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);
        int pauseKeyInt = PlayerPrefs.GetInt("PauseKey", (int)KeyCode.Escape);

        KeyCode forwardKeyCode = (KeyCode)forwardKeyInt;
        KeyCode backwardKeyCode = (KeyCode)backwardKeyInt;
        KeyCode leftKeyCode = (KeyCode)leftKeyInt;
        KeyCode rightKeyCode = (KeyCode)rightKeyInt;
        KeyCode runKeyCode = (KeyCode)runKeyInt;
        KeyCode confirmKeyCode = (KeyCode)confirmKeyInt;
        KeyCode serveKeyCode = (KeyCode)serveKeyInt;
        KeyCode pauseKeyCode = (KeyCode)pauseKeyInt;

        // テキストに反映
        if (forwardKeyText != null) forwardKeyText.text = forwardKeyCode.ToString();
        if (backwardKeyText != null) backwardKeyText.text = backwardKeyCode.ToString();
        if (leftKeyText != null) leftKeyText.text = leftKeyCode.ToString();
        if (rightKeyText != null) rightKeyText.text = rightKeyCode.ToString();
        if (runKeyText != null) runKeyText.text = runKeyCode.ToString();
        if (confirmKeyText != null) confirmKeyText.text = confirmKeyCode.ToString();
        if (serveKeyText != null) serveKeyText.text = serveKeyCode.ToString();
        if (pauseKeyText != null) pauseKeyText.text = pauseKeyCode.ToString();

        // 言語設定の読み込み
        if (PlayerPrefs.HasKey("Language"))
        {
            int loadedLanguage = PlayerPrefs.GetInt("Language");
            // ドロップダウンが存在する場合、値を適用
            if (languageDropdown != null)
            {
                languageDropdown.value = loadedLanguage;
                languageDropdown.RefreshShownValue();
            }
            // 実際の言語適用処理を呼び出し（条件分岐したい場合に活用）
            SetLanguage(loadedLanguage);
        }
        else
        {
            // PlayerPrefsに「Language」がない場合はデフォルトで0(English)をセット
            SetLanguage(0);
        }
    }

    void SetDefaultResolution()
    {
        int defaultResolutionIndex = -1;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == 1920 && Screen.resolutions[i].height == 1080)
            {
                defaultResolutionIndex = i;
                break;
            }
        }

        // 1920x1080が見つかった場合
        if (defaultResolutionIndex != -1)
        {
            SetResolution(defaultResolutionIndex);
            resolutionDropdown.value = defaultResolutionIndex;
        }
        else
        {
            // 1920x1080が見つからない場合はリストの最初を使用
            Debug.Log("1920x1080 解像度が見つかりませんでした。リストの最初の解像度を設定します。");
            SetResolution(0);
            resolutionDropdown.value = 0;
        }

        resolutionDropdown.RefreshShownValue();
    }


    void ResetGraphicsSettingsToDefaults()
    {
        isInitializing = true;  //クリック音の多重再生を防ぐため

        SoundManager.instance.PlayClickSound(); // クリック音を再生

        SetResolution(0);       // 解像度デフォルト: 1920x1080
        SetFullScreen(0);       // フルスクリーンデフォルト: 有効
        SetQuality(5);          // グラフィック品質デフォルト: Ultra
        SetAntiAliasing(0);     // アンチエイリアスデフォルト: off
        SetShadowQuality(2);    // 陰影の品質デフォルト: High
        SetFrameRate(1);        // フレームレートデフォルト: 60 FPS
        SetVSync(1);            // VSyncデフォルト: On

        // PlayerPrefsに保存
        PlayerPrefs.SetInt("ResolutionIndex", 0);
        PlayerPrefs.SetInt("FullScreenMode", 0);
        PlayerPrefs.SetInt("GraphicsQuality", 5);
        PlayerPrefs.SetInt("AntiAliasing", 0);
        PlayerPrefs.SetInt("ShadowQuality", 2);
        PlayerPrefs.SetInt("FrameRateLimit", 1);
        PlayerPrefs.SetInt("VSync", 1);

        // ドロップダウンをリセット
        resolutionDropdown.value = 0; 
        resolutionDropdown.RefreshShownValue(); 
        fullScreenDropdown.value = 0; // フルスクリーン
        fullScreenDropdown.RefreshShownValue();
        qualityDropdown.value = 5;    // Ultra
        qualityDropdown.RefreshShownValue();
        antiAliasingDropdown.value = 0; // off
        antiAliasingDropdown.RefreshShownValue();
        shadowQualityDropdown.value = 2; // High
        shadowQualityDropdown.RefreshShownValue();
        frameRateDropdown.value = 1;    // 60 FPS
        frameRateDropdown.RefreshShownValue();
        vSyncDropdown.value = 1;
        vSyncDropdown.RefreshShownValue();

        PlayerPrefs.Save(); // 設定を保存

        isInitializing = false;  //最後にこのbool変数を元に戻す
    }


    void ResetSoundSettingsToDefaults()
    {
        SoundManager.instance.PlayClickSound(); // クリック音を再生

        SetMasterVolume(100);    // マスター音量デフォルト: 100%
        SetBGMVolume(30);     // BGM音量デフォルト: 30%
        SetSFXVolume(100);     // 効果音音量デフォルト: 100%

        // PlayerPrefsに保存
        PlayerPrefs.SetFloat("MasterVolume", 100);
        PlayerPrefs.SetFloat("BGMVolume", 30);
        PlayerPrefs.SetFloat("SFXVolume", 100);

        // スライダーとテキストをリセット
        masterVolumeSlider.value = 100;
        masterVolumeText.text = "100"; // 100%
        bgmVolumeSlider.value = 30f;
        bgmVolumeText.text = "30";    // 30%
        sfxVolumeSlider.value = 100;
        sfxVolumeText.text = "100";    // 100%

        PlayerPrefs.Save(); // 設定を保存
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
        Cursor.lockState = CursorLockMode.None;
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

    // 操作方法画面を表示する処理
    void OpenInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvasを非表示
        if (instructionsCanvas != null) instructionsCanvas.SetActive(true);  // InstructionsCanvasCanvasを表示
    }

    // 操作方法画面を閉じる処理
    void CloseInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);  // InstructionsCanvasCanvasを非表示
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvasを表示
    }

    // マウス感度を適用する関数
    void SetLookSpeed(int value)
    {
        // intでそのまま保存
        PlayerPrefs.SetInt("LookSpeed", value);
        Debug.Log($"マウス感度を設定: {value}");
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
        PlayerPrefs.SetInt("LookSpeed", 50);

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

        isInitializing = false;  // 最後にこのbool変数を元に戻す
    }

    // 言語設定画面を開く
    void OpenLanguageSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        // MenuCanvasを非表示
        if (menuCanvas != null) menuCanvas.SetActive(false);

        // 言語設定Canvasを表示
        if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(true);
    }

    // 言語設定画面を閉じる
    void CloseLanguageSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // 決定音を再生

        if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(false);
        if (menuCanvas != null) menuCanvas.SetActive(true);  // メニュー画面を再表示
    }

    // 実際の言語設定処理
    void SetLanguage(int index)
    {
        // PlayerPrefsに選択した言語のインデックスを保存
        PlayerPrefs.SetInt("Language", index);
        PlayerPrefs.Save();

        // languageIndexを更新
        languageIndex = index;

        // ここで条件分岐して必要な処理を行う（UIのテキスト切り替えなど）
        if (index == 0)
        {
            // Englishが選択されたとき
            Debug.Log("Language set to: English");
            // 例）英語用の翻訳適用処理を呼ぶ
            // TranslateManager.ApplyLanguage("English");
        }
        else if (index == 1)
        {
            // 日本語が選択されたとき
            Debug.Log("Language set to: 日本語");
            // 例）日本語用の翻訳適用処理を呼ぶ
            // TranslateManager.ApplyLanguage("Japanese");
        }

        // 言語設定の変更を適用
        languageScript.ApplyLanguage(index);
    }

    // クレジット画面を表示するメソッド
    void OpenCredits()
    {
        SoundManager.instance.PlayDecisionSound(); // 決定音を再生

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false); // メニューCanvasを非表示
        }

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(true); // クレジットCanvasを表示
        }
    }

    // クレジット画面を閉じるメソッド
    void CloseCredits()
    {
        SoundManager.instance.PlayDecisionSound(); // 決定音を再生

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false); // クレジットCanvasを非表示
        }

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true); // メニューCanvasを再表示
        }
    }

}
