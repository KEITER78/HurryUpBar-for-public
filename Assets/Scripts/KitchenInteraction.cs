using UnityEngine;

public class KitchenInteraction : MonoBehaviour
{
    public GameObject drinkSelectionCanvas; // �h�����N�I����ʂ�Canvas
    public Transform player; // �v���C���[

    private bool isNearKitchen = false; // �v���C���[���L�b�`���ɋ߂����ǂ���

    void Start()
    {
        // ������Ԃł̓h�����N�I��UI�͔�\��
        drinkSelectionCanvas.SetActive(false);
    }

    void Update()
    {
        // �v���C���[�ƃL�b�`���̋����𑪂�
        if (Vector3.Distance(player.position, transform.position) < 3f)
        {
            isNearKitchen = true; // �L�b�`���ɋ߂Â������Ƃ��t���O�ŊǗ�
        }
        else
        {
            isNearKitchen = false; // ������������΃t���O��false�ɂ���
        }

        // �L�b�`���ɋ߂Â���E�{�^�����������Ƃ�
        if (isNearKitchen && Input.GetKeyDown(KeyCode.E))
        {
            // �h�����N�I��UI��\��
            ShowDrinkSelection();
        }
    }

    private void ShowDrinkSelection()
    {
        // �h�����N�I����ʂ�\��
        drinkSelectionCanvas.SetActive(true);
    }
}
