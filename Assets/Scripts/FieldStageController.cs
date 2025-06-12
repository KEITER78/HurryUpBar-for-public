using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FieldStageController : MonoBehaviour
{
    [Tooltip("プレイヤーの移動スクリプトを指定してください")]
    public FirstPersonMovement playerMovement;

    [Header("References")]
    [Tooltip("プレイヤーのオブジェクトを指定してください")]
    public Transform player;

    [Tooltip("目的地のオブジェクトを指定してください")]
    public Transform destination;

    [Header("Settings")]
    [Tooltip("目的地の半径を指定してください")]
    public float destinationRadius = 1.0f;

    [Header("Fade Settings")]
    [Tooltip("フェード用のCanvasを指定してください")]
    public CanvasGroup blackFadeCanvas;

    [Tooltip("フェードイン時間を指定してください")]
    public float fadeInDuration = 1.0f;

    [Tooltip("フェードアウト時間を指定してください")]
    public float fadeOutDuration = 1.0f;

    [Header("メッセージ表示関連")]
    public int destinationVariable; // 行先変数 (0:バー, 1:駅)
    public GameObject messageCanvas; // メッセージ表示Canvas
    public Image messageImage; // メッセージ表示用Image
    public Sprite goToBar; // GoToBarのSprite
    public Sprite goToBar_en; // GoToBar_enのSprite
    public Sprite goToStation; // GoToStationのSprite
    public Sprite goToStation_en; // GoToStation_enのSprite

    [Header("Scene Management")]
    [Tooltip("読み込むシーン名を指定してください")]
    public string nextSceneName;   // 読み込むシーン名をInspectorで指定

    private int languageIndex; // 言語設定インデックス
    private Coroutine messageFadeCoroutine; // フェード用コルーチンのインスタンス

    void Start()
    {
        // フェードイン処理
        if (blackFadeCanvas != null)
        {
            StartCoroutine(BlackFadeIn());
        }

        // フィールドのBGMを再生
        SoundManager.instance.PlayFieldBGM();

        // 言語設定のインデックスをPlayerPrefsから取得
        languageIndex = PlayerPrefs.GetInt("Language", 0); // デフォルトは0

        // destinationVariable に基づいて表示するSpriteを選択
        Sprite selectedSprite = null;

        switch (destinationVariable)
        {
            case 0: // GoToBar
                switch (languageIndex)
                {
                    case 0: // English
                        selectedSprite = goToBar_en;
                        break;
                    case 1: // 日本語
                        selectedSprite = goToBar;
                        break;
                    default:
                        Debug.LogWarning("未対応の言語インデックスです。デフォルトとして日本語を設定します。");
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
                    case 1: // 日本語
                        selectedSprite = goToStation;
                        break;
                    default:
                        Debug.LogWarning("未対応の言語インデックスです。デフォルトとして日本語を設定します。");
                        selectedSprite = goToStation;
                        break;
                }
                break;

            default:
                Debug.LogWarning("未対応の行先変数です。メッセージ表示をスキップします。");
                break;
        }

        // メッセージを表示
        if (selectedSprite != null)
        {
            ShowAndFadeOutImage(selectedSprite);
        }
    }


    void Update()
    {
        // プレイヤーと目的地の距離を計算
        if (IsPlayerWithinDestination())
        {
            OnDestinationReached();
        }
    }

    // プレイヤーが目的地内にいるか判定する関数
    private bool IsPlayerWithinDestination()
    {
        if (player == null || destination == null)
        {
            Debug.LogWarning("プレイヤーまたは目的地の参照が設定されていません");
            return false;
        }

        // XZ平面上の距離を計算
        Vector3 playerPositionXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 destinationPositionXZ = new Vector3(destination.position.x, 0, destination.position.z);
        float distance = Vector3.Distance(playerPositionXZ, destinationPositionXZ);

        return distance <= destinationRadius;
    }

    // 目的地到着時に呼び出される関数
    private void OnDestinationReached()
    {
        Debug.Log("目的地に到着しました！");

        // プレイヤーの移動を無効化
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }

        // フェードアウト処理
        if (blackFadeCanvas != null)
        {
            StartCoroutine(BlackFadeOut());
        }
    }

    // 黒いフェードイン処理
    private IEnumerator BlackFadeIn()
    {
        blackFadeCanvas.alpha = 1.0f; // アルファ値を1に設定
        blackFadeCanvas.gameObject.SetActive(true); // Canvasを有効化

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            blackFadeCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeInDuration); // 徐々に透明にする
            yield return null;
        }

        blackFadeCanvas.alpha = 0.0f; // 完全に透明に
        blackFadeCanvas.gameObject.SetActive(false); // Canvasを無効化
    }

    // 黒いフェードアウト処理
    private IEnumerator BlackFadeOut()
    {
        blackFadeCanvas.alpha = 0.0f; // アルファ値を0に設定
        blackFadeCanvas.gameObject.SetActive(true); // Canvasを有効化

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            blackFadeCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeOutDuration); // 徐々に黒くする
            yield return null;
        }

        blackFadeCanvas.alpha = 1.0f; // 完全に不透明に
                                      // Canvasは有効化のまま

        // 次のシーンをロード
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // メッセージ画像を表示してフェードイン・表示・フェードアウトさせるメソッド
    public void ShowAndFadeOutImage(Sprite imageSprite)
    {
        // 既存のフェードコルーチンが動作している場合は停止
        if (messageFadeCoroutine != null)
        {
            StopCoroutine(messageFadeCoroutine);
        }

        // メッセージ画像とCanvasを設定
        messageImage.sprite = imageSprite;
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 0f); // アルファ値を0にリセット（フェードイン用）
        messageCanvas.SetActive(true);

        // フェードイン・表示・フェードアウト処理を開始
        messageFadeCoroutine = StartCoroutine(FadeInDisplayFadeOutMessage(1.0f, 0.5f, 4.0f, 1.0f)); // 1秒待機、0.5秒フェードイン、4秒表示、1秒フェードアウト
    }

    // メッセージ画像をフェードイン、表示、フェードアウトさせるコルーチン
    private IEnumerator FadeInDisplayFadeOutMessage(float waitTime, float fadeInDuration, float displayTime, float fadeOutDuration)
    {
        // 1. 指定された待機時間を待つ
        yield return new WaitForSeconds(waitTime);

        //SoundManager.instance.PlayDecisionSound(); // クリック音を再生

        // 2. フェードイン処理
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
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 1f); // 完全に不透明に

        // 3. 指定された表示時間を待つ
        yield return new WaitForSeconds(displayTime);

        // 4. フェードアウト処理
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
        messageImage.color = new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 0f); // 完全に透明に

        // 5. フェードアウト後にCanvasを非表示にする
        messageCanvas.SetActive(false);
    }
}
