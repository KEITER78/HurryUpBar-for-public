using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro���g�p
using UnityEngine.SceneManagement;

public class ConversationSceneManager : MonoBehaviour
{
    public TextMeshProUGUI conversationText; // ��b�e�L�X�g��\������TextMeshPro
    public GameObject conversationCanvas; // ��b�p��Canvas�iUI�S�́j
    public TextMeshProUGUI pressEText; // �uE�������Ď��ցv��\������Text
    public float idleTimeBeforeHint = 5.0f; // �q���g��\������܂ł̑ҋ@����
    private Coroutine hintCoroutine; // �q���g�\���̃R���[�`���p
    public CanvasGroup fadeCanvasGroup; // �t�F�[�h�pCanvasGroup��Inspector�Ŏw��
    public float fadeDuration = 1.5f; // �t�F�[�h�C���E�A�E�g�̎���

    [TextArea(2, 5)] // Inspector�ł̕\���p�i�����s�̃e�L�X�g��\���\�j
    public string[] conversationMessages; // ��b���e��ۑ�����z�� (Inspector����ݒ�\)
    [TextArea(2, 5)] // Inspector�ł̕\���p�i�����s�̃e�L�X�g��\���\�j
    public string[] conversationMessages_en; // ��b���e��ۑ�����z�� (Inspector����ݒ�\)

    private int conversationIndex = 0; // ���݂̉�b���b�Z�[�W�̃C���f�b�N�X
    private bool isConversing = false; // ��b�����ǂ������Ǘ�����t���O
    public float typingSpeed = 0.05f; // 1�������\�����鑬�x�i�b�j

    public float targetRevenue; // �ڕW���z��Inspector�Ŏw��ł���悤�ɂ���ϐ�

    [Header("���ɓǂݍ��ރV�[����ݒ�")]
    public string nextSceneName;   // �ǂݍ��ރV�[������Inspector�Ŏw��

    [Header("Start���ŁABGM���Đ����鏈�����s����")]
    public bool playBGM = true; // BGM�Đ��̗L����Inspector�Ŏw��

    // �R�[���o�b�N��ݒ�\
    private System.Action onConversationEndCallback;

    private bool isTyping = false; // �^�C�s���O�����ǂ���
    private Coroutine typingCoroutine; // �R���[�`���̎Q�Ƃ�ۑ�����ϐ�

    [Header("�t�F�[�h�C���ݒ�")]
    public bool enableFadeIn = false; // �t�F�[�h�C����L���ɂ��邩�ǂ�����Inspector�Őݒ�
    private CanvasGroup fadeInCanvasGroup; // �t�F�[�h�C���p��CanvasGroup
    private float fadeInDuration = 1.6f;   // �t�F�[�h�C���̎��ԁi�b�j

    [Header("�J�n���Ԃ̐ݒ�")]
    public bool useLongStartDelay = false;  // ���߂̊J�n���Ԃ��g�p���邩�i�f�t�H���g��false�j
    private float defaultStartDelay = 0.1f; // �f�t�H���g�̊J�n�x�����ԁi�b�j
    private float longStartDelay = 1.8f;    // ���߂̊J�n�x�����ԁi�b�j

    private bool isCursorLocked = true; // �J�[�\�������b�N����Ă��邩�ǂ����̃t���O

    private KeyCode confirmKey; // �L�[�ݒ�p�̕ϐ�

    // ����ݒ�̃C���f�b�N�X
    private int languageIndex;

    [Header("Index 1:���j��, 2:�Ηj��, 3:���j��, 4:�ؗj��, 5:���j��, 6:�y�j��, 7:���j��")]
    public TMP_Text dayofWeekText;
    public int dayofWeekIndex = 1;

    [Header("Stage906_2�ł̂�True")]
    public bool saveStage116 = false;

    void Start()
    {
        // PlayerPrefs���猾��ݒ�̃C���f�b�N�X���擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g�l��0

        // �ÓI�I�u�W�F�N�g�Ɍ���ݒ��K�p
        ApplyLanguage(languageIndex);

        // �J�[�\�����\��
        LockCursor();

        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);

        // conversationText ���󔒂ɐݒ�
        conversationText.text = "";

        if (playBGM)
        {
            // ��b�V�[���p��BGM���Đ�
            SoundManager.instance.PlayConversationBGM();
        }

        // �uE�������Ď��ցv��Text���\����
        pressEText.gameObject.SetActive(false);

        
        switch (languageIndex)
        {
            case 0: // �p��
                pressEText.text = $"Press <size=90>{confirmKey}</size> to continue";
                break;
            case 1: // ���{��
                pressEText.text = $"<size=90>{confirmKey}</size> �������Ď���";
                break;
        }


