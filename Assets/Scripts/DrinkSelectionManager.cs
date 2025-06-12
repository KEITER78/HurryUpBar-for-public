using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkSelectionManager : MonoBehaviour
{
    //public SoundManager soundManager; // SoundManager �̎Q�Ƃ�����

    public GameObject drinkSelectionCanvas; // �h�����N�I����ʂ�Canvas
    public Image[] drinkImages; // SakeA ~ SakeF�̃h�����N�摜
    public GameObject[] drinkPrefabs; // �h�����N�ɑΉ�����Prefab�A�Z�b�g
    public Transform player; // �v���C���[
    public GameObject[] currentDrinks = new GameObject[2]; // �v���C���[�����Ă�h�����N��2�ɂ���
    public Transform kitchenLocation; // �L�b�`���̈ʒu
    public FirstPersonMovement playerMovement; // �v���C���[�̈ړ��Ǝ��_�ړ����Ǘ�����X�N���v�g
    public GameObject drinkSpotCanvas; // �h�����N�X�|�b�g�A�C�R���̉摜

    public Button removeDrinksButton; // �폜�{�^����Inspector�Ŏw��
    //public Image removeDrinksButtonImage; // �폜�{�^����Image�Ƃ���Inspector�Ŏw��
    //public Sprite removeDrinksButtonSprite; // �폜�{�^����SourceImage��Inspector�Ŏw��
    //public Sprite removeDrinksButtonSprite_en; // �폜�{�^����SourceImage��Inspector�Ŏw��_en

    public Vector3 firstDrinkPositionOffset = new Vector3(0, 0, 2);
    public Vector3 firstDrinkRotationOffset = Vector3.zero;
    public Vector3 firstDrinkScale = Vector3.one;

    public Vector3 secondDrinkPositionOffset = new Vector3(0, 0, 2);
    public Vector3 secondDrinkRotationOffset = Vector3.zero;
    public Vector3 secondDrinkScale = Vector3.one;


    // Trigger Distance��Inspector�Őݒ�\��
    public float triggerDistance = 3f; // �v���C���[���L�b�`���ɋ߂Â�������臒l

    // ���b�Z�[�W�\���Ɏg�p����Sprite
    public Sprite openMenuSprite; // �h�����N�I����ʂ��J�����b�Z�[�W��Sprite
    public Sprite closeMenuSprite; // �h�����N�I����ʂ���郁�b�Z�[�W��Sprite
    public GameObject messageCanvas; // ���b�Z�[�W��\������Canvas
    public Image messageImage; // ���b�Z�[�W�\���p��Image
    public TMP_Text messageText; // ���b�Z�[�W�\���p��Text

    private bool isNearKitchen = false; // �v���C���[���L�b�`���ɋ߂����ǂ���
    private bool isSelectionDisplayed = false; // �h�����N�I����ʂ��\������Ă��邩�ǂ���

    // �L�[�ݒ�p�̕ϐ�
    private KeyCode confirmKey;

    // �e��A�C�R����Inspector�Ŏw��
    //public Image drinkSelection; // �h�����N�I����ʂ̔w�iImage
    //public Sprite drinkSelectionSprite; // �h�����N�I����ʂ̔w�i��Sprite
    //public Sprite drinkSelectionSprite_en; // �h�����N�I����ʂ̔w�i��Sprite_en
    //public Image drinkSpotIcon; // �h�����N�I���ꏊ������Image
    //public Sprite drinkSpotIconSprite; // �h�����N�I���ꏊ�������摜��Sprite
    //public Sprite drinkSpotIconSprite_en; // �h�����N�I���ꏊ�������摜��Sprite_en

    // ����ݒ�̃C���f�b�N�X
    private int languageIndex;


    void Start()
    {
        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);

        // PlayerPrefs���猾��ݒ�̃C���f�b�N�X���擾
        languageIndex = PlayerPrefs.GetInt("Language", 0); // �f�t�H���g�l��0

        // ������Ԃł̓h�����N�I��UI�͔�\��
        drinkSelectionCanvas.SetActive(false);

        // ������Ԃł̓��b�Z�[�W�pCanvas����\��
        messageCanvas.SetActive(false);

        // �e�h�����N�摜�ɃN���b�N�C�x���g��ǉ�
        for (int i = 0; i < drinkImages.Length; i++)
        {
            int index = i; // ���[�J���ϐ���index��ۑ�
            drinkImages[i].GetComponent<Button>().onClick.AddListener(() => SelectDrink(index));
        }

        //SoundManager�̐ݒ肪����Ă��邩�m�F
        //if (soundManager == null)
        //{
        //    Debug.LogError("SoundManager���ݒ肳��Ă��܂���B");
        //}

        // �폜�{�^���ɃN���b�N�C�x���g��ǉ�
        removeDrinksButton.onClick.AddListener(RemoveDrinks);

        // �e��Image��Sprite��ݒ� // en��ǉ��������ׂ�
        //removeDrinksButtonImage.sprite = removeDrinksButtonSprite;
        //drinkSelection.sprite = drinkSelectionSprite;
        //drinkSpotIcon.sprite = drinkSpotIconSprite;
    }

    void Update()
    {

        // DrinkSpotIcon���v���C���[�̕����Ɍ�����
        Vector3 directionToDrinkSpotIcon = player.position - drinkSpotCanvas.transform.position;
        directionToDrinkSpotIcon.y = 0; // Y���̉�]��h��
        drinkSpotCanvas.transform.rotation = Quaternion.LookRotation(directionToDrinkSpotIcon);
        drinkSpotCanvas.transform.Rotate(0, 180, 0);

        // �v���C���[�ƃL�b�`���̋����𑪂�
        if (Vector3.Distance(player.position, kitchenLocation.position) < triggerDistance)
        {
            isNearKitchen = true; // �L�b�`���ɋ߂Â������Ƃ��t���O�ŊǗ�

            // ���b�Z�[�W��\������
            ShowMessage();
        }
        else
        {
            isNearKitchen = false; // ������������΃t���O��false�ɂ���

            // ���b�Z�[�W���\���ɂ���
            HideMessage();
        }

        // �L�b�`���ɋ߂Â���E�{�^�����������Ƃ�
        if (isNearKitchen && Input.GetKeyDown(confirmKey))
        {
            if (isSelectionDisplayed)
            {
                // ���ʉ��i���艹�j���Đ�
                SoundManager.instance.PlayDecisionSound();

                // �h�����N�I����ʂ����
                CloseDrinkSelection();
            }
            else
            {
                // ���ʉ��i���艹�j���Đ�
                SoundManager.instance.PlayDecisionSound();

                // �h�����N�I����ʂ��J��
                ShowDrinkSelection();
            }
        }
    }

    private void ShowDrinkSelection()
    {
        // �h�����N�I����ʂ�\��
        drinkSelectionCanvas.SetActive(true);
        isSelectionDisplayed = true;

        // �v���C���[�̈ړ��Ǝ��_�ړ��𖳌���
        playerMovement.DisableMovement();
        playerMovement.DisableLook();

        // ���b�Z�[�W��CloseMenu�ɐ؂�ւ���
        //messageImage.sprite = closeMenuSprite;
        
        switch (languageIndex)
        {
            case 0: // �p��
                messageText.text = $"Press <size=90>{confirmKey}</size> to close";
                break;
            case 1: // ���{��
                messageText.text = $"<size=90>{confirmKey}</size> �������ĕ���";
                break;
        }
        messageCanvas.SetActive(true);

        // �}�E�X�J�[�\����\��
        playerMovement.UnlockCursor();
    }

    private void CloseDrinkSelection()
    {
        // �h�����N�I����ʂ��\���ɂ���
        drinkSelectionCanvas.SetActive(false);
        isSelectionDisplayed = false;

        // �v���C���[�̈ړ��Ǝ��_�ړ���L����
        playerMovement.EnableMovement();
        playerMovement.EnableLook();

        // ���b�Z�[�W��OpenMenu�ɐ؂�ւ���
        //messageImage.sprite = openMenuSprite;       
        switch (languageIndex)
        {
            case 0: // �p��
                messageText.text = $"Press <size=90>{confirmKey}</size> to display the menu";
                break;
            case 1: // ���{��
                messageText.text = $"<size=90>{confirmKey}</size> �������ă��j���[��\��";
                break;
        }
        messageCanvas.SetActive(true);

        // �}�E�X�J�[�\�����\��
        playerMovement.LockCursor();
    }

    private void SelectDrink(int index)
    {
        // ���ʉ��i�N���b�N���j���Đ�
        SoundManager.instance.PlayClickSound();

        // ���ł�2�h�����N�������Ă���ꍇ��1�ڂ��폜���A2�ڂ�1�ڂɈڂ�
        if (currentDrinks[0] != null && currentDrinks[1] != null)
        {
            // 1�ڂ̃h�����N���폜
            Destroy(currentDrinks[0]);

            // 2�ڂ̃h�����N���폜���āA�V����1�ڂ̈ʒu�ɐ���������
            GameObject secondDrinkPrefab = currentDrinks[1]; // 2�ڂ̃h�����N���ꎞ�ۑ�
            Destroy(currentDrinks[1]); // 2�ڂ̃h�����N���폜
            currentDrinks[1] = null; // 2�ڂ̃X���b�g���N���A

            // 2�ڂ������h�����N��1�ڂ̃I�t�Z�b�g�ʒu�ɍĐ���
            Vector3 firstDrinkPosition = player.position + player.TransformDirection(firstDrinkPositionOffset);
            Quaternion firstDrinkRotation = Quaternion.Euler(firstDrinkRotationOffset);
            GameObject newFirstDrink = Instantiate(secondDrinkPrefab, firstDrinkPosition, firstDrinkRotation);
            newFirstDrink.transform.localScale = firstDrinkScale; // �X�P�[����K�p
            newFirstDrink.transform.SetParent(player);
            currentDrinks[0] = newFirstDrink; // 1�ڂɍĐݒ�
        }

        // �V�����h�����N�𐶐����A�ʒu�E��]�E�X�P�[����K�p
        if (currentDrinks[0] == null)
        {
            // 1�ڂ̃h�����N
            Vector3 spawnPosition = player.position + player.TransformDirection(firstDrinkPositionOffset); // 1�ڂ̃h�����N�̈ʒu�I�t�Z�b�g���g�p
            Quaternion spawnRotation = Quaternion.Euler(firstDrinkRotationOffset); // 1�ڂ̃h�����N�̉�]�I�t�Z�b�g���g�p
            GameObject newDrink = Instantiate(drinkPrefabs[index], spawnPosition, spawnRotation);
            newDrink.transform.localScale = firstDrinkScale; // 1�ڂ̃h�����N�̃X�P�[����K�p
            newDrink.transform.SetParent(player);
            currentDrinks[0] = newDrink;
        }
        else
        {
            // 2�ڂ̃h�����N
            Vector3 spawnPosition = player.position + player.TransformDirection(secondDrinkPositionOffset); // 2�ڂ̃h�����N�̈ʒu�I�t�Z�b�g���g�p
            Quaternion spawnRotation = Quaternion.Euler(secondDrinkRotationOffset); // 2�ڂ̃h�����N�̉�]�I�t�Z�b�g���g�p
            GameObject newDrink = Instantiate(drinkPrefabs[index], spawnPosition, spawnRotation);
            newDrink.transform.localScale = secondDrinkScale; // 2�ڂ̃h�����N�̃X�P�[����K�p
            newDrink.transform.SetParent(player);
            currentDrinks[1] = newDrink;
        }

        // �h�����N�I����ʂ����
        CloseDrinkSelection();
    }

    // �h�����N���폜����{�^���ɑΉ����郁�\�b�h
    public void RemoveDrinks()
    {
        // ���ʉ��i�N���b�N���j���Đ�
        SoundManager.instance.PlayClickSound();

        // �h�����N��S�č폜
        if (currentDrinks[0] != null)
        {
            Destroy(currentDrinks[0]);
            currentDrinks[0] = null;
        }
        if (currentDrinks[1] != null)
        {
            Destroy(currentDrinks[1]);
            currentDrinks[1] = null;
        }

        // �h�����N�I����ʂ����
        CloseDrinkSelection();
    }

    private void ShowMessage()
    {
        // �h�����N�I����ʂ̏�Ԃɉ����ă��b�Z�[�W��\��
        if (isSelectionDisplayed)
        {
            // CloseMenu���b�Z�[�W��\��
            //messageImage.sprite = closeMenuSprite;
            switch (languageIndex)
            {
                case 0: // �p��
                    messageText.text = $"Press <size=90>{confirmKey}</size> to close";
                    break;
                case 1: // ���{��
                    messageText.text = $"<size=90>{confirmKey}</size> �������ĕ���";
                    break;
            }
        }
        else
        {
            // OpenMenu���b�Z�[�W��\��
            //messageImage.sprite = openMenuSprite;
            switch (languageIndex)
            {
                case 0: // �p��
                    messageText.text = $"Press <size=90>{confirmKey}</size> to display the menu";
                    break;
                case 1: // ���{��
                    messageText.text = $"<size=90>{confirmKey}</size> �������ă��j���[��\��";
                    break;
            }
        }

        // ���b�Z�[�W��\��
        messageCanvas.SetActive(true);
    }

    private void HideMessage()
    {
        // ���b�Z�[�W���\���ɂ���
        messageCanvas.SetActive(false);
    }

    public void LoadKeySettings()
    {
        // PlayerPrefs����L�[�ݒ���擾�i�f�t�H���g�l�͎w�肳�ꂽ�l�j
        confirmKey = (KeyCode)PlayerPrefs.GetInt("ConfirmKey", (int)KeyCode.E);
    }
}
