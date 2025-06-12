using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AnimateSpotImages : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // �v���C���[��Transform��Inspector�Ŏw��

    [Header("Images to Animate")]
    public Image circleImage;       // Circle��Image
    public Image triangleImage;     // Triangle��Image

    [Header("Animation Settings")]
    [Tooltip("�A�j���[�V�����̎������ԁi�b�j")]
    public float period = 2f;       // �A�j���[�V�����̎�������

    [Header("Circle Scale Settings")]
    [Tooltip("Circle�̃X�P�[���̍ŏ��l")]
    public Vector3 circleScaleMin = Vector3.one;
    [Tooltip("Circle�̃X�P�[���̍ő�l")]
    public Vector3 circleScaleMax = Vector3.one * 2f;

    [Header("Triangle Position Settings")]
    [Tooltip("Triangle��Y�ʒu�̍ŏ��l")]
    public float trianglePosYMin = -50f;
    [Tooltip("Triangle��Y�ʒu�̍ő�l")]
    public float trianglePosYMax = 50f;

    // �����^�C�}�[
    private float timer = 0f;

    // �����ʒu
    private Vector3 triangleInitialPosition;

    void Start()
    {
        // Triangle�̏����ʒu��ۑ�
        if (triangleImage != null)
        {
            triangleInitialPosition = triangleImage.rectTransform.anchoredPosition;
            // �����ʒu��Y���ő�l�ɐݒ�
            Vector3 pos = triangleImage.rectTransform.anchoredPosition;
            pos.y = trianglePosYMax;
            triangleImage.rectTransform.anchoredPosition = pos;
        }

        // �����X�P�[����Circle�̍ŏ��X�P�[���ɐݒ�
        if (circleImage != null)
        {
            circleImage.rectTransform.localScale = circleScaleMin;
        }
    }

    void Update()
    {
        // ���Ԃ������Ń��[�v
        timer += Time.deltaTime;
        float normalizedTime = (timer % period) / period; // 0����1�͈̔�

        // �O�p�֐����g�p����0����1�ւ̃X���[�Y�ȕω�
        float sineValue = Mathf.Sin(normalizedTime * 2f * Mathf.PI - Mathf.PI / 2f); // -1����1�͈̔�
        float lerpFactor = (sineValue + 1f) / 2f; // 0����1�͈̔�

        // Circle�̃X�P�[���A�j���[�V����
        if (circleImage != null)
        {
            Vector3 newScale = Vector3.Lerp(circleScaleMin, circleScaleMax, lerpFactor);
            circleImage.rectTransform.localScale = newScale;
        }

        // Triangle�̈ʒu�A�j���[�V����
        if (triangleImage != null)
        {
            // Triangle�͍ő�l����ŏ��l�֊J�n���邽�߁A1 - lerpFactor���g�p
            float newY = Mathf.Lerp(trianglePosYMax, trianglePosYMin, lerpFactor);
            Vector3 pos = triangleImage.rectTransform.anchoredPosition;
            pos.y = newY;
            triangleImage.rectTransform.anchoredPosition = pos;
        }

        // Triangle���v���C���[�̕����Ɍ�����
        if (triangleImage != null && player != null)
        {
            // Triangle��RectTransform�̈ʒu���擾
            Vector3 trianglePosition = triangleImage.rectTransform.position;

            // �v���C���[��Triangle�̕����x�N�g�����v�Z
            Vector3 directionToPlayer = player.position - trianglePosition;
            directionToPlayer.y = 0; // Y���̉�]��h���i�K�v�ɉ����Ē����j

            if (directionToPlayer != Vector3.zero)
            {
                // �v���C���[�̕�����������]���v�Z
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

                // Triangle��RectTransform�ɉ�]��K�p
                triangleImage.rectTransform.rotation = lookRotation;

                // �K�v�ɉ�����180�x��]������i�A�C�R���̌��������j
                triangleImage.rectTransform.Rotate(0, 180, 0);
            }
        }
    }
}
