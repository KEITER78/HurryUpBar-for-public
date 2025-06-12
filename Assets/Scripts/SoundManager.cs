using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource; // ���ʉ��p��AudioSource
    public AudioSource bgmSource; // BGM�p��AudioSource
    public AudioSource footstepsSource; // �����p��AudioSource
    public AudioSource typingSource; // �^�C�s���O���p��AudioSource

    public AudioClip decisionSound;
    public AudioClip successSound;
    public AudioClip failureSound;
    public AudioClip callSound;
    public AudioClip serveSound;
    public AudioClip paymentSound;
    public AudioClip messageSound;
    public AudioClip clickSound;

    public AudioClip gameBGM; // �Q�[������BGM
    public AudioClip menuBGM; // ���C�����j���[��BGM
    public AudioClip resultBGM; // ���U���g��ʗp��BGM
    public AudioClip conversationBGM; // ��b�V�[����BGM
    public AudioClip fieldBGM; // �t�B�[���h�V�[����BGM

    public AudioClip walkFootsteps;     // �����Ă���Ƃ��̑���
    public AudioClip runFootsteps;      // �����Ă���Ƃ��̑���

    // �����̏�Ԃ��Ǘ�����񋓌^
    private enum FootstepState
    {
        None,
        Walking,
        Running
    }

    // ���݂̑����̏�Ԃ�ێ�����ϐ�
    private FootstepState currentFootstepState = FootstepState.None;

    // �t�F�[�h�A�E�g�p�̃R���[�`����ێ�����ϐ�
    private Coroutine footstepsFadeCoroutine;

    // ���肩������ւ̑J�ڗp�̃R���[�`����ێ�����ϐ�
    private Coroutine runToWalkTransitionCoroutine;

    private void Awake()
    {
        // �V���O���g���p�^�[���̐ݒ�
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����ׂ��ł��I�u�W�F�N�g���c��
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���ʉ��̍Đ��֐�
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
            Debug.LogWarning("�I�[�f�B�I�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // �Q�[������BGM���Đ�
    public void PlayGameBGM()
    {
        if (gameBGM != null && bgmSource != null)
        {
            bgmSource.clip = gameBGM;
            bgmSource.loop = true; // ���[�v�Đ�
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // ���C�����j���[��BGM���Đ�
    public void PlayMenuBGM()
    {
        if (menuBGM != null && bgmSource != null)
        {
            bgmSource.clip = menuBGM;
            bgmSource.loop = true; // ���[�v�Đ�
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // ���U���gBGM���Đ�
    public void PlayResultBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = resultBGM;
            bgmSource.loop = true; // ���[�v�Đ�
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGM�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // ��b�V�[���p��BGM���Đ�
    public void PlayConversationBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = conversationBGM;
            bgmSource.loop = true; // ���[�v�Đ�
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGM�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // ��b�V�[���p��BGM���Đ�
    public void PlayFieldBGM()
    {
        if (resultBGM != null && bgmSource != null)
        {
            bgmSource.clip = fieldBGM;
            bgmSource.loop = true; // ���[�v�Đ�
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("ResultBGM�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // BGM�̒�~
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // �������Đ�����֐��i���s�j
    public void PlayWalkFootsteps()
    {
        if (currentFootstepState == FootstepState.Running)
        {
            // ���ɑ��s���ŁA�J�ڃR���[�`�������s���łȂ���ΊJ�n
            if (runToWalkTransitionCoroutine == null)
            {
                runToWalkTransitionCoroutine = StartCoroutine(TransitionRunToWalk(0.25f)); // �Z�b��ɕ��s���ɐ؂�ւ�
            }
            return;
        }

        // �t�F�[�h�A�E�g���̃R���[�`�����~
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        if (walkFootsteps != null && footstepsSource != null)
        {
            footstepsSource.clip = walkFootsteps;
            footstepsSource.loop = true; // ���[�v�Đ�
            footstepsSource.volume = 1f; // ���ʂ����ɖ߂�
            footstepsSource.Play();
            currentFootstepState = FootstepState.Walking;
        }
        else
        {
            Debug.LogWarning("���s�p�̃I�[�f�B�I�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }


    // �������Đ�����֐��i���s�j
    public void PlayRunFootsteps()
    {
        // �t�F�[�h�A�E�g���̃R���[�`�����~
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        // ���s������s�ւ̑J�ڃR���[�`�����~
        if (runToWalkTransitionCoroutine != null)
        {
            StopCoroutine(runToWalkTransitionCoroutine);
            runToWalkTransitionCoroutine = null;
        }

        if (runFootsteps != null && footstepsSource != null)
        {
            footstepsSource.clip = runFootsteps;
            footstepsSource.loop = true; // ���[�v�Đ�
            footstepsSource.volume = 1f; // ���ʂ����ɖ߂�
            footstepsSource.Play();
            currentFootstepState = FootstepState.Running;
        }
        else
        {
            Debug.LogWarning("���s�p�̃I�[�f�B�I�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // �������t�F�[�h�A�E�g���Ē�~����R���[�`��
    private IEnumerator FadeOutFootsteps(float duration)
    {
        float startVolume = footstepsSource.volume;

        while (footstepsSource.volume > 0)
        {
            footstepsSource.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        footstepsSource.Stop();
        footstepsSource.volume = startVolume; // �ė��p�̂��߂Ɍ��̉��ʂɖ߂�
        currentFootstepState = FootstepState.None; // ��Ԃ����Z�b�g
    }

    // �������~����֐����t�F�[�h�A�E�g�ɕύX
    public void StopFootsteps()
    {
        // �܂���Ԃ����Z�b�g
        currentFootstepState = FootstepState.None;

        // �t�F�[�h�A�E�g���̃R���[�`�����~
        if (footstepsFadeCoroutine != null)
        {
            StopCoroutine(footstepsFadeCoroutine);
            footstepsFadeCoroutine = null;
        }

        // ���s������s�ւ̑J�ڃR���[�`�����~
        if (runToWalkTransitionCoroutine != null)
        {
            StopCoroutine(runToWalkTransitionCoroutine);
            runToWalkTransitionCoroutine = null;
        }

        // �t�F�[�h�A�E�g���J�n
        footstepsFadeCoroutine = StartCoroutine(FadeOutFootsteps(0.15f)); // �t�F�[�h�A�E�g�ɁZ�b���w��
    }


    // ���s��������s���ɑJ�ڂ���R���[�`��
    private IEnumerator TransitionRunToWalk(float delay)
    {
        // 1�b�ԑ��s�����ێ�
        yield return new WaitForSeconds(delay);

        // ���݂����s���ł���Ε��s���ɐ؂�ւ�
        if (currentFootstepState == FootstepState.Running)
        {
            if (walkFootsteps != null && footstepsSource != null)
            {
                footstepsSource.clip = walkFootsteps;
                footstepsSource.loop = true; // ���[�v�Đ�
                footstepsSource.volume = 1f; // ���ʂ����ɖ߂�
                footstepsSource.Play();
                currentFootstepState = FootstepState.Walking;
            }
            else
            {
                Debug.LogWarning("���s�p�̃I�[�f�B�I�N���b�v�܂���AudioSource���ݒ肳��Ă��܂���B");
            }
        }

        // �R���[�`���̎Q�Ƃ��N���A
        runToWalkTransitionCoroutine = null;
    }

    // �������͂̉��imessageSound�j���Đ����郁�\�b�h
    public void PlayTypingSound()
    {
        if (messageSound != null && typingSource != null)
        {
            typingSource.clip = messageSound;
            typingSource.loop = true; // ���[�v�Đ�
            typingSource.Play();
        }
        else
        {
            Debug.LogWarning("MessageSound�N���b�v�܂���Typing AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    // �������͂̉����~���郁�\�b�h
    public void StopTypingSound()
    {
        if (typingSource != null && typingSource.isPlaying)
        {
            typingSource.loop = false; // ���[�v�Đ�������
            typingSource.Stop();
        }
    }
}
