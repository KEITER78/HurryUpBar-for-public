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
    //public SoundManager soundManager; // SoundManager �̎Q�Ƃ�����
    public Animator animator;  // Animator�R���|�[�l���g�̎Q�Ƃ�ǉ�

    public GameObject callIcon;           // �ďo���A�C�R���iUI��Image�I�u�W�F�N�g�j
    public Transform customer;            // �q�̈ʒu
    public Transform player;              // �v���C���[�̈ʒu
    public Transform PlayerCamera;        // PlayerCamera�̈ʒu
    public Transform neckBone; // Inspector�Ŏ�̃{�[�����w��, cf_s_head�����蓖�Ă�
    public bool isLookingAtPlayer = false; // �v���C���[�̕����������ǂ������Ǘ�
    public bool isRotationRestricted = false; // ��]�𐧌�����t���O
    private Vector3 currentLookAtPosition;  // ��̃{�[����IK���W
    public float moveSpeed = 1f;  // �ړ����x��ݒ�iInspector���璲���\�j
    public float rotationSpeed = 20f;  // �ړ����̉�]���x��ݒ�iInspector�Œ����\�j
    public float neckRotationSpeed = 10f; // Inspector�Œ����\�Ȏ�̉�]���x
    public float seatArrivalDelay = 1f;  // ���Ȃɒ����Ă���̒x�����ԁi�f�t�H���g1�b�j
    public float paymentDepartureDelay = 1.5f;  // ��v��A�ړ��J�n�܂ł̒x������
    public GameObject callCanvas; // CallCanvas��Inspector�ŃA�T�C������
    public float triggerDistance = 3f;    // �v���C���[���߂Â�������臒l
    public GameObject orderCanvas;        // �����E�B���h�E��Canvas
    public Image drinkImage;              // �h�����N�̉摜��\������UI
    public Sprite[] drinkSprites;         // �h�����N�摜�iSakeA, SakeB, SakeC�Ȃǂ̉摜�j
    public float[] drinkPrices;           // �e�h�����N�̉��i���i�[����z��

    public Sprite waitingIconSprite;      // �ҋ@���A�C�R���̉摜
    public Sprite callingIconSprite;      // �ďo���A�C�R���̉摜
    public Sprite callingIconSprite_en;      // �ďo���A�C�R���̉摜_en
    public Sprite completeMessageSprite;  // �񋟊������b�Z�[�W�̉摜�iInspector�Ŏw��j
    public Sprite completeMessageSprite_en;  // �񋟊������b�Z�[�W�̉摜�iInspector�Ŏw��j_en
    public Sprite failMessageSprite;      // �s��v���b�Z�[�W�̉摜�iInspector�Ŏw��j
    public Sprite failMessageSprite_en;      // �s��v���b�Z�[�W�̉摜�iInspector�Ŏw��j_en
    public Sprite orderMessageSprite;     // �uE�{�^���Œ������󂯂�v���b�Z�[�W�̉摜�iInspector�Ŏw��j
    public Sprite recheckMessageSprite;   // �ҋ@���ɕ\�������u�Ċm�F�v���b�Z�[�W�̉摜�iInspector�Ŏw��j
    public Sprite closeMessageSprite;     // �����E�B���h�E����郁�b�Z�[�W�̉摜�iInspector�Ŏw��j
    public Sprite reorderMessageSprite;   // �Ē������p�̃��b�Z�[�W�摜�iInspector�Ŏw��j
    public Sprite reorderMessageSprite_en;   // �Ē������p�̃��b�Z�[�W�摜�iInspector�Ŏw��j_en
    public Sprite paymentFailureSprite;   // ���ώ��s���b�Z�[�W
    public Sprite paymentSuccessSprite;   // ���ϐ������b�Z�[�W
    public Sprite paymentSuccessSprite_en;   // ���ϐ������b�Z�[�W_en

    // ���b�Z�[�W�\���p��Canvas��Image��2�ɕ�����
    public GameObject messageCanvas1;     // Order, Recheck, Close�p��Canvas
    public Image messageImage1;           // Order, Recheck, Close�p��Image
    public TMP_Text messageText1;         // Order, Recheck, Close�p��Text
    public GameObject messageCanvas2;     // Complete, Fail�p��Canvas
    public Image messageImage2;           // Complete, Fail�p��Image

    // ServeMessage��Sprite�ƕ\������Canvas�AImage��ǉ�
    public Sprite serveMessageSprite;
    public GameObject messageCanvas3;
    public Image messageImage3;
    public TMP_Text messageText3;

    public float idleDurationMin = 10f; // Idle��Ԃ̍ŏ��������ԁiInspector�Őݒ�j
    public float idleDurationMax = 20f; // Idle��Ԃ̍ő厝�����ԁiInspector�Őݒ�j
    public Sprite payIconSprite;  // ����v��Ԃ������A�C�R��
    public Sprite payIconSprite_en;  // ����v��Ԃ������A�C�R��_en
    public Sprite payMessageSprite; // ����v���ɕ\������郁�b�Z�[�W�̉摜

    // �`�[�p��Canvas�Ƃ��̒��ɂ���w�i�摜�Ƌ��z�e�L�X�g
    public GameObject receiptCanvas;   // �`�[Canvas
    public Image receiptBackground;    // �`�[�̔w�i�摜
    public TextMeshProUGUI amountText;            // ���z��\������e�L�X�g
    public Button confirmButton;            // �����z�����肷��{�^��

    public static float dailyRevenue = 0f; // �����̔������ێ�����ϐ��i�v���C���[�S�̂�1�����K�v�j, �S�Ă̋q�ŋ��L���邽�߂�static�ɂ���
    public TextMeshProUGUI revenueText;    // �������\������TextMeshPro�̎Q��

    private Sprite selectedDrink;         // �q���I�񂾃h�����N�i�ʂɊǗ��j
    private bool isOrderDisplayed = false;// �����E�B���h�E���\������Ă��邩�ǂ���
    private bool isDrinkSelected = false; // �h�����N���I���ς݂��ǂ���
    private bool isPlayerInRange = false; // �v���C���[��Trigger Distance���ɂ��邩�ǂ���
    private bool isWaiting = false;       // �ҋ@�����ǂ������Ǘ�
    private DrinkSelectionManager drinkSelectionManager;  // DrinkSelectionManager�̎Q�Ƃ�ێ�����ϐ�
    private bool isServeMessageDisplayed = false;  // ServeMessage�̕\����Ԃ��ʂɊǗ�
    private bool isIdle = false; // Idle��Ԃ��Ǘ�����t���O
    private List<string> drinkHistory = new List<string>(); // �h�����N�̗�����ۑ����郊�X�g
    private int idleCount = 0; // Idle��ԂɂȂ����񐔂��J�E���g
    private bool isPaying = false; // ����v��Ԃ��Ǘ�����t���O
    private bool isAwaitingPayment = false; // ����v��Ԃ��Ǘ�����t���O
    private bool isSelectedCustomer = false; // ���݂̋q���ǂ����������t���O
    private bool isReceiptDisplayed = false; // �`�[���\������Ă��邩�ǂ������Ǘ�
    private bool isCalling = false; // �ďo���ł��邩�������t���O
    private bool isPayingInProgress = false; // ����v���i�s�����ǂ����������t���O�i��v�I�����E�������čĂщ�v���邱�Ƃ�h���j

    // �������z��\�����邽�߂̕ϐ���UI�Q��
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
    private int billAmount = 0;  // �������z��ێ�����ϐ�

    // �V���ȃ����o�ϐ��Ƃ��ă��b�Z�[�W�\���p�̕ϐ���ǉ�
    public GameObject confirmationMessageCanvas; // �u�{���ɗ��������Ă܂����H�v���b�Z�[�W�pCanvas
    private bool isConfirmationMessageActive = false; // ���b�Z�[�W���\�������ǂ������Ǘ�

    // �x�������j���[�p�̕ϐ���ǉ�
    public GameObject paymentMenuCanvas;    // �x�����I�����j���[��Canvas
    public TextMeshProUGUI standardAmountText;  // ���K������\������e�L�X�g
    public TextMeshProUGUI billedAmountText;    // �������z��\������e�L�X�g
    public Button standardAmountButton;     // ���K������I������{�^��
    public Button billedAmountButton;       // �������z��I������{�^��

    // �s���x��\���ϐ��ƃo�[�̎Q��
    public float satisfactionLevel = 0f;   // �s���x (Inspector�ŏ����l�ݒ�\)
    public Image satisfactionBar;            // �s���x�o�[��Image�I�u�W�F�N�g (Inspector�Őݒ�)
    public Sprite normalSatisfactionBarSprite;    // �ʏ펞�̕s���x�o�[�̉摜
    public Sprite blinkingSatisfactionBarSprite;  // �_�Ŏ��̕s���x�o�[�̉摜
    public Image satisfactionBarBackground; // �s���x�o�[�w�i��Image�I�u�W�F�N�g
    public Sprite satisfactionBarBackgroundSprite; // �s���x�o�[�w�i�̉摜
    public Sprite satisfactionBarBackgroundSprite_en; // �s���x�o�[�w�i�̉摜_en
    private float satisfactionBlinkTimer = 0f;          // �_�ł̎c�莞�Ԃ��Ǘ�
    private Coroutine satisfactionBlinkCoroutine;       // �u�����N�p�̃R���[�`���Q��
    private float satisfactionBlinkInterval1 = 0.25f;     // �_�ł̊Ԋu�i�_�ŗp�摜�̎��ԁj
    private float satisfactionBlinkInterval2 = 0.4f;     // �_�ł̊Ԋu�i�ʏ�p�摜�̎��ԁj
    private float satisfactionBlinkDuration = 1.0f;     // �_�ł𑱂��鎞��
    private float previousSatisfactionLevel;            // �O�t���[���̕s���x��ێ�

    // Inspector�Őݒ�\�Ȍďo���Ɋւ��鎞�ԁA�������x�A�A�C�R���Ȃǂ�ǉ�
    public float callDecayStartTime = 10f; // �s���x���������n�߂鎞��
    public float fastCallDecayStartTime = 20f; // �s���x�������������n�߂鎞��
    public float callDecaySpeed = 1f; // �ʏ�̑����X�s�[�h
    public float fastCallDecaySpeed = 2f; // ������������X�s�[�h
    public Sprite decayingCallIcon; // �������n�܂������̃A�C�R��
    public Sprite fastDecayingCallIcon; // �����������鎞�̃A�C�R��
    public Sprite decayingCallIcon_en; // �������n�܂������̃A�C�R��_en
    public Sprite fastDecayingCallIcon_en; // �����������鎞�̃A�C�R��_en

    // �ďo���Ɋւ��鎞�Ԃ�ǐՂ���ϐ�
    private float callTimeElapsed = 0f;
    private bool isCallDecaying = false;
    private bool isFastCallDecaying = false;

    // Inspector�Őݒ�\�ȑҋ@���Ɋւ��鎞�ԁA�������x�A�A�C�R���Ȃǂ�ǉ�
    public float waitDecayStartTime = 15f; // �s���x���������n�߂鎞�� (�ҋ@��)
    public float fastWaitDecayStartTime = 25f; // �s���x�������������n�߂鎞�� (�ҋ@��)
    public float waitDecaySpeed = 0.8f; // �ʏ�̑����X�s�[�h (�ҋ@��)
    public float fastWaitDecaySpeed = 1.6f; // ������������X�s�[�h (�ҋ@��)
    public Sprite decayingWaitIcon; // �ҋ@���ő������n�܂������̃A�C�R��
    public Sprite fastDecayingWaitIcon; // �ҋ@���ő����������鎞�̃A�C�R��

    // �ҋ@���Ɋւ��鎞�Ԃ�ǐՂ���ϐ�
    private float waitTimeElapsed = 0f;
    private bool isWaitDecaying = false;
    private bool isFastWaitDecaying = false;

    public float awaitingPaymentDecayStartTime = 10f;  // �x�����҂����������n�߂鎞��
    public float fastAwaitingPaymentDecayStartTime = 20f;  // �x�����҂��������������n�߂鎞��
    public float awaitingPaymentDecaySpeed = 1f;  // �x�����҂��̒ʏ푝�����x
    public float fastAwaitingPaymentDecaySpeed = 2f;  // �x�����҂��̑����������x
    public Sprite decayingAwaitingPaymentIcon;  // �x�����҂��������̃A�C�R��
    public Sprite fastDecayingAwaitingPaymentIcon;  // �x�����҂������������̃A�C�R��
    public Sprite decayingAwaitingPaymentIcon_en;  // �x�����҂��������̃A�C�R��_en
    public Sprite fastDecayingAwaitingPaymentIcon_en;  // �x�����҂������������̃A�C�R��_en

    private float awaitingPaymentTimeElapsed = 0f;  // �x�����҂����Ԍo��
    private bool isAwaitingPaymentDecaying = false;  // �x�����҂��ʏ팸���t���O
    private bool isFastAwaitingPaymentDecaying = false;  // �x�����҂����������t���O

    public float drinkMatchSatisfactionIncrease = 10f;  // �h�����N��v���̕s���x������
    public float drinkMismatchSatisfactionDecrease = 15f;  // �h�����N�s��v���̕s���x������
    public float recheckSatisfactionDecrease = 5f; // �Ē����m�F���̕s���x������

    public Transform spawnPoint; // ������̈ʒu��Inspector����w��
    public float spawnDelay = 5f; // �X�|�[���܂ł̒x�����ԁiInspector�Őݒ�j
    public int idleMaxCount = 3; // Idle��Ԃ̍ő�񐔂�ݒ�
    private bool isSpawned = false; // �q���X�|�[���������ǂ����̃t���O
    private SeatManager seatManager; // SeatManager�ւ̎Q��
    private int selectedSeatIndex;    // �I�����ꂽ���Ȃ̃C���f�b�N�X

    public Canvas conversationCanvas; // ��b�p��Canvas�iInspector�Ŏw��j
    public Image conversationBackground; // ��b�E�B���h�E�̔w�i�iInspector�Ŏw��j
    public TextMeshProUGUI conversationText; // ��b���b�Z�[�W��\������TextMeshPro�iInspector�Ŏw��j
    public string[] conversationMessages; // ��b���e��ۑ�����z��
    private int conversationIndex = 0; // ��b�̌��݂̃C���f�b�N�X
    private bool isConversing = false; // ��b�����ǂ������Ǘ�����t���O
    private System.Action onConversationEndCallback; // ��b�I�����̃R�[���o�b�N
    private bool isTyping = false; // �^�C�s���O�����ǂ����������t���O
    public float typingSpeed = 0.05f; // ������1�������\�����鑬�x���w��iInspector����ݒ�\�j
    private bool isConversationJustStarted = false;
    private Coroutine typingCoroutine; // �R���[�`���̎Q�Ƃ�ۑ�����ϐ�

    private static Queue<CustomerCallIcon> waitingCustomers = new Queue<CustomerCallIcon>(); // �ҋ@���̋q���Ǘ�����L���[

    private bool wasRotationRestricted = false; // �O�t���[���̏�Ԃ��L�^

    // �ÓI�J�E���^�[��ǉ�
    public static int totalCustomers = 0;
    public static int completedCustomers = 0;

    // �Q�[���I�[�o�[�p��Canvas�Ɗ֘A����UI�v�f
    public GameObject gameOverCanvas; // �Q�[���I�[�o�[Canvas
    public Image gameOverBlackImage; // ��ʑS�̂𕢂������摜
    public TextMeshProUGUI gameOverText; // "�Q�[���I�[�o�[" �ƕ\������e�L�X�g
    public Button returnToMenuButton; // ���j���[�ɖ߂�{�^��
    private static bool isGameOver = false; // �Q�[���I�[�o�[�������������ǂ����̃t���O

    private float playerFieldOfViewAngle = 100f; // �v���C���[�̎��E�̊p�x�iInspector�Œ����\�j

    // �Q�[���I�[�o�[��A���j���[��ʂɖ߂�ۂ̃t�F�[�h����
    private CanvasGroup fadeCanvasGroup; // �t�F�[�h�p��CanvasGroup
    private float fadeDuration = 0.6f; // �t�F�[�h�̎���

    //�@�q�̉�]�����m���A���IK����̃o�O��}��
    private float previousYRotation = 0f;   // �O���Y����]
    public float rotationThreshold = 0.1f;   // �}��]�Ƃ݂Ȃ��p�x�̂������l�i�x�P�ʁj

    // �L�[�ݒ�p�̕ϐ�
    private KeyCode confirmKey;
    private KeyCode serveKey;

    // ����ݒ�̃C���f�b�N�X
    private int languageIndex;

    void Start()
    {
        // static �ϐ��̃��Z�b�g
        dailyRevenue = 0f;                         // ����������Z�b�g
        waitingCustomers.Clear();                  // �ҋ@���̋q�L���[���N���A
        //totalCustomers = 0;                        // ���q�������Z�b�g�i���ꂼ��̋q����������s����ƍŏI�I��totalCustomers��1�ɂȂ邩��폜�j
        completedCustomers = 0;                    // ���������q�������Z�b�g
        isGameOver = false;                        // �Q�[���I�[�o�[��Ԃ����Z�b�g

        isLookingAtPlayer = true;

        triggerDistance = 1.3f; // ����������ݒ�

        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        serveKey = (KeyCode)PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);

        // PlayerPrefs���猾��ݒ�̃C���f�b�N�X���擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g�l��0

        // �ÓI�I�u�W�F�N�g�Ɍ���ݒ��K�p
        ApplyLanguage(languageIndex);

        // Animator�R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();

        // ���s�A�j���[�V�������J�n
        animator.SetBool("isWalking", true);  // Walking�A�j���[�V�������Đ�

        // �q�̌����ڂ��\���ɂ���i�I�u�W�F�N�g���̂̓A�N�e�B�u�̂܂܁j
        HideCustomer();

        // CallCanvas����\���ɂ���
        callCanvas.SetActive(false);

        // �Ȃɒ����܂ł͋q��Idle��Ԃɐݒ�
        isIdle = true;

        // �w�莞�Ԍ�ɋq���X�|�[��������
        StartCoroutine(SpawnCustomerAfterDelay());

        seatManager = FindObjectOfType<SeatManager>();

        if (seatManager == null)
        {
            Debug.LogError("SeatManager��������܂���ł����B�V�[�����ɔz�u����Ă��邩�m�F���Ă��������B");
        }

        // ������Ԃł͒����E�B���h�E�ƃh�����N�摜�͔�\��
        orderCanvas.SetActive(false);
        drinkImage.gameObject.SetActive(false);

        // ���b�Z�[�W�p��Images����\��
        messageCanvas1.SetActive(false);
        messageCanvas2.SetActive(false);

        drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();

        if (drinkSelectionManager == null)
        {
            Debug.LogError("DrinkSelectionManager��������܂���ł����B�V�[�����ɔz�u����Ă��邩�m�F���Ă��������B");
        }

        // drinkSprites��drinkPrices�̒������m�F
        if (drinkSprites.Length != drinkPrices.Length)
        {
            Debug.LogError("drinkSprites �� drinkPrices �̗v�f������v���Ă��܂���B");
        }

        // confirmButton�ɃN���b�N���̏�����ǉ�
        confirmButton.onClick.AddListener(OnConfirmButtonClick);

        // �`�[Canvas���ŏ��͔�\���ɐݒ�
        receiptCanvas.SetActive(false);

        // ������\����0�~�ɏ�����
        UpdateRevenueDisplay(); // �����\����0�~�ɐݒ�

        // �������z�̊e���𒲐����邽�߂̃{�^���Ƀ��X�i�[��ǉ�
        plusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, 1, billThousandsText));
        minusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, -1, billThousandsText));
        plusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, 1, billHundredsText));
        minusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, -1, billHundredsText));
        plusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, 1, billTensText));
        minusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, -1, billTensText));
        plusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, 1, billOnesText));
        minusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, -1, billOnesText));

        // �������z�̏����\����ݒ�
        UpdateBillAmount();

        // ���b�Z�[�W�L�����o�X���ŏ��͔�\��
        confirmationMessageCanvas.SetActive(false);

        // �x�������j���[�L�����o�X���ŏ��͔�\��
        paymentMenuCanvas.SetActive(false);

        // �s���x�o�[��������
        UpdateSatisfactionBar();

        //SoundManager�̐ݒ肪����Ă��邩�m�F
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManager���ݒ肳��Ă��܂���B");
        //}

        //�Q�[������BGM���Đ�
        SoundManager.instance.PlayGameBGM();

        // ��b�p��Canvas�͍ŏ��͔�\���ɂ��Ă���
        conversationCanvas.gameObject.SetActive(false);

        // �q�̓o�^
        RegisterCustomer();

        // �O��̕s���x��������
        previousSatisfactionLevel = satisfactionLevel;

        // �Q�[���I�[�o�[Canvas�ƃ��j���[�ɖ߂�{�^�����A�N�e�B�u�ɐݒ�
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        if (returnToMenuButton != null)
        {
            returnToMenuButton.gameObject.SetActive(false);
            returnToMenuButton.onClick.AddListener(OnReturnToMenuButtonClicked);
        }

        // FadeCanvas����BlackImage�������������ACanvasGroup���擾
        GameObject fadeCanvas = GameObject.Find("FadeCanvas");
        if (fadeCanvas != null)
        {
            GameObject blackImage = fadeCanvas.transform.Find("BlackImage")?.gameObject;
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
                    Debug.LogError("BlackImage��CanvasGroup�R���|�[�l���g��������܂���ł����B");
                }
            }
            else
            {
                Debug.LogError("FadeCanvas����BlackImage��������܂���ł����B");
            }
        }
        else
        {
            Debug.LogError("FadeCanvas��������܂���ł����B");
        }

        // �f�o�b�O��totalCustomers��completeCustomers��\��
        Debug.Log("Total Customers: " + totalCustomers);
        Debug.Log("Completed Customers: " + completedCustomers);
    }

    void Update()
    {
        // CallCanvas���v���C���[�̕��֌�����
        Vector3 directionToCallCanvas = player.position - callCanvas.transform.position;
        directionToCallCanvas.y = 0; // Y���̉�]��h��
        callCanvas.transform.rotation = Quaternion.LookRotation(directionToCallCanvas);
        callCanvas.transform.Rotate(0, 180, 0); // �v���C���[�𐳂��������悤�ɉ�]

        // �v���C���[���߂Â����Ƃ��̃��b�Z�[�W�\������
        float distance = Vector3.Distance(player.position, customer.position);

        // �x������ԂłȂ��ꍇ�A�ʏ�̒�����ҋ@�������s��
        if (!isPaying && distance <= triggerDistance && !isPlayerInRange && !isRotationRestricted && IsClosestCustomer() && IsCustomerInPlayerView())
        {
            // �v���C���[���͈͂ɓ������Ƃ��Ƀ��b�Z�[�W��\��
            isPlayerInRange = true;

            // �ҋ@���Ȃ�Ċm�F���b�Z�[�W��\���A����ȊO�͒������b�Z�[�W��\��
            if (isWaiting)
            {
                switch (languageIndex)
                {
                    case 0: // �p��
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                        break;
                    case 1: // ���{��
                        ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������Ċm�F����");
                        break;
                }
            }
            else
            {

                switch (languageIndex)
                {
                    case 0: // �p��
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                        break;
                    case 1: // ���{��
                        ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������󂯂�");
                        break;
                }
            }
        }
        else if (!isPaying && (!IsCustomerInPlayerView() || distance > triggerDistance) && isPlayerInRange)
        {
            // �v���C���[���͈͂��o���Ƃ��Ƀ��b�Z�[�W���\��
            isPlayerInRange = false;
            HideMessageImage1();
        }

        // ����v��Ԃ̏ꍇ�̏���
        if (isPaying && distance <= triggerDistance && !isPlayerInRange && !isRotationRestricted && IsClosestCustomer() && IsCustomerInPlayerView())
        {
            // �v���C���[���͈͂ɓ������ꍇ�ɂ���v���b�Z�[�W��\��
            isPlayerInRange = true;

            switch (languageIndex)
            {
                case 0: // �p��
                    ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                    break;
                case 1: // ���{��
                    ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ă���v����");
                    break;
            }
        }
        else if (isPaying && (!IsCustomerInPlayerView() || distance > triggerDistance) && isPlayerInRange)
        {
            // �v���C���[���͈͂��o���Ƃ��Ƀ��b�Z�[�W���\��
            isPlayerInRange = false;
            HideMessageImage1();
        }

        // isRotationRestricted �� true �ɕς�����u�ԂɃ��b�Z�[�W���\���ɂ���
        if (!wasRotationRestricted && isRotationRestricted)
        {
            HideMessageImage1();
        }

        // isRotationRestricted �� false �ɕς�����u�ԂɃ��b�Z�[�W���ĕ\������
        if (wasRotationRestricted && !isRotationRestricted)
        {
            if (distance <= triggerDistance && IsClosestCustomer() && IsCustomerInPlayerView())  // �ł��߂��q���ǂ������m�F
            {
                // �v���C���[���͈͓��ł���΃��b�Z�[�W��\��
                isPlayerInRange = true;

                if (!isPaying)
                {
                    if (isWaiting)
                    {
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                                break;
                            case 1: // ���{��
                                ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������Ċm�F����");
                                break;
                        }
                    }
                    else
                    {
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                                break;
                            case 1: // ���{��
                                ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������󂯂�");
                                break;
                        }
                    }
                }
                else
                {
                    switch (languageIndex)
                    {
                        case 0: // �p��
                            ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                            break;
                        case 1: // ���{��
                            ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ă���v����");
                            break;
                    }
                }
            }
        }

        // ���݂̏�Ԃ��L�^
        wasRotationRestricted = isRotationRestricted;

        // ��b���łȂ��A���z�m�F���b�Z�[�W�\������E�{�^���������ꂽ�ꍇ�A���b�Z�[�W���\���ɂ��A���j���[��\��
        if (isConfirmationMessageActive && Input.GetKeyDown(confirmKey) && !isConversing)
        {
            // ���b�Z�[�W���\���ɂ���
            confirmationMessageCanvas.SetActive(false);

            // �x�������j���[��\��
            ShowPaymentMenu();
        }

        // �����E�B���h�E�\�����̏���
        if (Input.GetKeyDown(confirmKey) && !isGameOver && isOrderDisplayed)
        {
            SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

            // �����E�B���h�E���\������Ă���ꍇ�͕���
            CloseOrderCanvas();
        }

        // ���z�m�F���b�Z�[�W���\������Ă��Ȃ��A����b���łȂ��ꍇ�̂݁A����E�{�^���̏������s��
        else if (!isConfirmationMessageActive && !isConversing && !isRotationRestricted && IsClosestCustomer() && !isGameOver)
        {
            // �v���C���[��E�{�^�����������ꍇ�̏����iIdle��ԂłȂ��ꍇ�̂ݔ����j
            if (!isIdle && isPlayerInRange && Input.GetKeyDown(confirmKey) && IsCustomerInPlayerView())
            {
                // �����Ƀv���C���[�̈ړ��𖳌����i���_�ړ���OK�j
                DisablePlayerMovement();

                if (!isPaying)
                {
                    if (isOrderDisplayed)
                    {
                        //�@�\��ʂ̏ꏊ�ֈړ�������
                    }
                    else
                    {
                        // �h�����N�����I���̏ꍇ�̓����_���ɑI��
                        if (!isDrinkSelected)
                        {
                            SelectDrink();

                            SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

                            // �����E�B���h�E��\�����A�v���C���[�̈ړ��𖳌���
                            ShowOrderCanvas();
                        }
                        else
                        {
                            // ���ʉ��i���b�Z�[�W���j���Đ�
                            //SoundManager.instance.PlayMessageSound();

                            // �����ŉ�b���e��ݒ�                           
                            switch (languageIndex)
                            {
                                case 0: // �p��
                                    conversationMessages = new string[]
                                    {
                                        "You: \"May I ask for your order again?\"",   // �ŏ��ɕ\��������b
                                        "Customer: \"Uh, oh, yes...\""  // E�{�^���������Ǝ��ɕ\�������
                                    };
                                    break;
                                case 1: // ���{��
                                    conversationMessages = new string[]
                                    {
                                        "���Ȃ��u�ēx�����������f�����Ă���낵���ł����H�v",   // �ŏ��ɕ\��������b
                                        "���q����u���A�����͂��c�c�v"  // E�{�^���������Ǝ��ɕ\�������
                                    };
                                    break;
                            }

                            // �v���C���[�̈ړ��𖳌����i���_�ړ���OK�j
                            DisablePlayerMovement();

                            // ��b���J�n���A��b�I����ɏ��������s
                            StartConversation(() =>
                            {
                                // 0.25�b�x�����Ă��珈�����s��
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // �s���x�����������鏈����ǉ�
                                    satisfactionLevel += recheckSatisfactionDecrease;
                                    satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100); // �s���x��0����100�͈̔͂ɐ���
                                    UpdateSatisfactionBar();  // �s���x�o�[���X�V

                                    Debug.Log("�ēx�����𕷂������߁A�s���x���������܂����B");

                                    //�Ē����p�̃��b�Z�[�W�摜��\��                                   
                                    switch (languageIndex)
                                    {
                                        case 0: // �p��
                                            ShowMessageImage2(reorderMessageSprite_en);
                                            break;
                                        case 1: // ���{��
                                            ShowMessageImage2(reorderMessageSprite);
                                            break;
                                    }

                                    //SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

                                    // �����E�B���h�E��\�����A�v���C���[�̈ړ��𖳌���
                                    ShowOrderCanvas();
                                }));
                            });
                        }
                    }
                }

                // �v���C���[��E�{�^�����������ꍇ�̏����i����v��Ԃ̂Ƃ��j
                if (isPaying && !isPayingInProgress)
                {
                    SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

                    isPayingInProgress = true;
                    StopAwaitingPayment();
                    SetAsSelectedCustomer(); // ���̋q��I����Ԃɐݒ�
                    // ShowReceipt();  // �`�[��\������֐����Ăяo��


                    // �v���C���[�̈ړ��Ǝ��_�ړ����ꎞ�I�ɒ�~
                    DisablePlayerControl();

                    // �t���O�𗧂Ă�i�`�[���\������Ă��邱�Ƃ������j
                    isReceiptDisplayed = true;

                    // MessageCanvas1���\���ɂ���
                    HideMessageImage1();

                    // ���z�v�Z���֐��ōs��
                    float totalAmount = CalculateTotalAmount();

                    // ��b�̊J�n�i�x�������z�̊m�F�j
                    StartCoroutine(DelayWithAction(0.1f, () =>
                    {
                        // ���q����̉�b                       
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                conversationMessages = new string[]
                                {
                                    $"You: \"The total is {totalAmount} yen.\"",
                                    $"Customer: \"Alright, {totalAmount} yen.\""
                                    // $"���q����u���Ⴀ{(UnityEngine.Random.value > 0.5f ? "����" : "PoyPoy")}�ŁB�v"  // �����_���ŃJ�[�h���L���b�V����I��
                                };
                                break;
                            case 1: // ���{��
                                conversationMessages = new string[]
                                {
                                    $"���Ȃ��u����v{totalAmount}�~�ł��B�v",
                                    $"���q����u�͂��B{totalAmount}�~�ˁB�v"  // �������z��\��
                                    // $"���q����u���Ⴀ{(UnityEngine.Random.value > 0.5f ? "����" : "PoyPoy")}�ŁB�v"  // �����_���ŃJ�[�h���L���b�V����I��
                                };
                                break;
                        }

                        // ReceiptCanvas���\���ɂ���
                        // receiptCanvas.SetActive(false);

                        // ���b�Z�[�W�����Đ�
                        //SoundManager.instance.PlayMessageSound();

                        // ��b���J�n���A���̏����ֈڍs
                        StartConversation(() =>
                        {
                            // 0.5�b�̒x����ǉ�
                            StartCoroutine(DelayWithAction(0.5f, () =>
                            {
                                // �x���������Đ�
                                SoundManager.instance.PlayPaymentSound();

                                //���ϐ����̃��b�Z�[�W�摜��\��                               
                                switch (languageIndex)
                                {
                                    case 0: // �p��
                                        ShowMessageImage2(paymentSuccessSprite_en);
                                        break;
                                    case 1: // ���{��
                                        ShowMessageImage2(paymentSuccessSprite);
                                        break;
                                }

                                // ����������Z
                                dailyRevenue += totalAmount;
                                UpdateRevenueDisplay(); // ������̕\�����X�V

                                // �x�����āA�v���C���[�̉�b
                                StartCoroutine(DelayWithAction(1.5f, () =>
                                {
                                    // ���Ȃ��̉�b                                   
                                    switch (languageIndex)
                                    {
                                        case 0: // �p��
                                            conversationMessages = new string[]
                                            {
                                                "You: \"Thank you for your payment.\"",
                                                "You: \"We look forward to serving you again.\""
                                            };
                                            break;
                                        case 1: // ���{��
                                            conversationMessages = new string[]
                                            {
                                                "���Ȃ��u���x�������肪�Ƃ��������܂��B�v",
                                                "���Ȃ��u�܂��̂��z�������҂����Ă���܂��B�v"
                                            };
                                            break;
                                    }

                                    // ���b�Z�[�W�����Đ�
                                    //SoundManager.instance.PlayMessageSound();

                                    // ��b���J�n���A�I����Ɏ��̏��������s
                                    StartConversation(() =>
                                    {
                                        StartCoroutine(DelayWithAction(0.25f, () =>
                                        {
                                            // �t���O�����Z�b�g�i�`�[������ꂽ���Ƃ������j
                                            isReceiptDisplayed = false;

                                            // �v���C���[�̈ړ��Ǝ��_�ړ���L����
                                            EnablePlayerControl();

                                            // �x������Ɍڋq��Ȃ���o�����܂ňړ������鏈�����J�n
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

        // �v���C���[��Q�{�^�����������ꍇ�̏����iIdle��ԁA��b���łȂ��ꍇ�̂ݔ����j
        if (!isIdle && distance <= triggerDistance && Input.GetKeyDown(serveKey) && !isConversing && !isRotationRestricted && !isOrderDisplayed &&
            IsClosestCustomer() && !isGameOver && IsCustomerInPlayerView())
        {
            if (!isPaying)
            {
                // �v���C���[�������Ă���h�����N���m�F���Ĉ�v���`�F�b�N
                DrinkSelectionManager drinkSelectionManager = FindObjectOfType<DrinkSelectionManager>();
                if (drinkSelectionManager != null && (drinkSelectionManager.currentDrinks[0] != null || drinkSelectionManager.currentDrinks[1] != null))
                {
                    if (CheckDrinkMatch(drinkSelectionManager.currentDrinks))
                    {
                        // �ҋ@���̉���
                        StopWaiting();

                        // ���ʉ��i�������j���Đ�
                        SoundManager.instance.PlaySuccessSound();

                        // �񋟊������b�Z�[�W��\��                       
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                ShowMessageImage2(completeMessageSprite_en);
                                break;
                            case 1: // ���{��
                                ShowMessageImage2(completeMessageSprite);
                                break;
                        }

                        //RecheckMessage���\���ɂ���
                        HideMessageImage1();

                        // �q��Idle��Ԃɐݒ�
                        isIdle = true;

                        // Idle��Ԃ̉񐔂𑝉�
                        idleCount++;

                        // Idle��ԉ����̃R���[�`�����J�n
                        StartCoroutine(ResetIdleAfterDelay());

                        Debug.Log("�h�����N����v���܂����B");
                    }
                    else
                    {
                        // ���ʉ��i���b�Z�[�W���j���Đ�
                        //SoundManager.instance.PlayMessageSound();

                        // �����ŉ�b���e��ݒ�                        
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                conversationMessages = new string[]
                                {
                                    "Customer: \"Huh? This seems different \nfrom what I ordered...\"",   // �ŏ��ɕ\��������b
                                    "It looks like the order doesn't match." ,
                                    "Let's bring the correct drink."// E�{�^���������Ǝ��ɕ\�������
                                };
                                break;
                            case 1: // ���{��
                                conversationMessages = new string[]
                                {
                                    "���q����u����H�����������̂ƈႤ�悤�ȁc�c�v",   // �ŏ��ɕ\��������b
                                    "�ǂ���璍���ƈ�v���Ȃ��悤�ł��B" ,
                                    "�������h�����N�������Ă��܂��傤�B"// E�{�^���������Ǝ��ɕ\�������
                                };
                                break;
                        }

                        // �v���C���[�̈ړ��𖳌����i���_�ړ���OK�j
                        DisablePlayerMovement();

                        // ��b���J�n���A��b�I����ɏ��������s
                        StartConversation(() =>
                        {
                            // 0.25�b�x�����Ă��珈�����s��
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                satisfactionLevel += drinkMismatchSatisfactionDecrease;  // Inspector�Ŏw�肵���s���x�����ʂ��g�p
                                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100); // �s���x��0����100�͈̔͂ɐ���
                                UpdateSatisfactionBar();  // �s���x�o�[���X�V

                                // �s��v�̏ꍇ�A�s��v���b�Z�[�W��\��                               
                                switch (languageIndex)
                                {
                                    case 0: // �p��
                                        ShowMessageImage2(failMessageSprite_en);
                                        break;
                                    case 1: // ���{��
                                        ShowMessageImage2(failMessageSprite);
                                        break;
                                }

                                // ���ʉ��i���s���j���Đ�
                                //SoundManager.instance.PlayFailureSound();

                                EnablePlayerMovement();
                            }));
                        });
                    }
                }
                else
                {
                    Debug.Log("�h�����N�������Ă��܂���B");
                }
            }
        }
        // �v���C���[���h�����N�������Ă��āATriggerDistance���ɂ���ꍇ���AisWaiting��true�̏ꍇ
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

        // �v���C���[��X�{�^�����������ꍇ�̏���
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("���̋q�̃h�����N����:");
            float totalAmount = 0f;  // ���v���z��ێ�����ϐ�
            if (drinkHistory.Count > 0)
            {
                for (int i = 0; i < drinkHistory.Count; i++)
                {
                    string drinkName = drinkHistory[i];
                    Debug.Log((i + 1) + "�Ԗ�: " + drinkName);

                    // �h�����N���Ɋ�Â��ċ��z���擾
                    for (int j = 0; j < drinkSprites.Length; j++)
                    {
                        if (drinkSprites[j].name == drinkName)
                        {
                            totalAmount += drinkPrices[j];
                            break;
                        }
                    }
                }
                Debug.Log("���v���z: " + totalAmount + "�~");
            }
            else
            {
                Debug.Log("���������݂��܂���B");
            }

            // Idle��Ԃ̉񐔂�\��
            Debug.Log("���̋q�� " + idleCount + " ��Idle��ԂɂȂ�܂����B");
        }

        // �ďo���̏ꍇ�A�o�ߎ��ԂɊ�Â��ĕs���x�𑝉������鏈��
        if (isCalling)
        {
            // �o�ߎ��Ԃ𑝉�
            callTimeElapsed += Time.deltaTime;

            // �ʏ�̑������n�܂�^�C�~���O
            if (!isCallDecaying && callTimeElapsed >= callDecayStartTime)
            {
                // �s���x�̑������J�n���A�A�C�R����ύX
                isCallDecaying = true;
                switch (languageIndex)
                {
                    case 0: // �p��
                        callIcon.GetComponent<Image>().sprite = decayingCallIcon_en;
                        break;
                    case 1: // ���{��
                        callIcon.GetComponent<Image>().sprite = decayingCallIcon;
                        break;
                }

                // �ʏ�̓_�ł��J�n
                StopCoroutine("BlinkFastCallIcon"); // ���������̃R���[�`�����~
                StopCoroutine("BlinkNormalCallIcon"); // �ʏ푝���̃R���[�`�����ēx��~���Ă���
                StartCoroutine(BlinkNormalCallIcon(0.5f)); // �ʏ�̑����̓_�ł��J�n
            }

            // �����������n�܂�^�C�~���O
            if (!isFastCallDecaying && callTimeElapsed >= fastCallDecayStartTime)
            {
                // �����s���x�����������ԂɈڍs���A�A�C�R����ύX
                isFastCallDecaying = true;
                switch (languageIndex)
                {
                    case 0: // �p��
                        callIcon.GetComponent<Image>().sprite = fastDecayingCallIcon_en;
                        break;
                    case 1: // ���{��
                        callIcon.GetComponent<Image>().sprite = fastDecayingCallIcon;
                        break;
                }

                // �����_�ł��J�n
                StopCoroutine("BlinkNormalCallIcon"); // �ʏ푝���̃R���[�`�����~
                StartCoroutine(BlinkFastCallIcon(0.25f)); // ���������̓_�ł��J�n
            }

            // �s���x�̑�������
            if (isCallDecaying)
            {
                if (isFastCallDecaying)
                {
                    // ��������
                    satisfactionLevel += fastCallDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // �ʏ�̑��x�ő���
                    satisfactionLevel += callDecaySpeed * Time.deltaTime;
                }

                // �s���x��͈͓��ɐ���
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // �o�[�̍X�V
            }
        }

        if (isWaiting)
        {
            // �o�ߎ��Ԃ𑝉�
            waitTimeElapsed += Time.deltaTime;

            // �ʏ�̑������n�܂�^�C�~���O
            if (!isWaitDecaying && waitTimeElapsed >= waitDecayStartTime)
            {
                isWaitDecaying = true;
                callIcon.GetComponent<Image>().sprite = decayingWaitIcon; // �����A�C�R���ɕύX

                // �ʏ�̓_�ł��J�n
                StopCoroutine("BlinkFastWaitIcon"); // ���������̃R���[�`�������삵�Ă���Β�~
                StopCoroutine("BlinkNormalWaitIcon"); // �ʏ푝���̃R���[�`�����ēx��~���Ă���
                StartCoroutine(BlinkNormalWaitIcon(0.5f)); // �ʏ�̑����̓_�ł��J�n
            }

            // �����������n�܂�^�C�~���O
            if (!isFastWaitDecaying && waitTimeElapsed >= fastWaitDecayStartTime)
            {
                isFastWaitDecaying = true;
                callIcon.GetComponent<Image>().sprite = fastDecayingWaitIcon; // ���������A�C�R���ɕύX

                // �����_�ł��J�n
                StopCoroutine("BlinkNormalWaitIcon"); // �ʏ푝���̃R���[�`�������삵�Ă���Β�~
                StartCoroutine(BlinkFastWaitIcon(0.25f)); // ���������̓_�ł��J�n
            }

            // �s���x�̑�������
            if (isWaitDecaying)
            {
                if (isFastWaitDecaying)
                {
                    // ��������
                    satisfactionLevel += fastWaitDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // �ʏ�̑��x�ő���
                    satisfactionLevel += waitDecaySpeed * Time.deltaTime;
                }

                // �s���x��͈͓��ɐ���
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // �o�[�̍X�V
            }
        }

        if (isAwaitingPayment)
        {
            // �o�ߎ��Ԃ𑝉�
            awaitingPaymentTimeElapsed += Time.deltaTime;

            // �ʏ�̑������n�܂�^�C�~���O
            if (!isAwaitingPaymentDecaying && awaitingPaymentTimeElapsed >= awaitingPaymentDecayStartTime)
            {
                isAwaitingPaymentDecaying = true;
                switch (languageIndex)
                {
                    case 0: // �p��
                        callIcon.GetComponent<Image>().sprite = decayingAwaitingPaymentIcon_en;
                        break;
                    case 1: // ���{��
                        callIcon.GetComponent<Image>().sprite = decayingAwaitingPaymentIcon;
                        break;
                }

                // �ʏ�̓_�ł��J�n
                StopCoroutine("BlinkFastAwaitingPaymentIcon"); // ���������̃R���[�`�������삵�Ă���Β�~
                StopCoroutine("BlinkNormalAwaitingPaymentIcon"); // �ʏ푝���̃R���[�`�����ēx��~���Ă���
                StartCoroutine(BlinkNormalAwaitingPaymentIcon(0.5f)); // �ʏ�̑����̓_�ł��J�n
            }

            // �����������n�܂�^�C�~���O
            if (!isFastAwaitingPaymentDecaying && awaitingPaymentTimeElapsed >= fastAwaitingPaymentDecayStartTime)
            {
                isFastAwaitingPaymentDecaying = true;
                switch (languageIndex)
                {
                    case 0: // �p��
                        callIcon.GetComponent<Image>().sprite = fastDecayingAwaitingPaymentIcon_en;
                        break;
                    case 1: // ���{��
                        callIcon.GetComponent<Image>().sprite = fastDecayingAwaitingPaymentIcon;
                        break;
                }

                // �����_�ł��J�n
                StopCoroutine("BlinkNormalAwaitingPaymentIcon"); // �ʏ푝���̃R���[�`�������삵�Ă���Β�~
                StartCoroutine(BlinkFastAwaitingPaymentIcon(0.25f)); // ���������̓_�ł��J�n
            }

            // �s���x�̑�������
            if (isAwaitingPaymentDecaying)
            {
                if (isFastAwaitingPaymentDecaying)
                {
                    // ��������
                    satisfactionLevel += fastAwaitingPaymentDecaySpeed * Time.deltaTime;
                }
                else
                {
                    // �ʏ�̑��x�ő���
                    satisfactionLevel += awaitingPaymentDecaySpeed * Time.deltaTime;
                }

                // �s���x��͈͓��ɐ���
                satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
                UpdateSatisfactionBar(); // �o�[�̍X�V
            }
        }

        // �v���C���[��E�L�[���������Ƃ��Ɏ��̃��b�Z�[�W��\��
        if (isConversing && Input.GetKeyDown(confirmKey) && !isConversationJustStarted)
        {
            if (!isTyping)
            {
                DisplayNextMessage();
            }
            else
            {
                // �܂��^�C�s���O���̏ꍇ�A���b�Z�[�W��S�\��
                if (typingCoroutine != null) // �R���[�`�������s���̏ꍇ
                {
                    StopCoroutine(typingCoroutine); // ���̃R���[�`�����~
                }
                conversationText.text = conversationMessages[conversationIndex - 1];
                isTyping = false;
                SoundManager.instance.StopTypingSound();
            }
        }

        // �s���x�̕ω������o
        if (satisfactionLevel != previousSatisfactionLevel)
        {
            // �^�C�}�[�����Z�b�g
            satisfactionBlinkTimer = satisfactionBlinkDuration;

            // �u�����N�R���[�`���������Ă��Ȃ��ꍇ�͊J�n
            if (satisfactionBlinkCoroutine == null)
            {
                satisfactionBlinkCoroutine = StartCoroutine(BlinkSatisfactionBar());
            }
        }

        // �^�C�}�[�̍X�V
        if (satisfactionBlinkTimer > 0)
        {
            satisfactionBlinkTimer -= Time.deltaTime;
        }
        else if (satisfactionBlinkCoroutine != null)
        {
            // �^�C�}�[��0�ɂȂ�����u�����N���~
            StopCoroutine(satisfactionBlinkCoroutine);
            satisfactionBlinkCoroutine = null;

            // �ʏ�̉摜�ɖ߂�
            satisfactionBar.sprite = normalSatisfactionBarSprite;
        }

        // �O��̕s���x���X�V
        previousSatisfactionLevel = satisfactionLevel;

        // SatisfactionLevel��100�ɒB������Q�[���I�[�o�[
        if (!isGameOver && satisfactionLevel >= 100f)
        {
            GameOver();
        }

        // �t���[���̍Ō�Ńt���O�����Z�b�g
        if (isConversationJustStarted)
        {
            isConversationJustStarted = false;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (neckBone != null && PlayerCamera != null)
        {
            // ��]���o�ƃ��Z�b�g����
            DetectAndResetForRapidRotation();

            float distanceToPlayer = Vector3.Distance(PlayerCamera.position, neckBone.position);

            // �v���C���[��triggerDistance���ɂ��Ȃ��ꍇ�́A���isRotationRestricted��true�ɐݒ�
            if (distanceToPlayer > triggerDistance)
            {
                isRotationRestricted = true;
            }
            else
            {
                // �v���C���[�̃J���������Ɍ������O���[�o���ȉ�]���v�Z
                Vector3 directionToPlayerCamera = (PlayerCamera.position - neckBone.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayerCamera);

                // �O���[�o���ȉ�]�����[�J�����W�n�ɕϊ�
                Quaternion predictedLocalRotation = Quaternion.Inverse(neckBone.parent.rotation) * lookRotation;
                float predictedYRotation = predictedLocalRotation.eulerAngles.y;

                // ���[�J��Y���̉�]��180�x�𒴂����ꍇ�̏���
                if (predictedYRotation > 180f)
                {
                    predictedYRotation -= 360f;
                }

                // Y���̉�]�� -65�x���� +65�x �͈̔͂Ɏ��܂��Ă���ꍇ
                if (predictedYRotation >= -65f && predictedYRotation <= 65f)
                {
                    isRotationRestricted = false; // ����������
                }
                else
                {
                    isRotationRestricted = true; // ������L����
                }
            }

            // ��������Ă��Ȃ��ꍇ�A�v���C���[�J�����̕��֎��������
            if (!isRotationRestricted && isLookingAtPlayer)
            {
                Vector3 targetPosition = PlayerCamera.position;
                if (currentLookAtPosition == Vector3.zero)
                {
                    currentLookAtPosition = targetPosition;
                }

                // �����x�N�g����Slerp���A�ŒZ��]�ŕ��
                Vector3 fromDir = (currentLookAtPosition - neckBone.position).normalized;
                Vector3 toDir = (targetPosition - neckBone.position).normalized;
                Vector3 newDir = Vector3.Slerp(fromDir, toDir, Time.deltaTime * neckRotationSpeed);
                float dist = (currentLookAtPosition - neckBone.position).magnitude;
                if (dist < 0.001f) dist = (targetPosition - neckBone.position).magnitude;
                currentLookAtPosition = neckBone.position + newDir * dist;

                animator.SetLookAtWeight(1.0f);
                animator.SetLookAtPosition(currentLookAtPosition);
            }
            // ��������Ă���ꍇ�A��𐳖ʂɌ�����
            else if (isRotationRestricted)
            {
                Vector3 targetForwardPosition = neckBone.position + neckBone.forward * 10f;
                if (currentLookAtPosition == Vector3.zero)
                {
                    currentLookAtPosition = targetForwardPosition;
                }

                // ���ʕ����ւ��ŒZ�o�H�ŕ��
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
                // isRotationRestricted��false�A����isLookingAtPlayer��false�̂Ƃ�����𐳖ʂ֖߂�
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

        // Y����]�̕ω��ʂ��v�Z
        float rotationDelta = Mathf.Abs(Mathf.DeltaAngle(previousYRotation, currentYRotation));

        // �}��]�����o
        if (rotationDelta > rotationThreshold)
        {
            // �}��]�������AcurrentLookAtPosition�����Z�b�g
            Vector3 resetForwardPosition = neckBone.position + customer.forward * 10f;
            currentLookAtPosition = new Vector3(resetForwardPosition.x, currentLookAtPosition.y, resetForwardPosition.z);

            Debug.Log("�}��]�����o���AcurrentLookAtPosition�����Z�b�g���܂����B");
        }

        // ���݂�Y����]��ۑ�
        previousYRotation = currentYRotation;
    }


    private void SelectDrink()
    {
        // �ʂ̋q���ƂɃ����_���Ńh�����N��I��
        selectedDrink = drinkSprites[UnityEngine.Random.Range(0, drinkSprites.Length)];
        isDrinkSelected = true;
        Debug.Log("�q�����������h�����N: " + selectedDrink.name);

        // �h�����N�𗚗��ɒǉ�
        drinkHistory.Add(selectedDrink.name);
    }

    private void ShowOrderCanvas()
    {
        // �����E�B���h�E�ƃh�����N�摜��\��
        orderCanvas.SetActive(true);
        drinkImage.gameObject.SetActive(true);
        drinkImage.sprite = selectedDrink; // �I�����ꂽ�h�����N��\��
        isOrderDisplayed = true;

        // �ҋ@���łȂ��Ȃ�A�ďo���̒�~
        if (!isWaiting)
        {
            StopCalling();
        }

        // �v���C���[�̈ړ��𖳌����i���_�ړ���OK�j
        DisablePlayerMovement();

        // CloseMessage��\��      
        switch (languageIndex)
        {
            case 0: // �p��
                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to close");
                break;
            case 1: // ���{��
                ShowMessageImage1($"<size=90>{confirmKey}</size> �������ĕ���");
                break;
        }
    }

    private void CloseOrderCanvas()
    {
        // �����E�B���h�E���\���ɂ���
        orderCanvas.SetActive(false);
        isOrderDisplayed = false;

        if (!isWaiting)
        {
            // �ҋ@���̊J�n
            StartWaiting();
        }

        // �v���C���[�̈ړ����ēx�L����
        EnablePlayerMovement();

        // �Ċm�F���b�Z�[�W��\��
        switch (languageIndex)
        {
            case 0: // �p��
                ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                break;
            case 1: // ���{��
                ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������Ċm�F����");
                break;
        }
    }

    // �h�����N��v���m�F���郁�\�b�h
    public bool CheckDrinkMatch(GameObject[] playerDrinks)
    {
        // �e�X�g�v���C���̏ꍇ�A�������ň�v�Ƃ���
        if (TestPlayManager.instance != null && TestPlayManager.instance.isTestPlay)
        {
            if (playerDrinks[1] != null) // 2�ڂ̃h�����N�����݂���ꍇ
            {
                Destroy(playerDrinks[1]); // 2�ڂ̃h�����N���폜
                playerDrinks[1] = null;
            }
            else if (playerDrinks[0] != null) // 1�ڂ̃h�����N�����݂���ꍇ
            {
                Destroy(playerDrinks[0]); // 1�ڂ̃h�����N���폜
                playerDrinks[0] = null;
            }
            else
            {
                return false; // �h�����N�������Ă��Ȃ��ꍇ��false
            }

            satisfactionLevel -= drinkMatchSatisfactionIncrease; // �s���x������
            satisfactionLevel = Mathf.Clamp(satisfactionLevel, 0, 100);
            UpdateSatisfactionBar();
            return true;
        }

        bool firstDrinkMatches = playerDrinks[0] != null && playerDrinks[0].name.Contains(selectedDrink.name);
        bool secondDrinkMatches = playerDrinks[1] != null && playerDrinks[1].name.Contains(selectedDrink.name);

        // �ǂ��炩�̃h�����N����v�����ꍇ
        if (firstDrinkMatches || secondDrinkMatches)
        {
            // 2�ڂ̃h�����N����v�����ꍇ
            if (secondDrinkMatches)
            {
                Destroy(playerDrinks[1]); // 2�ڂ̃h�����N���폜
                playerDrinks[1] = null;
            }

            // 1�ڂ̃h�����N����v�����ꍇ
            else if (firstDrinkMatches)
            {
                Destroy(playerDrinks[0]); // 1�ڂ̃h�����N���폜
                playerDrinks[0] = null;
            }

            // �s���x�����������鏈��
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

    // ���b�Z�[�W�摜1�iOrder, Recheck, Close�j��\���iIdle��ԂłȂ��ꍇ�̂݁j
    private void ShowMessageImage1(string message)
    {
        if (!isIdle && !isReceiptDisplayed && IsClosestCustomer()) // �A�C�h����Ԃł͂Ȃ��A���`�[���\������Ă��Ȃ��ꍇ�̂ݎ��s
        {
            // MessageImage1��Sprite�𓮓I�ɕύX
            //messageImage1.sprite = messageSprite;
            //messageImage1.color = new Color(1, 1, 1, 1); // �A���t�@�l�����Z�b�g

            // MessageText1��Sprite�𓮓I�ɕύX
            messageText1.text = message;
            messageText1.color = new Color(1, 1, 1, 1); // �A���t�@�l�����Z�b�g

            // MessageImage1��\��
            messageCanvas1.SetActive(true);
        }
    }

    // ���b�Z�[�W�摜1���\���ɂ���
    private void HideMessageImage1()
    {
        if (IsClosestCustomer())
        {
            messageCanvas1.SetActive(false);
        }
    }

    // ���b�Z�[�W�摜2�iComplete, Fail�j��\���iIdle��ԂłȂ��ꍇ�̂݁j
    private void ShowMessageImage2(Sprite messageSprite)
    {
        if (!isIdle)
        {
            // �����̃t�F�[�h�A�E�g�R���[�`�������삵�Ă���ꍇ�A��~������
            StopCoroutine(FadeOutMessageImage2());

            // MessageImage2��Sprite�𓮓I�ɕύX
            messageImage2.sprite = messageSprite;
            messageImage2.color = new Color(1, 1, 1, 1); // �A���t�@�l�����Z�b�g

            // MessageImage2��\��
            messageCanvas2.SetActive(true);

            // �t�F�[�h�A�E�g�R���[�`�����ăX�^�[�g
            StartCoroutine(FadeOutMessageImage2());
        }
    }

    // ���b�Z�[�W�摜2���t�F�[�h�A�E�g������R���[�`��
    private IEnumerator FadeOutMessageImage2()
    {
        // ��莞�Ԍ�Ƀt�F�[�h�A�E�g
        yield return new WaitForSeconds(2.0f); // 1.5�b�\��

        // �t�F�[�h�A�E�g����
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            messageImage2.color = new Color(messageImage2.color.r, messageImage2.color.g, messageImage2.color.b, alpha);
            yield return null;
        }

        // �摜���\��
        messageCanvas2.SetActive(false);
    }

    // �v���C���[�̈ړ��𖳌�������֐�
    private void DisablePlayerMovement()
    {
        // �ړ��𖳌������邪�A���_�ړ��͂��̂܂�
        player.GetComponent<FirstPersonMovement>().DisableMovement();
        player.GetComponent<FirstPersonMovement>().DisableLook(); // �����I�ɒǉ�
    }

    // �v���C���[�̈ړ���L��������֐�
    private void EnablePlayerMovement()
    {
        // �ړ����ēx�L����
        player.GetComponent<FirstPersonMovement>().EnableMovement();
        player.GetComponent<FirstPersonMovement>().EnableLook(); // �����I�ɒǉ�
    }

    // �v���C���[�̈ړ��Ǝ��_�ړ��𗼕�����������֐�
    private void DisablePlayerControl()
    {
        // �ړ��𖳌������邪�A���_�ړ��͂��̂܂�
        player.GetComponent<FirstPersonMovement>().DisableMovement();
        player.GetComponent<FirstPersonMovement>().DisableLook();
    }

    // �v���C���[�̈ړ��Ǝ��_�ړ��𗼕��L��������֐�
    private void EnablePlayerControl()
    {
        // �ړ����ēx�L����
        player.GetComponent<FirstPersonMovement>().EnableMovement();
        player.GetComponent<FirstPersonMovement>().EnableLook();
    }

    // ServeMessage��\���iIdle��ԂłȂ��ꍇ�̂݁j
    private void ShowServeMessage()
    {
        if (!isIdle && !isServeMessageDisplayed)
        {
            switch (languageIndex)
            {
                case 0: // �p��
                    messageText3.text = $"Press <size=90>{serveKey}</size> to serve";
                    break;
                case 1: // ���{��
                    messageText3.text = $"<size=90>{serveKey}</size> �������Ē�";
                    break;
            }

            messageText3.color = new Color(1, 1, 1, 1); // �A���t�@�l�����Z�b�g
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

    // Idle��Ԃɂ��Ă����莞�Ԍo�ߌ��isIdle��False�ɖ߂��R���[�`��
    private IEnumerator ResetIdleAfterDelay()
    {
        // 10�b����20�b�̊ԂŃ����_���Ȏ��Ԃ�ҋ@
        float idleTime = UnityEngine.Random.Range(idleDurationMin, idleDurationMax);
        yield return new WaitForSeconds(idleTime);

        // Idle��Ԃ��ő�񐔂ɒB�����ꍇ�͂���v��ԂɈڍs
        if (idleCount >= idleMaxCount)
        {
            isIdle = false;
            isPaying = true; // ����v��Ԃɐݒ�

            // ����v��ԂɂȂ������Ƀv���C���[��triggerDistance���ɂ���ꍇ�A���b�Z�[�W��\��
            float currentDistance = Vector3.Distance(player.position, customer.position);
            if (currentDistance <= triggerDistance && !isRotationRestricted && IsClosestCustomer())
            {
                switch (languageIndex)
                {
                    case 0: // �p��
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to process the payment");
                        break;
                    case 1: // ���{��
                        ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ă���v����");
                        break;
                }
            }

            StartAwaitingPayment(); // �x�����҂���Ԃ��J�n�iisAwaitingPayment��True�ɂȂ�j
            Debug.Log("����v��Ԃɐi�݂܂����B");
        }
        else
        {
            // Idle��Ԃ��������A�ʏ�̌Ăяo����Ԃɖ߂�
            isIdle = false;

            // �ďo���̊J�n
            StartCalling();

            // �h�����N���I����Ԃɖ߂�
            isDrinkSelected = false;

            Debug.Log("������t��Ԃɖ߂�܂����B");
        }
    }

    // �`�[��\������֐�
    private void ShowReceipt()
    {
        // �`�[Canvas��\��
        receiptCanvas.SetActive(true);

        // ���z���e�L�X�g�ɔ��f
        float totalAmount = CalculateTotalAmount(); // ���z�v�Z���֐��ōs��
        amountText.text = "���K����: " + totalAmount.ToString() + " �~";

        // �������z�̊e�������Z�b�g
        billThousands = 0;
        billHundreds = 0;
        billTens = 0;
        billOnes = 0;

        //�������z�����Z�b�g
        billAmount = 0;

        // UI�̕\�������Z�b�g
        billThousandsText.text = billThousands.ToString();
        billHundredsText.text = billHundreds.ToString();
        billTensText.text = billTens.ToString();
        billOnesText.text = billOnes.ToString();

        // �ēx�A�������z�̍��v���v�Z���� billAmount ���X�V
        UpdateBillAmount();

        // �{�^���̃��X�i�[���N���A���čēo�^
        ClearButtonListeners();

        // �v���C���[�̈ړ��Ǝ��_�ړ����ꎞ�I�ɒ�~
        DisablePlayerControl();

        // �t���O�𗧂Ă�i�`�[���\������Ă��邱�Ƃ������j
        isReceiptDisplayed = true;

        // MessageCanvas1���\���ɂ���
        HideMessageImage1();
    }

    // ���̋q��I����Ԃɐݒ肷��֐�
    private void SetAsSelectedCustomer()
    {
        // ���̂��ׂĂ̋q�̑I����Ԃ�����
        CustomerCallIcon[] allCustomers = FindObjectsOfType<CustomerCallIcon>();
        foreach (CustomerCallIcon customer in allCustomers)
        {
            customer.isSelectedCustomer = false;
        }

        // ���̋q������I����Ԃɂ���
        isSelectedCustomer = true;
    }

    // �������z�����肳�ꂽ�Ƃ��̏���
    private void OnConfirmButtonClick()
    {
        // �I������Ă��Ȃ��q�͏������Ȃ�
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        // ���v���z���v�Z
        float totalAmount = CalculateTotalAmount();

        // �x�����m�����v�Z
        bool willPay = CalculatePaymentProbability(totalAmount, billAmount);

        // �q���x�����������ꍇ�A����������Z���A�`�[Canvas���\���ɂ���
        if (willPay)
        {
            // ��b�̊J�n�i�x�������z�̊m�F�j
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // ���q����̉�b
                conversationMessages = new string[]
                {
                    $"���q����u�͂��B{billAmount}�~�ˁB�v",  // �������z��\��
                    $"���q����u���Ⴀ{(UnityEngine.Random.value > 0.5f ? "�J�[�h" : "�L���b�V��")}�ŁB�v"  // �����_���ŃJ�[�h���L���b�V����I��
                };

                // ReceiptCanvas���\���ɂ���
                receiptCanvas.SetActive(false);

                // ���b�Z�[�W�����Đ�
                //SoundManager.instance.PlayMessageSound();

                // ��b���J�n���A���̏����ֈڍs
                StartConversation(() =>
                {
                    // 0.5�b�̒x����ǉ�
                    StartCoroutine(DelayWithAction(0.5f, () =>
                    {
                        // �x���������Đ�
                        SoundManager.instance.PlayPaymentSound();

                        //���ϐ����̃��b�Z�[�W�摜��\��
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                ShowMessageImage2(paymentSuccessSprite_en);
                                break;
                            case 1: // ���{��
                                ShowMessageImage2(paymentSuccessSprite);
                                break;
                        }

                        // ����������Z
                        dailyRevenue += billAmount;
                        UpdateRevenueDisplay(); // ������̕\�����X�V

                        // �x�����āA�v���C���[�̉�b
                        StartCoroutine(DelayWithAction(1.5f, () =>
                        {
                            // ���Ȃ��̉�b
                            conversationMessages = new string[]
                            {
                                "���Ȃ��u���x�������肪�Ƃ��������܂��B�v",
                                "���Ȃ��u�܂��̂��z�������҂����Ă���܂��B�v"
                            };

                            // ���b�Z�[�W�����Đ�
                            //SoundManager.instance.PlayMessageSound();

                            // ��b���J�n���A�I����Ɏ��̏��������s
                            StartConversation(() =>
                            {
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // �t���O�����Z�b�g�i�`�[������ꂽ���Ƃ������j
                                    isReceiptDisplayed = false;

                                    // �v���C���[�̈ړ��Ǝ��_�ړ���L����
                                    EnablePlayerControl();

                                    // �x������Ɍڋq��Ȃ���o�����܂ňړ������鏈�����J�n
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
            // �x���������ۂ��ꂽ�ꍇ�A�m�F���b�Z�[�W��\������
            OnPaymentRefused();
        }

        // �f�o�b�O�Ɍ��ʂ�\��
        Debug.Log("�������z: " + billAmount + " �~");
        Debug.Log(willPay ? "�q�͎x�����܂����B" : "�q�͎x���������ۂ��܂����B");
    }

    // ���z���v�Z����֐�
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

    // �x�����m�����v�Z���A�x�����̉ۂ�Ԃ��֐�
    private bool CalculatePaymentProbability(float totalAmount, float billAmount)
    {
        // �������z�����K�����ȉ��̏ꍇ�A�K���x����
        if (billAmount <= totalAmount)
        {
            return true; // �x����
        }
        // �������z�����K������2�{�ȉ��̏ꍇ�A80%�̊m���Ŏx����
        else if (billAmount <= 2 * totalAmount)
        {
            return UnityEngine.Random.value <= 0.9f * ((100f - satisfactionLevel) / 100f); // 90���~(100 - �s���x)�i���j
        }
        // ����ȊO�̏ꍇ�A(totalAmount / billAmount) �~ 1.8 �~ 100%�̊m���Ŏx����
        else
        {
            float probability = (totalAmount / billAmount) * 1.8f * ((100f - satisfactionLevel) / 100f); // �s���x���l��
            return UnityEngine.Random.value <= probability; // �v�Z�����m���Ŏx����
        }
    }

    // ������̕\�����X�V����֐�
    private void UpdateRevenueDisplay()
    {
        // �������TextMeshPro�ɕ\��      
        switch (languageIndex)
        {
            case 0: // �p��
                revenueText.text = "Sales: " + Mathf.FloorToInt(dailyRevenue).ToString() + " yen";
                break;
            case 1: // ���{��
                revenueText.text = "�{���̔���: " + Mathf.FloorToInt(dailyRevenue).ToString() + " �~";
                break;
        }
    }

    // �������z�̊e���𒲐����邽�߂̊֐�
    private void AdjustBillDigit(ref int digit, int increment, TextMeshProUGUI text)
    {
        // ���̒l��0�̂Ƃ���-�{�^����������9�ɁA9�̂Ƃ���+�{�^����������0�ɂȂ�
        digit = (digit + increment + 10) % 10;

        // ���������l��UI�ɔ��f
        text.text = digit.ToString();

        // �S�̂̐������z���X�V
        UpdateBillAmount();
    }

    // �������z���X�V���ĕ\������֐�
    private void UpdateBillAmount()
    {
        // �e���̒l�����ɐ������z���v�Z
        billAmount = (billThousands * 1000) + (billHundreds * 100) + (billTens * 10) + billOnes;

        // �e���̕\���͒��ڃ{�^���ő��삳���̂ŁA�����ŕ\���͕s�v
        Debug.Log("�������z: " + billAmount + " �~");
    }

    // �x���������ۂ��ꂽ�Ƃ��̏���
    private void OnPaymentRefused()
    {
        // ���V�[�g�L�����o�X�����
        receiptCanvas.SetActive(false);

        // ���ʉ��i���b�Z�[�W���j���Đ�
        //SoundManager.instance.PlayMessageSound();

        // �����ŉ�b���e��ݒ�
        conversationMessages = new string[]
        {
                                "���q����u���A�{���ɂ��̒l�i�ō����Ă܂����H�v",   // �ŏ��ɕ\��������b
                                "���Ȃ��u���[���Ɓc�c�v"  // E�{�^���������Ǝ��ɕ\�������
        };

        // ��b���J�n���A��b�I����ɏ��������s
        StartConversation(() =>
        {
            // 0.25�b�x�����Ă��珈�����s��
            StartCoroutine(DelayWithAction(0.25f, () =>
            {
                // �x�������j���[��\��
                ShowPaymentMenu();
            }));
        });
    }

    // �x�������j���[��\������֐�
    private void ShowPaymentMenu()
    {
        // ���j���[�L�����o�X��\��
        paymentMenuCanvas.SetActive(true);

        // ���K�����Ɛ������z��\��
        float totalAmount = CalculateTotalAmount();
        standardAmountText.text = "���K����: " + totalAmount.ToString() + " �~";
        billedAmountText.text = "�������z: " + billAmount.ToString() + " �~";

        // �{�^���Ƀ��X�i�[��ǉ�
        standardAmountButton.onClick.AddListener(OnStandardAmountSelected);
        billedAmountButton.onClick.AddListener(OnBilledAmountSelected);
    }

    // ���K�������I�����ꂽ�Ƃ��̏���
    private void OnStandardAmountSelected()
    {
        // �I������Ă��Ȃ��q�͏������Ȃ�
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        //���K�������Z�o
        float totalAmount = CalculateTotalAmount();

        // ��b�̊J�n�i�x�������z�̊m�F�j
        StartCoroutine(DelayWithAction(0.1f, () =>
        {
            // ���q����̉�b
            conversationMessages = new string[]
            {
                $"���Ȃ��u���炵�܂����B���z���Ԉ���Ă��܂����B",
                $"���Ȃ��u��������{totalAmount}�~�ł��B",
                $"���q����u�Ȃ񂾁B{totalAmount}�~�ˁB�v",  // �������z��\��
                $"���q����u���Ⴀ{(UnityEngine.Random.value > 0.5f ? "�J�[�h" : "�L���b�V��")}�ŁB�v"  // �����_���ŃJ�[�h���L���b�V����I��
            };

            // ���j���[�����
            paymentMenuCanvas.SetActive(false);

            // ���b�Z�[�W�����Đ�
            //SoundManager.instance.PlayMessageSound();

            // ��b���J�n���A���̏����ֈڍs
            StartConversation(() =>
            {
                // 0.5�b�̒x����ǉ�
                StartCoroutine(DelayWithAction(0.5f, () =>
                {
                    // �x���������Đ�
                    SoundManager.instance.PlayPaymentSound();

                    //���ϐ����̃��b�Z�[�W�摜��\��
                    switch (languageIndex)
                    {
                        case 0: // �p��
                            ShowMessageImage2(paymentSuccessSprite_en);
                            break;
                        case 1: // ���{��
                            ShowMessageImage2(paymentSuccessSprite);
                            break;
                    }

                    // ����������Z
                    dailyRevenue += totalAmount;
                    UpdateRevenueDisplay(); // ������̕\�����X�V

                    // �x�����āA�v���C���[�̉�b
                    StartCoroutine(DelayWithAction(1.5f, () =>
                    {
                        // ���Ȃ��̉�b
                        conversationMessages = new string[]
                        {
                                "���Ȃ��u���x�������肪�Ƃ��������܂��I�v",
                                "���Ȃ��u�܂��̂��z�������҂����Ă���܂��B�v"
                        };

                        // ���b�Z�[�W�����Đ�
                        //SoundManager.instance.PlayMessageSound();

                        // ��b���J�n���A�I����Ɏ��̏��������s
                        StartConversation(() =>
                        {
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                // �t���O�����Z�b�g�i�`�[������ꂽ���Ƃ������j
                                isReceiptDisplayed = false;

                                // �v���C���[�̈ړ��Ǝ��_�ړ���L����
                                EnablePlayerControl();

                                // �x������Ɍڋq��Ȃ���o�����܂ňړ������鏈�����J�n
                                StartCoroutine(MoveToSpawnAfterPayment());
                            }));
                        });
                    }));
                }));
            });
        }));
    }

    // �������z���I�����ꂽ�Ƃ��̏���
    private void OnBilledAmountSelected()
    {
        // �I������Ă��Ȃ��q�͏������Ȃ�
        if (!isSelectedCustomer)
        {
            return;
        }

        //SoundManager.instance.PlayDecisionSound();  // ���艹���Đ�

        // �x�����m�����v�Z (totalAmount / billAmount)
        float totalAmount = CalculateTotalAmount();
        float paymentProbability = totalAmount / billAmount;

        // �m���Ŏx�������s���邩����
        if (UnityEngine.Random.value <= paymentProbability)
        {
            // ��b�̊J�n�i�x�������z�̊m�F�j
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // ���q����̉�b
                conversationMessages = new string[]
                {
                $"���Ȃ��u�����B������{billAmount}�~�ō����Ă���܂��B",
                $"���q����u�����H{billAmount}�~������́H�v",  // �������z��\��
                $"���q����u�͂��B���Ⴀ{(UnityEngine.Random.value > 0.5f ? "�J�[�h" : "�L���b�V��")}�ŁB�v"  // �����_���ŃJ�[�h���L���b�V����I��
                };

                // ���j���[�����
                paymentMenuCanvas.SetActive(false);

                // ���b�Z�[�W�����Đ�
                //SoundManager.instance.PlayMessageSound();

                // ��b���J�n���A���̏����ֈڍs
                StartConversation(() =>
                {
                    // 0.5�b�̒x����ǉ�
                    StartCoroutine(DelayWithAction(0.5f, () =>
                    {
                        // �x���������Đ�
                        SoundManager.instance.PlayPaymentSound();

                        //���ϐ����̃��b�Z�[�W�摜��\��
                        switch (languageIndex)
                        {
                            case 0: // �p��
                                ShowMessageImage2(paymentSuccessSprite_en);
                                break;
                            case 1: // ���{��
                                ShowMessageImage2(paymentSuccessSprite);
                                break;
                        }

                        // ����������Z
                        dailyRevenue += billAmount;
                        UpdateRevenueDisplay(); // ������̕\�����X�V

                        // �x�����āA�v���C���[�̉�b
                        StartCoroutine(DelayWithAction(1.5f, () =>
                        {
                            // ���Ȃ��̉�b
                            conversationMessages = new string[]
                            {
                                "���Ȃ��u���x�������肪�Ƃ��������܂��I�v",
                                "���Ȃ��u�܂��̂��z�������҂����Ă���܂��B�v"
                            };

                            // ���b�Z�[�W�����Đ�
                            //SoundManager.instance.PlayMessageSound();

                            // ��b���J�n���A�I����Ɏ��̏��������s
                            StartConversation(() =>
                            {
                                StartCoroutine(DelayWithAction(0.25f, () =>
                                {
                                    // �t���O�����Z�b�g�i�`�[������ꂽ���Ƃ������j
                                    isReceiptDisplayed = false;

                                    // �v���C���[�̈ړ��Ǝ��_�ړ���L����
                                    EnablePlayerControl();

                                    // �x������Ɍڋq��Ȃ���o�����܂ňړ������鏈�����J�n
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
            // ��b�̊J�n�i�x�������z�̊m�F�j
            StartCoroutine(DelayWithAction(0.1f, () =>
            {
                // ���q����̉�b
                conversationMessages = new string[]
                {
                $"���Ȃ��u�����B������{billAmount}�~�ō����Ă���܂��B",
                $"���q����u�����H{billAmount}�~������́H�v",  // �������z��\��
                $"���q����u��΂���������B����Ȃ̂ڂ��������I�v",
                $"���q����u���A����Ȃ�����I�I�v"
                };

                // ���j���[�����
                paymentMenuCanvas.SetActive(false);

                // ���b�Z�[�W�����Đ�
                //SoundManager.instance.PlayMessageSound();

                // ��b���J�n���A���̏����ֈڍs
                StartConversation(() =>
                {
                    // 0.3�b�̒x����ǉ�
                    StartCoroutine(DelayWithAction(0.3f, () =>
                    {
                        // ���ʉ��i���s���j���Đ�
                        SoundManager.instance.PlayFailureSound();

                        // 0.5�b�̒x����ǉ�
                        StartCoroutine(DelayWithAction(0.5f, () =>
                        {
                            //���ώ��s�̃��b�Z�[�W�摜��\��
                            ShowMessageImage2(paymentFailureSprite);

                            paymentDepartureDelay = 0f; //�����ɐȂ𗧂悤�ɕύX
                            moveSpeed = 2.25f; // �ړ����x��ύX
                            animator.speed = 1.5f; // �A�j���[�V�������x��ύX

                            // �x������Ɍڋq��Ȃ���o�����܂ňړ������鏈�����J�n
                            StartCoroutine(MoveToSpawnAfterPayment());

                            // �v���C���[�̎��_�ړ��̂ݗL����
                            EnablePlayerControl();
                            DisablePlayerMovement();

                            // �x�����āA�v���C���[�̉�b
                            StartCoroutine(DelayWithAction(0.25f, () =>
                            {
                                // ���Ȃ��̉�b
                                conversationMessages = new string[]
                                {
                                    "���Ȃ��u����A������Ƒ҂��āI�I�v"
                                };

                                // ���b�Z�[�W�����Đ�
                                //SoundManager.instance.PlayMessageSound();

                                // ��b���J�n���A�I����Ɏ��̏��������s
                                StartConversation(() =>
                                {
                                    StartCoroutine(DelayWithAction(0.25f, () =>
                                    {
                                        // �t���O�����Z�b�g�i�`�[������ꂽ���Ƃ������j
                                        isReceiptDisplayed = false;

                                        // �v���C���[�̈ړ��Ǝ��_�ړ���L����
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

    // �{�^�����X�i�[���N���A���čēo�^����֐�
    private void ClearButtonListeners()
    {
        // �{�^���̃��X�i�[���N���A
        plusThousandsButton.onClick.RemoveAllListeners();
        minusThousandsButton.onClick.RemoveAllListeners();
        plusHundredsButton.onClick.RemoveAllListeners();
        minusHundredsButton.onClick.RemoveAllListeners();
        plusTensButton.onClick.RemoveAllListeners();
        minusTensButton.onClick.RemoveAllListeners();
        plusOnesButton.onClick.RemoveAllListeners();
        minusOnesButton.onClick.RemoveAllListeners();

        // �V�������X�i�[��ǉ�
        plusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, 1, billThousandsText));
        minusThousandsButton.onClick.AddListener(() => AdjustBillDigit(ref billThousands, -1, billThousandsText));
        plusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, 1, billHundredsText));
        minusHundredsButton.onClick.AddListener(() => AdjustBillDigit(ref billHundreds, -1, billHundredsText));
        plusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, 1, billTensText));
        minusTensButton.onClick.AddListener(() => AdjustBillDigit(ref billTens, -1, billTensText));
        plusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, 1, billOnesText));
        minusOnesButton.onClick.AddListener(() => AdjustBillDigit(ref billOnes, -1, billOnesText));
    }

    // �s���x�o�[�̕\�����X�V����֐�
    private void UpdateSatisfactionBar()
    {
        // �s���x��0����1�͈̔͂ɐ��K�����A�o�[��fillAmount�ɔ��f
        satisfactionBar.fillAmount = satisfactionLevel / 100f;
    }

    private void StartCalling()
    {
        isCalling = true;
        callTimeElapsed = 0f; // �o�ߎ��Ԃ����Z�b�g
        isCallDecaying = false; // �ʏ�̌����t���O�����Z�b�g
        isFastCallDecaying = false; // ���������t���O�����Z�b�g

        // �ďo���A�C�R����ݒ�       
        switch (languageIndex)
        {
            case 0: // �p��
                callIcon.GetComponent<Image>().sprite = callingIconSprite_en;
                break;
            case 1: // ���{��
                callIcon.GetComponent<Image>().sprite = callingIconSprite;
                break;
        }
        callIcon.SetActive(true);
    }

    private void StopCalling()
    {
        isCalling = false;

        isCallDecaying = false; // �t���O�����Z�b�g
        isFastCallDecaying = false; // �t���O�����Z�b�g

        // �A�C�R���̓_�ł��~
        StopCoroutine("BlinkNormalCallIcon");
        StopCoroutine("BlinkFastCallIcon");

        callIcon.SetActive(false); // �ďo���A�C�R�����\��
    }

    private void StartWaiting()
    {
        // �ҋ@���̌o�ߎ��Ԃ����Z�b�g
        waitTimeElapsed = 0f;
        isWaitDecaying = false;
        isFastWaitDecaying = false;

        // �ҋ@���̃t���O��ݒ�
        isWaiting = true;

        // �ҋ@���A�C�R����\���i�ŏ��͒ʏ�̑ҋ@�A�C�R���j
        callIcon.GetComponent<Image>().sprite = waitingIconSprite;
        callIcon.SetActive(true);

        Debug.Log("�ҋ@�����J�n����܂����B");
    }

    private void StopWaiting()
    {
        // �ҋ@���̃t���O������
        isWaiting = false;

        isWaitDecaying = false; // �t���O�����Z�b�g
        isFastWaitDecaying = false; // �t���O�����Z�b�g

        // �A�C�R���̓_�ł��~
        StopCoroutine("BlinkNormalWaitIcon");
        StopCoroutine("BlinkFastWaitIcon");

        // �ҋ@���A�C�R�����\��
        callIcon.SetActive(false);

        Debug.Log("�ҋ@������~����܂����B");
    }

    private void StartAwaitingPayment()
    {
        // �x�����҂���Ԃ��J�n
        isAwaitingPayment = true;
        awaitingPaymentTimeElapsed = 0f; // �o�ߎ��Ԃ����Z�b�g
        isAwaitingPaymentDecaying = false; // �ʏ�̌����t���O�����Z�b�g
        isFastAwaitingPaymentDecaying = false; // ���������t���O�����Z�b�g

        // �x�����҂����A�C�R����ݒ�       
        switch (languageIndex)
        {
            case 0: // �p��
                callIcon.GetComponent<Image>().sprite = payIconSprite_en;
                break;
            case 1: // ���{��
                callIcon.GetComponent<Image>().sprite = payIconSprite;
                break;
        }
        callIcon.SetActive(true);

        Debug.Log("�x�����҂����J�n����܂����B");
    }

    private void StopAwaitingPayment()
    {
        isAwaitingPayment = false; // �x�����҂���Ԃ��~

        isAwaitingPaymentDecaying = false; // �t���O�����Z�b�g
        isFastAwaitingPaymentDecaying = false; // �t���O�����Z�b�g

        // �A�C�R���̓_�ł��~
        StopCoroutine("BlinkNormalAwaitingPaymentIcon");
        StopCoroutine("BlinkFastAwaitingPaymentIcon");

        callIcon.SetActive(false); // �x�����҂����A�C�R�����\��

        Debug.Log("�x�����҂����I�����܂����B");
    }

    private IEnumerator SpawnCustomerAfterDelay()
    {
        Debug.Log("�X�|�[���ҋ@�J�n");

        // �X�|�[�����Ԃ܂őҋ@
        yield return new WaitForSeconds(spawnDelay);

        // �Ȃ��󂢂Ă��邩�`�F�b�N
        if (seatManager.GetAvailableSeats().Count > 0)
        {
            // �Ȃ��󂢂Ă���ꍇ�A�q���X�|�[��������
            SpawnCustomer();
        }
        else
        {
            // �Ȃ��󂢂Ă��Ȃ��ꍇ�A�L���[�ɒǉ����đҋ@
            waitingCustomers.Enqueue(this);
            Debug.Log("��Ȃ��Ȃ����߁A�q���ҋ@���X�g�ɒǉ�����܂����B");
        }
    }

    // �Ȃ��󂢂��Ƃ��ɌĂ΂��֐�
    public static void OnSeatAvailable()
    {
        if (waitingCustomers.Count > 0)
        {
            // �ҋ@���Ă���ŏ��̋q���X�|�[��������
            CustomerCallIcon nextCustomer = waitingCustomers.Dequeue();
            nextCustomer.SpawnCustomer();
        }
    }

    // ���ۂɋq���X�|�[�������鏈�����s���֐�
    private void SpawnCustomer()
    {
        // ���ۂɋq���X�|�[�������鏈��
        ShowCustomer();
        transform.position = spawnPoint.position; // ������̈ʒu�Ɉړ�
        isSpawned = true;

        Debug.Log("�q���X�|�[�����܂����B");

        // �󂢂Ă�����ȂɈړ����鏈��
        MoveToRandomSeat();
    }

    private void HideCustomer()
    {
        // ���f���i�����ځj���\���ɂ���i��: Renderer���I�t�ɂ���j
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // �K�v�Ȃ�R���C�_�[�������ɂ���
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void ShowCustomer()
    {
        // ���f�����ĕ\������
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        // �R���C�_�[���L���ɂ���
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    // �󂢂Ă�����ȂɈړ����鏈��
    private void MoveToRandomSeat()
    {
        // �󂢂Ă�����Ȃ̃��X�g���擾
        List<int> availableSeats = seatManager.GetAvailableSeats();

        // �󂢂Ă�����Ȃ��Ȃ��ꍇ
        if (availableSeats.Count == 0)
        {
            Debug.LogError("�󂢂Ă���Ȃ�����܂���I");
            return;
        }

        // �����_���ɋ󂫐Ȃ�I��
        selectedSeatIndex = availableSeats[UnityEngine.Random.Range(0, availableSeats.Count)];
        seatManager.ReserveSeat(selectedSeatIndex); // ���Ȃ�\��

        // �I�����ꂽ���ȂɈړ�
        Transform targetSeat = seatManager.GetSeatTransform(selectedSeatIndex);
        StartCoroutine(MoveToSeat(targetSeat));  // ���Ȃւ̈ړ�����
    }

    // ���Ȃֈړ�����R���[�`��
    private IEnumerator MoveToSeat(Transform targetSeat)
    {
        // ���ȂɑΉ�����E�F�C�|�C���g�̃��X�g���擾
        Transform[] waypoints = seatManager.GetWaypointsForSeat(selectedSeatIndex);

        // �e�E�F�C�|�C���g�����ԂɈړ�
        foreach (var waypoint in waypoints)
        {
            while (Vector3.Distance(transform.position, waypoint.position) > 0.1f)
            {
                // �i�s�����������悤�ɉ�]
                Vector3 direction = (waypoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // ��]���x�𒲐�

                transform.position = Vector3.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // �Ō�ɍ��ȂɈړ�
        while (Vector3.Distance(transform.position, targetSeat.position) > 0.1f)
        {
            // �i�s�����������悤�ɉ�]
            Vector3 direction = (targetSeat.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // ��]���x�𒲐�

            transform.position = Vector3.MoveTowards(transform.position, targetSeat.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("�q���Ȃɒ����܂����B");

        // SeatManager����Y���̉�]�p���擾
        float targetYRotation = seatManager.seatRotations[selectedSeatIndex];

        // �w�肳�ꂽY���̉�]�p�Ɍ������ĉ�]������
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        Debug.Log("�q�����Ȃ̎w�肳�ꂽ�����Ɍ����܂����B");

        // ���s�A�j���[�V�������~���A�����Ă���A�j���[�V�������J�n
        animator.SetBool("isWalking", false);  // Walking�A�j���[�V�������~
        animator.SetBool("isSitting", true);   // Sitting�A�j���[�V�������Đ�

        // �w�肵���x�����Ԃ�ҋ@
        yield return new WaitForSeconds(seatArrivalDelay);

        // CallCanvas��\������
        callCanvas.SetActive(true);

        // �ďo���̊J�n
        StartCalling();

        // �Ȃɒ�������Idle��Ԃ�����
        isIdle = false;

        // �q���Ȃɂ����ۂɃv���C���[��triggerDistance���ɂ���ꍇ�A���b�Z�[�W��\��
        float currentDistance = Vector3.Distance(player.position, customer.position);
        if (currentDistance <= triggerDistance && !isRotationRestricted && IsClosestCustomer())
        {
            if (isWaiting)
            {
                switch (languageIndex)
                {
                    case 0: // �p��
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to ask for the order again");
                        break;
                    case 1: // ���{��
                        ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������Ċm�F����");
                        break;
                }
            }
            else
            {
                switch (languageIndex)
                {
                    case 0: // �p��
                        ShowMessageImage1($"Press <size=90>{confirmKey}</size> to take the order");
                        break;
                    case 1: // ���{��
                        ShowMessageImage1($"<size=90>{confirmKey}</size> �������Ē������󂯂�");
                        break;
                }
            }
        }
    }

    // �x������Ɍڋq�����Ȃ��������܂ňړ�������R���[�`��
    private IEnumerator MoveToSpawnAfterPayment()
    {
        // CallCanvas���\���ɂ���
        callCanvas.SetActive(false);

        // Idle��Ԃɂ���
        isIdle = true;

        // �w�肵���x�����Ԃ�ҋ@
        yield return new WaitForSeconds(paymentDepartureDelay);

        // �����Ă���A�j���[�V�������~���A���s�A�j���[�V�������Đ�
        animator.SetBool("isSitting", false);   // Sitting�A�j���[�V�������~
        animator.SetBool("isWalking", true);    // Walking�A�j���[�V�������Đ�

        // ���ȂɑΉ�����E�F�C�|�C���g�̃��X�g���t���Ɏ擾
        Transform[] waypoints = seatManager.GetWaypointsForSeat(selectedSeatIndex);
        for (int i = waypoints.Length - 1; i >= 0; i--)
        {
            Transform waypoint = waypoints[i];
            while (Vector3.Distance(transform.position, waypoint.position) > 0.1f)
            {
                // �i�s�����������悤�ɉ�]
                Vector3 direction = (waypoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // ��]���x�𒲐�

                transform.position = Vector3.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // �Ō��spawnPoint�ɖ߂�
        while (Vector3.Distance(transform.position, spawnPoint.position) > 0.1f)
        {
            // �i�s�����������悤�ɉ�]
            Vector3 direction = (spawnPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);  // ��]���x�𒲐�

            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // �ڋq���\���ɂ���
        HideCustomer();

        // ���Ȃ̏�Ԃ����Z�b�g
        seatManager.ReleaseSeat(selectedSeatIndex);

        // �q�̊�����o�^
        CompleteCustomer();
    }

    // 1�������e�L�X�g��\������R���[�`��
    private IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        conversationText.text = ""; // �e�L�X�g���N���A

        // �^�C�s���O�����Đ�
        SoundManager.instance.PlayTypingSound();

        foreach (char letter in message.ToCharArray())
        {
            conversationText.text += letter; // 1�������ǉ�
            yield return new WaitForSeconds(typingSpeed); // �w��̑��x�őҋ@
        }

        // �^�C�s���O�I�����ɉ����~
        SoundManager.instance.StopTypingSound();
        isTyping = false; // �^�C�s���O�I��
    }


    // ��b�̊J�n�����i�x���t���j
    private IEnumerator StartConversationWithDelay()
    {
        // ��b�p��Canvas��\��
        conversationCanvas.gameObject.SetActive(true);

        // �ŏ��̃��b�Z�[�W���Z�b�g
        conversationIndex = 0;

        // �R���[�`���ŕ��������X�ɕ\��
        yield return StartCoroutine(TypeMessage(conversationMessages[conversationIndex]));

        Debug.Log("��b���J�n����܂����B");

        isConversing = true;
    }

    public void StartConversation(System.Action callback = null)
    {
        //messageImage1��3��alpha��0�ɕύX
        if (messageImage1 != null)
        {
            Color color = messageImage1.color;
            color.a = 0f; // �A���t�@�l��0�ɐݒ�
            messageImage1.color = color;
        }
        if (messageImage3 != null)
        {
            Color color = messageImage3.color;
            color.a = 0f; // �A���t�@�l��0�ɐݒ�
            messageImage3.color = color;
        }

        conversationCanvas.gameObject.SetActive(true); // Canvas��\��
        isConversing = true; // ��b���ɐݒ�
        isConversationJustStarted = true; // �����Ńt���O��ݒ�
        onConversationEndCallback = callback; // ��b�I����̏�����ݒ�
        conversationIndex = 0; // ��b�̍ŏ��̃��b�Z�[�W����J�n
        DisplayNextMessage(); // �ŏ��̃��b�Z�[�W��\��
    }

    private void DisplayNextMessage()
    {
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
    }


    private void EndConversation()
    {
        //messageImage1��3��alpha��1�ɕύX
        if (messageImage1 != null)
        {
            Color color = messageImage1.color;
            color.a = 1f; // �A���t�@�l��1�ɐݒ�
            messageImage1.color = color;
        }
        if (messageImage3 != null)
        {
            Color color = messageImage3.color;
            color.a = 1f; // �A���t�@�l��1�ɐݒ�
            messageImage3.color = color;
        }

        isConversing = false;
        conversationCanvas.gameObject.SetActive(false); // ��b�p��Canvas���\��
        onConversationEndCallback?.Invoke(); // ��b�I�����̃R�[���o�b�N���Ăяo��
    }


    // �ėp�I�Ȓx���֐�
    private IEnumerator DelayWithAction(float delayTime, System.Action action)
    {
        // �w�莞�Ԃ̒x��
        yield return new WaitForSeconds(delayTime);

        // �x����ɃR�[���o�b�N�Ƃ��ēn���ꂽ�A�N�V���������s
        action?.Invoke();
    }

    // �ʏ�̌ďo���A�C�R���̓_�ŏ���
    private IEnumerator BlinkNormalCallIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isCallDecaying && !isFastCallDecaying) // �ʏ�̑������s���Ă����
        {
            switch (languageIndex)
            {
                case 0: // �p��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite_en : decayingCallIcon_en;
                    break;
                case 1: // ���{��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite : decayingCallIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // �����ďo���A�C�R���̓_�ŏ���
    private IEnumerator BlinkFastCallIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastCallDecaying) // �����������s���Ă����
        {
            switch (languageIndex)
            {
                case 0: // �p��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? callingIconSprite_en : fastDecayingCallIcon_en;
                    break;
                case 1: // ���{��
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

        while (isWaitDecaying && !isFastWaitDecaying) // �ʏ�̑������s���Ă����
        {
            callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? waitingIconSprite : decayingWaitIcon;
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator BlinkFastWaitIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastWaitDecaying) // �����������s���Ă����
        {
            callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? waitingIconSprite : fastDecayingWaitIcon;
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // �ʏ�̎x�����҂��A�C�R���̓_�ŏ���
    private IEnumerator BlinkNormalAwaitingPaymentIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isAwaitingPaymentDecaying && !isFastAwaitingPaymentDecaying) // �ʏ�̑������s���Ă����
        {
            switch (languageIndex)
            {
                case 0: // �p��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite_en : decayingAwaitingPaymentIcon_en;
                    break;
                case 1: // ���{��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite : decayingAwaitingPaymentIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // �����x�����҂��A�C�R���̓_�ŏ���
    private IEnumerator BlinkFastAwaitingPaymentIcon(float blinkInterval)
    {
        bool isIcon1Displayed = true;

        while (isFastAwaitingPaymentDecaying) // �����������s���Ă����
        {
            switch (languageIndex)
            {
                case 0: // �p��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite_en : fastDecayingAwaitingPaymentIcon_en;
                    break;
                case 1: // ���{��
                    callIcon.GetComponent<Image>().sprite = isIcon1Displayed ? payIconSprite : fastDecayingAwaitingPaymentIcon;
                    break;
            }
            isIcon1Displayed = !isIcon1Displayed;

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // �q�����J�E���g
    private void RegisterCustomer()
    {
        totalCustomers++;
    }

    //�@�q���A������̏���
    private void CompleteCustomer()
    {
        completedCustomers++;

        // �f�o�b�O��totalCustomers��completeCustomers��\��
        Debug.Log("Total Customers: " + totalCustomers);
        Debug.Log("Completed Customers: " + completedCustomers);

        // ���ׂĂ̋q�������������`�F�b�N
        if (completedCustomers >= totalCustomers)
        {
            // 2�b�̒x�������Ă��烊�U���g��ʂ�\��
            StartCoroutine(DelayWithAction(2.0f, () =>
            {
                StageManager.instance.ShowResultScreen();
            }));
        }
    }

    // �s���x�o�[��_�ł�����R���[�`��
    private IEnumerator BlinkSatisfactionBar()
    {
        while (true)
        {
            // �_�ŗp�̉摜�ɐ؂�ւ�
            satisfactionBar.sprite = blinkingSatisfactionBarSprite;
            yield return new WaitForSeconds(satisfactionBlinkInterval1);

            // �ʏ�̉摜�ɖ߂�
            satisfactionBar.sprite = normalSatisfactionBarSprite;
            yield return new WaitForSeconds(satisfactionBlinkInterval2);
        }
    }

    // �Q�[���I�[�o�[�����s���郁�\�b�h
    private void GameOver()
    {
        // ���ɃQ�[���I�[�o�[�ɂȂ��Ă���ꍇ�͉������Ȃ�
        if (isGameOver)
            return;

        isGameOver = true;

        // �v���C���[�̈ړ��Ǝ��_�ړ����~
        DisablePlayerControl();

        // �}�E�X�J�[�\����\��
        //player.GetComponent<FirstPersonMovement>().UnlockCursor();

        // �����摜�ƃQ�[���I�[�o�[�����̃A���t�@��0�ɐݒ�
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

        // �Q�[���I�[�o�[Canvas���A�N�e�B�u�ɂ���
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        // �t�F�[�h�C���������J�n
        StartCoroutine(FadeInGameOver());
    }

    // �Q�[���I�[�o�[Canvas���̍����摜�ƃe�L�X�g���t�F�[�h�C��������R���[�`��
    private IEnumerator FadeInGameOver()
    {
        // �����摜�����X�ɈÂ�����
        float blackFadeDuration = 2f; // �����摜�̃t�F�[�h�C������
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

        // �����摜�̃A���t�@�����S��1�ɐݒ�
        if (gameOverBlackImage != null)
        {
            Color blackColor = gameOverBlackImage.color;
            blackColor.a = 1f;
            gameOverBlackImage.color = blackColor;
        }

        // �Q�[���I�[�o�[�e�L�X�g��f�����t�F�[�h�C������
        float textFadeDuration = 1f; // �e�L�X�g�̃t�F�[�h�C������
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

        // �e�L�X�g�̃A���t�@�����S��1�ɐݒ�
        if (gameOverText != null)
        {
            Color textColor = gameOverText.color;
            textColor.a = 1f;
            gameOverText.color = textColor;
        }

        // ���j���[�ɖ߂�{�^�����A�N�e�B�u�ɂ���
        if (returnToMenuButton != null)
        {
            returnToMenuButton.gameObject.SetActive(true);
        }

        // �}�E�X�J�[�\����\��
        player.GetComponent<FirstPersonMovement>().UnlockCursor();
    }

    // ���j���[�ɖ߂�{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    private void OnReturnToMenuButtonClicked()
    {
        // �x�������Ă���t�F�[�h�A�E�g�ƃV�[���J�ڂ��s���R���[�`�����J�n
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

        // ���̋q���ł��߂��q�ł����true��Ԃ�
        return closestCustomer == this;
    }

    // �v���C���[�̎��E�ɋq�������Ă��邩�𔻒肷�郁�\�b�h
    private bool IsCustomerInPlayerView()
    {
        Vector3 directionToCustomer = (customer.position - PlayerCamera.position).normalized;
        float angle = Vector3.Angle(PlayerCamera.forward, directionToCustomer);
        return angle < playerFieldOfViewAngle * 0.5f;
    }

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

    public void LoadKeySettings()
    {
        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
        serveKey = (KeyCode)PlayerPrefs.GetInt("serveKey", (int)KeyCode.Q);
    }

    private void ApplyLanguage(int index)
    {
        switch (index)
        {
            case 0: // �p��
                satisfactionBarBackground.sprite = satisfactionBarBackgroundSprite_en;
                break;
            case 1: // ���{��
                satisfactionBarBackground.sprite = satisfactionBarBackgroundSprite;
                break;
        }
    }
}