using UnityEngine;

public class TestPlayManager : MonoBehaviour
{
    public static TestPlayManager instance;

    [Header("テストプレイ中かどうかのフラグ")]
    public bool isTestPlay = false;

    private void Awake()
    {
        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
