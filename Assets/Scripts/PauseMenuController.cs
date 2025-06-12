using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

// �|�[�Y���j���[�S�ʂ𐧌䂷��N���X�B
// ESC�L�[�Ń|�[�Y�A�e��ݒ�i�O���t�B�b�N�E�T�E���h�j��
// �^�C�g���ւ̖߂�A�Q�[���I���Ȃǂ��s�����j���[��\��/��\���ɂ���B
public class PauseMenuController : MonoBehaviour
{
    [Header("�T�E���h�ݒ�")]
    //public SoundManager soundManager; // SoundManager �̎Q�Ƃ�Inspector�Ŏw��

    [Header("�v���C���[����X�N���v�g")]
    public FirstPersonMovement playerMovement;  // �v���C���[�̓���X�N���v�g���w��

    [Header("�|�[�Y���j���[�֘A")]
    public GameObject pauseMenuCanvas;  // �|�[�Y���j���[Canvas��Inspector�Ŏw��
    public Button resumeButton;         // �Q�[���ĊJ�{�^��
    public Button graphicsSettingsButton; // �O���t�B�b�N�ݒ�{�^��
    public Button soundSettingsButton;    // �T�E���h�ݒ�{�^��
    public Button instructionsButton; // ������@��ʂ��J���{�^��
    public Button titleButton;            // �^�C�g���֖߂�{�^��
    public Button quitButton;             // �Q�[���I���{�^��

    [Header("Graphics�ݒ�֘A")]
    public GameObject graphicsSettingsCanvas;  // �O���t�B�b�N�ݒ�Canvas��Inspector�Ŏw��
    public Button closeGraphicsSettingsButton; // �O���t�B�b�N�ݒ�����{�^����Inspector�Ŏw��
    public TMP_Dropdown resolutionDropdown;     // �𑜓x�ݒ�p�h���b�v�_�E��
    public TMP_Dropdown fullScreenDropdown;     // �t���X�N���[���ݒ�p�h���b�v�_�E��
    public TMP_Dropdown qualityDropdown;        // �i���ݒ�p�h���b�v�_�E��
    public TMP_Dropdown antiAliasingDropdown;   // �A���`�G�C���A�X�ݒ�p�h���b�v�_�E��
    public TMP_Dropdown shadowQualityDropdown;  // �A�e�i���ݒ�p�h���b�v�_�E��
    public TMP_Dropdown frameRateDropdown;      // �t���[�����[�g�ݒ�p�h���b�v�_�E��
    public TMP_Dropdown vSyncDropdown;          // VSync�p�h���b�v�_�E��
    public Button graphicsSettingsDefaultButton;// �O���t�B�b�N�ݒ�f�t�H���g���A�{�^��

    [Header("�T�E���h�ݒ�֘A")]
    public GameObject soundSettingsCanvas;      // �T�E���h�ݒ�Canvas
    public Button closeSoundSettingsButton;     // �T�E���h�ݒ����{�^��
    public Slider masterVolumeSlider;           // �}�X�^�[���ʗp�X���C�_�[
    public Slider bgmVolumeSlider;              // BGM���ʗp�X���C�_�[
    public Slider sfxVolumeSlider;              // SFX���ʗp�X���C�_�[
    public TMP_Text masterVolumeText;           // �}�X�^�[���ʃe�L�X�g�\���p
    public TMP_Text bgmVolumeText;              // BGM���ʃe�L�X�g�\���p
    public TMP_Text sfxVolumeText;              // SFX���ʃe�L�X�g�\���p
    public Button soundSettingsDefaultButton;   // �T�E���h�ݒ�f�t�H���g���A�{�^��

    [Header("������@��ʊ֘A")]
    public GameObject instructionsCanvas;        // ������@�pCanvas
    public Button closeInstructionsButton;       // ������@��ʂ����{�^��
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
    public Sprite keyInputWaitingSprite; // �L�[���͑҂��̕\���摜
    public Sprite keyAssignedSprite;     // �L�[���蓖�Ċ������̕\���摜
    public Button instructionsSettingsDefaultButton; // ����ݒ�̃��Z�b�g�{�^��

    [Header("�m�F�pCanvas")]
    public GameObject titleConfirmCanvas; // �^�C�g���֖߂�m�FCanvas��Inspector�Ŏw��
    public Button titleConfirmYesButton;  // �^�C�g���֖߂�m�F�́u�͂��v�{�^��
    public Button titleConfirmNoButton;   // �^�C�g���֖߂�m�F�́u�������v�{�^��

