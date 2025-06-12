using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ApplyLanguage_MenuScene : MonoBehaviour
{
    // Inspector�Ŏw�肷��Text�R���|�[�l���g

    [Header("MenuCanvas")]
    public TMP_Text gameTitle;
    public TMP_Text loadGameText;
    public TMP_Text startGameText;
    public TMP_Text settingsText;
    public TMP_Text quitGameText;
    public TMP_Text languageText;
    public TMP_Text creditsText;

    [Header("ConfirmResetCanvas")]
    public TMP_Text confirmText_ConfirmReset;
    public TMP_Text yesText_ConfrimReset;
    public TMP_Text noText_ConfrimReset;

    [Header("SettingsCanvas")]
    public TMP_Text settings_Settings;
    public TMP_Text graphics_Settings;
    public TMP_Text sound_Settings;
    public TMP_Text instructions_Settings;
    public TMP_Text resetGame_Settings;
    public TMP_Text close_Settings;

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

    [Header("LanguageSettingsCanvas")]
    public TMP_Text settings_Language;
    public TMP_Text close_Language;
    public TMP_Text language_Language;

    [Header("CreditsCanvas")]
    public TMP_Text settings_Credits;
    public TMP_Text close_Credits;


    // ����ݒ��K�p����X�N���v�g�B
    // ����̃C���f�b�N�X�i0: �p��, 1: ���{��j

    public void ApplyLanguage(int index)
    {
        //if (gameTitle == null)
        //{
            //Debug.LogWarning("GameTitle��TMP_Text��Inspector�Őݒ肳��Ă��܂���B");
            //return;
        //}

        // MenuCanvas
        switch (index)
        {
            case 0: // �p��
                gameTitle.text = "Hard Work Bar";
                break;
            case 1: // ���{��
                gameTitle.text = "Hard Work Bar";
                break;
            //default:
                //Debug.LogWarning("���Ή��̌���C���f�b�N�X���w�肳��܂����B�f�t�H���g�̉p���K�p���܂��B");
                //gameTitle.text = "GameTitle";
                //break;
        }

        switch (index)
        {
            case 0: // �p��
                loadGameText.text = "Load";
                break;
            case 1: // ���{��
                loadGameText.text = "���[�h";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                startGameText.text = "New Game";
                break;
            case 1: // ���{��
                startGameText.text = "�j���[�Q�[��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                settingsText.text = "Settings";
                break;
            case 1: // ���{��
                settingsText.text = "�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                quitGameText.text = "Quit";
                break;
            case 1: // ���{��
                quitGameText.text = "�Q�[���I��";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                languageText.text = "Language";
                break;
            case 1: // ���{��
                languageText.text = "����(Language)";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                creditsText.text = "Credits";
                break;
            case 1: // ���{��
                creditsText.text = "�N���W�b�g";
                break;
        }

        // ConfirmResetCanvas
        switch (index)
        {
            case 0: // �p��
                confirmText_ConfirmReset.text = "Do you want to delete the save data?";
                break;
            case 1: // ���{��
                confirmText_ConfirmReset.text = "�Z�[�u�f�[�^���폜���܂����H";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                yesText_ConfrimReset.text = "Delete";
                break;
            case 1: // ���{��
                yesText_ConfrimReset.text = "�f�[�^���폜";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                noText_ConfrimReset.text = "No";
                break;
            case 1: // ���{��
                noText_ConfrimReset.text = "������";
                break;
        }

        // SettingsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Settings.text = "Settings";
                break;
            case 1: // ���{��
                settings_Settings.text = "�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                graphics_Settings.text = "Graphics";
                break;
            case 1: // ���{��
                graphics_Settings.text = "�O���t�B�b�N�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                sound_Settings.text = "Sound";
                break;
            case 1: // ���{��
                sound_Settings.text = "�T�E���h�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                instructions_Settings.text = "Control";
                break;
            case 1: // ���{��
                instructions_Settings.text = "������@�ݒ�";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                resetGame_Settings.text = "Delete Save Data";
                break;
            case 1: // ���{��
                resetGame_Settings.text = "�Z�[�u�f�[�^�폜";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Settings.text = "Close";
                break;
            case 1: // ���{��
                close_Settings.text = "����";
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

        // LanguageSettingsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Language.text = "Language";
                break;
            case 1: // ���{��
                settings_Language.text = "����(Language)";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Language.text = "Close";
                break;
            case 1: // ���{��
                close_Language.text = "����";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                language_Language.text = "Select Language";
                break;
            case 1: // ���{��
                language_Language.text = "����I��";
                break;
        }

        // CreditsCanvas
        switch (index)
        {
            case 0: // �p��
                settings_Credits.text = "Credits";
                break;
            case 1: // ���{��
                settings_Credits.text = "Credits";
                break;
        }

        switch (index)
        {
            case 0: // �p��
                close_Credits.text = "Close";
                break;
            case 1: // ���{��
                close_Credits.text = "����";
                break;
        }


    }
}
