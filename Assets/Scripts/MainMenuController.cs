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
    [Header("�T�E���h�ݒ�")]
    //public SoundManager soundManager; // SoundManager �̎Q�Ƃ�����

    [Header("���j���[��ʐݒ�")]
    public GameObject menuCanvas;  // ���C�����j���[Canvas��Inspector�Ŏw��
    public Button startGameButton; // �Q�[���X�^�[�g�{�^����Inspector�Ŏw��
    public Button quitGameButton;  // �Q�[���I���{�^����Inspector�Ŏw��
    public Button loadGameButton;  // �Q�[�����[�h�{�^����Inspector�Ŏw��
    public Button settingsButton;  // �V�����ݒ�{�^����Inspector�Ŏw��
    public Button languageSettingsButton; // ����ݒ�{�^����Inspector�Ŏw��
    public Button creditsButton;          // �N���W�b�g�{�^����Inspector�Ŏw��

    [Header("Settings��ʊ֘A")]
    public GameObject settingsCanvas;      // �ݒ�pCanvas��Inspector�Ŏw��
    public Button graphicsSettingsButton;  // �O���t�B�b�N�ݒ�{�^����Inspector�Ŏw��
    public Button soundSettingsButton;     // �T�E���h�ݒ�{�^����Inspector�Ŏw��
    public Button instructionsButton;            // ������@�{�^����Inspector�Ŏw��
    public Button resetGameButton;   // �ݒ��ʓ��̃��Z�b�g�{�^����Inspector�Ŏw��
    public Button closeSettingsButton;    // �ݒ��ʂ����{�^����Inspector�Ŏw��

    [Header("�O���t�B�b�N�ݒ�֘A")]
    public GameObject graphicsSettingsCanvas;  // �O���t�B�b�N�ݒ�Canvas��Inspector�Ŏw��
    public Button closeGraphicsSettingsButton; // �O���t�B�b�N�ݒ�����{�^����Inspector�Ŏw��
    public TMP_Dropdown resolutionDropdown;  // �𑜓x�ݒ�pTMP_Dropdown
    public TMP_Dropdown fullScreenDropdown;  // �t���X�N���[���ݒ�pTMP_Dropdown
    public TMP_Dropdown qualityDropdown;     // �O���t�B�b�N�i���ݒ�pTMP_Dropdown
    public TMP_Dropdown antiAliasingDropdown;  // �A���`�G�C���A�X�ݒ�pTMP_Dropdown
    public TMP_Dropdown shadowQualityDropdown; // �A�e�̕i���ݒ�pTMP_Dropdown
    public TMP_Dropdown frameRateDropdown;     // �t���[�����[�g�����pTMP_Dropdown
    public TMP_Dropdown vSyncDropdown;         // VSync�ݒ�pTMP_Dropdown
    public Button graphicsSettingsDefaultButton; // �O���t�B�b�N�ݒ�̃��Z�b�g�{�^��

    [Header("�T�E���h�ݒ�֘A")]
    public GameObject soundSettingsCanvas;     // �T�E���h�ݒ�Canvas��Inspector�Ŏw��
    public Button closeSoundSettingsButton;    // �T�E���h�ݒ�����{�^����Inspector�Ŏw��
    public Slider masterVolumeSlider;  // �}�X�^�[���ʃX���C�_�[
    public Slider bgmVolumeSlider;     // BGM���ʃX���C�_�[
    public Slider sfxVolumeSlider;     // ���ʉ����ʃX���C�_�[
    public TMP_Text masterVolumeText;  // �}�X�^�[���ʂ�\������e�L�X�g
    public TMP_Text bgmVolumeText;     // BGM���ʂ�\������e�L�X�g
    public TMP_Text sfxVolumeText;     // ���ʉ����ʂ�\������e�L�X�g
    public Button soundSettingsDefaultButton; // �O���t�B�b�N�ݒ�̃��Z�b�g�{�^��

    [Header("������@��ʊ֘A")]
    public GameObject instructionsCanvas;        // ������@�pCanvas��Inspector�Ŏw��
    public Button closeInstructionsButton;       // ������@��ʂ����{�^����Inspector�Ŏw��
    public Slider lookSpeedSlider;               // �}�E�X���x�p�X���C�_�[
    public TMP_Text lookSpeedText;               // �}�E�X���x��\������e�L�X�g
    // �L�[�ݒ�pUI
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
    private string waitingForKey = null; // ���݂ǂ̃L�[�Đݒ��҂��Ă��邩
    public Sprite keyInputWaitingSprite; // �L�[���͑҂��̕\���摜��Inspector�Ŏw��
    public Sprite keyAssignedSprite; // �L�[���蓖�Ċ������̕\���摜��Inspector�Ŏw��
    public Button instructionsSettingsDefaultButton; // ����ݒ�̃��Z�b�g�{�^����Inspector�Ŏw��

    [Header("�V�[���ݒ�")]
    public string gameSceneName;   // �ǂݍ��ރV�[������Inspector�Ŏw��

    [Header("�J�����ݒ�")]
    public Camera mainCamera;               // ���C���J������Inspector�Ŏw��
    public float cameraMoveSpeed = 1f;      // �J�����̈ړ����x
    public float cameraMoveDistance = 5f;   // �J�����̈ړ��͈́i�����j
    private Vector3 initialCameraPosition;  // �����J�����ʒu
    private bool movingRight = true;        // �J�������E�Ɉړ����Ă��邩�ǂ������Ǘ�

    [Header("���Z�b�g�����ݒ�")]
    public GameObject confirmResetCanvas;   // ���Z�b�g�m�FCanvas��Inspector�Ŏw��
    public Button confirmYesButton;         // �m�F�L�����o�X���́u�͂��v�{�^����Inspector�Ŏw��
    public Button confirmNoButton;          // �m�F�L�����o�X���́u�������v�{�^����Inspector�Ŏw��
    public Sprite resetCompleteSprite;  �@�@// �u���Z�b�g�����v�������摜��Inspector�Ŏw��
    public Sprite resetCompleteSprite_en;  �@�@// �u���Z�b�g�����v�������摜��Inspector�Ŏw��_en

    [Header("�t�F�[�h�A�E�g�p�ݒ�")]
    public GameObject fadeCanvas;  // �t�F�[�h�A�E�g�p�̃L�����o�X��Inspector�Ŏw��
    public Image fadeImage;        // �t�F�[�h�A�E�g������摜��Inspector�Ŏw��

    [Header("���[�h�ݒ�")]
    public Sprite noSaveDataSprite;  �@�@// �u�Z�[�u�f�[�^���Ȃ��v���Ƃ������摜��Inspector�Ŏw��
    public Sprite noSaveDataSprite_en;  �@�@// �u�Z�[�u�f�[�^���Ȃ��v���Ƃ������摜��Inspector�Ŏw��_en

    private Coroutine fadeCoroutine;  // �R���[�`���̃C���X�^���X��ۑ�����ϐ�
    private bool isInitializing = false; // �����������ǂ����������t���O

    private bool isCursorLocked = true; // �J�[�\�������b�N����Ă��邩�ǂ����̃t���O

    [Header("����ݒ�֘A")]
    public GameObject languageSettingsCanvas;   // ����ݒ�pCanvas��Inspector�Ŏw��
    public TMP_Dropdown languageDropdown;       // ����ݒ�p�̃h���b�v�_�E����Inspector�Ŏw��
    public Button closeLanguageSettingsButton;  // ����ݒ��ʂ����{�^����Inspector�Ŏw��
    private int languageIndex = 0; // ���݂̌���C���f�b�N�X�i0: English, 1: ���{��j
    public ApplyLanguage_MenuScene languageScript;

    [Header("�N���W�b�g�֘A")]
    public GameObject creditsCanvas;      // �N���W�b�g�pCanvas��Inspector�Ŏw��
    public Button closeCreditsButton;     // �N���W�b�g��ʂ����{�^����Inspector�Ŏw��

    void Start()
    {
        isInitializing = true; // �������J�n

        // �J�[�\����\���A����уE�B���h�E���ɗ��߂�
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // PlayerPrefs���猾��ݒ�̃C���f�b�N�X���擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g�l��0

        // �O���t�B�b�N�ݒ�̓ǂݍ���
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0); // �f�t�H���g�͍ŏ��̉𑜓x
        int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode", 0); // �f�t�H���g�̓t���X�N���[��
        int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 5); // �f�t�H���g��Ultra
        int antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0); // �f�t�H���g��Off
        int shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2); // �f�t�H���g��High
        int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit", 1); // �f�t�H���g��60 FPS
        int vSyncSetting = PlayerPrefs.GetInt("VSync", 1); // �f�t�H���g��On

        SetResolution(resolutionIndex);
        SetFullScreen(fullScreenMode);
        SetQuality(graphicsQuality);
        SetAntiAliasing(antiAliasing);
        SetShadowQuality(shadowQuality);
        SetFrameRate(frameRateLimit);
        SetVSync(vSyncSetting);

        // �h���b�v�_�E���ɔ��f
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

        // �T�E���h�ݒ�̓ǂݍ���
        int masterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // �f�t�H���g��100%
        int bgmVolume = PlayerPrefs.GetInt("BGMVolume", 30); // �f�t�H���g��50%
        int sfxVolume = PlayerPrefs.GetInt("SFXVolume", 100); // �f�t�H���g��50%

        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        // �X���C�_�[�ƃe�L�X�g�ɔ��f
        masterVolumeSlider.value = masterVolume;
        masterVolumeText.text = Mathf.RoundToInt(masterVolume).ToString();
        bgmVolumeSlider.value = bgmVolume;
        bgmVolumeText.text = Mathf.RoundToInt(bgmVolume).ToString();
        sfxVolumeSlider.value = sfxVolume;
        sfxVolumeText.text = Mathf.RoundToInt(sfxVolume).ToString();


        //SoundManager�̐ݒ肪����Ă��邩�m�F
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManager���ݒ肳��Ă��܂���B");
        //}

        //���C�����j���[��BGM���Đ�
        SoundManager.instance.PlayMenuBGM();

        // �{�^�����w�肳��Ă���΁A�Ή�����@�\�����蓖�Ă�
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }

        if (quitGameButton != null)
        {
            quitGameButton.onClick.AddListener(QuitGame);
        }

        // ���̃{�^���Ɠ��l�ɁA���[�h�{�^���Ƀ��X�i�[��ǉ�
        if (loadGameButton != null)
        {
            loadGameButton.onClick.AddListener(LoadGame);
        }

        // �J�����̏����ʒu���L�^
        if (mainCamera != null)
        {
            initialCameraPosition = mainCamera.transform.position;
        }

        // ������ԂŃZ�[�u�폜�m�FCanvas���\���ɐݒ�
        confirmResetCanvas.SetActive(false);

        // ���Z�b�g�{�^���Ƀ��X�i�[��ݒ�
        if (resetGameButton != null)
        {
            resetGameButton.onClick.AddListener(ShowResetConfirm);
        }

        // �m�F�L�����o�X�́u�͂��v�{�^���Ƀ��X�i�[��ݒ�
        if (confirmYesButton != null)
        {
            confirmYesButton.onClick.AddListener(ConfirmReset);
        }

        // �m�F�L�����o�X�́u�������v�{�^���Ƀ��X�i�[��ݒ�
        if (confirmNoButton != null)
        {
            confirmNoButton.onClick.AddListener(CancelReset);
        }

        // �m�F�L�����o�X��������\���ɐݒ�
        if (confirmResetCanvas != null)
        {
            confirmResetCanvas.SetActive(false);
        }

        // �t�F�[�h�A�E�g�\���p��Canvas���\���ɐݒ�
        fadeCanvas.SetActive(false);

        // �ݒ�{�^���Ƀ��X�i�[��ǉ�
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(ShowSettings);
        }

        // �ݒ��ʂ����{�^���Ƀ��X�i�[��ǉ�
        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.AddListener(CloseSettings);
        }


        // �ݒ�Canvas��������\���ɐݒ�
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }

        // ���j���[Canvas�������\���ɐݒ�
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }

        // �O���t�B�b�N�ݒ�{�^���Ƀ��X�i�[��ǉ�
        if (graphicsSettingsButton != null)
        {
            graphicsSettingsButton.onClick.AddListener(OpenGraphicsSettings);
        }

        // �T�E���h�ݒ�{�^���Ƀ��X�i�[��ǉ�
        if (soundSettingsButton != null)
        {
            soundSettingsButton.onClick.AddListener(OpenSoundSettings);
        }

        // �O���t�B�b�N�ݒ����{�^���Ƀ��X�i�[��ǉ�
        if (closeGraphicsSettingsButton != null)
        {
            closeGraphicsSettingsButton.onClick.AddListener(CloseGraphicsSettings);
        }

        // �T�E���h�ݒ����{�^���Ƀ��X�i�[��ǉ�
        if (closeSoundSettingsButton != null)
        {
            closeSoundSettingsButton.onClick.AddListener(CloseSoundSettings);
        }

        // �O���t�B�b�N�ݒ�Canvas��������\���ɐݒ�
        if (graphicsSettingsCanvas != null)
        {
            graphicsSettingsCanvas.SetActive(false);
        }

        // �T�E���h�ݒ�Canvas��������\���ɐݒ�
        if (soundSettingsCanvas != null)
        {
            soundSettingsCanvas.SetActive(false);
        }

        // �𑜓x���X�g��������
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> resolutions = new List<string> { "1920 x 1080" }; // 1920x1080������ɐݒ�
            resolutionDropdown.AddOptions(resolutions);

            // �f�t�H���g��1920x1080�ɐݒ�
            resolutionDropdown.value = 0; // ����Ȃ̂ŃC���f�b�N�X��0
            resolutionDropdown.RefreshShownValue();

            // �𑜓x�ύX�̃��X�i�[��ݒ�
            resolutionDropdown.onValueChanged.AddListener((int index) => {
                SetResolution(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
                }
            });
        }

        // �t���X�N���[���̑I������������
        if (fullScreenDropdown != null)
        {
            fullScreenDropdown.ClearOptions();
            List<string> fullScreenModes = new List<string> { "Full Screen", "Windowed" };
            fullScreenDropdown.AddOptions(fullScreenModes);

            // ���݂̃t���X�N���[�����[�h��Dropdown�ɔ��f
            fullScreenDropdown.value = Screen.fullScreen ? 0 : 1;
            fullScreenDropdown.RefreshShownValue();

            // �t���X�N���[���ύX�̃��X�i�[��ݒ�
            fullScreenDropdown.onValueChanged.AddListener((int index) => {
                SetFullScreen(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
                }
            });
        }

        // �O���t�B�b�N�i���ݒ�̃��X�g��������
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            List<string> qualityLevels = new List<string>(QualitySettings.names);
            qualityDropdown.AddOptions(qualityLevels);

            // ���݂̕i�����x����Dropdown�ɔ��f
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.RefreshShownValue();

            // �O���t�B�b�N�i���ύX�̃��X�i�[��ݒ�
            qualityDropdown.onValueChanged.AddListener((int index) => {
                SetQuality(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
                }
            });
        }

        // �A���`�G�C���A�X�ݒ�̏�����
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.ClearOptions();
            List<string> antiAliasingOptions = new List<string> { "Off", "2x", "4x", "8x" };
            antiAliasingDropdown.AddOptions(antiAliasingOptions);

            // ���݂̃A���`�G�C���A�X�ݒ�𔽉f
            int currentAntiAliasing = QualitySettings.antiAliasing > 0
                ? (int)Mathf.Log(QualitySettings.antiAliasing, 2)
                : 0; // 0�́uOff�v��\��

            antiAliasingDropdown.value = currentAntiAliasing;
            antiAliasingDropdown.RefreshShownValue();

            // �A���`�G�C���A�X�ύX�̃��X�i�[��ݒ�
            antiAliasingDropdown.onValueChanged.AddListener((int index) => {
                SetAntiAliasing(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
                }
            });
        }

        // �A�e�̕i���ݒ�̏�����
        if (shadowQualityDropdown != null)
        {
            shadowQualityDropdown.ClearOptions();
            List<string> shadowQualityOptions = new List<string> { "Low", "Medium", "High"};
            shadowQualityDropdown.AddOptions(shadowQualityOptions);

            // ���݂̉A�e�ݒ�𔽉f
            shadowQualityDropdown.value = (int)QualitySettings.shadows; // 0: Low, 1: Medium, 2: High, 3: Very High
            shadowQualityDropdown.RefreshShownValue();

            // �A�e�i���ύX�̃��X�i�[��ݒ�
            shadowQualityDropdown.onValueChanged.AddListener((int index) => {
                SetShadowQuality(index);
                if (!isInitializing)
                {
                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
                }
            });
        }

        // �t���[�����[�g�ݒ�̏�����
        if (frameRateDropdown != null)
        {
            frameRateDropdown.ClearOptions();

            // ��: 0��30FPS, 1��60FPS, 2��120FPS, 3��������
            List<string> frameRateOptions = new List<string> { "30 FPS", "60 FPS", "120 FPS", "������" };
            frameRateDropdown.AddOptions(frameRateOptions);

            // �t���[�����[�g�ݒ���h���b�v�_�E���ɔ��f
            frameRateDropdown.value = frameRateLimit;
            frameRateDropdown.RefreshShownValue();

            // �h���b�v�_�E���ύX���̃��X�i�[
            frameRateDropdown.onValueChanged.AddListener((int index) => {
                SetFrameRate(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
            });
        }

        // VSync�ݒ�̏�����
        if (vSyncDropdown != null)
        {
            vSyncDropdown.ClearOptions();

            // ��: 0��Off, 1��On
            List<string> vSyncOptions = new List<string> { "Off", "On" };
            vSyncDropdown.AddOptions(vSyncOptions);

            // VSync�ݒ���h���b�v�_�E���ɔ��f
            vSyncDropdown.value = vSyncSetting;
            vSyncDropdown.RefreshShownValue();

            // �h���b�v�_�E���ύX���̃��X�i�[
            vSyncDropdown.onValueChanged.AddListener((int index) => {
                SetVSync(index);
                if (!isInitializing) SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
            });
        }

        // �}�X�^�[���ʃX���C�_�[�̏�����
        if (masterVolumeSlider != null && masterVolumeText != null)
        {
            masterVolumeSlider.wholeNumbers = true;     // �����̂�
            masterVolumeSlider.minValue = 0;
            masterVolumeSlider.maxValue = 100;

            // PlayerPrefs ���琮���Ƃ��ēǂݍ���
            int storedMasterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // �f�t�H���g100
                                                                              // �X���C�_�[�ɔ��f
            masterVolumeSlider.value = storedMasterVolume;
            // �e�L�X�g�ɔ��f
            masterVolumeText.text = storedMasterVolume.ToString();
            // ���X�i�[�i�R�[���o�b�N�j��ݒ�
            masterVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetMasterVolume((int)value);
                masterVolumeText.text = ((int)value).ToString();
            });
        }

        // BGM���ʃX���C�_�[�̏�����
        if (bgmVolumeSlider != null && bgmVolumeText != null)
        {
            bgmVolumeSlider.wholeNumbers = true;       // �����̂�
            bgmVolumeSlider.minValue = 0;
            bgmVolumeSlider.maxValue = 100;

            int storedBgmVolume = PlayerPrefs.GetInt("BGMVolume", 30); // �f�t�H���g30
            bgmVolumeSlider.value = storedBgmVolume;
            bgmVolumeText.text = storedBgmVolume.ToString();
            bgmVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetBGMVolume((int)value);
                bgmVolumeText.text = ((int)value).ToString();
            });
        }

        // ���ʉ����ʃX���C�_�[�̏�����
        if (sfxVolumeSlider != null && sfxVolumeText != null)
        {
            sfxVolumeSlider.wholeNumbers = true;       // �����̂�
            sfxVolumeSlider.minValue = 0;
            sfxVolumeSlider.maxValue = 100;

            int storedSfxVolume = PlayerPrefs.GetInt("SFXVolume", 100); // �f�t�H���g100
            sfxVolumeSlider.value = storedSfxVolume;
            sfxVolumeText.text = storedSfxVolume.ToString();
            sfxVolumeSlider.onValueChanged.AddListener((value) =>
            {
                SetSFXVolume((int)value);
                sfxVolumeText.text = ((int)value).ToString();
            });
        }

        // �O���t�B�b�N�ݒ胊�Z�b�g�{�^���Ƀ��X�i�[��ǉ�
        if (graphicsSettingsDefaultButton != null)
        {
            graphicsSettingsDefaultButton.onClick.AddListener(ResetGraphicsSettingsToDefaults);
        }

        // �T�E���h�ݒ胊�Z�b�g�{�^���Ƀ��X�i�[��ǉ�
        if (soundSettingsDefaultButton != null)
        {
            soundSettingsDefaultButton.onClick.AddListener(ResetSoundSettingsToDefaults);
        }

        // ������@�{�^���Ƀ��X�i�[��ǉ�
        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(OpenInstructions);
        }

        // ������@��ʂ̕���{�^���Ƀ��X�i�[��ǉ�
        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.AddListener(CloseInstructions);
        }

        // ������@�L�����o�X��������\���ɐݒ�
        if (instructionsCanvas != null)
        {
            instructionsCanvas.SetActive(false);
        }

        // �}�E�X���x�X���C�_�[����уe�L�X�g�̏�����
        if (lookSpeedSlider != null && lookSpeedText != null)
        {
            lookSpeedSlider.wholeNumbers = true;  // �����X���C�_�[��L���ɂ���
            lookSpeedSlider.minValue = 1;
            lookSpeedSlider.maxValue = 100;

            // int�Ŏ擾���A�f�t�H���g��50
            int storedLookSpeed = PlayerPrefs.GetInt("LookSpeed", 50);
            SetLookSpeed(storedLookSpeed);

            // �X���C�_�[�ƃe�L�X�g�ɔ��f
            lookSpeedSlider.value = storedLookSpeed;
            lookSpeedText.text = storedLookSpeed.ToString();

            // �l�ύX���̃R�[���o�b�N
            lookSpeedSlider.onValueChanged.AddListener((value) =>
            {
                int newValue = (int)value;   // float��int�ɃL���X�g
                SetLookSpeed(newValue);
                lookSpeedText.text = newValue.ToString();
            });
        }

        // �e�L�[�ݒ�{�^���Ƀ��X�i�[��ǉ�
        if (forwardKeyButton != null) forwardKeyButton.onClick.AddListener(() => StartKeyRebind("ForwardKey"));
        if (backwardKeyButton != null) backwardKeyButton.onClick.AddListener(() => StartKeyRebind("BackwardKey"));
        if (leftKeyButton != null) leftKeyButton.onClick.AddListener(() => StartKeyRebind("LeftKey"));
        if (rightKeyButton != null) rightKeyButton.onClick.AddListener(() => StartKeyRebind("RightKey"));
        if (runKeyButton != null) runKeyButton.onClick.AddListener(() => StartKeyRebind("RunKey"));
        if (confirmKeyButton != null) confirmKeyButton.onClick.AddListener(() => StartKeyRebind("ConfirmKey"));
        if (serveKeyButton != null) serveKeyButton.onClick.AddListener(() => StartKeyRebind("serveKey"));
        if (pauseKeyButton != null) pauseKeyButton.onClick.AddListener(() => StartKeyRebind("PauseKey"));

        // ����ݒ胊�Z�b�g�{�^���Ƀ��X�i�[��ݒ�
        if (instructionsSettingsDefaultButton != null)
        {
            instructionsSettingsDefaultButton.onClick.AddListener(ResetInstructionsSettingsToDefaults);
        }

        // ����ݒ�{�^���Ƀ��X�i�[��ǉ��i����ݒ��ʂ��J���j
        if (languageSettingsButton != null)
        {
            languageSettingsButton.onClick.AddListener(OpenLanguageSettings);
        }

        // ����ݒ�Canvas��������\���ɐݒ�
        if (languageSettingsCanvas != null)
        {
            languageSettingsCanvas.SetActive(false);
        }

        // ����ݒ�Canvas�̕���{�^���Ƀ��X�i�[��ǉ�
        if (closeLanguageSettingsButton != null)
        {
            closeLanguageSettingsButton.onClick.AddListener(CloseLanguageSettings);
        }

        // ����h���b�v�_�E���̏�����
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            List<string> languageOptions = new List<string> { "English", "���{��" };
            languageDropdown.AddOptions(languageOptions);

            // �O��I�����������PlayerPrefs����ǂݍ��ށi�f�t�H���g��0: English�j
            int savedLanguage = PlayerPrefs.GetInt("Language", 0);

            // �h���b�v�_�E���̒l���Z�b�g
            languageDropdown.value = savedLanguage;
            languageDropdown.RefreshShownValue();

            // �h���b�v�_�E���ύX���̃��X�i�[
            languageDropdown.onValueChanged.AddListener((int index) =>
            {
                SetLanguage(index);
                SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
            });
        }

        // �N���W�b�g�{�^���Ƀ��X�i�[��ǉ�
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OpenCredits);
        }

        // �N���W�b�g��ʂ̕���{�^���Ƀ��X�i�[��ǉ�
        if (closeCreditsButton != null)
        {
            closeCreditsButton.onClick.AddListener(CloseCredits);
        }

        // �N���W�b�gCanvas��������\���ɐݒ�
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false);
        }

        // ����N�����Ɍ���ݒ��ʂ�\��
        if (!PlayerPrefs.HasKey("Language"))
        {
            // MenuCanvas���\��
            if (menuCanvas != null) menuCanvas.SetActive(false);

            // ����ݒ�Canvas��\��
            if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(true);

            Debug.Log("���ꂪ�I������Ă��Ȃ����߁A����ݒ��ʂ�\�����܂��B");
        }

        // �}�E�X�J�[�\����\��
        UnlockCursor();

        // �e��ݒ�̓ǂݍ��݁BStart�̍Ō�Ɏ��s�B
        LoadSettings();

        isInitializing = false; // �������I��
    }

    void Update()
    {
        // �J���������炩�ɍ��E�ɓ���������
        if (mainCamera != null)
        {
            // ���ԂɊ�Â���Sin�֐��Ŋ��炩�ɍ��E�Ɉړ�������
            // Mathf.PI / 2 ��ǉ����āA�T�C���֐���1�̈ʒu����n�܂�悤�ɒ���
            float offsetX = Mathf.Sin(Time.time * cameraMoveSpeed + Mathf.PI / 2) * cameraMoveDistance;

            // �J������X���W�����炩�Ɉړ�������
            mainCamera.transform.localPosition = new Vector3(initialCameraPosition.x + offsetX, mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z);
        }

        // �L�[���͂��Ď�
        if (!string.IsNullOrEmpty(waitingForKey))
        {
            // �ǂ̃L�[�������ꂽ���`�F�b�N
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    // PlayerPrefs��int�^�ŃL�[�R�[�h��ۑ�
                    PlayerPrefs.SetInt(waitingForKey, (int)code);
                    PlayerPrefs.Save();

                    // �\�����X�V
                    UpdateKeyText(waitingForKey, code.ToString());

                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�

                    // �L�[���蓖�Ċ������̉摜��\��
                    //if (keyAssignedSprite != null) ShowAndFadeOutImage(keyAssignedSprite);

                    Debug.Log(waitingForKey + " �� " + code.ToString() + " �Ɋ��蓖�Ă��܂����B");
                    waitingForKey = null;
                    break;
                }
            }
        }
    }


    // �Q�[���X�^�[�g�@�\
    public void StartGame()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            // �Q�[���V�[����ǂݍ���
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("�V�[�������ݒ肳��Ă��܂���B");
        }
    }

    // �Q�[���I���@�\
    public void QuitGame()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        Debug.Log("�Q�[�����I�����܂��B");
        Application.Quit();
    }

    // �Q�[�����[�h�@�\
    public void LoadGame()
    {
        // �Z�[�u�f�[�^�����݂��邩�m�F
        if (File.Exists(SaveManager.SaveFilePath)) // SaveFilePath��public�Ȃ̂Œ��ڃA�N�Z�X�\
        {
            // ���艹���Đ�
            SoundManager.instance.PlayDecisionSound();

            // SaveManager����Z�[�u�f�[�^�����[�h
            (int stageNumber, float loadedTotalRevenue) = SaveManager.LoadGame();

            // StageManager��totalRevenue�Ƀ��[�h�����f�[�^����
            StageManager.totalRevenue = loadedTotalRevenue;

            // ���[�h���ꂽ�X�e�[�W��ǂݍ��ށi��: Stage3�j
            string sceneName = "Stage" + stageNumber;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // �Z�[�u�f�[�^���Ȃ��ꍇ�A���s�����Đ�
            SoundManager.instance.PlayFailureSound();
            Debug.Log("�Z�[�u�f�[�^�����݂��܂���B");

            // �u�Z�[�u�f�[�^���Ȃ��v�e�L�X�g�摜��\��           
            switch (languageIndex)
            {
                case 0: // �p��
                    ShowAndFadeOutImage(noSaveDataSprite_en);
                    break;
                case 1: // ���{��
                    ShowAndFadeOutImage(noSaveDataSprite);
                    break;
            }
        }
    }


    // ���Z�b�g�m�FCanvas��\�����郁�\�b�h
    void ShowResetConfirm()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        //�ݒ�L�����o�X���\���ɂ���
        settingsCanvas.SetActive(false);

        // �m�F�L�����o�X��\���ɂ���
        confirmResetCanvas.SetActive(true);
    }

    // �u�͂��v���������Ƃ��̏���
    void ConfirmReset()
    {
        SoundManager.instance.PlaySuccessSound();  // ���������Đ�

        // �Z�[�u�f�[�^���폜
        SaveManager.ClearSave();
        // �m�F�L�����o�X���\���ɂ���
        confirmResetCanvas.SetActive(false);

        //���j���[�L�����o�X��\������
        menuCanvas.SetActive(true);

        Debug.Log("�Z�[�u�f�[�^���폜����܂����B");

        // ���Z�b�g�����̃e�L�X�g�摜��\��       
        switch (languageIndex)
        {
            case 0: // �p��
                ShowAndFadeOutImage(resetCompleteSprite_en);
                break;
            case 1: // ���{��
                ShowAndFadeOutImage(resetCompleteSprite);
                break;
        }
    }

    // �u�������v���������Ƃ��̏���
    void CancelReset()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        // �m�F�L�����o�X���Ăє�\���ɂ���
        confirmResetCanvas.SetActive(false);

        //�ݒ�L�����o�X��\������
        settingsCanvas.SetActive(true);
    }

    // �摜��\�����ăt�F�[�h�A�E�g������֐�
    public void ShowAndFadeOutImage(Sprite imageSprite)
    {
        // �����̃t�F�[�h�A�E�g�R���[�`�������삵�Ă���ꍇ�A��~������
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // �摜�ƃL�����o�X��\��
        fadeImage.sprite = imageSprite;
        fadeImage.color = new Color(1, 1, 1, 1); // �A���t�@�l�����Z�b�g
        fadeCanvas.SetActive(true);

        // �t�F�[�h�A�E�g�������J�n
        fadeCoroutine = StartCoroutine(FadeOutImage(2.0f, 0.5f)); // 2�b�\������0.5�b�����ăt�F�[�h�A�E�g
    }

    // �摜���t�F�[�h�A�E�g������R���[�`��
    private IEnumerator FadeOutImage(float displayTime, float fadeDuration)
    {
        // �w�肳�ꂽ�\�����Ԃ�҂�
        yield return new WaitForSeconds(displayTime);

        // �t�F�[�h�A�E�g����
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        // �t�F�[�h�A�E�g��ɃL�����o�X���\���ɂ���
        fadeCanvas.SetActive(false);
    }

    // �ݒ��ʂ�\�����鏈��
    void ShowSettings()
    {
        SoundManager.instance.PlayDecisionSound(); // ���艹���Đ�

        if (menuCanvas != null) menuCanvas.SetActive(false);  // MenuCanvas���\��
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvas��\��
    }

    // �ݒ��ʂ���鏈��
    void CloseSettings()
    {
        SoundManager.instance.PlayDecisionSound(); // ���艹���Đ�

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvas���\��
        if (menuCanvas != null) menuCanvas.SetActive(true);  // MenuCanvas��\��
    }

    // �O���t�B�b�N�ݒ���J������
    void OpenGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvas���\��
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(true);  // GraphicsSettingsCanvas��\��
    }

    // �O���t�B�b�N�ݒ����鏈��
    void CloseGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);  // GraphicsSettingsCanvas���\��
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvas��\��
    }

    // �T�E���h�ݒ���J������
    void OpenSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvas���\��
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(true);  // SoundSettingsCanvas��\��
    }

    // �T�E���h�ݒ����鏈��
    void CloseSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);  // SoundSettingsCanvas���\��
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvas��\��
    }

    // �𑜓x��ݒ肷��֐�
    void SetResolution(int resolutionIndex)
    {
        // �𑜓x���Œ��1920x1080�ɐݒ�
        int width = 1920;
        int height = 1080;

        // �t���X�N���[���̏�Ԃ��ێ����Ȃ���𑜓x��ݒ�
        Screen.SetResolution(width, height, Screen.fullScreen);

        // �ݒ��ۑ�
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex); // �ۑ�
        Debug.Log($"�𑜓x��ύX: {width} x {height}");
    }

    // ���݂̉𑜓x�̃C���f�b�N�X���擾����֐�
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
        return 0; // �f�t�H���g�Ƃ��čŏ��̉𑜓x��Ԃ�
    }

    // �t���X�N���[���ݒ��؂�ւ���֐�
    void SetFullScreen(int fullScreenModeIndex)
    {
        bool isFullScreen = (fullScreenModeIndex == 0); // 0: �t���X�N���[��, 1: �E�B���h�E���[�h
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreenMode", fullScreenModeIndex); // �ۑ�
        Debug.Log($"�t���X�N���[�����[�h: {isFullScreen}");
    }

    // �O���t�B�b�N�i����ݒ肷��֐�
    void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex); // �ۑ�
        Debug.Log($"�O���t�B�b�N�i����ύX: {QualitySettings.names[qualityIndex]}");
    }

    // �A���`�G�C���A�X��ݒ�
    void SetAntiAliasing(int index)
    {
        int[] antiAliasingValues = { 0, 2, 4, 8 }; // Off, 2x, 4x, 8x
        QualitySettings.antiAliasing = antiAliasingValues[index];
        PlayerPrefs.SetInt("AntiAliasing", index); // �ۑ�
        Debug.Log($"�A���`�G�C���A�X�ݒ�: {antiAliasingValues[index]}x");
    }

    // �A�e�̕i����ݒ�
    void SetShadowQuality(int index)
    {
        ShadowQuality[] shadowQualities = { ShadowQuality.Disable, ShadowQuality.HardOnly, ShadowQuality.All };
        QualitySettings.shadows = shadowQualities[index];
        PlayerPrefs.SetInt("ShadowQuality", index); // �ۑ�
        Debug.Log($"�A�e�̕i��: {shadowQualities[index]}");
    }

    // �t���[�����[�g��ݒ�
    void SetFrameRate(int index)
    {
        int[] frameRates = { 30, 60, 120, -1 }; // -1 �͖�����
        Application.targetFrameRate = frameRates[index];
        //QualitySettings.vSyncCount = (index == 3) ? 0 : 1; // �������̂Ƃ���VSync�𖳌���
        PlayerPrefs.SetInt("FrameRateLimit", index); // �ۑ�
        Debug.Log($"�t���[�����[�g����: {frameRates[index]} FPS");
    }

    // ���݂̃t���[�����[�g�ɑΉ�����C���f�b�N�X���擾
    int GetFrameRateIndex(int frameRate)
    {
        switch (frameRate)
        {
            case 30: return 0;
            case 60: return 1;
            case 120: return 2;
            default: return 3; // ������
        }
    }

    // VSync��ݒ肷��֐�
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
        PlayerPrefs.SetInt("VSync", index); // �ݒ��ۑ�
        Debug.Log($"VSync�ݒ�: {(index == 0 ? "Off" : "On")}");
    }

    // �}�X�^�[���ʂ�ݒ肷��֐�
    void SetMasterVolume(int value)
    {
        // ���ۂ̉��ʂ� 0.0�`1.0 �ň������߁A100�Ŋ���
        AudioListener.volume = value / 100f;

        // PlayerPrefs �� int �ŕۑ�
        PlayerPrefs.SetInt("MasterVolume", value);

        Debug.Log($"�}�X�^�[���ʂ�ݒ�: {value}");
    }

    // BGM���ʂ�ݒ肷��֐�
    void SetBGMVolume(int value)
    {
        if (SoundManager.instance.bgmSource != null)
        {
            SoundManager.instance.bgmSource.volume = value / 100f;
            PlayerPrefs.SetInt("BGMVolume", value);
            Debug.Log($"BGM���ʂ�ݒ�: {value}");
        }
    }

    // ���ʉ����ʂ�ݒ肷��֐�
    void SetSFXVolume(int value)
    {
        if (SoundManager.instance.sfxSource != null)
            SoundManager.instance.sfxSource.volume = value / 100f;
        if (SoundManager.instance.footstepsSource != null)
            SoundManager.instance.footstepsSource.volume = value / 100f;
        if (SoundManager.instance.typingSource != null)
            SoundManager.instance.typingSource.volume = value / 100f;

        PlayerPrefs.SetInt("SFXVolume", value);
        Debug.Log($"���ʉ����ʂ�ݒ�: {value}");
    }

    // �ۑ��ς݂̐ݒ�����[�h���鏈��
    void LoadSettings()
    {
        // �𑜓x
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            SetResolution(resolutionIndex);
            resolutionDropdown.value = resolutionIndex;
        }

        // �t���X�N���[��
        if (PlayerPrefs.HasKey("FullScreenMode"))
        {
            int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode");
            SetFullScreen(fullScreenMode);
            fullScreenDropdown.value = fullScreenMode;
        }

        // �O���t�B�b�N�i��
        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality");
            SetQuality(graphicsQuality);
            qualityDropdown.value = graphicsQuality;
        }

        // �A���`�G�C���A�X
        if (PlayerPrefs.HasKey("AntiAliasing"))
        {
            int antiAliasing = PlayerPrefs.GetInt("AntiAliasing");
            SetAntiAliasing(antiAliasing);
            antiAliasingDropdown.value = antiAliasing;
        }

        // �A�e�̕i��
        if (PlayerPrefs.HasKey("ShadowQuality"))
        {
            int shadowQuality = PlayerPrefs.GetInt("ShadowQuality");
            SetShadowQuality(shadowQuality);
            shadowQualityDropdown.value = shadowQuality;
        }

        // �t���[�����[�g
        if (PlayerPrefs.HasKey("FrameRateLimit"))
        {
            int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit");
            SetFrameRate(frameRateLimit);
            frameRateDropdown.value = frameRateLimit;
        }

        // VSync�ݒ�̓ǂݍ���
        if (PlayerPrefs.HasKey("VSync"))
        {
            int vSyncSetting = PlayerPrefs.GetInt("VSync");
            SetVSync(vSyncSetting);
            vSyncDropdown.value = vSyncSetting;
        }

        // �}�X�^�[����
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            int masterVolume = PlayerPrefs.GetInt("MasterVolume");
            SetMasterVolume(masterVolume);
            masterVolumeSlider.value = masterVolume;
            masterVolumeText.text = masterVolume.ToString();
        }

        // BGM����
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            int bgmVolume = PlayerPrefs.GetInt("BGMVolume");
            SetBGMVolume(bgmVolume);
            bgmVolumeSlider.value = bgmVolume;
            bgmVolumeText.text = bgmVolume.ToString();
        }

        // ���ʉ�����
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            int sfxVolume = PlayerPrefs.GetInt("SFXVolume");
            SetSFXVolume(sfxVolume);
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeText.text = sfxVolume.ToString();
        }

        // �}�E�X���x�̓ǂݍ���
        if (PlayerPrefs.HasKey("LookSpeed"))
        {
            int lookSpeedValue = PlayerPrefs.GetInt("LookSpeed");
            SetLookSpeed(lookSpeedValue);

            if (lookSpeedSlider != null) lookSpeedSlider.value = lookSpeedValue;
            if (lookSpeedText != null) lookSpeedText.text = lookSpeedValue.ToString();
        }

        // �L�[�ݒ�̓ǂݍ��݁i�Ȃ���΃f�t�H���g�L�[�j
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

        // �e�L�X�g�ɔ��f
        if (forwardKeyText != null) forwardKeyText.text = forwardKeyCode.ToString();
        if (backwardKeyText != null) backwardKeyText.text = backwardKeyCode.ToString();
        if (leftKeyText != null) leftKeyText.text = leftKeyCode.ToString();
        if (rightKeyText != null) rightKeyText.text = rightKeyCode.ToString();
        if (runKeyText != null) runKeyText.text = runKeyCode.ToString();
        if (confirmKeyText != null) confirmKeyText.text = confirmKeyCode.ToString();
        if (serveKeyText != null) serveKeyText.text = serveKeyCode.ToString();
        if (pauseKeyText != null) pauseKeyText.text = pauseKeyCode.ToString();

        // ����ݒ�̓ǂݍ���
        if (PlayerPrefs.HasKey("Language"))
        {
            int loadedLanguage = PlayerPrefs.GetInt("Language");
            // �h���b�v�_�E�������݂���ꍇ�A�l��K�p
            if (languageDropdown != null)
            {
                languageDropdown.value = loadedLanguage;
                languageDropdown.RefreshShownValue();
            }
            // ���ۂ̌���K�p�������Ăяo���i�������򂵂����ꍇ�Ɋ��p�j
            SetLanguage(loadedLanguage);
        }
        else
        {
            // PlayerPrefs�ɁuLanguage�v���Ȃ��ꍇ�̓f�t�H���g��0(English)���Z�b�g
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

        // 1920x1080�����������ꍇ
        if (defaultResolutionIndex != -1)
        {
            SetResolution(defaultResolutionIndex);
            resolutionDropdown.value = defaultResolutionIndex;
        }
        else
        {
            // 1920x1080��������Ȃ��ꍇ�̓��X�g�̍ŏ����g�p
            Debug.Log("1920x1080 �𑜓x��������܂���ł����B���X�g�̍ŏ��̉𑜓x��ݒ肵�܂��B");
            SetResolution(0);
            resolutionDropdown.value = 0;
        }

        resolutionDropdown.RefreshShownValue();
    }


    void ResetGraphicsSettingsToDefaults()
    {
        isInitializing = true;  //�N���b�N���̑��d�Đ���h������

        SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�

        SetResolution(0);       // �𑜓x�f�t�H���g: 1920x1080
        SetFullScreen(0);       // �t���X�N���[���f�t�H���g: �L��
        SetQuality(5);          // �O���t�B�b�N�i���f�t�H���g: Ultra
        SetAntiAliasing(0);     // �A���`�G�C���A�X�f�t�H���g: off
        SetShadowQuality(2);    // �A�e�̕i���f�t�H���g: High
        SetFrameRate(1);        // �t���[�����[�g�f�t�H���g: 60 FPS
        SetVSync(1);            // VSync�f�t�H���g: On

        // PlayerPrefs�ɕۑ�
        PlayerPrefs.SetInt("ResolutionIndex", 0);
        PlayerPrefs.SetInt("FullScreenMode", 0);
        PlayerPrefs.SetInt("GraphicsQuality", 5);
        PlayerPrefs.SetInt("AntiAliasing", 0);
        PlayerPrefs.SetInt("ShadowQuality", 2);
        PlayerPrefs.SetInt("FrameRateLimit", 1);
        PlayerPrefs.SetInt("VSync", 1);

        // �h���b�v�_�E�������Z�b�g
        resolutionDropdown.value = 0; 
        resolutionDropdown.RefreshShownValue(); 
        fullScreenDropdown.value = 0; // �t���X�N���[��
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

        PlayerPrefs.Save(); // �ݒ��ۑ�

        isInitializing = false;  //�Ō�ɂ���bool�ϐ������ɖ߂�
    }


    void ResetSoundSettingsToDefaults()
    {
        SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�

        SetMasterVolume(100);    // �}�X�^�[���ʃf�t�H���g: 100%
        SetBGMVolume(30);     // BGM���ʃf�t�H���g: 30%
        SetSFXVolume(100);     // ���ʉ����ʃf�t�H���g: 100%

        // PlayerPrefs�ɕۑ�
        PlayerPrefs.SetFloat("MasterVolume", 100);
        PlayerPrefs.SetFloat("BGMVolume", 30);
        PlayerPrefs.SetFloat("SFXVolume", 100);

        // �X���C�_�[�ƃe�L�X�g�����Z�b�g
        masterVolumeSlider.value = 100;
        masterVolumeText.text = "100"; // 100%
        bgmVolumeSlider.value = 30f;
        bgmVolumeText.text = "30";    // 30%
        sfxVolumeSlider.value = 100;
        sfxVolumeText.text = "100";    // 100%

        PlayerPrefs.Save(); // �ݒ��ۑ�
    }

    // �J�[�\�������b�N���郁�\�b�h
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    // �J�[�\�����A�����b�N���郁�\�b�h
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    // �J�[�\���̃��b�N/�A�����b�N��؂�ւ��郁�\�b�h
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

    // ������@��ʂ�\�����鏈��
    void OpenInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (settingsCanvas != null) settingsCanvas.SetActive(false);  // SettingsCanvas���\��
        if (instructionsCanvas != null) instructionsCanvas.SetActive(true);  // InstructionsCanvasCanvas��\��
    }

    // ������@��ʂ���鏈��
    void CloseInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);  // InstructionsCanvasCanvas���\��
        if (settingsCanvas != null) settingsCanvas.SetActive(true);  // SettingsCanvas��\��
    }

    // �}�E�X���x��K�p����֐�
    void SetLookSpeed(int value)
    {
        // int�ł��̂܂ܕۑ�
        PlayerPrefs.SetInt("LookSpeed", value);
        Debug.Log($"�}�E�X���x��ݒ�: {value}");
    }

    // �L�[�Đݒ���J�n���郁�\�b�h
    void StartKeyRebind(string keyName)
    {
        SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�
        waitingForKey = keyName;
        Debug.Log(keyName + " �̐V�����L�[���͑҂�...");

        // �L�[���͑҂��摜��\��
        //if (keyInputWaitingSprite != null) ShowAndFadeOutImage(keyInputWaitingSprite);

        // �Y������L�[���̃e�L�X�g���u�c�c�v�ɕύX
        if (keyName == "ForwardKey" && forwardKeyText != null) forwardKeyText.text = "�c�c";
        else if (keyName == "BackwardKey" && backwardKeyText != null) backwardKeyText.text = "�c�c";
        else if (keyName == "LeftKey" && leftKeyText != null) leftKeyText.text = "�c�c";
        else if (keyName == "RightKey" && rightKeyText != null) rightKeyText.text = "�c�c";
        else if (keyName == "RunKey" && runKeyText != null) runKeyText.text = "�c�c";
        else if (keyName == "ConfirmKey" && confirmKeyText != null) confirmKeyText.text = "�c�c";
        else if (keyName == "serveKey" && serveKeyText != null) serveKeyText.text = "�c�c";
        else if (keyName == "PauseKey" && pauseKeyText != null) pauseKeyText.text = "�c�c";
    }

    // �L�[�Đݒ莞�̃e�L�X�g�X�V���\�b�h
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
        isInitializing = true;  // �N���b�N���̑��d�Đ���h������

        SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�

        SetLookSpeed(50); // �}�E�X���x�f�t�H���g: 50

        // PlayerPrefs�ɕۑ�
        PlayerPrefs.SetInt("ForwardKey", (int)KeyCode.W);
        PlayerPrefs.SetInt("BackwardKey", (int)KeyCode.S);
        PlayerPrefs.SetInt("LeftKey", (int)KeyCode.A);
        PlayerPrefs.SetInt("RightKey", (int)KeyCode.D);
        PlayerPrefs.SetInt("RunKey", (int)KeyCode.LeftShift);
        PlayerPrefs.SetInt("ConfirmKey", (int)KeyCode.E);
        PlayerPrefs.SetInt("serveKey", (int)KeyCode.Q);
        PlayerPrefs.SetInt("PauseKey", (int)KeyCode.Escape);

        // �}�E�X���x��PlayerPrefs�ɕۑ�
        PlayerPrefs.SetInt("LookSpeed", 50);

        // �e�L�X�g���f�t�H���g�l�ɍX�V
        if (forwardKeyText != null) forwardKeyText.text = KeyCode.W.ToString();
        if (backwardKeyText != null) backwardKeyText.text = KeyCode.S.ToString();
        if (leftKeyText != null) leftKeyText.text = KeyCode.A.ToString();
        if (rightKeyText != null) rightKeyText.text = KeyCode.D.ToString();
        if (runKeyText != null) runKeyText.text = KeyCode.LeftShift.ToString();
        if (confirmKeyText != null) confirmKeyText.text = KeyCode.E.ToString();
        if (serveKeyText != null) serveKeyText.text = KeyCode.Q.ToString();
        if (pauseKeyText != null) pauseKeyText.text = KeyCode.Escape.ToString();

        // �}�E�X���x�̃X���C�_�[�ƃe�L�X�g���f�t�H���g�l�ɍX�V
        if (lookSpeedSlider != null) lookSpeedSlider.value = 50; // �f�t�H���g�l�ɐݒ�
        if (lookSpeedText != null) lookSpeedText.text = "50"; // �f�t�H���g�l��\��

        PlayerPrefs.Save(); // �ݒ��ۑ�

        isInitializing = false;  // �Ō�ɂ���bool�ϐ������ɖ߂�
    }

    // ����ݒ��ʂ��J��
    void OpenLanguageSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        // MenuCanvas���\��
        if (menuCanvas != null) menuCanvas.SetActive(false);

        // ����ݒ�Canvas��\��
        if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(true);
    }

    // ����ݒ��ʂ����
    void CloseLanguageSettings()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (languageSettingsCanvas != null) languageSettingsCanvas.SetActive(false);
        if (menuCanvas != null) menuCanvas.SetActive(true);  // ���j���[��ʂ��ĕ\��
    }

    // ���ۂ̌���ݒ菈��
    void SetLanguage(int index)
    {
        // PlayerPrefs�ɑI����������̃C���f�b�N�X��ۑ�
        PlayerPrefs.SetInt("Language", index);
        PlayerPrefs.Save();

        // languageIndex���X�V
        languageIndex = index;

        // �����ŏ������򂵂ĕK�v�ȏ������s���iUI�̃e�L�X�g�؂�ւ��Ȃǁj
        if (index == 0)
        {
            // English���I�����ꂽ�Ƃ�
            Debug.Log("Language set to: English");
            // ��j�p��p�̖|��K�p�������Ă�
            // TranslateManager.ApplyLanguage("English");
        }
        else if (index == 1)
        {
            // ���{�ꂪ�I�����ꂽ�Ƃ�
            Debug.Log("Language set to: ���{��");
            // ��j���{��p�̖|��K�p�������Ă�
            // TranslateManager.ApplyLanguage("Japanese");
        }

        // ����ݒ�̕ύX��K�p
        languageScript.ApplyLanguage(index);
    }

    // �N���W�b�g��ʂ�\�����郁�\�b�h
    void OpenCredits()
    {
        SoundManager.instance.PlayDecisionSound(); // ���艹���Đ�

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false); // ���j���[Canvas���\��
        }

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(true); // �N���W�b�gCanvas��\��
        }
    }

    // �N���W�b�g��ʂ���郁�\�b�h
    void CloseCredits()
    {
        SoundManager.instance.PlayDecisionSound(); // ���艹���Đ�

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false); // �N���W�b�gCanvas���\��
        }

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true); // ���j���[Canvas���ĕ\��
        }
    }

}
