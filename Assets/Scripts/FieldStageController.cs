using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FieldStageController : MonoBehaviour
{
    [Tooltip("�v���C���[�̈ړ��X�N���v�g���w�肵�Ă�������")]
    public FirstPersonMovement playerMovement;

    [Header("References")]
    [Tooltip("�v���C���[�̃I�u�W�F�N�g���w�肵�Ă�������")]
    public Transform player;

    [Tooltip("�ړI�n�̃I�u�W�F�N�g���w�肵�Ă�������")]
    public Transform destination;

    [Header("Settings")]
    [Tooltip("�ړI�n�̔��a���w�肵�Ă�������")]
    public float destinationRadius = 1.0f;

    [Header("Fade Settings")]
    [Tooltip("�t�F�[�h�p��Canvas���w�肵�Ă�������")]
    public CanvasGroup blackFadeCanvas;

    [Tooltip("�t�F�[�h�C�����Ԃ��w�肵�Ă�������")]
    public float fadeInDuration = 1.0f;

    [Tooltip("�t�F�[�h�A�E�g���Ԃ��w�肵�Ă�������")]
    public float fadeOutDuration = 1.0f;

    [Header("���b�Z�[�W�\���֘A")]
    public int destinationVariable; // �s��ϐ� (0:�o�[, 1:�w)
    public GameObject messageCanvas; // ���b�Z�[�W�\��Canvas
    public Image messageImage; // ���b�Z�[�W�\���pImage
    public Sprite goToBar; // GoToBar��Sprite
    public Sprite goToBar_en; // GoToBar_en��Sprite
    public Sprite goToStation; // GoToStation��Sprite
    public Sprite goToStation_en; // GoToStation_en��Sprite

    [Header("Scene Management")]
    [Tooltip("�ǂݍ��ރV�[�������w�肵�Ă�������")]
    public string nextSceneName;   // �ǂݍ��ރV�[������Inspector�Ŏw��

    private int languageIndex; // ����ݒ�C���f�b�N�X
    private Coroutine messageFadeCoroutine; // �t�F�[�h�p�R���[�`���̃C���X�^���X

    void Start()
    {
        // �t�F�[�h�C������
        if (blackFadeCanvas != null)
        {
            StartCoroutine(BlackFadeIn());
        }

        // �t�B�[���h��BGM���Đ�
        SoundManager.instance.PlayFieldBGM();

        // ����ݒ�̃C���f�b�N�X��PlayerPrefs����擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g��0

        // destinationVariable �Ɋ�Â��ĕ\������Sprite��I��
        Sprite selectedSprite = null;

        switch (destinationVariable)
        {
            case 0: // GoToBar
                switch (languageIndex)
                {
                    case 0: // English
                        selectedSprite = goToBar_en;
                        break;
                    case 1: // ���{��
                        selectedSprite = goToBar;
                        break;
                    default:
                        Debug.LogWarning("���Ή��̌���C���f�b�N�X�ł��B�f�t�H���g�Ƃ��ē��{���ݒ肵�܂��B");
                        selectedSprite = goToBar;
                        break;
                }
                break;

            case 1: // GoToStation
                switch (languageIndex)
                {
                    case 0: // English
                        selectedSprite = goToStation_en;
                        break;
                    case 1: // ���{��
                        selectedSprite = goToStation;
                        break;
                    default:
                        Debug.LogWarning("���Ή��̌���C���f�b�N�X�ł��B�f�t�H���g�Ƃ��ē��{���ݒ肵�܂��B");
                        selectedSprite = goToStation;
                        break;
                }
                break;

            default:
                Debug.LogWarning("���Ή��̍s��ϐ��ł��B���b�Z�[�W�\�����X�L�b�v���܂��B");
                break;
        }

        // ���b�Z�[�W��\��
        if (selectedSprite != null)
        {
            ShowAndFadeOutImage(selectedSprite);
        }
    }


    void Update()
    {
        // �v���C���[�ƖړI�n�̋������v�Z
        if (IsPlayerWithinDestination())
        {
            OnDestinationReached();
        }
    }

    // �v���C���[���ړI�n���ɂ��邩���肷��֐�
    private bool IsPlayerWithinDestination()
    {
        if (player == null || destination == null)
        {
            Debug.LogWarning("�v���C���[�܂��͖ړI�n�̎Q�Ƃ��ݒ肳��Ă��܂���");
            return false;
        }

        // XZ���ʏ�̋������v�Z
        Vector3 playerPositionXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 destinationPositionXZ = new Vector3(destination.position.x, 0, destination.position.z);
        float distance = Vector3.Distance(playerPositionXZ, destinationPositionXZ);

        return distance <= destinationRadius;
    }

    // �ړI�n�������ɌĂяo�����֐�
    private void OnDestinationReached()
    {
        Debug.Log("�ړI�n�ɓ������܂����I");

        // �v���C���[�̈ړ��𖳌���
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }

        // �t�F�[�h�A�E�g����
        if (blackFadeCanvas != null)
        {
            StartCoroutine(BlackFadeOut());
        }
    }

    // �����t�F�[�h�C������
    private IEnumerator BlackFadeIn()
    {
        blackFadeCanvas.alpha = 1.0f; // �A���t�@�l��1�ɐݒ�
        blackFadeCanvas.gameObject.SetActive(true); // Canvas��L����

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            blackFadeCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeInDuration); // ���X�ɓ����ɂ���
            yield return null;
        }

        blackFadeCanvas.alpha = 0.0f; // ���S�ɓ�����
        blackFadeCanvas.gameObject.SetActive(false); // Canvas�𖳌���
    }

    // �����t�F�[�h�A�E�g����
    private IEnumerator BlackFadeOut()
    {
        blackFadeCanvas.alpha = 0.0f; // �A���t�@�l��0�ɐݒ�
        blackFadeCanvas.gameObject.SetActive(true); // Canvas��L����

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            blackFadeCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeOutDuration); // ���X�ɍ�������
            yield return null;
        }

        blackFadeCanvas.alpha = 1.0f; // ���S�ɕs������
                                      // Canvas�͗L�����̂܂�

        // ���̃V�[�������[�h
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // ���b�Z�[�W�摜��\�����ăt�F�[�h�C���E�\���E�t�F�[�h�A�E�g�����郁�\�b�h
    public void ShowAndFadeOutImage(Sprite imageSprite)
    {
        // �����̃t�F�[�h�R���[�`�������삵�Ă���ꍇ�͒�~
        if (messageFadeCoroutine != null)
        {
            StopCoroutine(messageFadeCoroutine);
        }

        // ���b�Z�[�W�摜��Canvas��ݒ�
        messageImage.sprite = imageSprite;
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 0f); // �A���t�@�l��0�Ƀ��Z�b�g�i�t�F�[�h�C���p�j
        messageCanvas.SetActive(true);

        // �t�F�[�h�C���E�\���E�t�F�[�h�A�E�g�������J�n
        messageFadeCoroutine = StartCoroutine(FadeInDisplayFadeOutMessage(1.0f, 0.5f, 4.0f, 1.0f)); // 1�b�ҋ@�A0.5�b�t�F�[�h�C���A4�b�\���A1�b�t�F�[�h�A�E�g
    }

    // ���b�Z�[�W�摜���t�F�[�h�C���A�\���A�t�F�[�h�A�E�g������R���[�`��
    private IEnumerator FadeInDisplayFadeOutMessage(float waitTime, float fadeInDuration, float displayTime, float fadeOutDuration)
    {
        // 1. �w�肳�ꂽ�ҋ@���Ԃ�҂�
        yield return new WaitForSeconds(waitTime);

        //SoundManager.instance.PlayDecisionSound(); // �N���b�N�����Đ�

        // 2. �t�F�[�h�C������
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            Color color = messageImage.color;
            color.a = alpha;
            messageImage.color = color;
            yield return null;
        }
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 1f); // ���S�ɕs������

        // 3. �w�肳�ꂽ�\�����Ԃ�҂�
        yield return new WaitForSeconds(displayTime);

        // 4. �t�F�[�h�A�E�g����
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            Color color = messageImage.color;
            color.a = alpha;
            messageImage.color = color;
            yield return null;
        }
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 0f); // ���S�ɓ�����

        // 5. �t�F�[�h�A�E�g���Canvas���\���ɂ���
        messageCanvas.SetActive(false);
    }
}
