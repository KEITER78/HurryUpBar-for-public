using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource; // 効果音用のAudioSource
    public AudioSource bgmSource; // BGM用のAudioSource
    public AudioSource footstepsSource; // 足音用のAudioSource
    public AudioSource typingSource; // タイピング音用のAudioSource

    public AudioClip decisionSound;
    public AudioClip successSound;
    public AudioClip failureSound;
    public AudioClip callSound;
    public AudioClip serveSound;
    public AudioClip paymentSound;
    public AudioClip messageSound;
    public AudioClip clickSound;

    public AudioClip gameBGM; // ゲーム中のBGM
    public AudioClip menuBGM; // メインメニューのBGM
    public AudioClip resultBGM; // リザルト画面用のBGM
    public AudioClip conversationBGM; // 会話シーンのBGM
    public AudioClip fieldBGM; // フィールドシーンのBGM

    public AudioClip walkFootsteps;     // 歩いているときの足音
    public AudioClip runFootsteps;      // 走っているときの足音

    // 足音の状態を管理する列挙型
    private enum FootstepState
    {
        None,
        Walking,
        Running
    }

    // 現在の足音の状態を保持する変数
    private FootstepState currentFootstepState = FootstepState.None;

    // フェードアウト用のコルーチンを保持する変数
    private Coroutine footstepsFadeCoroutine;

    // 走りから歩きへの遷移用のコルーチンを保持する変数
    private Coroutine runToWalkTransitionCoroutine;

    private void Awake()
    {
        // シングルトンパターンの設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでもオブジェクトを残す
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 効果音の再生関数
    public void PlayDecisionSound()
    {
        PlaySound(decisionSound);
    }

    public void PlaySuccessSound()
    {
        PlaySound(successSound);
    }

    public void PlayFailureSound()
    {
        PlaySound(failureSound);
    }

    public void PlayCallSound()
    {
        PlaySound(callSound);
    }

    public void PlayServeSound()
    {
        PlaySound(serveSound);
    }

    public void PlayPaymentSound()
    {
        PlaySound(paymentSound);
    }

    public void PlayMessageSound()
    {
        PlaySound(messageSound);
    }

    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("オーディオクリップまたはAudioSourceが設定されていません。");
        }
    }

    // ゲーム中のBGMを再生
    public void PlayGameBGM()
    {
        if (gameBGM != null && bgmSource != null)
        {
            bgmSource.clip = gameBGM;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGMクリップまたはAudioSourceが設定されていません。");
        }
    }

    // メインメニューのBGMを再生
    public void PlayMenuBGM()
    {
        if (menuBGM != null && bgmSource != null)
        {
            bgmSource.clip = menuBGM;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGMクリップまたはAudioSourceが設定されていません。");
        }
    }

    // リザルトBGMを再生
    public void PlayResultBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = resultBGM;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGMクリップまたはAudioSourceが設定されていません。");
        }
    }

    // 会話シーン用のBGMを再生
    public void PlayConversationBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = conversationBGM;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGMクリップまたはAudioSourceが設定されていません。");
        }
    }

    // 会話シーン用のBGMを再生
    public void PlayFieldBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = fieldBGM;
            bgmSource.loop = true; // ループ再生
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGMクリップまたはAudioSourceが設定されていません。");
        }
    }

    // BGMの停止
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // 足音を再生する関数（歩行）
    public void PlayWalkFootsteps()
    {
        if (currentFootstepState == FootstepState.Running)
        {
            // 既に走行中で、遷移コルーチンが実行中でなければ開始
            if (runToWalkTransitionCoroutine == null)
            {
                runToWalkTransitionCoroutine = StartCoroutine(TransitionRunToWalk(0.25f)); // 〇秒後に歩行音に切り替え
            }
            return;
        }

        // フェードアウト中のコルーチンを停止
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        if (walkFootsteps != null && footstepsSource != null)
        {
            footstepsSource.clip = walkFootsteps;
            footstepsSource.loop = true; // ループ再生
            footstepsSource.volume = 1f; // 音量を元に戻す
            footstepsSource.Play();
            currentFootstepState = FootstepState.Walking;
        }
        else
        {
            Debug.LogWarning("歩行用のオーディオクリップまたはAudioSourceが設定されていません。");
        }
    }


    // 足音を再生する関数（走行）
    public void PlayRunFootsteps()
    {
        // フェードアウト中のコルーチンを停止
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        // 走行から歩行への遷移コルーチンを停止
        if (runToWalkTransitionCoroutine != null)
        {
            StopCoroutine(runToWalkTransitionCoroutine);
            runToWalkTransitionCoroutine = null;
        }

        if (runFootsteps != null && footstepsSource != null)
        {
            footstepsSource.clip = runFootsteps;
            footstepsSource.loop = true; // ループ再生
            footstepsSource.volume = 1f; // 音量を元に戻す
            footstepsSource.Play();
            currentFootstepState = FootstepState.Running;
        }
        else
        {
            Debug.LogWarning("走行用のオーディオクリップまたはAudioSourceが設定されていません。");
        }
    }

    // 足音をフェードアウトして停止するコルーチン
    private IEnumerator FadeOutFootsteps(float duration)
    {
        float startVolume = footstepsSource.volume;

        while (footstepsSource.volume > 0)
        {
            footstepsSource.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        footstepsSource.Stop();
        footstepsSource.volume = startVolume; // 再利用のために元の音量に戻す
        currentFootstepState = FootstepState.None; // 状態をリセット
    }

    // 足音を停止する関数をフェードアウトに変更
    public void StopFootsteps()
    {
        // まず状態をリセット
        currentFootstepState = FootstepState.None;

        // フェードアウト中のコルーチンを停止
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        // 走行から歩行への遷移コルーチンを停止
        if (runToWalkTransitionCoroutine != null)
        {
            StopCoroutine(runToWalkTransitionCoroutine);
            runToWalkTransitionCoroutine = null;
        }

        // フェードアウトを開始
        footstepsFadeCoroutine = StartCoroutine(FadeOutFootsteps(0.15f)); // フェードアウトに〇秒を指定
    }


    // 走行中から歩行中に遷移するコルーチン
    private IEnumerator TransitionRunToWalk(float delay)
    {
        // 1秒間走行音を維持
        yield return new WaitForSeconds(delay);

        // 現在が走行中であれば歩行音に切り替え
        if (currentFootstepState == FootstepState.Running)
        {
            if (walkFootsteps != null && footstepsSource != null)
            {
                footstepsSource.clip = walkFootsteps;
                footstepsSource.loop = true; // ループ再生
                footstepsSource.volume = 1f; // 音量を元に戻す
                footstepsSource.Play();
                currentFootstepState = FootstepState.Walking;
            }
            else
            {
                Debug.LogWarning("歩行用のオーディオクリップまたはAudioSourceが設定されていません。");
            }
        }

        // コルーチンの参照をクリア
        runToWalkTransitionCoroutine = null;
    }

    // 文字入力の音（messageSound）を再生するメソッド
    public void PlayTypingSound()
    {
        if (messageSound != null && typingSource != null)
        {
            typingSource.clip = messageSound;
            typingSource.loop = true; // ループ再生
            typingSource.Play();
        }
        else
        {
            Debug.LogWarning("MessageSoundクリップまたはTyping AudioSourceが設定されていません。");
        }
    }

    // 文字入力の音を停止するメソッド
    public void StopTypingSound()
    {
        if (typingSource != null && typingSource.isPlaying)
        {
            typingSource.loop = false; // ループ再生を解除
            typingSource.Stop();
        }
    }
}