    public GameObject quitConfirmCanvas;  // �Q�[���I���m�FCanvas��Inspector�Ŏw��
    public Button quitConfirmYesButton;   // �Q�[���I���m�F�́u�͂��v�{�^��
    public Button quitConfirmNoButton;    // �Q�[���I���m�F�́u�������v�{�^��

    [Header("�t�F�[�h�A�E�g�p�ݒ�")]
    public GameObject fadeCanvas;  // �t�F�[�h�A�E�g�p�̃L�����o�X��Inspector�Ŏw��
    public Image fadeImage;        // �t�F�[�h�A�E�g������摜��Inspector�Ŏw��

    public bool isPaused = false;        // �|�[�Y�����ǂ���
    private bool isInitializing = false;  // �ݒ�ǂݍ��ݒ����ǂ���

    private Coroutine fadeCoroutine;  // �R���[�`���̃C���X�^���X��ۑ�����ϐ�

    // �L�[�ݒ�p�̕ϐ�
    private KeyCode pauseKey;

    void Start()
    {
        // �J�n���̓|�[�Y�������
        Time.timeScale = 1f;

        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        pauseKey = (KeyCode)PlayerPrefs.GetInt("PauseKey", (int)KeyCode.Escape);

        // �e��Canvas���\���ɐݒ�i�ŏ��̓|�[�Y���j���[�͕�����ԁj
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);
        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(false);     // �^�C�g���m�FCanvas��\��
        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(false);       // �I���m�FCanvas��\��

        // �e��{�^���ɃN���b�N���̃C�x���g���X�i�[��o�^
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (graphicsSettingsButton != null) graphicsSettingsButton.onClick.AddListener(OpenGraphicsSettings);
        if (soundSettingsButton != null) soundSettingsButton.onClick.AddListener(OpenSoundSettings);
        if (titleButton != null) titleButton.onClick.AddListener(ShowTitleConfirmCanvas);
        if (quitButton != null) quitButton.onClick.AddListener(ShowQuitConfirmCanvas);
        if (closeGraphicsSettingsButton != null) closeGraphicsSettingsButton.onClick.AddListener(CloseGraphicsSettings);
        if (closeSoundSettingsButton != null) closeSoundSettingsButton.onClick.AddListener(CloseSoundSettings);
        if (graphicsSettingsDefaultButton != null) graphicsSettingsDefaultButton.onClick.AddListener(ResetGraphicsSettingsToDefaults);
        if (soundSettingsDefaultButton != null) soundSettingsDefaultButton.onClick.AddListener(ResetSoundSettingsToDefaults);

        // �^�C�g���m�FCanvas�p�{�^��
        if (titleConfirmYesButton != null) titleConfirmYesButton.onClick.AddListener(ReturnToTitle);
        if (titleConfirmNoButton != null) titleConfirmNoButton.onClick.AddListener(CloseTitleConfirmCanvas);

        // �I���m�FCanvas�p�{�^��
        if (quitConfirmYesButton != null) quitConfirmYesButton.onClick.AddListener(QuitGame);
        if (quitConfirmNoButton != null) quitConfirmNoButton.onClick.AddListener(CloseQuitConfirmCanvas);

        // ������@�{�^���Ƀ��X�i�[��ǉ�
        if (instructionsButton != null) instructionsButton.onClick.AddListener(OpenInstructions);
        
        // ������@��ʂ̕���{�^���Ƀ��X�i�[��ǉ�
        if (closeInstructionsButton != null) closeInstructionsButton.onClick.AddListener(CloseInstructions);
        
