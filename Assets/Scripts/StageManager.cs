using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // TextMeshPro���g�p���邽�߂ɒǉ�

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public static float totalRevenue = 0f;  // ���v�����ێ�����ϐ�

    public GameObject resultScreenCanvas;  // ���U���g��ʂ�Canvas
    public Image resultBackgroundImage;     // ���U���g��ʂ̔w�i�摜�i�t�F�[�h�C���p�j
    public TextMeshProUGUI revenueText;    // ���U���g��ʓ��̔�����\���p�e�L�X�g
    public TextMeshProUGUI totalRevenueText;    // ���v����\���p�e�L�X�g
    public TextMeshProUGUI targetRevenueText;    // �ڕW���z�\���p�e�L�X�g
    public TextMeshProUGUI stageClearText;  // �X�e�[�W�N���A�p�̃e�L�X�g
    public TextMeshProUGUI clearTimeText; // �N���A���ԕ\���p�e�L�X�g

    public FirstPersonMovement playerMovement;  // �v���C���[�̓���X�N���v�g���w��

    public int nextStageNumber = 1;  // ���̃X�e�[�W�ԍ����Ǘ�����ϐ�
    public Button nextStageButton;  // ���̃X�e�[�W�ɐi�ނ��߂̃{�^��
    
    public TextMeshProUGUI elapsedTimeText;   // �c�莞�Ԃ�\������TextMeshProUGUI��Inspector�Ŏw��
    public float elapsedTime = 0f;  // �o�ߎ��ԁi�b�j 
    private bool hasShownResult = false;    // ShowResultScreen�����ɌĂяo���ꂽ�����Ǘ�����t���O

    private Coroutine fadeInCoroutine;  // �t�F�[�h�C���������Ǘ�����R���[�`���̎Q��

    // �u�Z�[�u���Ď��ցv�N���b�N��̃t�F�[�h����
    private CanvasGroup fadeCanvasGroup; // �t�F�[�h�p��CanvasGroup
    private float fadeDuration = 0.6f; // �t�F�[�h�̎���

    // �Q�[���J�n���̃t�F�[�h����
    private CanvasGroup fadeInCanvasGroup; // �t�F�[�h�C���p��CanvasGroup
    private float fadeInDuration = 2.0f;   // �t�F�[�h�C���̎��ԁi�b�j

    // ����ݒ�̃C���f�b�N�X
    private int languageIndex;

    [Header("Index 1:���j��, 2:�Ηj��, 3:���j��, 4:�ؗj��, 5:���j��, 6:�y�j��, 7:���j��")]
    public TMP_Text dayofWeekText;
    public int dayofWeekIndex = 1;


    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͔j��
        }
    }

    void Start()
    {
        // PlayerPrefs���猾��ݒ�̃C���f�b�N�X���擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g�l��0

        // �ÓI�I�u�W�F�N�g�Ɍ���ݒ��K�p
        ApplyLanguage(languageIndex);

        // ���������Ƀ��U���g�L�����o�X���\���ɐݒ�
        resultScreenCanvas.SetActive(false);

        // ���̃X�e�[�W�֐i�ރ{�^���Ƀ��X�i�[��ݒ�
        nextStageButton.onClick.AddListener(LoadNextStage);

        // �ڋq�J�E���g�����Z�b�g
        //CustomerCallIcon.totalCustomers = 0;
        //CustomerCallIcon.completedCustomers = 0;

        // WhiteFadeCanvas����WhiteImage�������������ACanvasGroup���擾
        GameObject fadeCanvas = GameObject.Find("WhiteFadeCanvas");
        if (fadeCanvas != null)
        {
            GameObject blackImage = fadeCanvas.transform.Find("WhiteImage")?.gameObject;
            if (blackImage != null)
            {
                fadeCanvasGroup = blackImage.GetComponent<CanvasGroup>();
                if (fadeCanvasGroup != null)
                {
                    // ������Ԃœ����ɐݒ�
                    fadeCanvasGroup.alpha = 0f;
                }
                else
                {
                    Debug.LogError("WhiteImage��CanvasGroup�R���|�[�l���g��������܂���ł����B");
                }
            }
            else
            {
                Debug.LogError("WhiteFadeCanvas����BlackImage��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogError("WhiteFadeCanvas��������܂���ł����B");
        }

        // �t�F�[�h�C���p��CanvasGroup���擾
        GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
        if (fadeInCanvas != null)
        {
            GameObject blackImage = fadeInCanvas.transform.Find("BlackImage")?.gameObject;
            if (blackImage != null)
            {
                fadeInCanvasGroup = blackImage.GetComponent<CanvasGroup>();
                if (fadeInCanvasGroup != null)
                {
                    // ������ԂŊ��S�ɕs�����ɐݒ�
                    fadeInCanvasGroup.alpha = 1f;
                    // �t�F�[�h�C���R���[�`�����J�n
                    StartCoroutine(FadeIn());
                }
                else
                {
                    Debug.LogError("BlackImage��CanvasGroup�R���|�[�l���g��������܂���ł����B");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvas����BlackImage��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogError("FadeInCanvas��������܂���ł����B");
        }
    }

    void Update()
    {
        // �o�ߎ��Ԃ𑝉�
        elapsedTime += Time.deltaTime;

        // �o�ߎ��Ԃ�0�����ɂȂ�Ȃ��悤�ɐ���
        if (elapsedTime < 0)
            elapsedTime = 0;

        // �o�ߎ��Ԃ��e�L�X�g�ɕ\���i��:�b�`���j
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        if (elapsedTimeText != null)
        {           
            switch (languageIndex)
            {
                case 0: // �p��
                    elapsedTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
                    break;
                case 1: // ���{��
                    elapsedTimeText.text = string.Format("�o�ߎ���: {0:00}:{1:00}", minutes, seconds);
                    break;
            }
        }   
    }

    // ���U���g��ʂ�\�����郁�\�b�h
    public void ShowResultScreen()
    {
        // ���Ƀ��U���g��ʂ��\������Ă���ꍇ�͉������Ȃ�
        if (hasShownResult)
            return;

        hasShownResult = true; // �t���O�𗧂Ă�

        // �}�E�X�J�[�\����\��
        playerMovement.UnlockCursor();

        // elapsedTime�̉��Z���~�����邽�߂ɁAUpdate���\�b�h�ł̉��Z�������~�߂�
        elapsedTimeText.enabled = false;

        // �N���A���Ԃ�\��
        if (clearTimeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            
            switch (languageIndex)
            {
                case 0: // �p��
                    clearTimeText.text = string.Format("Clear Time: {0:00}:{1:00}", minutes, seconds);
                    break;
                case 1: // ���{��
                    clearTimeText.text = string.Format("�N���A����: {0:00}:{1:00}", minutes, seconds);
                    break;
            }
        }

        if (resultScreenCanvas != null && revenueText != null)
        {
            // ��������e�L�X�g�ɐݒ�           
            switch (languageIndex)
            {
                case 0: // �p��
                    revenueText.text = "Today's Sales: " + Mathf.FloorToInt(CustomerCallIcon.dailyRevenue).ToString() + " yen";
                    break;
                case 1: // ���{��
                    revenueText.text = "�{���̔���: " + Mathf.FloorToInt(CustomerCallIcon.dailyRevenue).ToString() + " �~";
                    break;
            }

            // ���v����ɖ{���̔�������Z
            totalRevenue += CustomerCallIcon.dailyRevenue;

            // ���v������e�L�X�g�ɐݒ�
            if (totalRevenueText != null)
            {     
                switch (languageIndex)
                {
                    case 0: // �p��
                        totalRevenueText.text = "Total Sales: " + Mathf.FloorToInt(totalRevenue).ToString() + " yen";
                        break;
                    case 1: // ���{��
                        totalRevenueText.text = "���v����: " + Mathf.FloorToInt(totalRevenue).ToString() + " �~";
                        break;
                }
            }

            // PlayerPrefs����TargetRevenue���擾���ĕ\��
            if (targetRevenueText != null)
            {
                float targetRevenue = PlayerPrefs.GetFloat("TargetRevenue", 0f); // �ڕW���z���擾
                
                switch (languageIndex)
                {
                    case 0: // �p��
                        targetRevenueText.text = "Target Sales: " + Mathf.FloorToInt(targetRevenue).ToString() + " yen";
                        break;
                    case 1: // ���{��
                        targetRevenueText.text = "�ڕW���z: " + Mathf.FloorToInt(targetRevenue).ToString() + " �~";
                        break;
                }
            }

            // ���U���g�L�����o�X����UI�v�f�̃A���t�@��0�ɐݒ�
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

            // ���U���g�L�����o�X��\��
            resultScreenCanvas.SetActive(true);

            // �v���C���[�̓����Ǝ��_�ړ����֎~�iInspector�Ŏw�肳�ꂽ playerMovement ���g�p�j
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
                playerMovement.DisableLook();
            }
            // ���U���gBGM���Đ�
            SoundManager.instance.PlayResultBGM();

            // �t�F�[�h�C���������J�n
            if (fadeInCoroutine == null)
            {
                fadeInCoroutine = StartCoroutine(FadeInResultScreen());
            }
        }
    }


    // ���̃V�[����ǂݍ���
    void LoadNextStage()
    {
        // ���U���g��ʂ��\���ɂ���
        //resultScreenCanvas.SetActive(false);

        // �ڋq�J�E���g�����Z�b�g
        CustomerCallIcon.totalCustomers = 0;
        CustomerCallIcon.completedCustomers = 0;

        // ����������Z�b�g�i���̓��ɂȂ邽�߁j
        CustomerCallIcon.dailyRevenue = 0f;

        // �v���C���[�̓������ēx�L����
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
            playerMovement.EnableLook();
        }

        // �Q�[���̐i�����Z�[�u
        SaveManager.SaveGame(nextStageNumber, totalRevenue);

        // ���̃X�e�[�W�����[�h
        string nextSceneName = "Stage" + nextStageNumber;  // �X�e�[�W���� "Stage1", "Stage2" �ȂǂƉ���

        // �t�F�[�h�A�E�g���ăV�[�������[�h����R���[�`�����J�n
        StartCoroutine(DelayWithAction(0.1f, () =>
        {
            StartCoroutine(FadeAndLoadScene(nextSceneName));
        }));
    }

    // ���U���g��ʂ��t�F�[�h�C��������R���[�`��
    private IEnumerator FadeInResultScreen()
    {
        float fadeDuration = 1f; // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            // �w�i�摜�̃t�F�[�h�C��
            if (resultBackgroundImage != null)
            {
                Color bgColor = resultBackgroundImage.color;
                bgColor.a = alpha;
                resultBackgroundImage.color = bgColor;
            }

            // ������e�L�X�g�̃t�F�[�h�C��
            if (revenueText != null)
            {
                Color revColor = revenueText.color;
                revColor.a = alpha;
                revenueText.color = revColor;
            }

            // ���v����e�L�X�g�̃t�F�[�h�C��
            if (totalRevenueText != null)
            {
                Color totalRevColor = totalRevenueText.color;
                totalRevColor.a = alpha;
                totalRevenueText.color = totalRevColor;
            }

            // �ڕW���z�e�L�X�g�̃t�F�[�h�C��
            if (targetRevenueText != null)
            {
                Color targetRevColor = targetRevenueText.color;
                targetRevColor.a = alpha;
                targetRevenueText.color = targetRevColor;
            }

            // �X�e�[�W�N���A�p�e�L�X�g�̃t�F�[�h�C��
            if (stageClearText != null)
            {
                Color targetRevColor = stageClearText.color;
                targetRevColor.a = alpha;
                stageClearText.color = targetRevColor;
            }

            // �N���A���ԃe�L�X�g�̃t�F�[�h�C��
            if (clearTimeText != null)
            {
                Color clearTimeColor = clearTimeText.color;
                clearTimeColor.a = alpha;
                clearTimeText.color = clearTimeColor;
            }

            yield return null;
        }

        // ���S�Ƀt�F�[�h�C��������Ԃɂ���
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

        fadeInCoroutine = null; // �R���[�`���̎Q�Ƃ��N���A
    }

    // �ėp�I�Ȓx���֐�
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // �w�莞�Ԃ̒x��
        yield return new WaitForSeconds(delayTime);

        // �x����ɃR�[���o�b�N�Ƃ��ēn���ꂽ�A�N�V���������s
        action?.Invoke();
    }

    // �t�F�[�h�A�E�g���ăV�[�������[�h����R���[�`��
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // �t�F�[�h�A�E�g
        yield return StartCoroutine(Fade(1f));

        // �x��������
        yield return StartCoroutine(DelayWithAction(0.3f, null));

        // �V�[�������[�h
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // �t�F�[�h�C��
        // yield return StartCoroutine(Fade(0f));
    }

    // �t�F�[�h�����̃R���[�`��
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

        // �ŏI�I��Alpha�l��ڕW�l�ɐݒ�
        fadeCanvasGroup.alpha = targetAlpha;
    }

    // �t�F�[�h�C���p�̃R���[�`��
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

        // ���S�Ƀt�F�[�h�C�����I�������alpha��0�ɐݒ�
        fadeInCanvasGroup.alpha = 0f;
    }

    private void ApplyLanguage(int index)
    {
        switch (dayofWeekIndex)
        {
            case 1: // ���j��                
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Monday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "���j��";
                        break;
                }
                break;

            case 2: // �Ηj��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Tuesday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "�Ηj��";
                        break;
                }
                break;

            case 3: // ���j��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Wednesday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "���j��";
                        break;
                }
                break;

            case 4: // �ؗj��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Thursday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "�ؗj��";
                        break;
                }
                break;

            case 5: // ���j��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Friday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "���j��";
                        break;
                }
                break;

            case 6: // �y�j��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Saturday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "�y�j��";
                        break;
                }
                break;

            case 7: // ���j��
                switch (index)
                {
                    case 0: // �p��
                        dayofWeekText.text = "Sunday";
                        break;
                    case 1: // ���{��
                        dayofWeekText.text = "���j��";
                        break;
                }
                break;
        }
    }
}
