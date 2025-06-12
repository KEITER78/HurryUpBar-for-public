using UnityEngine;

public class TestPlayManager : MonoBehaviour
{
    public static TestPlayManager instance;

    [Header("�e�X�g�v���C�����ǂ����̃t���O")]
    public bool isTestPlay = false;

    private void Awake()
    {
        // �V���O���g���̐ݒ�
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