        // ������@�L�����o�X��������\���ɐݒ�
        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);

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
        if (instructionsSettingsDefaultButton != null) instructionsSettingsDefaultButton.onClick.AddListener(ResetInstructionsSettingsToDefaults);
        
        // �ݒ�̃��[�h�����J�n
        isInitializing = true;
        LoadAllSettings(); // �ۑ��ς݂̐ݒ�l�𔽉f
        isInitializing = false;
    }

    void Update()
    {
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

                    // ���X�N���v�g�ɃL�[�ݒ�����[�h������
                    UpdateKeySettings();

                    SoundManager.instance.PlayClickSound(); // �N���b�N�����Đ�

                    // �L�[���蓖�Ċ������̉摜��\��
                    //if (keyAssignedSprite != null) ShowAndFadeOutImage(keyAssignedSprite);

                    Debug.Log(waitingForKey + " �� " + code.ToString() + " �Ɋ��蓖�Ă��܂����B");
                    waitingForKey = null;
                    break;
                }
            }
        }

        // ESC�L�[�Ń|�[�Y���j���[�̕\��/��\����؂�ւ���
        else if (Input.GetKeyDown(pauseKey))
        {
            // �O���t�B�b�N�ݒ肪�\�����̏ꍇ�͕��ă|�[�Y���j���[�ɖ߂�
            if (graphicsSettingsCanvas != null && graphicsSettingsCanvas.activeSelf)
            {
                CloseGraphicsSettings();
                return;
            }

            // �T�E���h�ݒ肪�\�����̏ꍇ�͕��ă|�[�Y���j���[�ɖ߂�
            if (soundSettingsCanvas != null && soundSettingsCanvas.activeSelf)
            {
                CloseSoundSettings();
                return;
            }

            // �^�C�g���m�F��ʂ��\�����̏ꍇ�͕��ă|�[�Y���j���[�ɖ߂�
            if (titleConfirmCanvas != null && titleConfirmCanvas.activeSelf)
            {
                CloseTitleConfirmCanvas();
                return;
            }

            // �I���m�F��ʂ��\�����̏ꍇ�͕��ă|�[�Y���j���[�ɖ߂�
            if (quitConfirmCanvas != null && quitConfirmCanvas.activeSelf)
            {
                CloseQuitConfirmCanvas();
                return;
            }

            // ������@��ʂ��\�����̏ꍇ�͕��ă|�[�Y���j���[�ɖ߂�
            if (instructionsCanvas != null && instructionsCanvas.activeSelf)
            {
                CloseInstructions();
                return;
            }

            // �ʏ�̃|�[�Y/�ĊJ�؂�ւ�
            if (isPaused)
            {
                // �|�[�Y���ł���΍ĊJ
                ResumeGame();
            }
            else
            {
                // �|�[�Y���j���[���J��
                PauseGame();
            }
        }
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

    // �Q�[�����|�[�Y��Ԃɂ��鏈��
    void PauseGame()
    {
        // ���艹�Đ�
        SoundManager.instance.PlayDecisionSound();

        isPaused = true;     // �|�[�Y���t���O

        // �|�[�Y���j���[Canvas��\��
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);

        // �v���C���[�̈ړ��E���_����𖳌���
        FirstPersonMovement playerMovement = FindObjectOfType<FirstPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
            playerMovement.DisableLook();
        }

        // �����̍Đ����~�i��isPaused�t���O�ɂ�莩���I��FistPersonMovement�Œ�~����邽�ߕs�v�ƂȂ����j
        //if (SoundManager.instance != null)
        //{
        //    SoundManager.instance.StopFootsteps();
        //}

        // �}�E�X�J�[�\����\������UI����\��
        playerMovement.UnlockCursor();

        Time.timeScale = 0f; // ���Ԓ�~
    }

    // �Q�[�����ĊJ���鏈��
    public void ResumeGame()
    {
        // ���艹�Đ�
        SoundManager.instance.PlayDecisionSound();

        isPaused = false;    // �|�[�Y�t���O����

        // �|�[�Y���j���[���\��
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);

        // �v���C���[�̑����L����
        FirstPersonMovement playerMovement = FindObjectOfType<FirstPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
            playerMovement.EnableLook();
        }

        // �}�E�X�J�[�\����\���ŃQ�[������ɕ��A
        playerMovement.LockCursor();

        Time.timeScale = 1f; // ���Ԑi�s�ĊJ
    }

    // �^�C�g���֖߂�m�FCanvas��\�����鏈��
    void ShowTitleConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        // �|�[�Y���j���[����ă^�C�g���m�FCanvas��\��
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(true);
    }

    // �^�C�g���m�FCanvas����A�|�[�Y���j���[�ɖ߂�
    void CloseTitleConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        if (titleConfirmCanvas != null) titleConfirmCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // �Q�[���I���m�FCanvas��\�����鏈��
    void ShowQuitConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        // �|�[�Y���j���[����ďI���m�FCanvas��\��
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(true);
    }

    // �I���m�FCanvas����A�|�[�Y���j���[�֖߂�
    void CloseQuitConfirmCanvas()
    {
        SoundManager.instance.PlayDecisionSound();

        if (quitConfirmCanvas != null) quitConfirmCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // �O���t�B�b�N�ݒ�Canvas��\�����鏈��
    void OpenGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(true);
    }

    // �O���t�B�b�N�ݒ�Canvas����A�|�[�Y���j���[�֖߂�
    void CloseGraphicsSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (graphicsSettingsCanvas != null) graphicsSettingsCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // �T�E���h�ݒ�Canvas��\�����鏈��
    void OpenSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(true);
    }

    // �T�E���h�ݒ�Canvas����A�|�[�Y���j���[�֖߂�
    void CloseSoundSettings()
    {
        SoundManager.instance.PlayDecisionSound();
        if (soundSettingsCanvas != null) soundSettingsCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    // �^�C�g���֖߂�i�^�C�g���V�[���ֈڍs�j
    void ReturnToTitle()
    {
        SoundManager.instance.PlayDecisionSound();
        Time.timeScale = 1f; // ���Ԑi�s�����ɖ߂�
        SceneManager.LoadScene("MenuScene"); // "MenuScene"�����[�h
    }

    // �Q�[�����I�����鏈���i�r���h��̂ݗL���j
    void QuitGame()
    {
        SoundManager.instance.PlayDecisionSound();
        Debug.Log("�Q�[�����I�����܂��B");
        Application.Quit();
    }

    // �S�ݒ�����[�h���Ĕ��f���鏈��
    void LoadAllSettings()
    {
        // �e��O���t�B�b�N�X�ݒ�l��PlayerPrefs����擾
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        int fullScreenMode = PlayerPrefs.GetInt("FullScreenMode", 0);
        int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 5);
        int antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0);
        int shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2);
        int frameRateLimit = PlayerPrefs.GetInt("FrameRateLimit", 1);
        int vSyncSetting = PlayerPrefs.GetInt("VSync", 1); // VSync�ݒ���ǂݍ���

        // �l�����ۂ̐ݒ�֔��f
        SetResolution(resolutionIndex);
        SetFullScreen(fullScreenMode);
        SetQuality(graphicsQuality);
        SetAntiAliasing(antiAliasing);
        SetShadowQuality(shadowQuality);
        SetFrameRate(frameRateLimit);
        SetVSync(vSyncSetting);

        // �h���b�v�_�E�����j���[�̕\�����X�V
        UpdateGraphicsDropdownValues(resolutionIndex, fullScreenMode, graphicsQuality, antiAliasing, shadowQuality, frameRateLimit, vSyncSetting);

        // �T�E���h�ݒ�ǂݍ���
        int masterVolume = PlayerPrefs.GetInt("MasterVolume", 100); // �f�t�H���g100
        int bgmVolume = PlayerPrefs.GetInt("BGMVolume", 30);     // �f�t�H���g30
        int sfxVolume = PlayerPrefs.GetInt("SFXVolume", 100);    // �f�t�H���g100

        // �T�E���h�ݒ蔽�f
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        // �T�E���h�p�X���C�_�[�\���X�V
        UpdateSoundSliderValues(masterVolume, bgmVolume, sfxVolume);

        // �h���b�v�_�E����X���C�_�[�ɕύX�����X�i�[��ݒ�
        SetupDropdownListeners();
        SetupSoundSliderListeners();

        // �L�[�ݒ�̃��[�h
        LoadKeySettings();
    }

    // �O���t�B�b�N�p�h���b�v�_�E���̏����l�Z�b�g
    void UpdateGraphicsDropdownValues(int resolutionIndex, int fullScreenMode, int graphicsQuality, int antiAliasing, int shadowQuality, int frameRateLimit, int vSyncSetting)
    {
        // �𑜓x�h���b�v�_�E��
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            // ����Œ�𑜓x
            List<string> resolutions = new List<string> { "1920 x 1080" };
            resolutionDropdown.AddOptions(resolutions);
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        // �t���X�N���[���h���b�v�_�E��
        if (fullScreenDropdown != null)
        {
            fullScreenDropdown.ClearOptions();
            fullScreenDropdown.AddOptions(new List<string> { "Full Screen", "Windowed" });
            fullScreenDropdown.value = fullScreenMode;
            fullScreenDropdown.RefreshShownValue();
        }

        // �i���ݒ�h���b�v�_�E��
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
            qualityDropdown.value = graphicsQuality;
            qualityDropdown.RefreshShownValue();
        }

        // �A���`�G�C���A�X�h���b�v�_�E��
        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.ClearOptions();
            antiAliasingDropdown.AddOptions(new List<string> { "Off", "2x", "4x", "8x" });
            antiAliasingDropdown.value = antiAliasing;
            antiAliasingDropdown.RefreshShownValue();
        }

        // �A�e�i���h���b�v�_�E��
        if (shadowQualityDropdown != null)
        {
            shadowQualityDropdown.ClearOptions();
            shadowQualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High" });
            shadowQualityDropdown.value = shadowQuality;
            shadowQualityDropdown.RefreshShownValue();
        }

        // �t���[�����[�g�h���b�v�_�E��
        if (frameRateDropdown != null)
        {
            frameRateDropdown.ClearOptions();
            frameRateDropdown.AddOptions(new List<string> { "30 FPS", "60 FPS", "120 FPS", "Unlimited" });
            frameRateDropdown.value = frameRateLimit;
            frameRateDropdown.RefreshShownValue();
        }

        // VSync�h���b�v�_�E��������
        if (vSyncDropdown != null)
        {
            vSyncDropdown.ClearOptions();
            vSyncDropdown.AddOptions(new List<string> { "Off", "On" });
            vSyncDropdown.value = vSyncSetting;
            vSyncDropdown.RefreshShownValue();
        }
    }

    // �T�E���h�p�X���C�_�[�ƃe�L�X�g�̏����\���ݒ�
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

    // �O���t�B�b�N�p�h���b�v�_�E���ɒl�ύX���̃��X�i�[��ǉ�����
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

    // �T�E���h�p�X���C�_�[�ɒl�ύX���̃��X�i�[��ǉ�����
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

    // �𑜓x�ݒ菈���i�{�R�[�h�ł͌Œ��1920x1080�j
    void SetResolution(int resolutionIndex)
    {
        // �Œ��1920x1080�ɐݒ肵�Ă���
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    // �t���X�N���[���ݒ菈��
    void SetFullScreen(int fullScreenModeIndex)
    {
        bool isFullScreen = (fullScreenModeIndex == 0);
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreenMode", fullScreenModeIndex);
    }

    // �i���ݒ菈��
    void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    // �A���`�G�C���A�X�ݒ菈��
    void SetAntiAliasing(int index)
    {
        int[] antiAliasingValues = { 0, 2, 4, 8 };
        QualitySettings.antiAliasing = antiAliasingValues[index];
        PlayerPrefs.SetInt("AntiAliasing", index);
    }

    // �V���h�E�i���ݒ菈��
    void SetShadowQuality(int index)
    {
        //0:Low(Disable),1:Medium(HardOnly),2:High(All)
        ShadowQuality[] shadowQualities = { ShadowQuality.Disable, ShadowQuality.HardOnly, ShadowQuality.All };
        QualitySettings.shadows = shadowQualities[index];
        PlayerPrefs.SetInt("ShadowQuality", index);
    }

    // �t���[�����[�g�ݒ菈��
    void SetFrameRate(int index)
    {
        // index=0:30fps,1:60fps,2:120fps,3:Unlimited(-1�Ő����Ȃ�)
        int[] frameRates = { 30, 60, 120, -1 };
        Application.targetFrameRate = frameRates[index];

        // Unlimited����vSyncCount��0�ɁA����1(���d�l)
        //QualitySettings.vSyncCount = (index == 3) ? 0 : 1;

        PlayerPrefs.SetInt("FrameRateLimit", index);
    }

    void SetVSync(int index)
    {
        // index == 0 -> �I�t, 1 -> �I��
        if (index == 0)
        {
            QualitySettings.vSyncCount = 0; // VSync Off
        }
        else
        {
            QualitySettings.vSyncCount = 1; // VSync On
        }

        PlayerPrefs.SetInt("VSync", index);
        Debug.Log($"VSync�ݒ�: {(index == 0 ? "Off" : "On")}");
    }

    // �}�X�^�[���ʐݒ菈��
    void SetMasterVolume(int value)
    {
        AudioListener.volume = value / 100f;       // ���ۂ̉��ʂ� 0.0�`1.0 �ň���
        PlayerPrefs.SetInt("MasterVolume", value); // int �ŕۑ�
    }

    // BGM���ʐݒ菈��
    void SetBGMVolume(int value)
    {
        if (SoundManager.instance != null && SoundManager.instance.bgmSource != null)
        {
            SoundManager.instance.bgmSource.volume = value / 100f;
        }
        PlayerPrefs.SetInt("BGMVolume", value); // int �ŕۑ�
    }

    // SFX���ʐݒ菈��
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
        PlayerPrefs.SetInt("SFXVolume", value); // int �ŕۑ�
    }

    // �O���t�B�b�N�ݒ���f�t�H���g�l�փ��Z�b�g
    void ResetGraphicsSettingsToDefaults()
    {
        isInitializing = true;
        SoundManager.instance.PlayClickSound();

        // �e��ݒ�������l��
        SetResolution(0);
        SetFullScreen(0);
        SetQuality(5);
        SetAntiAliasing(0);
        SetShadowQuality(2);
        SetFrameRate(1);
        SetVSync(1);

        // PlayerPrefs�ɂ��ۑ�
        PlayerPrefs.SetInt("ResolutionIndex", 0);
        PlayerPrefs.SetInt("FullScreenMode", 0);
        PlayerPrefs.SetInt("GraphicsQuality", 5);
        PlayerPrefs.SetInt("AntiAliasing", 0);
        PlayerPrefs.SetInt("ShadowQuality", 2);
        PlayerPrefs.SetInt("FrameRateLimit", 1);
        PlayerPrefs.SetInt("VSync", 1);

        // �h���b�v�_�E���ĕ\��
        UpdateGraphicsDropdownValues(0, 0, 5, 0, 2, 1, 1);
        PlayerPrefs.Save();
        isInitializing = false;
    }

    // �T�E���h�ݒ���f�t�H���g�l�փ��Z�b�g
    void ResetSoundSettingsToDefaults()
    {
        SoundManager.instance.PlayClickSound();

        // �e�퉹�ʂ��f�t�H���g��
        SetMasterVolume(100);
        SetBGMVolume(30);
        SetSFXVolume(100);

        // PlayerPrefs�ɂ��ۑ�
        PlayerPrefs.SetFloat("MasterVolume", 100);
        PlayerPrefs.SetFloat("BGMVolume", 30);
        PlayerPrefs.SetFloat("SFXVolume", 100);

        // �X���C�_�[�\���X�V
        UpdateSoundSliderValues(100, 30, 100);
        PlayerPrefs.Save();
    }

    // ������@��ʂ�\�����鏈��
    void OpenInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);  // PauseMenuCanvas���\��
        if (instructionsCanvas != null) instructionsCanvas.SetActive(true);  // InstructionsCanvasCanvas��\��
    }

    // ������@��ʂ���鏈��
    void CloseInstructions()
    {
        SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);  // InstructionsCanvasCanvas���\��
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);  // PauseMenuCanvas��\��
    }

    // �}�E�X���x��K�p����֐�
    void SetLookSpeed(int value)
    {
        PlayerPrefs.SetInt("LookSpeed", value); // int �ŕۑ�
        Debug.Log($"�}�E�X���x��ݒ�: {value}");

        // FirstPersonMovement.cs �̃X�N���v�g��T��
        FirstPersonMovement firstPersonMovement = FindObjectOfType<FirstPersonMovement>();
        if (firstPersonMovement != null)
        {
            firstPersonMovement.LoadKeySettings();
        }
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
        PlayerPrefs.SetFloat("LookSpeed", 50);

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

        // ���X�N���v�g�ɃL�[�ݒ�����[�h������
        UpdateKeySettings();

        isInitializing = false;  // �Ō�ɂ���bool�ϐ������ɖ߂�
    }

    void UpdateKeySettings()
    {
        // Hierarchy��̑S�Ă̋q�I�u�W�F�N�g���擾
        CustomerCallIcon[] allIcons = FindObjectsOfType<CustomerCallIcon>();

        // �S�Ă�CustomerCallIcon�ɑ΂���LoadKeySettings���Ăяo��
        foreach (CustomerCallIcon icon in allIcons)
        {
            icon.LoadKeySettings();
        }

        // Hierarchy��̑S�Ă̋q�I�u�W�F�N�g���擾
        CustomerCallIcon_Endless[] allIcons_Endless = FindObjectsOfType<CustomerCallIcon_Endless>();

        // �S�Ă�CustomerCallIcon_Endless�ɑ΂���LoadKeySettings���Ăяo��
        foreach (CustomerCallIcon_Endless icon_Endless in allIcons_Endless)
        {
            icon_Endless.LoadKeySettings();
        }

        // DrinkSelectionManager.cs �̃X�N���v�g��T��
        DrinkSelectionManager drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();
        if (drinkSelectionManager != null)
        {
            drinkSelectionManager.LoadKeySettings();
        }

        // FirstPersonMovement.cs �̃X�N���v�g��T��
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
