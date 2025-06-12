using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ApplyLanguage_GameScene : MonoBehaviour
{
    private int languageIndex = 0; // 現在の言語インデックス（0: English, 1: 日本語）

    // Inspectorで指定するTextコンポーネント

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


    // 言語設定を適用するスクリプト。
    // 言語のインデックス（0: 英語, 1: 日本語）

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
            case 0: // 英語
                settings_Pause.text = "Pause";
                break;
            case 1: // 日本語
                settings_Pause.text = "Pause";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                resume_Pause.text = "Resume";
                break;
            case 1: // 日本語
                resume_Pause.text = "ゲーム再開";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                instructions_Pause.text = "Control";
                break;
            case 1: // 日本語
                instructions_Pause.text = "操作方法設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                graphics_Pause.text = "Graphics";
                break;
            case 1: // 日本語
                graphics_Pause.text = "グラフィック設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                sound_Pause.text = "Sound";
                break;
            case 1: // 日本語
                sound_Pause.text = "サウンド設定";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                title_Pause.text = "Return to Title";
                break;
            case 1: // 日本語
                title_Pause.text = "タイトルへ戻る";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                quit_Pause.text = "Exit Game";
                break;
            case 1: // 日本語
                quit_Pause.text = "ゲーム終了";
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

        // TitleConfirmCanvas
        switch (index)
        {
            case 0: // 英語
                confirm_Title.text = "Return to the title?\n\n<size=36>Current progress will not be saved.</size>";
                break;
            case 1: // 日本語
                confirm_Title.text = "タイトルへ戻りますか？\n\n<size=36>現在の進捗はセーブされません</size>";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                yes_Title.text = "Yes";
                break;
            case 1: // 日本語
                yes_Title.text = "はい";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                no_Title.text = "No";
                break;
            case 1: // 日本語
                no_Title.text = "いいえ";
                break;
        }

        // QuitConfirmCanvas
        switch (index)
        {
            case 0: // 英語
                confirm_Quit.text = "Exit the game?\n\n<size=36>Current progress will not be saved.</size>";
                break;
            case 1: // 日本語
                confirm_Quit.text = "ゲームを終了しますか？\n\n<size=36>現在の進捗はセーブされません</size>";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                yes_Quit.text = "Yes";
                break;
            case 1: // 日本語
                yes_Quit.text = "はい";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                no_Quit.text = "No";
                break;
            case 1: // 日本語
                no_Quit.text = "いいえ";
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

        // GameOverCanvas
        switch (index)
        {
            case 0: // 英語
                gameOver_GameOver.text = "Game Over\n\n<size=36>Angry level has reached its limit.</size>";
                break;
            case 1: // 日本語
                gameOver_GameOver.text = "Game Over\n\n<size=36>お客さんの不満が限界に達した</size>";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                menuButton_GameOver.text = "Return to title";
                break;
            case 1: // 日本語
                menuButton_GameOver.text = "タイトルへ戻る";
                break;
        }

        // ResultScreenCanvas
        switch (index)
        {
            case 0: // 英語
                stageClear_Result.text = "Result";
                break;
            case 1: // 日本語
                stageClear_Result.text = "Result";
                break;
        }

        switch (index)
        {
            case 0: // 英語
                nextStage_Result.text = "Save <size=50>and proceed</size>";
                break;
            case 1: // 日本語
                nextStage_Result.text = "セーブ<size=50>して次へ</size>";
                break;
        }

        // DrinkSelectionCanvas
        switch (index)
        {
            case 0: // 英語
                backgroundImage_Drink.sprite = drinkSelectionSprite_en;
                break;
            case 1: // 日本語
                backgroundImage_Drink.sprite = drinkSelectionSprite;
                break;
        }

        switch (index)
        {
            case 0: // 英語
                removeDrinks_Drink.sprite = removeDrinksButtonSprite_en;
                break;
            case 1: // 日本語
                removeDrinks_Drink.sprite = removeDrinksButtonSprite;
                break;
        }

        // DrinkManager
        switch (index)
        {
            case 0: // 英語
                drinkSpotIcon.sprite = drinkSpotIconSprite_en;
                break;
            case 1: // 日本語
                drinkSpotIcon.sprite = drinkSpotIconSprite;
                break;
        }

        // OrderCanvas
        switch (index)
        {
            case 0: // 英語
                orderWindow.sprite = orderFormSprite_en;
                break;
            case 1: // 日本語
                orderWindow.sprite = orderFormSprite;
                break;
        }
    }
}