        // �t�F�[�h�p��CanvasGroup���������i�������j
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f; // ���S�ɓ���
        }

        // �t�F�[�h�C��������ǉ�
        if (enableFadeIn)
        {
            // FadeInCanvas����WhiteImage���擾
            GameObject fadeInCanvas = GameObject.Find("FadeInCanvas");
            if (fadeInCanvas != null)
            {
                GameObject whiteImage = fadeInCanvas.transform.Find("WhiteImage")?.gameObject;
                if (whiteImage != null)
                {
                    fadeInCanvasGroup = whiteImage.GetComponent<CanvasGroup>();
                    if (fadeInCanvasGroup != null)
                    {
                        // ������ԂŊ��S�ɕs�����ɐݒ�
                        fadeInCanvasGroup.alpha = 1f;
                        // �t�F�[�h�C���R���[�`�����J�n
                        StartCoroutine(FadeIn());
                    }
                    else
                    {
                        Debug.LogError("WhiteImage��CanvasGroup�R���|�[�l���g��������܂���ł����B");
                    }
                }
                else
                {
                    Debug.LogError("FadeInCanvas����WhiteImage��������܂���ł����B");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvas��������܂���ł����B");
            }
        }
        else
        {
            // �t�F�[�h�C�����s��Ȃ��ꍇ�́AWhiteImage��alpha��0�ɐݒ�
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
                        Debug.LogError("WhiteImage��CanvasGroup�R���|�[�l���g��������܂���ł����B");
                    }
                }
                else
                {
                    Debug.LogError("FadeInCanvas����WhiteImage��������܂���ł����B");
                }
            }
            else
            {
                Debug.LogError("FadeInCanvas��������܂���ł����B");
            }
        }

        // �x�����Ԃ�I��
        float selectedDelay = useLongStartDelay ? longStartDelay : defaultStartDelay;

        // ��b��x�����ĊJ�n���A��b�I����̏�����ݒ�
        StartCoroutine(DelayWithAction(selectedDelay, () =>
        {
            StartConversation(() =>
            {
                LoadNextScene(); // ��b�I����Ɏ��̃V�[�������[�h
            });
        }));

        // �ڕW���z��ۑ�
        SaveTargetRevenue(); // �֐����Ăяo����targetRevenue��ۑ�
    }

    void Update()
    {
        // �v���C���[��E�L�[���������Ƃ��Ɏ��̃��b�Z�[�W��\��
        if (isConversing && Input.GetKeyDown(confirmKey))
        {
            if (!isTyping) // ���������ׂĕ\������Ă���Ύ���
            {
                DisplayNextMessage();

                // �q���g�\���̃R���[�`�����~���A��\���ɂ���
                if (hintCoroutine != null)
                {
                    StopCoroutine(hintCoroutine);
                    pressEText.gameObject.SetActive(false);
                }
            }
            else
            {
                // �܂��^�C�s���O���̏ꍇ�A���b�Z�[�W��S�\��
                if (typingCoroutine != null) // �R���[�`�������s���̏ꍇ
                {
                    StopCoroutine(typingCoroutine); // ���̃R���[�`�����~
                }
                
                switch (languageIndex)
                {
                    case 0: // �p��
                        conversationText.text = conversationMessages_en[conversationIndex - 1]; // ���݂̑S����\��
                        break;
                    case 1: // ���{��
                        conversationText.text = conversationMessages[conversationIndex - 1]; // ���݂̑S����\��
                        break;
                }

                isTyping = false;

                // �^�C�s���O�I�����ɉ�b�����~
                SoundManager.instance.StopTypingSound();
            }

            // ���̃A�C�h�����Ԃ܂ōēx�R���[�`�����J�n
            if (hintCoroutine != null) // ���s���̃R���[�`�����~���ă��Z�b�g
            {
                StopCoroutine(hintCoroutine);
                hintCoroutine = null;
            }
            hintCoroutine = StartCoroutine(ShowHintAfterDelay());
        }
    }

    // ��b��i�߂�֐�
    private void DisplayNextMessage()
    {
        
        switch (languageIndex)
        {
            case 0: // �p��
                // �܂����b�Z�[�W���c���Ă���ꍇ
                if (conversationIndex < conversationMessages_en.Length)
                {
                    // ���ʉ��i���b�Z�[�W���j���Đ�
                    SoundManager.instance.PlayMessageSound();

                    // ���b�Z�[�W��1�������\������R���[�`�����J�n
                    typingCoroutine = StartCoroutine(TypeMessage(conversationMessages_en[conversationIndex])); // �R���[�`���̎Q�Ƃ�ۑ�
                    conversationIndex++;
                }
                else
                {
                    EndConversation(); // ��b�I��
                }
                break;
            case 1: // ���{��
                // �܂����b�Z�[�W���c���Ă���ꍇ
                if (conversationIndex < conversationMessages.Length)
                {
                    // ���ʉ��i���b�Z�[�W���j���Đ�
                    SoundManager.instance.PlayMessageSound();

                    // ���b�Z�[�W��1�������\������R���[�`�����J�n
                    typingCoroutine = StartCoroutine(TypeMessage(conversationMessages[conversationIndex])); // �R���[�`���̎Q�Ƃ�ۑ�
                    conversationIndex++;
                }
                else
                {
                    EndConversation(); // ��b�I��
                }
                break;
        }
    }

    // 1�������e�L�X�g��\������R���[�`��
    IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        conversationText.text = ""; // �e�L�X�g���N���A

        // �^�C�s���O�������[�v�Đ��J�n
        SoundManager.instance.PlayTypingSound(); // �V���ɒǉ��������\�b�h�Ń^�C�s���O�����Đ�

        foreach (char letter in message.ToCharArray())
        {
            conversationText.text += letter; // 1�������ǉ�
            yield return new WaitForSeconds(typingSpeed); // �w��̑��x�őҋ@
        }

        // �^�C�s���O�I�����ɉ����~
        SoundManager.instance.StopTypingSound();
        isTyping = false; // �^�C�s���O�I��
    }

    // ��b���I�������ۂ̏���
    private void EndConversation()
    {
        isConversing = false;
        conversationCanvas.SetActive(false); // ��b�p��Canvas���\��
        onConversationEndCallback?.Invoke(); // ��b�I�����̃R�[���o�b�N���Ăяo��
    }

    // ��b���J�n���郁�\�b�h�i�O��������Ăׂ�悤�ɂ���j
    public void StartConversation(System.Action callback = null)
    {
        conversationCanvas.SetActive(true); // Canvas��\��
        isConversing = true; // ��b���ɐݒ�
        onConversationEndCallback = callback; // ��b�I����̏�����ݒ�
        conversationIndex = 0; // ��b�̍ŏ��̃��b�Z�[�W����J�n
        DisplayNextMessage(); // �ŏ��̃��b�Z�[�W��\��

        // �q���g�\���̃R���[�`�����J�n
        hintCoroutine = StartCoroutine(ShowHintAfterDelay());
    }

    // �ڕW���z��ۑ�����֐�
    private void SaveTargetRevenue()
    {
        PlayerPrefs.SetFloat("TargetRevenue", targetRevenue); // PlayerPrefs�ɕۑ�
        PlayerPrefs.Save(); // �ۑ�
        Debug.Log("�ڕW���z���ۑ�����܂���: " + targetRevenue);
    }

    // ���̃V�[�������[�h����֐�
    public void LoadNextScene()
    {
        // ��b���ȗ�������b�V�[���ł���Stage916���Z�[�u
        if (saveStage116)
        {
            SaveManager.SaveGame(116, 0f);
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // �t�F�[�h�ƃV�[���J�ڂ��R���[�`���Ŏ��s
            StartCoroutine(FadeAndLoadScene(nextSceneName));
        }
        else
        {
            Debug.LogError("�V�[�������ݒ肳��Ă��܂���B");
        }
    }


    //�uE�������Ď��ցv�̃q���g�\���R���[�`��
    private IEnumerator ShowHintAfterDelay()
    {
        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(idleTimeBeforeHint);

        // Text��L�������A�_�ŃA�j���[�V�������J�n
        pressEText.gameObject.SetActive(true);
        float alpha = 0f;
        bool increasing = true;

        while (true)
        {
            // Alpha�l�����X�ɑ���������
            alpha += (increasing ? 0.02f : -0.02f);
            if (alpha >= 1f) increasing = false;
            else if (alpha <= 0f) increasing = true;

            // Alpha�l��Text�ɓK�p
            pressEText.color = new Color(pressEText.color.r, pressEText.color.g, pressEText.color.b, alpha);

            // ���̃t���[���܂őҋ@
            yield return new WaitForSeconds(0.05f);
        }
    }

    //�t�F�[�h�����p�̃R���[�`��
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // �t�F�[�h�A�E�g
        yield return StartCoroutine(Fade(1f));

        // �x��������
        yield return StartCoroutine(DelayWithAction(0.3f, null));

        // �V�[�������[�h
        SceneManager.LoadScene(sceneName);

        // �t�F�[�h�C��
        //yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha; // ���݂̃A���t�@�l���擾
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration); // Alpha����`���
            yield return null; // ���̃t���[���܂őҋ@
        }

        // �ŏI�I��Alpha�l��ڕW�l�ɐݒ�i��Ԍ덷�̉����j
        fadeCanvasGroup.alpha = targetAlpha;
    }

    // �ėp�I�Ȓx���֐�
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // �w�莞�Ԃ̒x��
        yield return new WaitForSeconds(delayTime);

        // �x����ɃR�[���o�b�N�Ƃ��ēn���ꂽ�A�N�V���������s
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

        // ���S�Ƀt�F�[�h�C�����I�������alpha��0�ɐݒ�
        fadeInCanvasGroup.alpha = 0f;
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
        Cursor.lockState = CursorLockMode.Confined;
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

    public void LoadKeySettings()
    {
        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
    }

    private void ApplyLanguage(int index)
    {
        // dayofWeekText �� null �Ȃ珈�����X�L�b�v
        if (dayofWeekText == null)
        {
            Debug.Log("dayofWeekText ���������ł��B�j���\�����X�L�b�v���܂��B");
            return;
        }

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
