using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ApplyLanguage_GameScene : MonoBehaviour
{
    private int languageIndex = 0; // ���݂̌���C���f�b�N�X�i0: English, 1: ���{��j

    // Inspector�Ŏw�肷��Text�R���|�[�l���g

    [Header("PauseMenuCanvas")]
    public TMP_Text settings_Pause;
    public TMP_Text resume_Pause;
    public TMP_Text instructions_Pause;
    public TMP_Text graphics_Pause;
    public TMP_Text sound_Pause;
    public TMP_Text title_Pause;
    public TMP_Text quit_Pause;

    [Header("GraphicsSettingsCanvas")]
    public TMP_Text settings_Graphics;
    public TMP_Text close_Graphics;
    public TMP_Text reset_Graphics;
    public TMP_Text resolution_Graphics;
    public TMP_Text quality_Graphics;
    public TMP_Text fullScreen_Graphics;
    public TMP_Text antiAliasing_Graphics;
    public TMP_Text shadowQuality_Graphics;
    public TMP_Text frameRate_Graphics;
    public TMP_Text vSync_Graphics;

    [Header("SoundSettingsCanvas")]
    public TMP_Text settings_Sound;
    public TMP_Text close_Sound;
    public TMP_Text reset_Sound;
    public TMP_Text masterVolume_Sound;
    public TMP_Text bgmVolume_Sound;
    public TMP_Text sfxVolume_Sound;

    [Header("TitleConfirmCanvas")]
    public TMP_Text confirm_Title;
    public TMP_Text yes_Title;
    public TMP_Text no_Title;

    [Header("QuitConfirmCanvas")]
    public TMP_Text confirm_Quit;
    public TMP_Text yes_Quit;
    public TMP_Text no_Quit;

    [Header("InstructionsCanvas")]
    public TMP_Text settings_Instructions;
    public TMP_Text close_Instructions;
    public TMP_Text reset_Instructions;
    public TMP_Text lookSpeed_Instructions;
    public TMP_Text currentKey1_Instructions;
    public TMP_Text currentKey2_Instructions;
    public TMP_Text forwardKey_Instructions;
    public TMP_Text backwardKey_Instructions;
    public TMP_Text leftKey_Instructions;
    public TMP_Text rightKey_Instructions;
    public TMP_Text runKey_Instructions;
    public TMP_Text confirmKey_Instructions;
    public TMP_Text serveKey_Instructions;
    public TMP_Text pauseKey_Instructions;
    public List<TMP_Text> changeKeys_Instructions;

    [Header("GameOverCanvas")]
    public TMP_Text gameOver_GameOver;
    public TMP_Text menuButton_GameOver;

    [Header("ResultScreenCanvas")]
    public TMP_Text stageClear_Result;
    public TMP_Text nextStage_Result;

    [Header("DrinkSelectionCanvas")]
    public Image backgroundImage_Drink;
    public Image removeDrinks_Drink;
    public Sprite drinkSelectionSprite;
    public Sprite drinkSelectionSprite_en;
    public Sprite removeDrinksButtonSprite;
    public Sprite removeDrinksButtonSprite_en;

    [Header("DrinkManager")]
    public Image drinkSpotIcon;
    public Sprite drinkSpotIconSprite;
    public Sprite drinkSpotIconSprite_en;

    [Header("OrderCanvas")]
    public Image orderWindow;
    public Sprite orderFormSprite;
    public Sprite orderFormSprite_en;


    // ����ݒ��K�p����X�N���v�g�B
    // ����̃C���f�b�N�X�i0: �p��, 1: ���{��j

    void Start()
    {
        languageIndex = PlayerPrefs.GetInt("Language", 0);

        ApplyLanguage(languageIndex);
    }

    public void ApplyLanguage(int index)
    {
        // PauseMenuCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Pause.text = "Pause";
                break;
            case 1: // ���{��
                settings_Pause.text = "Pause";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                resume_Pause.text = "Resume";
                break;
            case 1: // ���{��
                resume_Pause.text = "�Q�[���ĊJ";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                instructions_Pause.text = "Control";
                break;
            case 1: // ���{��
                instructions_Pause.text = "������@�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                graphics_Pause.text = "Graphics";
                break;
            case 1: // ���{��
                graphics_Pause.text = "�O���t�B�b�N�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                sound_Pause.text = "Sound";
                break;
            case 1: // ���{��
                sound_Pause.text = "�T�E���h�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                title_Pause.text = "Return to Title";
                break;
            case 1: // ���{��
                title_Pause.text = "�^�C�g���֖߂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                quit_Pause.text = "Exit Game";
                break;
            case 1: // ���{��
                quit_Pause.text = "�Q�[���I��";
                break;
        }

        // GraphicsSettingsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Graphics.text = "Graphics Settings";
                break;
            case 1: // ���{��
                settings_Graphics.text = "�O���t�B�b�N�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Graphics.text = "Close";
                break;
            case 1: // ���{��
                close_Graphics.text = "����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                reset_Graphics.text = "Reset to Default";
                break;
            case 1: // ���{��
                reset_Graphics.text = "�f�t�H���g�ɖ߂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                resolution_Graphics.text = "Resolution";
                break;
            case 1: // ���{��
                resolution_Graphics.text = "�𑜓x";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                quality_Graphics.text = "Graphic Quality";
                break;
            case 1: // ���{��
                quality_Graphics.text = "�O���t�B�b�N�i��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                fullScreen_Graphics.text = "FullScreen";
                break;
            case 1: // ���{��
                fullScreen_Graphics.text = "�t���X�N���[��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                antiAliasing_Graphics.text = "Anti-Aliasing";
                break;
            case 1: // ���{��
                antiAliasing_Graphics.text = "�A���`�G�C���A�X";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                shadowQuality_Graphics.text = "Shadow";
                break;
            case 1: // ���{��
                shadowQuality_Graphics.text = "�V���h�[";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                frameRate_Graphics.text = "FrameRate";
                break;
            case 1: // ���{��
                frameRate_Graphics.text = "�t���[�����[�g";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                vSync_Graphics.text = "V-Sync";
                break;
            case 1: // ���{��
                vSync_Graphics.text = "��������";
                break;
        }

        // SoundSettingsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Sound.text = "Sound Settings";
                break;
            case 1: // ���{��
                settings_Sound.text = "�T�E���h�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Sound.text = "Close";
                break;
            case 1: // ���{��
                close_Sound.text = "����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                reset_Sound.text = "Reset to Default";
                break;
            case 1: // ���{��
                reset_Sound.text = "�f�t�H���g�ɖ߂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                masterVolume_Sound.text = "Master Volume";
                break;
            case 1: // ���{��
                masterVolume_Sound.text = "�}�X�^�[����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                bgmVolume_Sound.text = "BGM";
                break;
            case 1: // ���{��
                bgmVolume_Sound.text = "BGM";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                sfxVolume_Sound.text = "SFX";
                break;
            case 1: // ���{��
                sfxVolume_Sound.text = "���ʉ�";
                break;
        }

        // TitleConfirmCanvas
        switch (index)
        {
            case 0: // �p��
                confirm_Title.text = "Return to the title?\n\n<size=36>Current progress will not be saved.</size>";
                break;
            case 1: // ���{��
                confirm_Title.text = "�^�C�g���֖߂�܂����H\n\n<size=36>���݂̐i���̓Z�[�u����܂���</size>";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                yes_Title.text = "Yes";
                break;
            case 1: // ���{��
                yes_Title.text = "�͂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                no_Title.text = "No";
                break;
            case 1: // ���{��
                no_Title.text = "������";
                break;
        }

        // QuitConfirmCanvas
        switch (index)
        {
            case 0: // �p��
                confirm_Quit.text = "Exit the game?\n\n<size=36>Current progress will not be saved.</size>";
                break;
            case 1: // ���{��
                confirm_Quit.text = "�Q�[�����I�����܂����H\n\n<size=36>���݂̐i���̓Z�[�u����܂���</size>";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                yes_Quit.text = "Yes";
                break;
            case 1: // ���{��
                yes_Quit.text = "�͂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                no_Quit.text = "No";
                break;
            case 1: // ���{��
                no_Quit.text = "������";
                break;
        }

        // InstructionsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Instructions.text = "Control Settings";
                break;
            case 1: // ���{��
                settings_Instructions.text = "������@�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Instructions.text = "Close";
                break;
            case 1: // ���{��
                close_Instructions.text = "����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                reset_Instructions.text = "Reset to Default";
                break;
            case 1: // ���{��
                reset_Instructions.text = "�f�t�H���g�ɖ߂�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                lookSpeed_Instructions.text = "Mouse Sensitivity";
                break;
            case 1: // ���{��
                lookSpeed_Instructions.text = "�}�E�X���x";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                currentKey1_Instructions.text = "Current Key";
                break;
            case 1: // ���{��
                currentKey1_Instructions.text = "���݂̃L�[";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                currentKey2_Instructions.text = "Current Key";
                break;
            case 1: // ���{��
                currentKey2_Instructions.text = "���݂̃L�[";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                forwardKey_Instructions.text = "Forward";
                break;
            case 1: // ���{��
                forwardKey_Instructions.text = "�O";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                backwardKey_Instructions.text = "Backward";
                break;
            case 1: // ���{��
                backwardKey_Instructions.text = "��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                leftKey_Instructions.text = "Left";
                break;
            case 1: // ���{��
                leftKey_Instructions.text = "��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                rightKey_Instructions.text = "Right";
                break;
            case 1: // ���{��
                rightKey_Instructions.text = "�E";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                runKey_Instructions.text = "Run";
                break;
            case 1: // ���{��
                runKey_Instructions.text = "�_�b�V��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                confirmKey_Instructions.text = "Confirm";
                break;
            case 1: // ���{��
                confirmKey_Instructions.text = "����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                serveKey_Instructions.text = "Serve";
                break;
            case 1: // ���{��
                serveKey_Instructions.text = "��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                pauseKey_Instructions.text = "Pause";
                break;
            case 1: // ���{��
                pauseKey_Instructions.text = "�|�[�Y";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                foreach (var text in changeKeys_Instructions)
                {
                    text.text = "<size=20>change";
                }
                break;
            case 1: // ���{��
                foreach (var text in changeKeys_Instructions)
                {
                    text.text = "<size=29>�ύX";
                }
                break;
        }

        // GameOverCanvas
        switch (index)
        {
            case 0: // �p��
                gameOver_GameOver.text = "Game Over\n\n<size=36>Angry level has reached its limit.</size>";
                break;
            case 1: // ���{��
                gameOver_GameOver.text = "Game Over\n\n<size=36>���q����̕s�������E�ɒB����</size>";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                menuButton_GameOver.text = "Return to title";
                break;
            case 1: // ���{��
                menuButton_GameOver.text = "�^�C�g���֖߂�";
                break;
        }

        // ResultScreenCanvas
        switch (index)
        {
            case 0: // �p��
                stageClear_Result.text = "Result";
                break;
            case 1: // ���{��
                stageClear_Result.text = "Result";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                nextStage_Result.text = "Save <size=50>and proceed</size>";
                break;
            case 1: // ���{��
                nextStage_Result.text = "�Z�[�u<size=50>���Ď���</size>";
                break;
        }

        // DrinkSelectionCanvas
        switch (index)
        {
            case 0: // �p��
                backgroundImage_Drink.sprite = drinkSelectionSprite_en;
                break;
            case 1: // ���{��
                backgroundImage_Drink.sprite = drinkSelectionSprite;
                break;
        }

        switch (index)
        {
            case 0: // �p��
                removeDrinks_Drink.sprite = removeDrinksButtonSprite_en;
                break;
            case 1: // ���{��
                removeDrinks_Drink.sprite = removeDrinksButtonSprite;
                break;
        }

        // DrinkManager
        switch (index)
        {
            case 0: // �p��
                drinkSpotIcon.sprite = drinkSpotIconSprite_en;
                break;
            case 1: // ���{��
                drinkSpotIcon.sprite = drinkSpotIconSprite;
                break;
        }

        // OrderCanvas
        switch (index)
        {
            case 0: // �p��
                orderWindow.sprite = orderFormSprite_en;
                break;
            case 1: // ���{��
                orderWindow.sprite = orderFormSprite;
                break;
        }
    }
}
