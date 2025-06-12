using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ApplyLanguage_MenuScene : MonoBehaviour
{
    // Inspectorで指定するTextコンポーネント

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


    // 言語設定を適用するスクリプト。
    // 言語のインデックス（0: 英語, 1: 日本語）

    public void ApplyLanguage(int index)
    {
        //if (gameTitle == null)
        //{
            //Debug.LogWarning("GameTitleのTMP_TextがInspectorで設定されていません。");
            //return;
        //}

        // MenuCanvas
        switch (index)
        {
            case 0: // 英語
                gameTitle.text = "Hard Work Bar";
                break;
            case 1: // 日本語
                gameTitle.text = "Hard Work Bar";
                break;
            //default:
                //Debug.LogWarning("未対応の言語インデックスが指定されました。デフォルトの英語を適用します。");
                //gameTitle.text = "GameTitle";
                //break;
        }

        switch (index)
        {
            case 0: // 英語
                loadGameText.text = "Load";
                break;
            case 1: // 日本語
                loadGameText.text = "ロード";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                startGameText.text = "New Game";
                break;
            case 1: // 日本語
                startGameText.text = "ニューゲーム";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                settingsText.text = "Settings";
                break;
            case 1: // 日本語
                settingsText.text = "設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                quitGameText.text = "Quit";
                break;
            case 1: // 日本語
                quitGameText.text = "ゲーム終了";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                languageText.text = "Language";
                break;
            case 1: // 日本語
                languageText.text = "言語(Language)";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                creditsText.text = "Credits";
                break;
            case 1: // 日本語
                creditsText.text = "クレジット";
                break;
        }

        // ConfirmResetCanvas
        switch (index)
        {
            case 0: // 英語
                confirmText_ConfirmReset.text = "Do you want to delete the save data?";
                break;
            case 1: // 日本語
                confirmText_ConfirmReset.text = "セーブデータを削除しますか？";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                yesText_ConfrimReset.text = "Delete";
                break;
            case 1: // 日本語
                yesText_ConfrimReset.text = "データを削除";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                noText_ConfrimReset.text = "No";
                break;
            case 1: // 日本語
                noText_ConfrimReset.text = "いいえ";
                break;
        }

        // SettingsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Settings.text = "Settings";
                break;
            case 1: // 日本語
                settings_Settings.text = "設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                graphics_Settings.text = "Graphics";
                break;
            case 1: // 日本語
                graphics_Settings.text = "グラフィック設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                sound_Settings.text = "Sound";
                break;
            case 1: // 日本語
                sound_Settings.text = "サウンド設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                instructions_Settings.text = "Control";
                break;
            case 1: // 日本語
                instructions_Settings.text = "操作方法設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                resetGame_Settings.text = "Delete Save Data";
                break;
            case 1: // 日本語
                resetGame_Settings.text = "セーブデータ削除";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Settings.text = "Close";
                break;
            case 1: // 日本語
                close_Settings.text = "閉じる";
                break;
        }

        // GraphicsSettingsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Graphics.text = "Graphics Settings";
                break;
            case 1: // 日本語
                settings_Graphics.text = "グラフィック設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Graphics.text = "Close";
                break;
            case 1: // 日本語
                close_Graphics.text = "閉じる";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                reset_Graphics.text = "Reset to Default";
                break;
            case 1: // 日本語
                reset_Graphics.text = "デフォルトに戻す";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                resolution_Graphics.text = "Resolution";
                break;
            case 1: // 日本語
                resolution_Graphics.text = "解像度";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                quality_Graphics.text = "Graphic Quality";
                break;
            case 1: // 日本語
                quality_Graphics.text = "グラフィック品質";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                fullScreen_Graphics.text = "FullScreen";
                break;
            case 1: // 日本語
                fullScreen_Graphics.text = "フルスクリーン";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                antiAliasing_Graphics.text = "Anti-Aliasing";
                break;
            case 1: // 日本語
                antiAliasing_Graphics.text = "アンチエイリアス";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                shadowQuality_Graphics.text = "Shadow";
                break;
            case 1: // 日本語
                shadowQuality_Graphics.text = "シャドー";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                frameRate_Graphics.text = "FrameRate";
                break;
            case 1: // 日本語
                frameRate_Graphics.text = "フレームレート";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                vSync_Graphics.text = "V-Sync";
                break;
            case 1: // 日本語
                vSync_Graphics.text = "垂直同期";
                break;
        }

        // SoundSettingsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Sound.text = "Sound Settings";
                break;
            case 1: // 日本語
                settings_Sound.text = "サウンド設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Sound.text = "Close";
                break;
            case 1: // 日本語
                close_Sound.text = "閉じる";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                reset_Sound.text = "Reset to Default";
                break;
            case 1: // 日本語
                reset_Sound.text = "デフォルトに戻す";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                masterVolume_Sound.text = "Master Volume";
                break;
            case 1: // 日本語
                masterVolume_Sound.text = "マスター音量";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                bgmVolume_Sound.text = "BGM";
                break;
            case 1: // 日本語
                bgmVolume_Sound.text = "BGM";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                sfxVolume_Sound.text = "SFX";
                break;
            case 1: // 日本語
                sfxVolume_Sound.text = "効果音";
                break;
        }

        // InstructionsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Instructions.text = "Control Settings";
                break;
            case 1: // 日本語
                settings_Instructions.text = "操作方法設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Instructions.text = "Close";
                break;
            case 1: // 日本語
                close_Instructions.text = "閉じる";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                reset_Instructions.text = "Reset to Default";
                break;
            case 1: // 日本語
                reset_Instructions.text = "デフォルトに戻す";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                lookSpeed_Instructions.text = "Mouse Sensitivity";
                break;
            case 1: // 日本語
                lookSpeed_Instructions.text = "マウス感度";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                currentKey1_Instructions.text = "Current Key";
                break;
            case 1: // 日本語
                currentKey1_Instructions.text = "現在のキー";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                currentKey2_Instructions.text = "Current Key";
                break;
            case 1: // 日本語
                currentKey2_Instructions.text = "現在のキー";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                forwardKey_Instructions.text = "Forward";
                break;
            case 1: // 日本語
                forwardKey_Instructions.text = "前";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                backwardKey_Instructions.text = "Backward";
                break;
            case 1: // 日本語
                backwardKey_Instructions.text = "後";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                leftKey_Instructions.text = "Left";
                break;
            case 1: // 日本語
                leftKey_Instructions.text = "左";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                rightKey_Instructions.text = "Right";
                break;
            case 1: // 日本語
                rightKey_Instructions.text = "右";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                runKey_Instructions.text = "Run";
                break;
            case 1: // 日本語
                runKey_Instructions.text = "ダッシュ";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                confirmKey_Instructions.text = "Confirm";
                break;
            case 1: // 日本語
                confirmKey_Instructions.text = "決定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                serveKey_Instructions.text = "Serve";
                break;
            case 1: // 日本語
                serveKey_Instructions.text = "提供";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                pauseKey_Instructions.text = "Pause";
                break;
            case 1: // 日本語
                pauseKey_Instructions.text = "ポーズ";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                foreach (var text in changeKeys_Instructions)
                {
                    text.text = "<size=20>change";
                }
                break;
            case 1: // 日本語
                foreach (var text in changeKeys_Instructions)
                {
                    text.text = "<size=29>変更";
                }
                break;
        }

        // LanguageSettingsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Language.text = "Language";
                break;
            case 1: // 日本語
                settings_Language.text = "言語(Language)";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Language.text = "Close";
                break;
            case 1: // 日本語
                close_Language.text = "閉じる";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                language_Language.text = "Select Language";
                break;
            case 1: // 日本語
                language_Language.text = "言語選択";
                break;
        }

        // CreditsCanvas
        switch (index)
        {
            case 0: // 英語
                settings_Credits.text = "Credits";
                break;
            case 1: // 日本語
                settings_Credits.text = "Credits";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                close_Credits.text = "Close";
                break;
            case 1: // 日本語
                close_Credits.text = "閉じる";
                break;
        }


    }
}
