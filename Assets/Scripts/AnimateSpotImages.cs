using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AnimateSpotImages : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // プレイヤーのTransformをInspectorで指定

    [Header("Images to Animate")]
    public Image circleImage;       // CircleのImage
    public Image triangleImage;     // TriangleのImage

    [Header("Animation Settings")]
    [Tooltip("アニメーションの周期時間（秒）")]
    public float period = 2f;       // アニメーションの周期時間

    [Header("Circle Scale Settings")]
    [Tooltip("Circleのスケールの最小値")]
    public Vector3 circleScaleMin = Vector3.one;
    [Tooltip("Circleのスケールの最大値")]
    public Vector3 circleScaleMax = Vector3.one * 2f;

    [Header("Triangle Position Settings")]
    [Tooltip("TriangleのY位置の最小値")]
    public float trianglePosYMin = -50f;
    [Tooltip("TriangleのY位置の最大値")]
    public float trianglePosYMax = 50f;

    // 内部タイマー
    private float timer = 0f;

    // 初期位置
    private Vector3 triangleInitialPosition;

    void Start()
    {
        // Triangleの初期位置を保存
        if (triangleImage != null)
        {
            triangleInitialPosition = triangleImage.rectTransform.anchoredPosition;
            // 初期位置のYを最大値に設定
            Vector3 pos = triangleImage.rectTransform.anchoredPosition;
            pos.y = trianglePosYMax;
            triangleImage.rectTransform.anchoredPosition = pos;
        }

        // 初期スケールをCircleの最小スケールに設定
        if (circleImage != null)
        {
            circleImage.rectTransform.localScale = circleScaleMin;
        }
    }

    void Update()
    {
        // 時間を周期でループ
        timer += Time.deltaTime;
        float normalizedTime = (timer % period) / period; // 0から1の範囲

        // 三角関数を使用して0から1へのスムーズな変化
        float sineValue = Mathf.Sin(normalizedTime * 2f * Mathf.PI - Mathf.PI / 2f); // -1から1の範囲
        float lerpFactor = (sineValue + 1f) / 2f; // 0から1の範囲

        // Circleのスケールアニメーション
        if (circleImage != null)
        {
            Vector3 newScale = Vector3.Lerp(circleScaleMin, circleScaleMax, lerpFactor);
            circleImage.rectTransform.localScale = newScale;
        }

        // Triangleの位置アニメーション
        if (triangleImage != null)
        {
            // Triangleは最大値から最小値へ開始するため、1 - lerpFactorを使用
            float newY = Mathf.Lerp(trianglePosYMax, trianglePosYMin, lerpFactor);
            Vector3 pos = triangleImage.rectTransform.anchoredPosition;
            pos.y = newY;
            triangleImage.rectTransform.anchoredPosition = pos;
        }

        // Triangleをプレイヤーの方向に向ける
        if (triangleImage != null && player != null)
        {
            // TriangleのRectTransformの位置を取得
            Vector3 trianglePosition = triangleImage.rectTransform.position;

            // プレイヤーとTriangleの方向ベクトルを計算
            Vector3 directionToPlayer = player.position - trianglePosition;
            directionToPlayer.y = 0; // Y軸の回転を防ぐ（必要に応じて調整）

            if (directionToPlayer != Vector3.zero)
            {
                // プレイヤーの方向を向く回転を計算
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

                // TriangleのRectTransformに回転を適用
                triangleImage.rectTransform.rotation = lookRotation;

                // 必要に応じて180度回転させる（アイコンの向き調整）
                triangleImage.rectTransform.Rotate(0, 180, 0);
            }
        }
    }
}
